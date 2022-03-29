use sea_orm::{DatabaseConnection, DbErr};

use crate::entities::{prelude::*, *};

use sea_orm::entity::*;

pub async fn by_ftp_id(
    db: &DatabaseConnection,
    ftp_id: i32,
) -> Result<Option<plotsystem_ftp_configurations::Model>, DbErr> {
    return PlotsystemFtpConfigurations::find_by_id(ftp_id)
        .one(db)
        .await;
}

pub async fn by_server_id(
    db: &DatabaseConnection,
    server_id: i32,
) -> plotsystem_ftp_configurations::Model {
    let ftp_id = super::server::by_server_id(db, server_id)
        .await
        .ftp_configuration_id
        .unwrap();

    println!("ftp id is {}", ftp_id);

    let ftp = by_ftp_id(db, ftp_id).await;

    return ftp;
}

pub async fn by_country_id(
    db: &DatabaseConnection,
    country_id: i32,
) -> plotsystem_ftp_configurations::Model {
    let server_id = super::country::by_country_id(db, country_id)
        .await
        .server_id;

    let ftp = by_server_id(db, server_id).await;

    return ftp;
}

pub async fn by_cp_id(db: &DatabaseConnection, cp_id: i32) -> plotsystem_ftp_configurations::Model {
    let country_id = super::city_project::by_cp_id(db, cp_id).await.country_id;

    let ftp = by_country_id(db, country_id).await;

    return ftp;
}
