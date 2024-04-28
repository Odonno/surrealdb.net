use std::{collections::BTreeMap, sync::Arc};
use surrealdb::{engine::{any::Any, local::Db}, sql::{Array, Strand, Value}, Surreal};

use crate::{bindgen::callback::{send_failure, send_success, FailureAction, SuccessAction}, surrealdb::args::Take};

pub async fn select_async(client: Arc<Surreal<Db>>, params: Array, success: SuccessAction, failure: FailureAction) {
    let Ok(what) = params.needs_one() else {
        send_failure("Invalid params", failure);
        return;
    };

    let one = what.is_thing();

    let sql = "SELECT * FROM $what";

    let mut vars: BTreeMap<&str, Value> = BTreeMap::new();
    vars.insert("what", what.could_be_table());//.could_be_table());

    let response = client.query(sql).bind(vars).await;

    match response {
        Ok(mut response) => {
            let result = response.take::<Value>(0);

            match result {
                Ok(value) => {
                    let value = match one {
                        true => {
                            match value {
                                Value::Array(v) => {
                                    if v.is_empty() {
                                        Value::None
                                    } else {
                                        v.first().unwrap_or(&Value::None).clone()
                                    }
                                },
                                _ => value,
                            }
                        },
                        false => value,
                    };

                    send_success(value, success, failure);
                },
                Err(error) => {
                    send_failure(error.to_string().as_str(), failure);
                },
            }
            // let value = match one {
            //     true => value,//.first(),
            //     false => value,
            // };

            //send_success(value, success, failure);
        },
        Err(error) => {
            send_failure(error.to_string().as_str(), failure);
        },
    }

    // let vars = Some(map! {
    //     String::from("what") => what.could_be_table(),
    //     => &self.vars()
    // });

    // ---

    // let value = match what {
    //     Value::Strand(Strand(v)) => {
    //         let response: Result<Vec<Value>, surrealdb::Error> = client.select(v).await;
    //         response.map(|values | {
    //             let array = Array::from(values);
    //             let value = Value::Array(array);
    //             value
    //         }).map_err(|e| e.to_string())
    //     },
    //     Value::Thing(v) => {
    //         let response: Result<Option<Value>, surrealdb::Error> = client.select(v).await;
    //         response
    //             .map(|v| v.unwrap_or(Value::None))
    //             .map_err(|e| e.to_string())
    //     },
    //     _ => {
    //         Err("Invalid params".to_string())
    //         // send_failure("Invalid params", failure);
    //         // return;
    //     }
    // };

    // //let response: Result<Vec<Value>, surrealdb::Error> = client.select("what").await;
    
    // match value {
    //     Ok(value) => {
    //         send_success(value, success, failure);
    //     },
    //     Err(error) => {
    //         send_failure(error.as_str(), failure);
    //     },
    // }
}
