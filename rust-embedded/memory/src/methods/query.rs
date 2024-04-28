use std::{collections::BTreeMap, sync::Arc};
use surrealdb::{engine::{any::Any, local::Db}, sql::{Array, Object, Strand, Value}, Surreal};

use crate::{bindgen::{alloc::alloc_u8_buffer, callback::{send_failure, send_success, FailureAction, SuccessAction}}, surrealdb::{args::Take, cbor::convert::Cbor}};

pub async fn query_async(client: Arc<Surreal<Db>>, params: Array, success: SuccessAction, failure: FailureAction) {
// pub async fn query_async(
//     query: String, 
//     callback: extern "C" fn(res: *mut ByteBuffer), 
//     error_callback: Option<extern "C" fn(str: *mut c_char)>
// ) {
    let Ok((query, o)) = params.needs_one_or_two() else {
        send_failure("Invalid params", failure);
        return;
    };

    let query = match query {
        Value::Strand(Strand(v)) => v,
        _ => {
            send_failure("Invalid params", failure);
            return;
        }
    };

    let o = match o {
        Value::Object(v) => Some(v),
        Value::None | Value::Null => None,
        _ => {
            send_failure("Invalid params", failure);
            return;
        }
    };

    let vars = match o {
        Some(v) => Some(v.0),
        None => None,
    };

    //let mut query = client.query(&query).bind(vars);

    // if let Some(vars) = vars {
    //     for (k, v) in vars {
    //         query.bind((k, v));
    //     } 
    //     // vars.iter().for_each(|(k, v)| {
    //     //     query.bind((k, v));
    //     // });
    // }
    
    let response = client.query(&query).bind(vars).with_stats().await;

    match response {
        Ok(mut response) => {
            let count = response.num_statements();

            let mut array = Array::with_capacity(count);

            // TODO
            for index in 0..count {
                // TODO : no unwrap
                let (stats, result) = response.take::<Value>(index).unwrap();

                let mut o: BTreeMap<&str, Value> = BTreeMap::new();
                
                let time = stats.execution_time
                    .map(|_| "0ns") // TODO
                    .unwrap_or("0ns");
                o.insert("time", Value::Strand(Strand::from(time)));

                match result {
                    Ok(v) => {
                        o.insert("status", Value::Strand(Strand::from("OK")));
                        o.insert("result", v);
                    },
                    Err(e) => {
                        o.insert("status", Value::Strand(Strand::from("ERR")));
                        o.insert("errorDetails", Value::Strand(Strand::from(e.to_string())));
                    }
                }

                array.push(Value::Object(Object::from(o)));
            }

            let value = Value::Array(array);

            send_success(value, success, failure);

            // ---

            // let count = response.num_statements();
            
            // let mut values = Vec::<_>::new();

            // for index in 0..count {
            //     // We extract the value from the response.
            //     let result = response.take(index).unwrap();

            //     let res = dbs::Response {
            //         time: result.0.execution_time.unwrap(),
            //         result: result.1.map_err(Into::into),
            //         query_type: dbs::QueryType::Other
            //     };
            //     //let value: Value = response.take(index).unwrap().0.;

			// 	// // Then we convert the SurrealQL in to CBOR.
			// 	// let cbor = Cbor::try_from(value).unwrap();

            //     // values.push(cbor.0);
            // }

            // let cbor = Cbor::try_from(values).unwrap();

            // // Then serialize the CBOR as binary data.
            // let mut output = Vec::new();
            // ciborium::into_writer(&value, &mut output).unwrap();

            // ---
            //let value: Value = response.try_into().unwrap();
            // ---

            // let count = response.num_statements();
            
            // let mut values = Vec::<_>::new();

            // for index in 0..count {
            //     // We extract the value from the response.
            //     let value: Value = response.take(index).unwrap();

			// 	// Then we convert the SurrealQL in to CBOR.
			// 	let cbor = Cbor::try_from(value).unwrap();

            //     values.push(cbor.0);
            // }

            // // Then serialize the CBOR as binary data.
            // let mut output = Vec::new();
            // ciborium::into_writer(&values, &mut output).unwrap();

            // let res = alloc_u8_buffer(output);

            // success.invoke(res);
        },
        Err(error) => {
            send_failure(error.to_string().as_str(), failure);

            // let value = Value::Strand(error.to_string().into());

            // let mut output = Vec::new();
            // ciborium::into_writer(&value, &mut output).unwrap();

            // let res = alloc_u8_buffer(output);

            // failure.invoke(res);
        },
    }
}
