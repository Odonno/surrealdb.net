use cargo_metadata::MetadataCommand;
use surrealdb::sql::{Strand, Value};

use crate::bindgen::callback::{send_failure, send_success, FailureAction, SuccessAction};

    // let path = std::env::var("CARGO_MANIFEST_DIR").unwrap();
    // let meta = MetadataCommand::new()
    //     .manifest_path("./Cargo.toml")
    //     .current_dir(&path)
    //     .exec()
    //     .map_err(|e| e.to_string());

    // let root = meta.root_package().unwrap();

    // // find "surrealdb" dependency
    // let surrealdb_dep = root.dependencies.iter()
    //     .find(|d| d.name == "surrealdb")
    //     .unwrap();

    // let result = &surrealdb_dep.req;

fn get_version() -> Result<String, String> {
    let path= env!("CARGO_MANIFEST_DIR");

    let metadata = MetadataCommand::new()
        .manifest_path("./Cargo.toml")
        .current_dir(&path)
        .exec()
        .map_err(|e| e.to_string())?;

    metadata.packages.iter()
        .find(|d| d.name == "surrealdb")
        .ok_or_else(|| "Cannot surrealdb package".to_string())
        .map(|package| package.version.to_string())
}

pub async fn version_async(success: SuccessAction, failure: FailureAction) {
    match get_version() {
        Ok(value) => {
            let value = Value::Strand(Strand::from(value));
            send_success(value, success, failure);
        },
        Err(error) => {
            send_failure(error.to_string().as_str(), failure);
        },
    }

    // root.dependencies.iter().for_each(|(name, dep)| {
    //     println!("{}: {}", name, dep.version());
    // });
    // //let option = root.metadata["dependencies"]["surrealdb"].as_str().unwrap();
    //let version = &root.version;

    //let result = root.dependencies;
    //let result = client.version().await;

    // match result {
    //     Ok(value) => {
    //         let value = Value::Strand(Strand::from(value.to_string()));
    //         send_success(value, success, failure);
    //     },
    //     Err(error) => {
    //         send_failure(error.to_string().as_str(), failure);
    //     },
    // }
}