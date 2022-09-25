use crate::auth::{
    auth_preflag_request_guard::AuthPreflag, auth_put_plot_request_guard::AuthPutGuard,
};
use crate::{db_get, entities::*, pool::Db};
use rocket::http::Status;
use sea_orm::{ActiveModelTrait, ActiveValue::*};
use sea_orm_rocket::Connection;

#[put("/plot/set_pasted/<plot_id>?<pasted>")]
pub async fn set_pasted(
    conn: Connection<'_, Db>,
    _auth_preflag: AuthPreflag,
    _auth: AuthPutGuard,
    plot_id: i32,
    pasted: bool,
) -> Status {
    let db = conn.into_inner();

    let mut plot: plotsystem_plots::ActiveModel = match db_get::plot::by_plot_id(db, plot_id).await
    {
        Ok(plot) => plot.into(),
        Err(_) => return Status::BadRequest,
    };

    plot.pasted = Set(pasted);

    return match plot.update(db).await {
        Ok(_) => Status::Ok,
        Err(_) => Status::InternalServerError,
    };
}
