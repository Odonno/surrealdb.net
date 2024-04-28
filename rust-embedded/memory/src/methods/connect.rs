use std::sync::Arc;
use surrealdb::{engine::{any::Any, local::Db}, sql::Value, Surreal};

use crate::{bindgen::{alloc::alloc_u8_buffer, callback::{send_failure, send_success, FailureAction, SuccessAction}}};
//pub async fn connect_async(callback: extern "C" fn(), error_callback: Option<extern "C" fn(str: *mut c_char)>) {
pub async fn connect_async(client: Arc<Surreal<Any>>, success: SuccessAction, failure: FailureAction) {
    let result = client.connect("mem://").await;

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