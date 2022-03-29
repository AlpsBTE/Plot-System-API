use sea_orm::{DatabaseConnection, DbErr};

use crate::entities::{prelude::*, *};

use sea_orm::entity::*;

pub async fn by_cp_id(
    db: &DatabaseConnection,
    cp_id: i32,
) -> Result<Option<plotsystem_city_projects::Model>, DbErr> {
    return PlotsystemCityProjects::find_by_id(cp_id).one(db).await;
}

pub async fn by_plot_id(db: &DatabaseConnection, plot_id: i32) -> plotsystem_city_projects::Model {
    let cp_id = super::plot::by_plot_id(db, plot_id).await.city_project_id;

    let city_project = by_cp_id(db, cp_id);

    return city_project.await;
}

pub async fn all(db: &DatabaseConnection) -> Vec<plotsystem_city_projects::Model> {
    let city_project = PlotsystemCityProjects::find().all(db).await.unwrap();

    return city_project;
}
