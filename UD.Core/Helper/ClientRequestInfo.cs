namespace UD.Core.Helper
{
    using UD.Core.Attributes.DataAnnotations;
    using UD.Core.Extensions;
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using static UD.Core.Helper.GlobalConstants;
    public class ClientRequestInfo : IEquatable<ClientRequestInfo>
    {
        #region Equals
        public override bool Equals(object other) => this.Equals(other as ClientRequestInfo);
        public override int GetHashCode() => HashCode.Combine(this.ismobil, this.ipaddress);
        public bool Equals(ClientRequestInfo other) => (other != null && this.ismobil == other.ismobil && this.ipaddress == other.ipaddress);
        #endregion
        [Validation_Required]
        [Display(Name = "Mobil")]
        [DefaultValue(false)]
        public bool ismobil { get; set; }
        [Validation_StringLength(_maximumlength.ipaddress)]
        [Validation_IPAddress]
        [Display(Name = "IP Adresi")]
        [DefaultValue(null)]
        public string? ipaddress { get; set; }
        public ClientRequestInfo() : this(default, default) { }
        public ClientRequestInfo(bool ismobil, object ipaddress)
        {
            this.ismobil = ismobil;
            this.ipaddress = this.ipaddressCast(ipaddress);
        }
        private string? ipaddressCast(object ipaddress)
        {
            if (ipaddress is IPAddress _ip) { return _ip.MapToIPv4().ToString(); }
            if (ipaddress is String _s && IPAddress.TryParse(_s, out _ip)) { return _ip.MapToIPv4().ToString(); }
            return null;
        }
        /// <summary>
        /// value için tanımlanan nesneler: ClientRequestInfo, HttpContext, IFormCollection, AnonymousObjectClass
        /// </summary>
        public static ClientRequestInfo ToEntityFromObject(object value)
        {
            if (value == null) { return new(); }
            if (value is ClientRequestInfo _c) { return _c; }
            if (value is HttpContext _context) { return new(_context.IsMobileDevice(), _context.GetIPAddress()); }
            if (value is IFormCollection _form) { return new(_form.ParseOrDefault<bool>(nameof(ismobil)), _form.ParseOrDefault<string>(nameof(ipaddress)) ?? ""); }
            return value.ToEnumerable().Select(x => x.ToDynamic()).Select(x => new ClientRequestInfo((bool)x.ismobil, (object)x.ipaddress)).FirstOrDefault();
        }
    }
}