//use std::error::Error;
use ciborium::Value as Data;
use surrealdb::sql::{Array, Value};

use crate::surrealdb::cbor::convert::Cbor;

//use crate::{models::request::Request, surrealdb::cbor::convert::Cbor};

// pub fn req(val: Vec<u8>) -> Result<Request, ()> {
// 	ciborium::from_reader::<Data, _>(&mut val.as_slice())
// 		.map_err(|_| ()) // TODO
// 		.map(Cbor)?
// 		.try_into()
// 		.map_err(|_| ()) // TODO
// }

pub fn get_params(val: Vec<u8>) -> Result<Array, ()> {
	let x = ciborium::from_reader::<Data, _>(&mut val.as_slice())
		.map_err(|_| ()); // TODO
	let x = x.map(Cbor)?;

	let x: Value= x.try_into().map_err(|_| ())?; // TODO

	if let Value::Array(arr) = x {
		Ok(arr)
	} else {
		Err(())
	}

	//<Cbor as TryInto<Value>>::try_into(val).map_err(|_| ())?.try_into()

	// if let Value::Array(arr) = x.0 {
	// 	Ok(arr)
	// } else {
	// 	Err(())
	// }

	// ciborium::from_reader::<Data, _>(&mut val.as_slice())
	// 	.map_err(|_| ()) // TODO
	// 	.map(Cbor)?
	// 	.try_into()
	// 	.map_err(|_| ()) // TODO
}

pub fn res(res: Value) -> Result<Vec<u8>, ()> {
	// Convert the response into a value
	let val: Value = res.into();
	let val: Cbor = val.try_into().unwrap(); // TODO
	// Create a new vector for encoding output
	let mut res = Vec::new();
	// Serialize the value into CBOR binary data
	ciborium::into_writer(&val.0, &mut res).unwrap();
	// Return the message length, and message as binary
	Ok(res)
}