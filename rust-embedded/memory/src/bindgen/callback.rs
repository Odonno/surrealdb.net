use super::byte_buffer::ByteBuffer;

type GCHandlePtr = isize;

#[repr(C)]
pub struct RustGCHandle {
    ptr: GCHandlePtr,
    drop_callback: extern "C" fn(GCHandlePtr),
}

impl Drop for RustGCHandle {
    fn drop(&mut self) {
        (self.drop_callback)(self.ptr);
    }
}

#[repr(C)]
pub struct SuccessAction {
    handle: RustGCHandle,
    callback: unsafe extern "C" fn(GCHandlePtr, *mut ByteBuffer),
}

impl SuccessAction {
    pub fn invoke(&self, value: *mut ByteBuffer) {
        unsafe { (self.callback)(self.handle.ptr, value) };
    }
}

#[repr(C)]
pub struct FailureAction {
    handle: RustGCHandle,
    callback: unsafe extern "C" fn(GCHandlePtr, *mut ByteBuffer),
}

impl FailureAction {
    pub fn invoke(&self, value: *mut ByteBuffer) {
        unsafe { (self.callback)(self.handle.ptr, value) };
    }
}