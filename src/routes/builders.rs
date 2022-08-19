use crate::auth::auth_preflag_request_guard::AuthPreflag;
use crate::{db_get, entities::*, pool::Db};
use rocket::Request;
use rocket::{http::Status, response, response::Responder, response::Response, serde::json::Json};
use sea_orm::{ActiveValue::*, EntityTrait};
use sea_orm_rocket::Connection;
use serde::Deserialize;
use uuid::Uuid;

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

#[get("/builders")]
pub async fn builders_get_all(
    conn: Connection<'_, Db>,
) -> Result<Json<Vec<plotsystem_builders::Model>>, APIResponse<String>> {
    let db = conn.into_inner();

    match db_get::builder::all(db).await {
        Ok(builders) => Ok(Json(builders)),
        Err(e) => Err(APIResponse(
            Status::InternalServerError,
            Some(e.to_string()),
        )),
    }
}

#[get("/builder/<uuid>")]
pub async fn builders_get(
    conn: Connection<'_, Db>,
    uuid: String,
) -> Result<APIResponse<Json<plotsystem_builders::Model>>, APIResponse<String>> {
    let db = conn.into_inner();

    let parsed_uuid = Uuid::parse_str(&uuid);

    if parsed_uuid.is_err() {
        return Err(APIResponse(Status::BadRequest, None));
    }

    match db_get::builder::by_uuid(db, parsed_uuid.unwrap()).await {
        Ok(builder) => {
            if (builder.is_none()) {
                Ok(APIResponse(Status::NotFound, None))
            } else {
                Ok(APIResponse(Status::Ok, Some(Json(builder.unwrap()))))
            }
        }
        Err(e) => Err(APIResponse(
            Status::InternalServerError,
            Some(e.to_string()),
        )),
    }
}
