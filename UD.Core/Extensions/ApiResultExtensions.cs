namespace UD.Core.Extensions
{
    using UD.Core.Helper.Results;
    ///<summary><see cref="ApiResult{T}"/> geri dönüşleri için oluşturulmuş yardımcı foksiyonlar</summary>
    public static class ApiResultExtensions
    {
        public static ApiResult<T> ReturnFailed<T>(this string error) => new[] { error }.ReturnFailed<T>();
        public static ApiResult<T> ReturnFailed<T>(this string[] errors) => new(default, false, errors);
        public static ApiResult<object[]> ReturnFailedObjectArray(this string error) => new[] { error }.ReturnFailedObjectArray();
        public static ApiResult<object[]> ReturnFailedObjectArray(this string[] errors) => new(default, false, errors);
        public static ApiResult<T> ReturnSuccess<T>(this T response) => new(response, true, default);
    }
}