use std::ffi::CString;

use crate::{bindgen::callback::{FailureAction, SuccessAction}, runtime::DB};

pub async fn use_ns_db_async(ns: String, db: String, success: SuccessAction, failure: FailureAction) {
// pub async fn use_ns_db_async(
//     ns: String,
//     db: String,
//     callback: extern "C" fn(), 
//     error_callback: Option<extern "C" fn(str: *mut c_char)>
// ) {
    let result = DB.use_ns(ns).use_db(db).await;

    match result {
        Ok(_) => {
            success();
        },
        Err(error) => {
            match failure {
                Some(f) => {
                    // TODO : String to Value
                    let str = CString::new(error.to_string()).unwrap();
                    f(str.into_raw())
                },
                None => (),
            }
        },
    }
}