namespace UD.Core.Helper.Requests
{
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using UD.Core.Attributes.DataAnnotations;
    using UD.Core.Extensions;
    using UD.Core.Helper.Resources;
    using static UD.Core.Helper.GlobalConstants;
    public interface IClientRequestInfo
    {
        bool isMobil { get; set; }
        string? ipAddress { get; set; }
    }
    public class ClientRequestInfo : IEquatable<ClientRequestInfo>, IClientRequestInfo
    {
        #region Equals
        public override bool Equals(object other) => this.Equals(other as ClientRequestInfo);
        public override int GetHashCode() => HashCode.Combine(this.isMobil, this.ipAddress);
        public bool Equals(ClientRequestInfo other) => (other != null && this.isMobil == other.isMobil && this.ipAddress == other.ipAddress);
        #endregion
        [UDRequired]
        [Display(Name = nameof(DisplayNames.ClientRequestInfoResultMobile), ResourceType = typeof(DisplayNames))]
        public bool isMobil { get; set; }
        [UDStringLength(MaximumLengthConstants.IPAddress)]
        [UDIPAddress]
        [Display(Name = nameof(DisplayNames.ClientRequestInfoResultIpAddress), ResourceType = typeof(DisplayNames))]
        public string? ipAddress { get; set; }
        public ClientRequestInfo() : this(default, default) { }
        public ClientRequestInfo(bool isMobil, object ipAddress)
        {
            this.isMobil = isMobil;
            this.ipAddress = this.ipAddressCast(ipAddress);
        }
        private string? ipAddressCast(object ipaddress)
        {
            if (ipaddress is IPAddress _ip) { return _ip.MapToIPv4().ToString(); }
            if (ipaddress is String _s && IPAddress.TryParse(_s, out _ip)) { return _ip.MapToIPv4().ToString(); }
            return null;
        }
        /// <summary><paramref name="value"/> için tanımlanan nesneler: ClientRequestInfo, IHttpContextAccessor, HttpContext, IFormCollection, AnonymousObjectClass</summary>
        public static ClientRequestInfo ToEntityFromObject(object value)
        {
            if (value == null) { return new(); }
            if (value is ClientRequestInfo _c) { return _c; }
            if (value is IHttpContextAccessor _hca) { return ToEntityFromObject(_hca.HttpContext); }
            if (value is HttpContext _context) { return new(_context.IsMobileDevice(), _context.GetIPAddress()); }
            if (value is IFormCollection _form)
            {
                var (hasError, model, errors) = _form.TryBindFromFormAsync<ClientRequestInfo>().GetAwaiter().GetResult();
                if (hasError) { throw errors.ToNestedException(); }
                return model;
            }
            return value.ToEnumerable().Select(x => x.ToDynamic()).Select(x => new ClientRequestInfo((bool)x.ismobil, (object)x.ipaddress)).FirstOrDefault();
        }
    }
}