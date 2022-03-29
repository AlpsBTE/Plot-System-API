use crate::entities::{prelude::*, *};
use sea_orm::DatabaseConnection;

use sea_orm::entity::*;
use sea_orm::DbErr;

pub async fn by_server_id(
    db: &DatabaseConnection,
    server_id: i32,
) -> Result<Option<plotsystem_servers::Model>, DbErr> {
    return PlotsystemServers::find_by_id(server_id).one(db).await;
}

pub async fn by_country_id(
    db: &DatabaseConnection,
    country_id: i32,
) -> Result<Option<plotsystem_servers::Model>, DbErr> {
    let server_id = match super::country::by_country_id(db, country_id).await {
        Ok(T) => match T {
            Some(T) => T,
            None => {}
        },
        Err(E) => return E,
    }
    .server_id;

    return by_server_id(db, server_id).await;
}
