use std::{error::Error, time::Duration};

// TODO : check diff between src/surrealdb and GitHub repository
// TODO : check diff between src/surrealdb/cbor.rs and https://github.com/surrealdb/surrealdb/blob/main/core/src/rpc/format/cbor/convert.rs
// TODO : check diff between src/surrealdb/method.rs and https://github.com/surrealdb/surrealdb/blob/main/core/src/rpc/method.rs

fn main() -> Result<(), Box<dyn Error>> {
    csbindgen::Builder::default()
        .input_extern_file("src/lib.rs")
        .input_extern_file("src/bindgen/byte_buffer.rs")
        .input_extern_file("src/bindgen/callback.rs")
        .input_extern_file("src/bindgen/free.rs")
        .csharp_dll_name("memory")
        .generate_csharp_file("../../SurrealDb.Embedded.InMemory/NativeMethods.g.cs")?;

    std::thread::sleep(Duration::from_secs(1));

    Ok(())
}