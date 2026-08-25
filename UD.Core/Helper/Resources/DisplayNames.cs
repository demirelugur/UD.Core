namespace UD.Core.Helper.Resources
{
    using System.Resources;
    public sealed class DisplayNames
    {
        private static readonly ResourceManager _resourceManager = new(String.Join(".", typeof(DisplayNames).Namespace, nameof(DisplayNames)), typeof(DisplayNames).Assembly);
        public static string ClientRequestInfoResultIpAddress => _resourceManager.GetString(nameof(ClientRequestInfoResultIpAddress));
        public static string ClientRequestInfoResultMobile => _resourceManager.GetString(nameof(ClientRequestInfoResultMobile));
        public static string FileSettingsHelperAccept => _resourceManager.GetString(nameof(FileSettingsHelperAccept));
        public static string FileSettingsHelperFileCount => _resourceManager.GetString(nameof(FileSettingsHelperFileCount));
        public static string FileSettingsHelperSize => _resourceManager.GetString(nameof(FileSettingsHelperSize));
        public static string RangeValidationError => _resourceManager.GetString(nameof(RangeValidationError));
        public static string SmtpClientBasicEmail => _resourceManager.GetString(nameof(SmtpClientBasicEmail));
        public static string SmtpClientBasicEnableSsl => _resourceManager.GetString(nameof(SmtpClientBasicEnableSsl));
        public static string SmtpClientBasicHost => _resourceManager.GetString(nameof(SmtpClientBasicHost));
        public static string SmtpClientBasicPassword => _resourceManager.GetString(nameof(SmtpClientBasicPassword));
        public static string SmtpClientBasicPort => _resourceManager.GetString(nameof(SmtpClientBasicPort));
        public static string SmtpClientBasicTimeout => _resourceManager.GetString(nameof(SmtpClientBasicTimeout));
    }
}