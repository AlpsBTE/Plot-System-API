use sea_orm::{DatabaseConnection, DbErr};

use crate::entities::{prelude::*, *};
use sea_orm::entity::*;
use uuid::Uuid;

pub async fn by_uuid(
    db: &DatabaseConnection,
    uuid: Uuid,
) -> Result<Option<plotsystem_builders::Model>, DbErr> {
    PlotsystemBuilders::find_by_id(uuid.as_hyphenated().to_string())
        .one(db)
        .await
}

pub async fn all(db: &DatabaseConnection) -> Result<Vec<plotsystem_builders::Model>, DbErr> {
    PlotsystemBuilders::find().all(db).await
}
