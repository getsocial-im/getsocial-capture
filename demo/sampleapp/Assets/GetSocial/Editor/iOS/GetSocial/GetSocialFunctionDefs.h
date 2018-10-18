typedef void(VoidCallbackDelegate)(void *actionPtr);

typedef void(BoolCallbackDelegate)(void *actionPtr, bool result);

typedef void(IntCallbackDelegate)(void *actionPtr, int result);

typedef void(StringCallbackDelegate)(void *actionPtr, const char *data);

typedef void(FailureCallbackDelegate)(void *actionPtr, const char *error);

typedef void(FailureWithDataCallbackDelegate)(void *actionPtr, const char *data, const char *error);

typedef bool(NotificationListener)(void *funcPtr, const char *notification);
