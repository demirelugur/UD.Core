namespace UD.Core.Helper.Results
{
    using System;
    using System.Collections.Generic;
    using UD.Core.Extensions;
    using static UD.Core.Enums.CRetMesaj;
    public class ApiResult
    {
        public static readonly ApiResult setSuccess = new(true, default);
        public bool status { get; set; }
        public string[] errors { get; set; }
        public ApiResult() : this(default, default) { }
        public ApiResult(bool status, string[] errors)
        {
            this.status = status;
            this.errors = (status ? [] : (errors.IsNullOrCountZero() ? new[] { RetMesaj.hata.GetDescriptionFromEnum() } : errors));
        }
        public static ApiResult setFailed(params string[] errors) => new(false, errors);
    }
    public class ApiResult<T> : ApiResult
    {
        public T response { get; set; }
        public ApiResult() : this(default, default, default) { }
        public ApiResult(T response, bool status, string[] errors) : base(status, errors)
        {
            this.response = (status ? response : this.getCustomDefaultValue());
        }
        private T getCustomDefaultValue()
        {
            var t = typeof(T);
            if (t == typeof(string)) { return (T)(object)String.Empty; }
            if (t.IsArray) { return (T)(object)Array.CreateInstance(t.GetElementType(), 0); }
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>)) { return (T)Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(t.GetGenericArguments())); }
            return default(T);
        }
    }
}