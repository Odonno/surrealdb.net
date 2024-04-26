use std::ffi::CString;

use crate::{bindgen::callback::{FailureAction, SuccessAction}, runtime::DB};

//pub async fn connect_async(callback: extern "C" fn(), error_callback: Option<extern "C" fn(str: *mut c_char)>) {
pub async fn connect_async(success: SuccessAction, failure: FailureAction) {
    let result = DB.connect("mem://").await;

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