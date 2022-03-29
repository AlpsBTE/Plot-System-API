use sea_orm::{DatabaseConnection, DbErr};

use crate::entities::{prelude::*, *};

use sea_orm::entity::*;

pub async fn by_country_id(
    db: &DatabaseConnection,
    country_id: i32,
) -> Result<Option<plotsystem_countries::Model>, DbErr> {
    return PlotsystemCountries::find_by_id(country_id).one(db).await;
}

pub async fn by_cp_id(db: &DatabaseConnection, cp_id: i32) -> plotsystem_countries::Model {
    let country_id = super::city_project::by_cp_id(db, cp_id).await.country_id;

    let country = by_country_id(db, country_id).await;

    return country;
}

pub async fn by_plot_id(db: &DatabaseConnection, plot_id: i32) -> plotsystem_countries::Model {
    let country_id = super::city_project::by_plot_id(db, plot_id)
        .await
        .country_id;

    let country = by_country_id(db, country_id).await;

    return country;
}
