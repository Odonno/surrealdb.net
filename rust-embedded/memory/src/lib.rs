use bindgen::{callback::{send_failure, send_success, FailureAction, SuccessAction}, csharp_to_rust::convert_csharp_to_rust_bytes};
use methods::{connect::connect_async, create::create_async, delete::delete_async, merge::merge_async, patch::patch_async, ping::ping_async, query::query_async, select::select_async, set::set_async, unset::unset_async, update::update_async, version::version_async};
use models::method::Method;
use runtime::{db::get_db, get_global_runtime};
use ::surrealdb::sql::{Array, Value};
//use surrealdb::{cbor::convert::Cbor, method::Method};
// use methods::{connect::connect_async, query::query_async, use_ns_db::use_ns_db_async};
// use runtime::create_global_runtime;
//use ::surrealdb::sql::Value;

use crate::methods::use_ns_db::use_ns_db_async;

mod bindgen;
mod cbor;
mod methods;
mod models;
mod runtime;
mod surrealdb;

// TODO : remove files from surrealdb module to use as much function from surrealdb package as possible

fn read_params(bytes: *const u8, len: i32) -> Result<Array, ()> {
    let bytes = convert_csharp_to_rust_bytes(bytes, len);
    cbor::get_params(bytes)
}

#[no_mangle]
pub extern "C" fn execute(
    id: i32,
    method: Method, 
    bytes: *const u8, 
    len: i32,
    success: SuccessAction, 
    failure: FailureAction
) {
    // let error = "Error";
    // let value = Value::Strand(error.into());

    // send_failure(value, failure);


	// let value: Cbor = value.try_into().unwrap();

    // let mut output = Vec::new();
    // ciborium::into_writer(&value.0, &mut output).unwrap();

    // let res = alloc_u8_buffer(output);

    // failure.invoke(res);

    // ---

    // TODO : No request type but only (enum Method, Option<Array> params) 

    // let bytes = convert_csharp_to_rust_bytes(bytes, len);
    // let params = cbor::get_params(bytes).ok();
    // let params = match cbor::get_params(bytes) {
    //     Ok(params) => params,
    //     Err(_) => {
    //         send_failure("Cannot retrieve params", failure);
    //         return;
    //     },
    // };
    //let request = cbor::req(bytes);

    //if let Ok(request) = request {
        // TODO
        //let Request { method, params } = request;

        // if method == Method::Connect {
        //     // unsafe {
        //     //     create_global_runtime();
        //     // };
        // }

        //let value = Value::None;

    match method {
        Method::Connect => {
            get_global_runtime().spawn(async move {
                // let Ok(db) = get_db(id).await else {
                //     send_failure("Cannot retrieve db", failure);
                //     return;
                // };
                send_success(Value::None, success, failure);
                // TODO : connect is useless
                //connect_async(db, success, failure).await;
            });
        },
        Method::Ping => {
            get_global_runtime().spawn(async move {
                let Ok(db) = get_db(id).await else {
                    send_failure("Cannot retrieve db", failure);
                    return;
                };
                ping_async(db, success, failure).await;
            });
        },
        Method::Use => {
            if let Ok(params) = read_params(bytes, len) {
                get_global_runtime().spawn(async move {
                    let Ok(db) = get_db(id).await else {
                        send_failure("Cannot retrieve db", failure);
                        return;
                    };
                    use_ns_db_async(db, params, success, failure).await;
                });
            } else {
                send_failure("Cannot retrieve params", failure);
            }
        },
        Method::Set => {
            if let Ok(params) = read_params(bytes, len) {
                get_global_runtime().spawn(async move {
                    let Ok(db) = get_db(id).await else {
                        send_failure("Cannot retrieve db", failure);
                        return;
                    };
                    set_async(db, params, success, failure).await;
                });
            } else {
                send_failure("Cannot retrieve params", failure);
            }
        },
        Method::Unset => {
            if let Ok(params) = read_params(bytes, len) {
                get_global_runtime().spawn(async move {
                    let Ok(db) = get_db(id).await else {
                        send_failure("Cannot retrieve db", failure);
                        return;
                    };
                    unset_async(db, params, success, failure).await;
                });
            } else {
                send_failure("Cannot retrieve params", failure);
            }
        },
        Method::Select => {
            if let Ok(params) = read_params(bytes, len) {
                get_global_runtime().spawn(async move {
                    let Ok(db) = get_db(id).await else {
                        send_failure("Cannot retrieve db", failure);
                        return;
                    };
                    select_async(db, params, success, failure).await;
                });
            } else {
                send_failure("Cannot retrieve params", failure);
            }
        },
        Method::Create => {
            if let Ok(params) = read_params(bytes, len) {
                get_global_runtime().spawn(async move {
                    let Ok(db) = get_db(id).await else {
                        send_failure("Cannot retrieve db", failure);
                        return;
                    };
                    create_async(db, params, success, failure).await;
                });
            } else {
                send_failure("Cannot retrieve params", failure);
            }
        },
        Method::Update => {
            if let Ok(params) = read_params(bytes, len) {
                get_global_runtime().spawn(async move {
                    let Ok(db) = get_db(id).await else {
                        send_failure("Cannot retrieve db", failure);
                        return;
                    };
                    update_async(db, params, success, failure).await;
                });
            } else {
                send_failure("Cannot retrieve params", failure);
            }
        },
        Method::Merge => {
            if let Ok(params) = read_params(bytes, len) {
                get_global_runtime().spawn(async move {
                    let Ok(db) = get_db(id).await else {
                        send_failure("Cannot retrieve db", failure);
                        return;
                    };
                    merge_async(db, params, success, failure).await;
                });
            } else {
                send_failure("Cannot retrieve params", failure);
            }
        },
        Method::Patch => {
            if let Ok(params) = read_params(bytes, len) {
                get_global_runtime().spawn(async move {
                    let Ok(db) = get_db(id).await else {
                        send_failure("Cannot retrieve db", failure);
                        return;
                    };
                    patch_async(db, params, success, failure).await;
                });
            } else {
                send_failure("Cannot retrieve params", failure);
            }
        },
        Method::Delete => {
            if let Ok(params) = read_params(bytes, len) {
                get_global_runtime().spawn(async move {
                    let Ok(db) = get_db(id).await else {
                        send_failure("Cannot retrieve db", failure);
                        return;
                    };
                    delete_async(db, params, success, failure).await;
                });
            } else {
                send_failure("Cannot retrieve params", failure);
            }
        },
        Method::Version => {
            get_global_runtime().spawn(async move {
                version_async(success, failure).await;
            });
        },
        Method::Query => {
            if let Ok(params) = read_params(bytes, len) {
                get_global_runtime().spawn(async move {
                    let Ok(db) = get_db(id).await else {
                        send_failure("Cannot retrieve db", failure);
                        return;
                    };
                    query_async(db, params, success, failure).await;
                });
            } else {
                send_failure("Cannot retrieve params", failure);
            }
        },
        _ => send_failure("Method not found", failure),
    }

        //send_success(value, success);
        // if let Ok(response) = response {
        //     send_success(value, success);
        // } else {
        //     let error = "Cannot serialize to CBOR";
        //     let value = Value::Strand(error.into());

        //     send_failure(value, failure);
        // }
    // } else {
    //     send_failure("Cannot deserialize from CBOR", failure);
    // }

    // match method {
    //     Method::Ping => Ok(Value::None.into()),
    //     Method::Info => self.info().await.map(Into::into).map_err(Into::into),
    //     Method::Use => self.yuse(params).await.map(Into::into).map_err(Into::into),
    //     Method::Signup => self.signup(params).await.map(Into::into).map_err(Into::into),
    //     Method::Signin => self.signin(params).await.map(Into::into).map_err(Into::into),
    //     Method::Invalidate => self.invalidate().await.map(Into::into).map_err(Into::into),
    //     Method::Authenticate => {
    //         self.authenticate(params).await.map(Into::into).map_err(Into::into)
    //     }
    //     Method::Kill => self.kill(params).await.map(Into::into).map_err(Into::into),
    //     Method::Live => self.live(params).await.map(Into::into).map_err(Into::into),
    //     Method::Set => self.set(params).await.map(Into::into).map_err(Into::into),
    //     Method::Unset => self.unset(params).await.map(Into::into).map_err(Into::into),
    //     Method::Select => self.select(params).await.map(Into::into).map_err(Into::into),
    //     Method::Insert => self.insert(params).await.map(Into::into).map_err(Into::into),
    //     Method::Create => self.create(params).await.map(Into::into).map_err(Into::into),
    //     Method::Update => self.update(params).await.map(Into::into).map_err(Into::into),
    //     Method::Merge => self.merge(params).await.map(Into::into).map_err(Into::into),
    //     Method::Patch => self.patch(params).await.map(Into::into).map_err(Into::into),
    //     Method::Delete => self.delete(params).await.map(Into::into).map_err(Into::into),
    //     Method::Version => self.version(params).await.map(Into::into).map_err(Into::into),
    //     Method::Query => self.query(params).await.map(Into::into).map_err(Into::into),
    //     Method::Relate => self.relate(params).await.map(Into::into).map_err(Into::into),
    //     Method::Run => todo!,
    //     Method::Unknown => Err(RpcError::MethodNotFound),
    // }
    
    // unsafe {
    //     create_global_runtime();
    // };

    // unsafe { get_global_runtime() }.spawn(async move {
    //     connect_async(callback, error_callback).await;
    // });
}

