use crate::auth::auth_preflag_request_guard::AuthPreflag;
use crate::entities::{prelude::*, *};
use crate::{db_get, entities::*, pool::Db};
use rocket::Request;
use rocket::{http::Status, response, response::Responder, response::Response, serde::json::Json};
use sea_orm::{ActiveValue::*, EntityTrait};
use sea_orm_rocket::Connection;
use serde::Deserialize;

#[get("/ftp/<id>")]
pub async fn ftp_get(
    conn: Connection<'_, Db>,
    id: i32,
) -> Result<Json<plotsystem_ftp_configurations::Model>, Status> {
    let db = conn.into_inner();

    match db_get::ftp_configuration::by_ftp_id(db, id).await {
        Ok(cp) => Ok(Json(cp)),
        // Return error message in status
        Err(_) => Err(Status::BadRequest),
    }
}
