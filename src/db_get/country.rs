use sea_orm::{DatabaseConnection, DbErr};

use crate::entities::{prelude::*, *};

use sea_orm::entity::*;

pub async fn by_id(
    db: &DatabaseConnection,
    id: i32,
) -> Result<plotsystem_countries::Model, DbErr> {
    match PlotsystemCountries::find_by_id(id).one(db).await? {
        Some(cp) => Ok(cp),
        None => Err(DbErr::RecordNotFound(format!(
            "Country with id {} does not exists",
            id
        ))),
    }
}

pub async fn all(db: &DatabaseConnection) -> Result<Vec<plotsystem_countries::Model>, DbErr> {
    PlotsystemCountries::find().all(db).await
}
