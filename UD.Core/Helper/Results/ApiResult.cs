namespace UD.Core.Helper.Results
{
    using System;
    using System.Collections.Generic;
    using UD.Core.Enums;
    using UD.Core.Extensions;
    public class ApiResult
    {
        public static readonly ApiResult setSuccess = new(EnumAlertState.Success, default);
        public static readonly ApiResult setInfo = new(EnumAlertState.Info, default);
        public EnumAlertState state { get; set; }
        public string[] messages { get; set; }
        internal bool isSuccess => this.state.Includes(EnumAlertState.Success, EnumAlertState.Info);
        public ApiResult() : this(default, default) { }
        public ApiResult(EnumAlertState state, string[] messages)
        {
            this.state = state;
            this.messages = (messages.IsNullOrEmptyOrAllNull() ? (this.isSuccess ? [] : [EnumResponseMessage.Error.GetLocalizedDescriptionFromEnum()]) : messages);
        }
        public static ApiResult setError(params string[] messages) => new(EnumAlertState.Error, messages);
        public static ApiResult setWarning(params string[] messages) => new(EnumAlertState.Warning, messages);
    }
    public class ApiResult<T> : ApiResult
    {
        public T response { get; set; }
        public ApiResult() : this(default, default, default) { }
        public ApiResult(T response, EnumAlertState state, string[] messages) : base(state, messages)
        {
            this.response = (base.isSuccess ? response : this.getCustomDefaultValue());
        }
        private T getCustomDefaultValue()
        {
            var t = typeof(T);
            if (t == typeof(string)) { return (T)(object)String.Empty; }
            if (t.IsArray) { return (T)(object)Array.CreateInstance(t.GetElementType(), 0); }
            if (t.IsGenericType)
            {
                if (t.GetGenericTypeDefinition() == typeof(Dictionary<,>)) { return (T)Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(t.GetGenericArguments())); }
                if (t.GetGenericTypeDefinition() == typeof(List<>)) { return (T)Activator.CreateInstance(typeof(List<>).MakeGenericType(t.GetGenericArguments())); }
            }
            return default;
        }
    }
}