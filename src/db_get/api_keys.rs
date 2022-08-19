use sea_orm::{Condition, DatabaseConnection, DbErr, JoinType, QueryFilter, QuerySelect};

use crate::entities::{prelude::*, *};

use sea_orm::entity::*;

pub async fn api_key_exists(db: &DatabaseConnection, api_key: &str) -> Result<bool, DbErr> {
    match PlotsystemApiKeys::find()
        .filter(Condition::all().add(plotsystem_api_keys::Column::ApiKey.eq(api_key)))
        .one(db)
        .await?
    {
        Some(_) => Ok(true),
        None => Ok(false),
    }
}

pub async fn country_related_to_api_key(
    db: &DatabaseConnection,
    api_key: &String,
    country_id: i32,
) -> Result<bool, DbErr> {
    match plotsystem_api_keys::Entity::find()
        .join(
            JoinType::InnerJoin,
            plotsystem_api_keys::Relation::PlotsystemBuildteams.def(),
        )
        .join(
            JoinType::InnerJoin,
            plotsystem_buildteams::Relation::PlotsystemBuildteamHasCountries.def(),
        )
        .join(
            JoinType::InnerJoin,
            plotsystem_buildteam_has_countries::Relation::PlotsystemCountries.def(),
        )
        .filter(
            Condition::all()
                .add(plotsystem_api_keys::Column::ApiKey.eq(api_key.to_owned()))
                .add(plotsystem_countries::Column::Id.eq(country_id)),
        )
        .all(db)
        .await?
        .len()
    {
        0 => Ok(false),
        _ => Ok(true),
    }
}

pub async fn cp_related_to_api_key(
    db: &DatabaseConnection,
    api_key: &String,
    cp_id: i32,
) -> Result<bool, DbErr> {
    match plotsystem_api_keys::Entity::find()
        .join(
            JoinType::InnerJoin,
            plotsystem_api_keys::Relation::PlotsystemBuildteams.def(),
        )
        .join(
            JoinType::InnerJoin,
            plotsystem_buildteams::Relation::PlotsystemBuildteamHasCountries.def(),
        )
        .join(
            JoinType::InnerJoin,
            plotsystem_buildteam_has_countries::Relation::PlotsystemCountries.def(),
        )
        .join(
            JoinType::InnerJoin,
            plotsystem_countries::Relation::PlotsystemCityProjects.def(),
        )
        .filter(
            Condition::all()
                .add(plotsystem_api_keys::Column::ApiKey.eq(api_key.to_owned()))
                .add(plotsystem_city_projects::Column::Id.eq(cp_id)),
        )
        .all(db)
        .await?
        .len()
    {
        0 => Ok(false),
        _ => Ok(true),
    }
}

pub async fn plot_related_to_api_key(
    db: &DatabaseConnection,
    api_key: &String,
    plot_id: i32,
) -> Result<bool, DbErr> {
    match plotsystem_api_keys::Entity::find()
        .join(
            JoinType::InnerJoin,
            plotsystem_api_keys::Relation::PlotsystemBuildteams.def(),
        )
        .join(
            JoinType::InnerJoin,
            plotsystem_buildteams::Relation::PlotsystemBuildteamHasCountries.def(),
        )
        .join(
            JoinType::InnerJoin,
            plotsystem_buildteam_has_countries::Relation::PlotsystemCountries.def(),
        )
        .join(
            JoinType::InnerJoin,
            plotsystem_countries::Relation::PlotsystemCityProjects.def(),
        )
        .join(
            JoinType::InnerJoin,
            plotsystem_city_projects::Relation::PlotsystemPlots.def(),
        )
        .filter(
            Condition::all()
                .add(plotsystem_api_keys::Column::ApiKey.eq(api_key.to_owned()))
                .add(plotsystem_plots::Column::Id.eq(plot_id)),
        )
        .all(db)
        .await?
        .len()
    {
        0 => Ok(false),
        _ => Ok(true),
    }
}

pub async fn server_related_to_api_key(
    db: &DatabaseConnection,
    api_key: &String,
    server_id: i32,
) -> Result<bool, DbErr> {
    match plotsystem_api_keys::Entity::find()
        .join(
            JoinType::InnerJoin,
            plotsystem_api_keys::Relation::PlotsystemBuildteams.def(),
        )
        .join(
            JoinType::InnerJoin,
            plotsystem_buildteams::Relation::PlotsystemBuildteamHasCountries.def(),
        )
        .join(
            JoinType::InnerJoin,
            plotsystem_buildteam_has_countries::Relation::PlotsystemCountries.def(),
        )
        .join(
            JoinType::InnerJoin,
            plotsystem_countries::Relation::PlotsystemServers.def(),
        )
        .filter(
            Condition::all()
                .add(plotsystem_api_keys::Column::ApiKey.eq(api_key.to_owned()))
                .add(plotsystem_servers::Column::Id.eq(server_id)),
        )
        .all(db)
        .await?
        .len()
    {
        0 => Ok(false),
        _ => Ok(true),
    }
}

pub async fn ftp_configuration_related_to_api_key(
    db: &DatabaseConnection,
    api_key: &String,
    ftp_id: i32,
) -> Result<bool, DbErr> {
    match plotsystem_api_keys::Entity::find()
        .join(
            JoinType::InnerJoin,
            plotsystem_api_keys::Relation::PlotsystemBuildteams.def(),
        )
        .join(
            JoinType::InnerJoin,
            plotsystem_buildteams::Relation::PlotsystemBuildteamHasCountries.def(),
        )
        .join(
            JoinType::InnerJoin,
            plotsystem_buildteam_has_countries::Relation::PlotsystemCountries.def(),
        )
        .join(
            JoinType::InnerJoin,
            plotsystem_countries::Relation::PlotsystemServers.def(),
        )
        .join(
            JoinType::InnerJoin,
            plotsystem_servers::Relation::PlotsystemFtpConfigurations.def(),
        )
        .filter(
            Condition::all()
                .add(plotsystem_api_keys::Column::ApiKey.eq(api_key.to_owned()))
                .add(plotsystem_ftp_configurations::Column::Id.eq(ftp_id)),
        )
        .all(db)
        .await?
        .len()
    {
        0 => Ok(false),
        _ => Ok(true),
    }
}
