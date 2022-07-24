use crate::auth::{auth_get_ftp_config_guard::FtpAuth, auth_preflag_request_guard::AuthPreflag};
use crate::{db_get, entities::*, pool::Db};
use rocket::Request;
use rocket::{
    http::Status, response, response::status, response::Responder, response::Response,
    serde::json::Json,
};
use sea_orm::{ActiveValue::*, EntityTrait};
use sea_orm_rocket::Connection;
use serde::Deserialize;

#[get("/city_project/<id>")]
pub async fn city_get(
    conn: Connection<'_, Db>,
    _auth_preflag: AuthPreflag,
    id: i32,
) -> Result<Json<plotsystem_city_projects::Model>, Status> {
    let db = conn.into_inner();

    match db_get::city_project::by_cp_id(db, id).await {
        Ok(cp) => Ok(Json(cp)),
        // Return error message in status
        Err(_) => Err(Status::BadRequest),
    }
}

// all fields except ID are required when creating a new city
#[derive(Deserialize)]
pub struct NewCityJson {
    pub country_id: i32,
    pub name: String,
    pub description: String,
    pub visible: bool,
}

#[post("/city_project", format = "json", data = "<city_json>")]
pub async fn city_post(
    conn: Connection<'_, Db>,
    auth_preflag: AuthPreflag,
    city_json: Json<NewCityJson>,
) -> Result<Status, status::BadRequest<String>> {
    let db = conn.into_inner();

    let AuthPreflag(api_key) = auth_preflag;

    match match db_get::api_keys::api_key_exists(db, &api_key).await {
        Ok(cp) => cp,
        Err(e) => return Err(status::BadRequest(Some(e.to_string()))),
    } {
        true => {
            let city = plotsystem_city_projects::ActiveModel {
                id: NotSet,
                country_id: Set(city_json.country_id.to_owned()),
                name: Set(city_json.name.to_owned()),
                description: Set(city_json.description.to_owned()),
                visible: Set(city_json.visible.to_owned()),
            };

            match plotsystem_city_projects::Entity::insert(city)
                .exec(db)
                .await
            {
                Ok(_) => Ok(Status::Ok),
                Err(e) => Err(status::BadRequest(Some(e.to_string()))),
            }
        }
        false => Ok(Status::Unauthorized),
    }
}

// all modify request values are optional; if one is omitted, don't change it
#[derive(Deserialize)]
pub struct EditCityJson {
    pub country_id: Option<i32>,
    pub name: Option<String>,
    pub description: Option<String>,
    pub visible: Option<bool>,
}

pub struct APIResponse<R>(pub Status, pub Option<R>);

impl<'r, 'o: 'r, R: Responder<'r, 'o>> Responder<'r, 'o> for APIResponse<R> {
    fn respond_to(self, req: &'r Request<'_>) -> response::Result<'o> {
        let mut build = Response::build();
        if let Some(responder) = self.1 {
            build.merge(responder.respond_to(req)?);
        }

        build.status(self.0).ok()
    }
}

#[put("/city_project/<id>", format = "json", data = "<city_json>")]
pub async fn city_put(
    conn: Connection<'_, Db>,
    auth_preflag: AuthPreflag,
    city_json: Json<EditCityJson>,
    id: i32,
) -> Result<Status, APIResponse<String>> {
    let db = conn.into_inner();

    let AuthPreflag(api_key) = auth_preflag;

    match match db_get::api_keys::cp_related_to_api_key(db, &api_key, &id).await {
        Ok(cp) => cp,
        Err(e) => return Err(APIResponse(Status::BadRequest, Some(e.to_string()))), //status::BadRequest(Some(e.to_string()))
    } {
        true => {
            let mut city: plotsystem_city_projects::ActiveModel =
                match db_get::city_project::by_cp_id(db, id).await {
                    Ok(city) => city.into(),
                    Err(e) => return Err(APIResponse(Status::BadRequest, Some(e.to_string()))), //status::BadRequest(Some(e.to_string()))
                };

            let mut modified = false;

            if !city_json.country_id.is_none() {
                city.country_id = Set(city_json.country_id.unwrap().to_owned());
                modified = true;
            }

            if !city_json.name.is_none() {
                city.name = Set(city_json.name.as_ref().unwrap().to_owned());
                modified = true;
            }

            if !city_json.description.is_none() {
                city.description = Set(city_json.description.as_ref().unwrap().to_owned());
                modified = true;
            }

            if !city_json.visible.is_none() {
                city.visible = Set(city_json.visible.unwrap().to_owned());
                modified = true;
            }

            if !modified {
                return Err(APIResponse(Status::NotModified, None));
            }

            match plotsystem_city_projects::Entity::update(city)
                .exec(db)
                .await
            {
                Ok(_) => Ok(Status::Ok),
                Err(e) => Err(APIResponse(Status::BadRequest, Some(e.to_string()))),
            }
        }
        false => Ok(Status::Unauthorized),
    }
}

#[delete("/city_project/<id>")]
pub async fn city_delete(
    conn: Connection<'_, Db>,
    auth_preflag: AuthPreflag,
    id: i32,
) -> Result<Status, APIResponse<String>> {
    let db = conn.into_inner();
    let AuthPreflag(api_key) = auth_preflag;

    match match db_get::api_keys::cp_related_to_api_key(db, &api_key, &id).await {
        Ok(cp) => cp,
        Err(e) => return Err(APIResponse(Status::BadRequest, Some(e.to_string()))),
    } {
        true => {
            let mut city: plotsystem_city_projects::ActiveModel =
                match db_get::city_project::by_cp_id(db, id).await {
                    Ok(city) => city.into(),
                    Err(e) => return Err(APIResponse(Status::BadRequest, Some(e.to_string()))),
                };

            match plotsystem_city_projects::Entity::delete(city)
                .exec(db)
                .await
            {
                Ok(_) => Ok(Status::Ok),
                Err(e) => Err(APIResponse(Status::BadRequest, Some(e.to_string()))),
            }
        }
        false => Ok(Status::Unauthorized),
    }
}
