namespace UD.Core.Helper.MailManagement
{
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using System.Net.Mail;
    using UD.Core.Attributes.DataAnnotations;
    using UD.Core.Extensions;
    using UD.Core.Extensions.Common;
    using static UD.Core.Helper.GlobalConstants;
    public sealed class SmtpClientBasic : IEquatable<SmtpClientBasic>
    {
        #region Equals
        public override bool Equals(object other) => this.Equals(other as SmtpClientBasic);
        public override int GetHashCode() => HashCode.Combine(this.Email, this.Password, this.Host, this.Port, this.EnableSsl, this.Timeout);
        public bool Equals(SmtpClientBasic other) => (other != null && this.Email == other.Email && this.Password == other.Password && this.Host == other.Host && this.Port == other.Port && this.EnableSsl == other.EnableSsl && this.Timeout == other.Timeout);
        #endregion
        private string _password;
        private string _host;
        private int? _timeout;
        [UDRequired]
        [UDStringLength(MaximumLengthConstants.EMail)]
        [UDEmail]
        [Display(Name = "e-Posta")]
        public string Email { get; set; }
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
        [UDRangePositiveInt32]
        [Display(Name = "Timeout")]
        public int? Timeout { get { return _timeout; } set { _timeout = value.NullOrDefault(); } }
        public SmtpClient toSmtpClient()
        {
            var sc = new SmtpClient
            {
                Credentials = new NetworkCredential(this.Email, this.Password),
                Host = this.Host,
                Port = this.Port,
                EnableSsl = this.EnableSsl
            };
            if (this.Timeout.HasValue) { sc.Timeout = this.Timeout.Value; }
            return sc;
        }
        public SmtpClientBasic() : this("", "", "", default, default, default) { }
        public SmtpClientBasic(string email, string password, string host, int port, bool enablessl, int? timeout)
        {
            this.Email = email;
            this.Password = password;
            this.Host = host;
            this.Port = port;
            this.EnableSsl = enablessl;
            this.Timeout = timeout;
        }
        public static SmtpClientBasic SetGmail(string email, string password, int? timeout = null) => new(email, password, "smtp.gmail.com", 587, true, timeout);
        public static SmtpClientBasic SetOutlook(string email, string password, int? timeout = null) => new(email, password, "smtp.office365.com", 587, true, timeout);
        /// <summary><paramref name="value"/> için tanımlanan nesneler: SmtpClientBasic, IFormCollection, AnonymousObjectClass</summary>
        public static SmtpClientBasic ToEntityFromObject(object value)
        {
            if (value == null) { return new(); }
            if (value is SmtpClientBasic _ssh) { return _ssh; }
            if (value is IFormCollection _form)
            {
                var (hasError, model, errors) = _form.TryBindFromFormAsync<SmtpClientBasic>().GetAwaiter().GetResult();
                if (hasError) { throw errors.ToNestedException(); }
                return model;
            }
            return value.ToEnumerable().Select(x => x.ToDynamic()).Select(x => new SmtpClientBasic((string)x.Email, (string)x.Password, (string)x.Host, (int)x.Port, (bool)x.EnableSsl, (int?)x.Timeout)).FirstOrDefault();
        }
    }
}