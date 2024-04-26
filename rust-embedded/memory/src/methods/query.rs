use std::ffi::CString;
use surrealdb::sql::Value;

use crate::{bindgen::{byte_buffer::ByteBuffer, callback::{FailureAction, SuccessAction}}, cbor::Cbor, runtime::DB};

pub async fn query_async(query: String, success: SuccessAction, failure: FailureAction) {
// pub async fn query_async(
//     query: String, 
//     callback: extern "C" fn(res: *mut ByteBuffer), 
//     error_callback: Option<extern "C" fn(str: *mut c_char)>
// ) {
    let result = DB.query(&query).await;
    
    match result {
        Ok(mut response) => {
            let count = response.num_statements();
            
            let mut values = Vec::<_>::new();

            for index in 0..count {
                // We extract the value from the response.
                let value: Value = response.take(index).unwrap();

				// Then we convert the SurrealQL in to CBOR.
				let cbor = Cbor::try_from(value).unwrap();

                values.push(cbor.0);
            }

            // Then serialize the CBOR as binary data.
            let mut output = Vec::new();
            ciborium::into_writer(&values, &mut output).unwrap();

            let buffer = ByteBuffer::from_vec(output);
            let res = Box::into_raw(Box::new(buffer));

            success(res);
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
