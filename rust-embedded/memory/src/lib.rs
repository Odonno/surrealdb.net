use bindgen::{alloc::alloc_u8_buffer, callback::{FailureAction, SuccessAction}, csharp_to_rust::convert_csharp_to_rust_bytes};
use methods::{connect::connect_async, query::query_async, use_ns_db::use_ns_db_async};
use runtime::create_global_runtime;

mod bindgen;
mod methods;
mod runtime;
mod surrealdb;

// TODO : remove files from surrealdb module to use as much function from surrealdb package as possible

#[no_mangle]
pub extern "C" fn execute(bytes: *const u8, len: i32, success: SuccessAction, failure: FailureAction) {
    let bytes = convert_csharp_to_rust_bytes(bytes, len);
    let request = surrealdb::cbor::req(bytes);

    if let Some(request) = request {
        // TODO
        // let { method, params, .. } = request;

        // if method == Method::Connect {
        //     // unsafe {
        //     //     create_global_runtime();
        //     // };
        // }

        let value = Value::None;

        let response = surrealdb::cbor::res(value);

        if let Some(response) = response {
            let buffer = alloc_u8_buffer(response);
            success(buffer)
        } else {
            let error = "Cannot serialize to CBOR";
            failure(error)
        }
    } else {
        let error = "Cannot deserialize from CBOR";
        failure(error)
    }

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