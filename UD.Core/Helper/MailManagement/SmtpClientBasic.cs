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
        private string _host;
        [UDRequired]
        [EmailAddress(ErrorMessage = ValidationErrorMessageConstants.EMail)]
        [UDStringLength(MaximumLengthConstants.EMail)]
        [Display(Name = "e-Posta")]
        public string Email { get { return _email; } set { _email = value.ToStringOrEmpty().ToLower(); } }
        [UDRequired]
        [UDStringLength(16, 8)]
        [Display(Name = "Şifre")]
        public string Password { get { return _password; } set { _password = value.ToStringOrEmpty(); } }
        [UDRequired]
        [UDStringLength(30)]
        [Display(Name = "Host")]
        public string Host { get { return _host; } set { _host = value.ToStringOrEmpty(); } }
        [UDRequired]
        [UDRangePositiveInt32]
        [Display(Name = "Port")]
        [DefaultValue(25)]
        public int Port { get; set; }
        [UDRequired]
        [Display(Name = "Enable SSL")]
        public bool EnableSsl { get; set; }
        [UDRequired]
        [Display(Name = "Use Default Credentials")]
        public bool UseDefaultCredentials { get; set; }
        [UDRequired]
        [EnumDataType(typeof(SmtpDeliveryMethod), ErrorMessage = ValidationErrorMessageConstants.EnumDataType)]
        [Display(Name = "Delivery Method")]
        public SmtpDeliveryMethod DeliveryMethod { get; set; }
        [UDRequired]
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
        public static SmtpClientBasic SetGmail(string email, string password) => new(email, password, "smtp.gmail.com", 587, true, true, SmtpDeliveryMethod.Network, 0);
        public static SmtpClientBasic SetOutlook(string email, string password) => new(email, password, "smtp.office365.com", 587, true, false, SmtpDeliveryMethod.Network, 0);
        /// <summary>
        /// value için tanımlanan nesneler: SmtpClientBasic, IFormCollection, String(JTokenType.Object), AnonymousObjectClass
        /// </summary>
        public static SmtpClientBasic ToEntityFromObject(object value)
        {
            if (value == null) { return new(); }
            if (value is SmtpClientBasic _ssh) { return _ssh; }
            if (value is IFormCollection _form)
            {
                return ToEntityFromObject(new
                {
                    Email = _form.ParseOrDefault<string>(nameof(Email)) ?? "",
                    Password = _form.ParseOrDefault<string>(nameof(Password)) ?? "",
                    Host = _form.ParseOrDefault<string>(nameof(Host)) ?? "",
                    Port = _form.ParseOrDefault<int>(nameof(Port)),
                    EnableSsl = _form.ParseOrDefault<bool>(nameof(EnableSsl)),
                    UseDefaultCredentials = _form.ParseOrDefault<bool>(nameof(UseDefaultCredentials)),
                    DeliveryMethod = _form.ParseOrDefault<SmtpDeliveryMethod>(nameof(DeliveryMethod)),
                    Timeout = _form.ParseOrDefault<int>(nameof(Timeout))
                });
            }
            if (value is String _json)
            {
                if (Validators.TryJson(_json, JTokenType.Object, out JObject _jo)) { return _jo.ToObject<SmtpClientBasic>(); }
                return new();
            }
            return value.ToEnumerable().Select(x => x.ToDynamic()).Select(x => new SmtpClientBasic((string)x.Email, (string)x.Password, (string)x.Host, (int)x.Port, (bool)x.EnableSsl, (bool)x.UseDefaultCredentials, (SmtpDeliveryMethod)x.DeliveryMethod, (int)x.Timeout)).FirstOrDefault();
        }
    }
}