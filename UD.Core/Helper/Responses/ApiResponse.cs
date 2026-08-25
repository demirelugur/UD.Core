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
        public ApiResponse(EnumAlertState State, string[] Messages)
        {
            this.State = State;
            this.Messages = (Messages.IsNullOrEmptyOrAllNull() ? [this.State.GetDisplayNameLocalized()] : Messages);
        }
        public static ApiResponse setError(params string[] Messages) => new(EnumAlertState.error, Messages);
        public static ApiResponse setWarning(params string[] Messages) => new(EnumAlertState.warning, Messages);
    }
    public class ApiResponse<T> : ApiResponse
    {
        public T Response { get; set; }
        public ApiResponse() : this(default, default, default) { }
        public ApiResponse(T Response, EnumAlertState State, string[] Messages) : base(State, Messages)
        {
            this.Response = (State.IsFailed() ? this.GetDefaultValue() : Response);
        }
        private T GetDefaultValue()
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