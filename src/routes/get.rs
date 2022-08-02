use crate::auth::{auth_get_ftp_config_guard::FtpAuth, auth_preflag_request_guard::AuthPreflag};
use crate::{db_get, entities::*, pool::Db};
use rocket::{http::Status, serde::json::Json};
use sea_orm_rocket::Connection;

#[get("/server/<id_type>/<id>")]
pub async fn server(
    conn: Connection<'_, Db>,
    _auth_preflag: AuthPreflag,
    id_type: String,
    id: i32,
) -> Result<Json<plotsystem_servers::Model>, Status> {
    let db = conn.into_inner();

    match match id_type.as_str() {
        "server_id" => db_get::server::by_server_id(db, id).await,
        "country_id" => db_get::server::by_country_id(db, id).await,
        _ => return Err(Status::BadRequest),
    } {
        Ok(server) => Ok(Json(server)),
        Err(_) => Err(Status::BadRequest),
    }
}
