pub mod db;

use once_cell::sync::OnceCell;
//use surrealdb::{engine::any::Any, Surreal};
use tokio::runtime::{Builder, Runtime};

pub static mut RUNTIME: OnceCell<Runtime> = OnceCell::new();
// pub static mut RUNTIME: OnceCell<Runtime> = OnceCell::new();
//pub static DB: Lazy<Surreal<Any>> = Lazy::new(Surreal::init);

// fn create_global_runtime() {
//     unsafe {
//         RUNTIME
//             .set(
//                 Builder::new_multi_thread()
//                     .worker_threads(num_cpus::get())
//                     .enable_all()
//                     .build()
//                     .unwrap(),
//             )
//             .unwrap();
//     }
// }
#[no_mangle]
pub unsafe extern "C" fn create_global_runtime() {
    unsafe {
        RUNTIME
            .set(
                Builder::new_multi_thread()
                    .worker_threads(num_cpus::get())
                    .enable_all()
                    .build()
                    .unwrap(),
            ).unwrap();
    }
}

pub fn get_global_runtime<'local>() -> &'local Runtime {
    unsafe {
        // RUNTIME.get_or_init(
        //     || {
        //         Builder::new_multi_thread()
        //             .worker_threads(num_cpus::get())
        //             .enable_all()
        //             .build()
        //             .unwrap()
        //     }
        // )
        // match RUNTIME.get() {
        //     Some(runtime) => runtime,
        //     None => {
        //         create_global_runtime();
        //         RUNTIME.get_unchecked()
        //     }
        // }
        // RUNTIME.get_or_init(|| {
        //     create_global_runtime();
        //     RUNTIME.get_unchecked()
        // })
        RUNTIME.get_unchecked()
    }
}