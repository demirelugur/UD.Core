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
    public sealed class SmtpSettingsHelper : IEquatable<SmtpSettingsHelper>
    {
        #region Equals
        public override bool Equals(object other) => this.Equals(other as SmtpSettingsHelper);
        public override int GetHashCode() => HashCode.Combine(this.email, this.password, this.host, this.port, this.enablessl, this.usedefaultcredentials, this.deliverymethod, this.timeout);
        public bool Equals(SmtpSettingsHelper other) => (other != null && this.email == other.email && this.password == other.password && this.host == other.host && this.port == other.port && this.enablessl == other.enablessl && this.usedefaultcredentials == other.usedefaultcredentials && this.deliverymethod == other.deliverymethod && this.timeout == other.timeout);
        #endregion
        private string _Email;
        private string _Password;
        private string _Host;
        private int _Port;
        private bool _Enablessl;
        private bool _Usedefaultcredentials;
        private SmtpDeliveryMethod _Deliverymethod;
        private int _Timeout;
        [Validation_Required]
        [EmailAddress(ErrorMessage = _validationerrormessage.email)]
        [Validation_StringLength(_maximumlength.eposta)]
        [Display(Name = "e-Posta")]
        public string email { get { return _Email; } set { _Email = value.ToStringOrEmpty().ToLower(); } }
        [Validation_Required]
        [Validation_StringLength(16, 8)]
        [Display(Name = "Şifre")]
        public string password { get { return _Password; } set { _Password = value.ToStringOrEmpty(); } }
        [Validation_Required]
        [Validation_StringLength(30)]
        [Display(Name = "Host")]
        public string host { get { return _Host; } set { _Host = value.ToStringOrEmpty(); } }
        [Validation_Required]
        [Validation_RangePositiveInt32]
        [Display(Name = "Port")]
        [DefaultValue(25)]
        public int port { get { return _Port; } set { _Port = value; } }
        [Validation_Required]
        [Display(Name = "Enable SSL")]
        public bool enablessl { get { return _Enablessl; } set { _Enablessl = value; } }
        [Validation_Required]
        [Display(Name = "Use Default Credentials")]
        public bool usedefaultcredentials { get { return _Usedefaultcredentials; } set { _Usedefaultcredentials = value; } }
        [Validation_Required]
        [EnumDataType(typeof(SmtpDeliveryMethod), ErrorMessage = _validationerrormessage.enumdatatype)]
        [Display(Name = "Delivery Method")]
        public SmtpDeliveryMethod deliverymethod { get { return _Deliverymethod; } set { _Deliverymethod = value; } }
        [Validation_Required]
        [Display(Name = "Timeout")]
        public int timeout { get { return _Timeout; } set { _Timeout = value; } }
        public SmtpClient toSmtpClient()
        {
            var sc = new SmtpClient
            {
                Port = this.port,
                Host = this.host,
                EnableSsl = this.enablessl,
                UseDefaultCredentials = this.usedefaultcredentials,
                Credentials = new NetworkCredential(this.email, this.password),
                DeliveryMethod = this.deliverymethod
            };
            if (this.timeout > 0) { sc.Timeout = this.timeout; }
            return sc;
        }
        public SmtpSettingsHelper() : this("", "", "", default, default, default, default, default) { }
        public SmtpSettingsHelper(string email, string password, string host, int port, bool enablessl, bool usedefaultcredentials, SmtpDeliveryMethod deliverymethod, int timeout)
        {
            this.email = email;
            this.password = password;
            this.host = host;
            this.port = port;
            this.enablessl = enablessl;
            this.usedefaultcredentials = usedefaultcredentials;
            this.deliverymethod = deliverymethod;
            this.timeout = timeout;
        }
        public static SmtpSettingsHelper CreateSmtpSettings_gmail(string email, string password) => new(email, password, "smtp.gmail.com", 587, true, true, SmtpDeliveryMethod.Network, 0);
        public static SmtpSettingsHelper CreateSmtpSettings_outlook(string email, string password) => new(email, password, "smtp.office365.com", 587, true, false, SmtpDeliveryMethod.Network, 0);
        public static SmtpSettingsHelper ToEntityFromObject(object value)
        {
            if (value == null) { return new(); }
            if (value is SmtpSettingsHelper _ssh) { return _ssh; }
            if (value is IFormCollection _form)
            {
                return ToEntityFromObject(new
                {
                    email = _form.ParseOrDefault<string>(nameof(email)) ?? "",
                    password = _form.ParseOrDefault<string>(nameof(password)) ?? "",
                    host = _form.ParseOrDefault<string>(nameof(host)) ?? "",
                    port = _form.ParseOrDefault<int>(nameof(port)),
                    enablessl = _form.ParseOrDefault<bool>(nameof(enablessl)),
                    usedefaultcredentials = _form.ParseOrDefault<bool>(nameof(usedefaultcredentials)),
                    deliverymethod = _form.ParseOrDefault<SmtpDeliveryMethod>(nameof(deliverymethod)),
                    timeout = _form.ParseOrDefault<int>(nameof(timeout))
                });
            }
            if (value is String _json)
            {
                if (_try.TryJson(_json, JTokenType.Object, out JObject _jo)) { return _jo.ToObject<SmtpSettingsHelper>(); }
                return new();
            }
            return value.ToEnumerable().Select(x => x.ToDynamic()).Select(x => new SmtpSettingsHelper((string)x.email, (string)x.password, (string)x.host, (int)x.port, (bool)x.enablessl, (bool)x.usedefaultcredentials, (SmtpDeliveryMethod)x.deliverymethod, (int)x.timeout)).FirstOrDefault();
        }
    }
}