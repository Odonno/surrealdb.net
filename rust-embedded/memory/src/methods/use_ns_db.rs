use std::sync::Arc;
use surrealdb::{engine::{any::Any, local::Db}, sql::{Array, Value}, Surreal};

use crate::{bindgen::{alloc::alloc_u8_buffer, callback::{send_failure, send_success, FailureAction, SuccessAction}}, surrealdb::args::Take};

pub async fn use_ns_db_async(client: Arc<Surreal<Db>>, params: Array, success: SuccessAction, failure: FailureAction) {
// pub async fn use_ns_db_async(
//     ns: String,
//     db: String,
//     callback: extern "C" fn(), 
//     error_callback: Option<extern "C" fn(str: *mut c_char)>
// ) {
    let (ns, db) = match params.needs_two() {
        Ok((Value::Strand(ns), Value::Strand(db))) => (ns.0, db.0),
        _ => {
            send_failure("Invalid parameters", failure);
            return;
        }
    };
    // if let Value::Strand(ns) = ns {
    //     self.session_mut().ns = Some(ns.0);
    // }
    // if let Value::Strand(db) = db {
    //     self.session_mut().db = Some(db.0);
    // }

    let result = client.use_ns(ns).use_db(db).await;

    match result {
        Ok(_) => {
            send_success(Value::None, success, failure);
            // let value = Value::None;

            // let mut output = Vec::new();
            // ciborium::into_writer(&value, &mut output).unwrap();

            // let res = alloc_u8_buffer(output);

            // success.invoke(res);
        },
        Err(error) => {
            send_failure(error.to_string().as_str(), failure);
            // let value = Value::Strand(error.to_string().into());

            // let mut output = Vec::new();
            // ciborium::into_writer(&value, &mut output).unwrap();

            // let res = alloc_u8_buffer(output);

            // failure.invoke(res);
        },
    }
}