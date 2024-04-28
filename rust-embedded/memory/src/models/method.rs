#[repr(u8)]
pub enum Method {
    Connect = 1,
    Ping = 2,
    Use = 3,
    Kill = 4,
    Live = 5,
    Set = 6,
    Unset = 7,
    Select = 8,
    //Insert = 9, // TODO
    Create = 10,
    Update = 11,
    Merge = 12,
    Patch = 13,
    Delete = 14,
    Version = 15,
    Query = 16,
    // TODO : Relate
}