// #[no_mangle]
// pub extern "C" fn connect(callback: extern "C" fn(), error_callback: Option<extern "C" fn(str: *mut c_char)>) {
//     unsafe {
//         create_global_runtime();
//     };

//     unsafe { get_global_runtime() }.spawn(async move {
//         connect_async(callback, error_callback).await;
//     });
// }

// #[no_mangle]
// pub extern "C" fn query(
//     query_str: *const u16, 
//     query_len: i32, 
//     callback: extern "C" fn(res: *mut ByteBuffer), 
//     error_callback: Option<extern "C" fn(str: *mut c_char)>
// ) {
//     let query = convert_csharp_to_rust_string(query_str, query_len);

//     unsafe { get_global_runtime() }.spawn(async move {
//         query_async(query, callback, error_callback).await;
//     });
// }

// #[no_mangle]
// pub extern "C" fn use_ns_db(
//     ns_str: *const u16, 
//     ns_len: i32, 
//     db_str: *const u16, 
//     db_len: i32, 
//     callback: extern "C" fn(), 
//     error_callback: Option<extern "C" fn(str: *mut c_char)>
// ) {
//     let ns = convert_csharp_to_rust_string(ns_str, ns_len);
//     let db = convert_csharp_to_rust_string(db_str, db_len);

//     unsafe { get_global_runtime() }.spawn(async move {
//         use_ns_db_async(ns, db, callback, error_callback).await;
//     });
// }