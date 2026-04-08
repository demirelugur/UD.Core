namespace UD.Core.Helper.Resources
{
    using System.Resources;
    public sealed class DisplayNames
    {
        private static readonly ResourceManager resourceManager = new(String.Join(".", typeof(DisplayNames).Namespace, nameof(DisplayNames)), typeof(DisplayNames).Assembly);
        public static string ClientRequestInfoResultIpAddress => resourceManager.GetString(nameof(ClientRequestInfoResultIpAddress));
        public static string ClientRequestInfoResultMobile => resourceManager.GetString(nameof(ClientRequestInfoResultMobile));
        public static string FileSettingsHelperAccept => resourceManager.GetString(nameof(FileSettingsHelperAccept));
        public static string FileSettingsHelperFileCount => resourceManager.GetString(nameof(FileSettingsHelperFileCount));
        public static string FileSettingsHelperSize => resourceManager.GetString(nameof(FileSettingsHelperSize));
        public static string SmtpClientBasicEmail => resourceManager.GetString(nameof(SmtpClientBasicEmail));
        public static string SmtpClientBasicEnableSsl => resourceManager.GetString(nameof(SmtpClientBasicEnableSsl));
        public static string SmtpClientBasicHost => resourceManager.GetString(nameof(SmtpClientBasicHost));
        public static string SmtpClientBasicPassword => resourceManager.GetString(nameof(SmtpClientBasicPassword));
        public static string SmtpClientBasicPort => resourceManager.GetString(nameof(SmtpClientBasicPort));
        public static string SmtpClientBasicTimeout => resourceManager.GetString(nameof(SmtpClientBasicTimeout));
        public static string RangeValidationError => resourceManager.GetString(nameof(RangeValidationError));
    }
}