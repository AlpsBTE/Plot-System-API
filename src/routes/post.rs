use crate::{entities::*, pool::Db};
use rocket::fs::TempFile;
use rocket::{http::Status, serde::json::Json};
use sea_orm::{ActiveValue::*, EntityTrait};
use sea_orm_rocket::Connection;

#[post("/plot/add", format = "json", data = "<plot_json>")]
pub async fn plot_add(
    conn: Connection<'_, Db>,
    auth_preflag: crate::auth::auth_preflag_request_guard::AuthPreflag,
    plot_json: Json<plotsystem_plots::Model>,
) -> Status {
    let db = conn.into_inner();

    let crate::auth::auth_preflag_request_guard::AuthPreflag(api_key) = auth_preflag;

    let authorized_api_keys =
        crate::db_get::api_keys::by_cp_id(db, plot_json.city_project_id).await;

    match authorized_api_keys
        .iter()
        .filter(|k| k.api_key == api_key)
        .collect::<Vec<&api_keys::Model>>()
        .len()
    {
        0 => return Status::Unauthorized,
        _ => {
            // print!("{:?}", plot_json.id);

            // this horrible chunk of code could probably be optimized using this:
            // https://www.sea-ql.org/SeaORM/docs/basic-crud/insert/
            // but I couldn't get it to work, so here we are

            let plot = plotsystem_plots::ActiveModel {
                id: NotSet,
                city_project_id: Set(plot_json.city_project_id.to_owned()),
                difficulty_id: Set(plot_json.difficulty_id.to_owned()),
                review_id: Set(plot_json.review_id.to_owned()),
                owner_uuid: Set(plot_json.owner_uuid.to_owned()),
                member_uuids: Set(plot_json.member_uuids.to_owned()),
                status: Set(plot_json.status.to_owned()),
                mc_coordinates: Set(plot_json.mc_coordinates.to_owned()),
                score: Set(plot_json.score.to_owned()),
                last_activity: Set(plot_json.last_activity.to_owned()),
                create_date: Set(plot_json.create_date.to_owned()),
                create_player: Set(plot_json.create_player.to_owned()),
                pasted: Set(plot_json.pasted.to_owned()),
            };

            print!("plot: {:#?}", plot_json);

            return match plotsystem_plots::Entity::insert(plot).exec(db).await {
                Ok(_) => Status::Ok,
                Err(_) => Status::InternalServerError,
            };
        }
    };
}

#[post("/upload_test", data = "<file>")]
pub async fn upload_test(mut file: TempFile<'_>) -> std::io::Result<()> {
    print!("{:#?}", file.content_type());
    file.persist_to("/file.txt").await
}
