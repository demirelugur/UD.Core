namespace UD.Core.Extensions
{
    using UD.Core.Enums;
    using UD.Core.Helper.Results;
    ///<summary><see cref="ApiResult{T}"/> geri dönüşleri için oluşturulmuş yardımcı foksiyonlar</summary>
    public static class ApiResultExtensions
    {
        public static ApiResult<T> ReturnError<T>(this string message) => new[] { message }.ReturnError<T>();
        public static ApiResult<T> ReturnError<T>(this string[] messages) => new(default, EnumAlertState.error, messages);
        public static ApiResult<object[]> ReturnErrorObjectArray(this string message) => new[] { message }.ReturnErrorObjectArray();
        public static ApiResult<object[]> ReturnErrorObjectArray(this string[] messages) => new(default, EnumAlertState.error, messages);
        public static ApiResult<T> ReturnWarning<T>(this string message) => new[] { message }.ReturnWarning<T>();
        public static ApiResult<T> ReturnWarning<T>(this string[] messages) => new(default, EnumAlertState.warning, messages);
        public static ApiResult<object[]> ReturnWarningObjectArray(this string message) => new[] { message }.ReturnWarningObjectArray();
        public static ApiResult<object[]> ReturnWarningObjectArray(this string[] messages) => new(default, EnumAlertState.warning, messages);
        public static ApiResult<T> ReturnSuccess<T>(this T response) => new(response, EnumAlertState.success, default);
        public static ApiResult<T> ReturnInfo<T>(this T response) => new(response, EnumAlertState.info, default);
    }
}