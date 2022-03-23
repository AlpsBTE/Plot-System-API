#[macro_use]
extern crate rocket;
use sea_orm_rocket::Database;

mod pool;
use pool::Db;

mod auth;
mod db_get;
mod entities;
mod routes;

#[launch]
fn rocket() -> _ {
    rocket::build().attach(Db::init()).mount(
        "/api/v1",
        routes![
            routes::get::get_ftp_configuration,
            routes::get::get_city_project,
            routes::get::get_city_projects,
            routes::get::get_server,
            routes::get::get_plot,
            routes::get::get_plots,
            routes::get::byte_arr,
            routes::post::plot_add,
            routes::put::set_pasted,
            routes::post::upload_test,
        ],
    )
}
