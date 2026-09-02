namespace UD.Core.Helper.Responses
{
    using System;
    using System.Collections.Generic;
    using UD.Core.Enums;
    using UD.Core.Extensions;
    public class ApiResponse
    {
        public static readonly ApiResponse setSuccess = new(EnumAlertState.success, default);
        public static readonly ApiResponse setInfo = new(EnumAlertState.info, default);
        public EnumAlertState State { get; set; }
        public string[] Messages { get; set; } = [];
        public ApiResponse() : this(default, default) { }
        public ApiResponse(EnumAlertState state, string[] messages)
        {
            this.State = state;
            this.Messages = (messages.IsNullOrEmptyOrAllNull() ? [this.State.GetDisplayNameLocalized()] : messages);
        }
        public static ApiResponse setError(params string[] messages) => new(EnumAlertState.error, messages);
        public static ApiResponse setWarning(params string[] messages) => new(EnumAlertState.warning, messages);
    }
    public class ApiResponse<T> : ApiResponse
    {
        public T Response { get; set; }
        public ApiResponse() : this(default, default, default) { }
        public ApiResponse(T response, EnumAlertState state, string[] messages) : base(state, messages)
        {
            this.Response = (state.IsFailed() ? GetDefaultValue() : response);
        }
        private static T GetDefaultValue()
        {
            var t = typeof(T);
            if (t == typeof(string)) { return (T)(object)String.Empty; }
            if (t.IsArray) { return (T)(object)Array.CreateInstance(t.GetElementType(), 0); }
            if (t.IsGenericType)
            {
                if (t.GetGenericTypeDefinition() == typeof(Dictionary<,>)) { return (T)Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(t.GetGenericArguments())); }
                if (t.GetGenericTypeDefinition() == typeof(List<>)) { return (T)Activator.CreateInstance(typeof(List<>).MakeGenericType(t.GetGenericArguments())); }
            }
            return (t.GetDefaultValue() is T _t ? _t : default);
        }
    }
}