use crate::auth::{
    auth_preflag_request_guard::AuthPreflag, auth_put_plot_request_guard::AuthPutGuard,
};
use crate::entities::{prelude::*, *};
use crate::{db_get, entities::*, pool::Db};
use rocket::Request;
use rocket::{
    http::Status, response::status, response::Responder, response::Response, serde::json::Json,
};
use sea_orm::{ActiveValue::*, EntityTrait};
use sea_orm_rocket::Connection;
use serde::Deserialize;

#[get("/plot/<plot_id>")]
pub async fn plot_get(
    conn: Connection<'_, Db>,
    _auth_preflag: AuthPreflag,
    plot_id: i32,
) -> Result<Json<plotsystem_plots::Model>, Status> {
    let db = conn.into_inner();

    match db_get::plot::by_plot_id(db, plot_id).await {
        Ok(plot) => Ok(Json(plot)),
        // Return error message in status
        Err(_) => Err(Status::BadRequest),
    }
}

#[get("/plots?<status>&<pasted>&<limit>")]
pub async fn plots_get(
    conn: Connection<'_, Db>,
    _auth_preflag: AuthPreflag,
    status: Option<sea_orm_active_enums::Status>,
    pasted: Option<bool>,
    limit: Option<u32>,
) -> Result<Json<Vec<plotsystem_plots::Model>>, Status> {
    let db = conn.into_inner();

    match db_get::plot::filtered(db, status, pasted, limit).await {
        Ok(plots) => Ok(Json(plots)),
        // Return error message in status
        Err(_) => Err(Status::BadRequest),
    }
}

#[derive(Deserialize)]
pub struct PlotRequestJson {
    pub city_project_id: i32,
    pub difficulty_id: i32,
    pub mc_coordinates: String,
    pub create_player: String,
    pub outline: String,
}

#[post("/plot", format = "json", data = "<plot_json>")]
pub async fn plot_add(
    conn: Connection<'_, Db>,
    auth_preflag: AuthPreflag,
    plot_json: Json<PlotRequestJson>,
) -> Result<Status, status::BadRequest<String>> {
    let db = conn.into_inner();

    let AuthPreflag(api_key) = auth_preflag;

    match match db_get::api_keys::cp_related_to_api_key(db, &api_key, plot_json.city_project_id)
        .await
    {
        Ok(cp) => cp,
        Err(e) => return Err(status::BadRequest(Some(e.to_string()))),
    } {
        true => {
            // this horrible chunk of code could probably be optimized using this:
            // https://www.sea-ql.org/SeaORM/docs/basic-crud/insert/
            // but I couldn't get it to work, so here we are

            let plot = plotsystem_plots::ActiveModel {
                id: NotSet,
                city_project_id: Set(plot_json.city_project_id.to_owned()),
                difficulty_id: Set(plot_json.difficulty_id.to_owned()),
                review_id: NotSet,
                owner_uuid: NotSet,
                member_uuids: NotSet,
                status: Set(sea_orm_active_enums::Status::Unclaimed),
                mc_coordinates: Set(plot_json.mc_coordinates.to_owned()),
                score: NotSet,
                last_activity: NotSet,
                create_date: NotSet,
                create_player: Set(plot_json.create_player.to_owned()),
                pasted: Set(0),
                outline: Set(Some(plot_json.outline.to_owned())),
            };

            match plotsystem_plots::Entity::insert(plot).exec(db).await {
                Ok(_) => Ok(Status::Ok),
                Err(e) => Err(status::BadRequest(Some(format!("{:#?}", e)))), // I'm gonna go to hell for this
            }
        }
        false => Ok(Status::Unauthorized),
    }
}

#[put("/plot/set_pasted/<plot_id>?<pasted>")]
pub async fn set_pasted(
    conn: Connection<'_, Db>,
    _auth_preflag: AuthPreflag,
    _auth: AuthPutGuard,
    plot_id: i32,
    pasted: i8,
) -> Status {
    let db = conn.into_inner();

    let mut plot: plotsystem_plots::ActiveModel = match db_get::plot::by_plot_id(db, plot_id).await
    {
        Ok(plot) => plot.into(),
        Err(_) => return Status::BadRequest,
    };

    plot.pasted = Set(pasted);

    return match plotsystem_plots::Entity::update(plot).exec(db).await {
        Ok(_) => Status::Ok,
        Err(_) => Status::InternalServerError,
    };
}
