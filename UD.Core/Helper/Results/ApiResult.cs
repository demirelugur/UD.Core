namespace UD.Core.Helper.Results
{
    using UD.Core.Extensions;
    using static UD.Core.Enums.CRetMesaj;
    /// <summary>API response dönüşleri için oluşturulmuş yardımcı class</summary>
    public class ApiResult
    {
        public static readonly ApiResult set_Success = new(true, default);
        public bool status { get; set; }
        public string[] errors { get; set; }
        public ApiResult() : this(default, default) { }
        public ApiResult(bool status, string[] errors)
        {
            this.status = status;
            this.errors = (status ? Array.Empty<string>() : (errors.IsNullOrCountZero() ? new string[] { RetMesaj.hata.GetDescriptionFromEnum() } : errors));
        }
        public static ApiResult set_Failed(params string[] errors) => new(false, errors);
    }
    /// <summary>API response dönüşleri için oluşturulmuş yardımcı class</summary>
    public class ApiResult<T> : ApiResult
    {
        public T response { get; set; }
        public ApiResult() : this(default, default, default) { }
        public ApiResult(T response, bool status, string[] errors) : base(status, errors)
        {
            this.response = (status ? response : this.getcustomdefault());
        }
        private T getcustomdefault()
        {
            var _t = typeof(T);
            if (_t == typeof(string)) { return (T)(object)String.Empty; }
            if (_t.IsArray) { return (T)(object)Array.CreateInstance(_t.GetElementType(), 0); }
            if (_t.IsGenericType && _t.GetGenericTypeDefinition() == typeof(Dictionary<,>)) { return (T)Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(_t.GetGenericArguments())); }
            return default(T);
        }
    }
    /// <summary>API response dönüşleri için oluşturulmuş yardımcı class</summary>
    public class PagedApiResult<T> : ApiResult
    {
        public T[] response { get; set; }
        public long totalcount { get; set; }
        public PagedApiResult() : this(default, default, default, default) { }
        public PagedApiResult(T[] response, long totalcount, bool status, string[] errors) : base(status, errors)
        {
            this.response = response ?? Array.Empty<T>();
            this.totalcount = totalcount;
        }
        public ApiResult<T[]> ToApiResult() => new(this.response, this.status, this.errors);
    }
}