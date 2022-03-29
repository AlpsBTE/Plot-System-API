use std::process::exit;

use sea_orm::{DatabaseConnection, DbErr, QueryFilter};

use crate::entities::{prelude::*, *};

use sea_orm::entity::*;

pub async fn api_key_exists(db: &DatabaseConnection, api_key: &str) -> bool {
    return match ApiKeys::find_by_id(api_key.to_owned()).one(db).await {
        Ok(m) => match m {
            Some(_) => true,
            None => false,
        },
        Err(e) => {
            print!("{:#?}", e);
            exit(0)
        }
    };
}

pub async fn by_plot_id(
    db: &DatabaseConnection,
    plot_id: i32,
) -> Result<Vec<api_keys::Model>, DbErr> {
    let country_id = super::country::by_plot_id(db, plot_id).await.id;

    return ApiKeys::find()
        .filter(api_keys::Column::CountryId.eq(country_id))
        .all(db)
        .await;
}

pub async fn by_cp_id(db: &DatabaseConnection, cp_id: i32) -> Vec<api_keys::Model> {
    let country_id = super::country::by_cp_id(db, cp_id).await.id;

    let api_keys = ApiKeys::find()
        .filter(api_keys::Column::CountryId.eq(country_id))
        .all(db)
        .await
        .unwrap();

    return api_keys;
}
