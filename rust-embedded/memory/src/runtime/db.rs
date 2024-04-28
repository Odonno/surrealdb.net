use std::{collections::HashMap, sync::{Arc, RwLock}};
use once_cell::sync::Lazy;
use surrealdb::{engine::local::{Db, Mem}, Surreal};

static DBS: Lazy<Arc<RwLock<HashMap<i32, Arc<Surreal<Db>>>>>> = 
    Lazy::new(|| Arc::new(RwLock::new(HashMap::new())));

pub async fn get_db(id: i32) -> Result<Arc<Surreal<Db>>, String> {
    {
        // Scope to ensure the read lock is dropped before the await.
        let dbs = DBS.read().map_err(|e| e.to_string())?;
        if let Some(db) = dbs.get(&id) {
            return Ok(Arc::clone(db));
        }
    } // The read lock is dropped here.
    
    // let dbs = DBS.read().map_err(|e| e.to_string())?;
    // if let Some(db) = dbs.get(&id) {
    //     return Ok(Arc::clone(db));
    // }

    // drop(dbs); // Explicitly drop the read lock before acquiring a write lock.

    let db = Surreal::new::<Mem>(()).await.map_err(|e| e.to_string())?;
    let db_arc = Arc::new(db);
    let mut write_dbs = DBS.write().unwrap();
    write_dbs.insert(id, Arc::clone(&db_arc));
    Ok(db_arc)
}

// static mut DBS: Lazy<Arc<RwLock<HashMap<i32, Surreal<Db>>>>> = 
//     Lazy::new(|| Arc::new(RwLock::new(HashMap::new())));

// pub async fn get_db(id: i32) -> Result<&'static Surreal<Db>, String> {
//     unsafe {
//         let dbs = DBS.read().map_err(|e| e.to_string())?;
//         if let Some(db) = dbs.get(&id) {
//             // Clone the Surreal<Db> if it's cloneable, or implement another way to return the value.
//             return Ok(db);
//         }

//         drop(dbs); // Explicitly drop the read lock before acquiring a write lock.

//         let db = Surreal::new::<Mem>(()).await.map_err(|e| e.to_string())?;
//         let mut write_dbs = DBS.write().unwrap();
//         write_dbs.insert(id, db); // Clone the db before inserting.
//         Ok(&db)
//     }
// }

// pub async fn get_db(id: i32) -> Result<Surreal<Db>, String> {
//     unsafe {
//         let dbs = DBS.read().map_err(|e| e.to_string())?;
//         if let Some(db) = dbs.get(&id) {
//             // Clone the Surreal<Db> if it's cloneable, or implement another way to return the value.
//             return Ok(db.clone());
//         }

//         drop(dbs); // Explicitly drop the read lock before acquiring a write lock.

//         let db = Surreal::new::<Mem>(()).await.map_err(|e| e.to_string())?;
//         let mut write_dbs = DBS.write().unwrap();
//         write_dbs.insert(id, db.clone()); // Clone the db before inserting.
//         Ok(db)
//     }
// }

// pub async fn get_db(id: i32) -> Option<&'static Surreal<Db>> {
//     unsafe {
//         match DBS.read().expect("Cannot read DBS").get(&id) {
//             Some(db) => Some(db),
//             None => {
//                 let db = Surreal::new::<Mem>(()).await;

//                 match db {
//                     Ok(db) => {
//                         let mut write_dbs = DBS.write().unwrap();
//                         write_dbs.insert(id, db);
//                         DBS.read().expect("Cannot read DBS").get(&id)
//                     },
//                     Err(_) => None,
//                 }
//             }
//         }
//     }
// }

// pub static DB: Lazy<Surreal<Any>> = Lazy::new(Surreal::init);

// static mut DBS: Lazy<HashMap<i32, Surreal<Db>>> = Lazy::new(HashMap::new);

// pub async fn get_db(id: i32) -> Option<&'static Surreal<Db>> {
//     println!("get_db: {}", id);
//     unsafe {
//         match DBS.get(&id) {
//             Some(db) => Some(db),
//             None => {
//                 let db = Surreal::new::<Mem>(()).await;

//                 match db {
//                     Ok(db) => {
//                         DBS.insert(id, db);
//                         DBS.get(&id)
//                     },
//                     Err(_) => None,
//                 }
//                 // let value = DBS.insert(id, db);
//                 // value.as_ref()
//             }
//         }
//     }
// }

// -- 

// pub fn get_db(id: i32) -> &'static Surreal<Any> {
//     unsafe {
//         match DBS.get(&id) {
//             Some(db) => db,
//             None => {
//                 let db = Surreal::init();
//                 DBS.insert(id, db)
//                 // DBS.insert(id, &db);
//                 // &db
//             }
//         }
//     }
// }

#[no_mangle]
pub extern "C" fn dispose(id: i32) {
    unsafe {
        // TODO : impl drop for Surreal<Any>
        //DBS.remove(&id);
        DBS.write().unwrap().remove(&id);
    }
}