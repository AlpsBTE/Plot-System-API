use crate::auth::auth_preflag_request_guard::AuthPreflag;
use crate::entities::{prelude::*, *};
use crate::{db_get, entities::*, pool::Db};
use rocket::Request;
use rocket::{http::Status, response, response::Responder, response::Response, serde::json::Json};
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