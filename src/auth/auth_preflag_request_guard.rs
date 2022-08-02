use rocket::http::Status;
use rocket::outcome::Outcome;
use rocket::outcome::Outcome::Success;
use rocket::request::{self, FromRequest, Request};
use sea_orm_rocket::Connection;

use super::AuthError;

use crate::pool::Db;

// this will check if the api key is even present in the database

#[derive(Debug)]
pub struct AuthPreflag(pub String);

#[rocket::async_trait]
impl<'r> FromRequest<'r> for AuthPreflag {
    type Error = AuthError;

    async fn from_request(req: &'r Request<'_>) -> request::Outcome<Self, Self::Error> {
        let api_key = match req
            .headers()
            .get("authorization")
            .collect::<Vec<&str>>()
            .get(0)
        {
            Some(key) => key.to_string(),

            // 401 Unauthorized is sent due to that status code being used to signify you don't have authorization in your request
            // None => return Outcome::Failure((Status::BadRequest, AuthError::Missing)),
            None => return Outcome::Failure((Status::Unauthorized, AuthError::Missing)),
        };

        let db = match req.guard::<Connection<'_, Db>>().await {
            Success(db) => db.into_inner(),

            // 403 Forbidden is sent due to that status code being used to signify you don't have permission
            // _ => return Outcome::Failure((Status::BadRequest, AuthError::Invalid)),
            _ => return Outcome::Failure((Status::BadRequest, AuthError::Invalid)),
        };

        match match crate::db_get::api_keys::api_key_exists(db, &api_key).await {
            Ok(exists) => exists,
            Err(e) => {
                // 500 Internal Server Error is sent because of a database error
                return Outcome::Failure((
                    Status::InternalServerError,
                    AuthError::DataBaseError(e.to_string()),
                ));
            }
        } {
            true => Outcome::Success(AuthPreflag(api_key)),
            false => Outcome::Failure((Status::Unauthorized, AuthError::Unauthorized)),
        }
    }
}
