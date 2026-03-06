namespace UD.Core.Helper.MailManagement
{
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json.Linq;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using System.Net.Mail;
    using UD.Core.Attributes.DataAnnotations;
    using UD.Core.Extensions;
    using static UD.Core.Helper.GlobalConstants;
    using static UD.Core.Helper.OrtakTools;
    public sealed class SmtpClientBasic : IEquatable<SmtpClientBasic>
    {
        #region Equals
        public override bool Equals(object other) => this.Equals(other as SmtpClientBasic);
        public override int GetHashCode() => HashCode.Combine(this.Email, this.Password, this.Host, this.Port, this.EnableSsl, this.UseDefaultCredentials, this.DeliveryMethod, this.Timeout);
        public bool Equals(SmtpClientBasic other) => (other != null && this.Email == other.Email && this.Password == other.Password && this.Host == other.Host && this.Port == other.Port && this.EnableSsl == other.EnableSsl && this.UseDefaultCredentials == other.UseDefaultCredentials && this.DeliveryMethod == other.DeliveryMethod && this.Timeout == other.Timeout);
        #endregion
        private string _email;
        private string _password;
        [Validation_Required]
        [EmailAddress(ErrorMessage = ValidationErrorMessageConstants.EMail)]
        [Validation_StringLength(MaximumLengthConstants.EMail)]
        [Display(Name = "e-Posta")]
        public string Email { get { return _email; } set { _email = value.ToStringOrEmpty().ToLower(); } }
        [Validation_Required]
        [Validation_StringLength(16, 8)]
        [Display(Name = "Şifre")]
        public string Password { get { return _password; } set { _password = value.ToStringOrEmpty(); } }
        [Validation_Required]
        [Validation_StringLength(30)]
        [Display(Name = "Host")]
        public string Host { get; set; }
        [Validation_Required]
        [Validation_RangePositiveInt32]
        [Display(Name = "Port")]
        [DefaultValue(25)]
        public int Port { get; set; }
        [Validation_Required]
        [Display(Name = "Enable SSL")]
        public bool EnableSsl { get; set; }
        [Validation_Required]
        [Display(Name = "Use Default Credentials")]
        public bool UseDefaultCredentials { get; set; }
        [Validation_Required]
        [EnumDataType(typeof(SmtpDeliveryMethod), ErrorMessage = ValidationErrorMessageConstants.EnumDataType)]
        [Display(Name = "Delivery Method")]
        public SmtpDeliveryMethod DeliveryMethod { get; set; }
        [Validation_Required]
        [Display(Name = "Timeout")]
        public int Timeout { get; set; }
        public SmtpClient toSmtpClient()
        {
            var sc = new SmtpClient
            {
                Port = this.Port,
                Host = this.Host,
                EnableSsl = this.EnableSsl,
                UseDefaultCredentials = this.UseDefaultCredentials,
                Credentials = new NetworkCredential(this.Email, this.Password),
                DeliveryMethod = this.DeliveryMethod
            };
            if (this.Timeout > 0) { sc.Timeout = this.Timeout; }
            return sc;
        }
        public SmtpClientBasic() : this("", "", "", default, default, default, default, default) { }
        public SmtpClientBasic(string email, string password, string host, int port, bool enablessl, bool usedefaultcredentials, SmtpDeliveryMethod deliverymethod, int timeout)
        {
            this.Email = email;
            this.Password = password;
            this.Host = host;
            this.Port = port;
            this.EnableSsl = enablessl;
            this.UseDefaultCredentials = usedefaultcredentials;
            this.DeliveryMethod = deliverymethod;
            this.Timeout = timeout;
        }
        public static SmtpClientBasic CreateSmtpSettings_gmail(string email, string password) => new(email, password, "smtp.gmail.com", 587, true, true, SmtpDeliveryMethod.Network, 0);
        public static SmtpClientBasic CreateSmtpSettings_outlook(string email, string password) => new(email, password, "smtp.office365.com", 587, true, false, SmtpDeliveryMethod.Network, 0);
        public static SmtpClientBasic ToEntityFromObject(object value)
        {
            if (value == null) { return new(); }
            if (value is SmtpClientBasic _ssh) { return _ssh; }
            if (value is IFormCollection _form)
            {
                return ToEntityFromObject(new
                {
                    email = _form.ParseOrDefault<string>(nameof(Email)) ?? "",
                    password = _form.ParseOrDefault<string>(nameof(Password)) ?? "",
                    host = _form.ParseOrDefault<string>(nameof(Host)) ?? "",
                    port = _form.ParseOrDefault<int>(nameof(Port)),
                    enablessl = _form.ParseOrDefault<bool>(nameof(EnableSsl)),
                    usedefaultcredentials = _form.ParseOrDefault<bool>(nameof(UseDefaultCredentials)),
                    deliverymethod = _form.ParseOrDefault<SmtpDeliveryMethod>(nameof(DeliveryMethod)),
                    timeout = _form.ParseOrDefault<int>(nameof(Timeout))
                });
            }
            if (value is String _json)
            {
                if (Validators.TryJson(_json, JTokenType.Object, out JObject _jo)) { return _jo.ToObject<SmtpClientBasic>(); }
                return new();
            }
            return value.ToEnumerable().Select(x => x.ToDynamic()).Select(x => new SmtpClientBasic((string)x.email, (string)x.password, (string)x.host, (int)x.port, (bool)x.enablessl, (bool)x.usedefaultcredentials, (SmtpDeliveryMethod)x.deliverymethod, (int)x.timeout)).FirstOrDefault();
        }
    }
}