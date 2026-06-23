namespace UD.Core.Extensions
{
    using UD.Core.Enums;
    using UD.Core.Helper.Responses;
    ///<summary><see cref="ApiResponse{T}"/> geri dönüşleri için oluşturulmuş yardımcı foksiyonlar</summary>
    public static class ApiResponseExtensions
    {
        public static ApiResponse<T> ReturnError<T>(this string message) => new[] { message }.ReturnError<T>();
        public static ApiResponse<T> ReturnError<T>(this string[] messages) => new(default, EnumAlertState.error, messages);
        public static ApiResponse<object[]> ReturnErrorObjectArray(this string message) => new[] { message }.ReturnErrorObjectArray();
        public static ApiResponse<object[]> ReturnErrorObjectArray(this string[] messages) => new(default, EnumAlertState.error, messages);
        public static ApiResponse<T> ReturnWarning<T>(this string message) => new[] { message }.ReturnWarning<T>();
        public static ApiResponse<T> ReturnWarning<T>(this string[] messages) => new(default, EnumAlertState.warning, messages);
        public static ApiResponse<object[]> ReturnWarningObjectArray(this string message) => new[] { message }.ReturnWarningObjectArray();
        public static ApiResponse<object[]> ReturnWarningObjectArray(this string[] messages) => new(default, EnumAlertState.warning, messages);
        public static ApiResponse<T> ReturnSuccess<T>(this T response) => new(response, EnumAlertState.success, default);
        public static ApiResponse<T> ReturnInfo<T>(this T response) => new(response, EnumAlertState.info, default);
    }
}