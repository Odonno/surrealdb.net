use once_cell::sync::{Lazy, OnceCell};
use surrealdb::Surreal;
use tokio::runtime::{Builder, Runtime};

pub static mut RUNTIME: OnceCell<Runtime> = OnceCell::new();
pub static DB: Lazy<Surreal<Any>> = Lazy::new(Surreal::init);

pub unsafe fn create_global_runtime() {
    RUNTIME
        .set(
            Builder::new_multi_thread()
                .worker_threads(num_cpus::get())
                .enable_all()
                .build()
                .unwrap(),
        )
        .unwrap();
}

pub unsafe fn get_global_runtime<'local>() -> &'local Runtime {
    RUNTIME.get_unchecked()
}