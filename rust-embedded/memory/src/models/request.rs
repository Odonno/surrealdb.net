// use once_cell::sync::Lazy;
// use surrealdb::sql::{Array, Number, Part, Value};

// use crate::surrealdb::{cbor::convert::Cbor, method::Method};

// pub static METHOD: Lazy<[Part; 1]> = Lazy::new(|| [Part::from("method")]);
// pub static PARAMS: Lazy<[Part; 1]> = Lazy::new(|| [Part::from("params")]);

// pub struct Request {
// 	pub method: Method,
// 	pub params: Array,
// }

// impl TryFrom<Cbor> for Request {
// 	type Error = (); // TODO
// 	fn try_from(val: Cbor) -> Result<Self, Self::Error> {
// 		<Cbor as TryInto<Value>>::try_into(val).map_err(|_| ())?.try_into()
// 	}
// }

// impl TryFrom<Value> for Request {
// 	type Error = (); // TODO
// 	fn try_from(val: Value) -> Result<Self, Self::Error> {
// 		// Fetch the 'method' argument
// 		let method = match val.pick(&*METHOD) {
// 			Value::Strand(v) => Method::parse(v.to_raw()),
// 			Value::Number(Number::Int(v)) => match u8::try_from(v) {
// 				Ok(v) => Method::from(v),
// 				_ => return Err(()),
// 			},
// 			_ => return Err(()),
// 		};
// 		// Fetch the 'params' argument
// 		let params = match val.pick(&*PARAMS) {
// 			Value::Array(v) => v,
// 			_ => Array::new(),
// 		};
// 		// Return the parsed request
// 		Ok(Request {
// 			method,
// 			params,
// 		})
// 	}
// }