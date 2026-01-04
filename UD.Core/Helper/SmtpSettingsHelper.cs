namespace UD.Core.Helper
{
    using UD.Core.Attributes.DataAnnotations;
    using UD.Core.Extensions;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json.Linq;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using System.Net.Mail;
    using static UD.Core.Helper.GlobalConstants;
    using static UD.Core.Helper.OrtakTools;
    public sealed class SmtpSettingsHelper : IEquatable<SmtpSettingsHelper>
    {
        #region Equals
        public override bool Equals(object other) => this.Equals(other as SmtpSettingsHelper);
        public override int GetHashCode() => HashCode.Combine(this.email, this.password, this.host, this.port, this.enablessl, this.usedefaultcredentials, this.deliverymethod, this.timeout);
        public bool Equals(SmtpSettingsHelper other) => (other != null && this.email == other.email && this.password == other.password && this.host == other.host && this.port == other.port && this.enablessl == other.enablessl && this.usedefaultcredentials == other.usedefaultcredentials && this.deliverymethod == other.deliverymethod && this.timeout == other.timeout);
        #endregion
        [Validation_Required]
        [EmailAddress(ErrorMessage = _validationerrormessage.email)]
        [Validation_StringLength(_maximumlength.eposta)]
        [Display(Name = "e-Posta")]
        [DefaultValue("")]
        public string email { get; set; }
        [Validation_Required]
        [Validation_StringLength(16, 8)]
        [Display(Name = "Şifre")]
        [DefaultValue("")]
        public string password { get; set; }
        [Validation_Required]
        [Validation_StringLength(30)]
        [Display(Name = "Host")]
        [DefaultValue("")]
        public string host { get; set; }
        [Validation_Required]
        [Validation_RangePositiveInt32]
        [Display(Name = "Port")]
        [DefaultValue(25)]
        public int port { get; set; }
        [Validation_Required]
        [Display(Name = "Enable SSL")]
        [DefaultValue(false)]
        public bool enablessl { get; set; }
        [Validation_Required]
        [Display(Name = "Use Default Credentials")]
        [DefaultValue(false)]
        public bool usedefaultcredentials { get; set; }
        [Validation_Required]
        [EnumDataType(typeof(SmtpDeliveryMethod), ErrorMessage = _validationerrormessage.enumdatatype)]
        [Display(Name = "Delivery Method")]
        [DefaultValue(SmtpDeliveryMethod.Network)]
        public SmtpDeliveryMethod deliverymethod { get; set; }
        [Validation_Required]
        [Display(Name = "Timeout")]
        [DefaultValue(0)]
        public int timeout { get; set; }
        public SmtpClient toSmtpClient()
        {
            var _sc = new SmtpClient
            {
                Port = this.port,
                Host = this.host,
                EnableSsl = this.enablessl,
                UseDefaultCredentials = this.usedefaultcredentials,
                Credentials = new NetworkCredential(this.email, this.password),
                DeliveryMethod = this.deliverymethod
            };
            if (this.timeout > 0) { _sc.Timeout = this.timeout; }
            return _sc;
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
        /// <summary>
        /// Gmail için SMTP ayarlarını oluşturur. 
        /// <code>new(email, password, &quot;smtp.gmail.com&quot;, 587, true, true, SmtpDeliveryMethod.Network, 0);</code>
        /// </summary>
        /// <param name="email">e-Posta adresi.</param>
        /// <param name="password">Parola.</param>
        /// <returns>Oluşturulan SMTP ayarları.</returns>
        public static SmtpSettingsHelper CreateSmtpSettings_gmail(string email, string password) => new(email, password, "smtp.gmail.com", 587, true, true, SmtpDeliveryMethod.Network, 0);
        /// <summary>
        /// Outlook için SMTP ayarlarını oluşturur. 
        /// <code>new(email, password, &quot;smtp.office365.com&quot;, 587, true, false, SmtpDeliveryMethod.Network, 0);</code>
        /// </summary>
        /// <param name="email">e-Posta adresi.</param>
        /// <param name="password">Parola.</param>
        /// <returns>Oluşturulan SMTP ayarları.</returns>
        public static SmtpSettingsHelper CreateSmtpSettings_outlook(string email, string password) => new(email, password, "smtp.office365.com", 587, true, false, SmtpDeliveryMethod.Network, 0);
        /// <summary>
        /// value için tanımlanan nesneler: SmtpSettingsHelper, IFormCollection, String(JTokenType.Object), AnonymousObjectClass
        /// </summary>
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