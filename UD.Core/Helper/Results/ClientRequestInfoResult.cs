namespace UD.Core.Helper.Results
{
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using UD.Core.Attributes.DataAnnotations;
    using UD.Core.Extensions;
    using static UD.Core.Helper.GlobalConstants;
    public class ClientRequestInfoResult : IEquatable<ClientRequestInfoResult>
    {
        #region Equals
        public override bool Equals(object other) => this.Equals(other as ClientRequestInfoResult);
        public override int GetHashCode() => HashCode.Combine(this.ismobil, this.ipaddress);
        public bool Equals(ClientRequestInfoResult other) => (other != null && this.ismobil == other.ismobil && this.ipaddress == other.ipaddress);
        #endregion
        private string? _Ipaddress;
        [UDRequired]
        [Display(Name = "Mobil")]
        public bool ismobil { get; set; }
        [UDStringLength(MaximumLengthConstants.IPAddress)]
        [UDIPAddress]
        [Display(Name = "IP Adresi")]
        public string? ipaddress { get { return _Ipaddress; } set { _Ipaddress = value.ParseOrDefault<string>(); } }
        public ClientRequestInfoResult() : this(default, default) { }
        public ClientRequestInfoResult(bool ismobil, object ipaddress)
        {
            this.ismobil = ismobil;
            this.ipaddress = this.ipAddressCast(ipaddress);
        }
        private string? ipAddressCast(object ipaddress)
        {
            if (ipaddress is IPAddress _ip) { return _ip.MapToIPv4().ToString(); }
            if (ipaddress is String _s && IPAddress.TryParse(_s, out _ip)) { return _ip.MapToIPv4().ToString(); }
            return null;
        }
        /// <summary>
        /// value için tanımlanan nesneler: ClientRequestInfo, IHttpContextAccessor, HttpContext, IFormCollection, AnonymousObjectClass
        /// </summary>
        public static ClientRequestInfoResult ToEntityFromObject(object value)
        {
            if (value == null) { return new(); }
            if (value is ClientRequestInfoResult _c) { return _c; }
            if (value is IHttpContextAccessor _hca) { return ToEntityFromObject(_hca?.HttpContext); }
            if (value is HttpContext _context) { return new(_context.IsMobileDevice(), _context.GetIPAddress()); }
            if (value is IFormCollection _form) { return new(_form.ParseOrDefault<bool>(nameof(ismobil)), _form.ParseOrDefault<string>(nameof(ipaddress)) ?? ""); }
            return value.ToEnumerable().Select(x => x.ToDynamic()).Select(x => new ClientRequestInfoResult((bool)x.ismobil, (object)x.ipaddress)).FirstOrDefault();
        }
    }
}