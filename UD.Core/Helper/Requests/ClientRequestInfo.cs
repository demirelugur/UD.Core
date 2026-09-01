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
        bool IsMobil { get; set; }
        string? IpAddress { get; set; }
    }
    public class ClientRequestInfo : IEquatable<ClientRequestInfo>, IClientRequestInfo
    {
        #region Equals
        public override bool Equals(object other) => this.Equals(other as ClientRequestInfo);
        public override int GetHashCode() => HashCode.Combine(this.IsMobil, this.IpAddress);
        public bool Equals(ClientRequestInfo other) => (other != null && this.IsMobil == other.IsMobil && this.IpAddress == other.IpAddress);
        #endregion
        [UDRequired]
        [Display(Name = nameof(DisplayNames.ClientRequestInfoResultMobile), ResourceType = typeof(DisplayNames))]
        public bool IsMobil { get; set; }
        [UDStringLength(MaximumLengthConstants.IPAddress)]
        [UDIPAddress]
        [Display(Name = nameof(DisplayNames.ClientRequestInfoResultIpAddress), ResourceType = typeof(DisplayNames))]
        public string? IpAddress { get; set; }
        public ClientRequestInfo() : this(default, default) { }
        public ClientRequestInfo(bool isMobil, object ipAddress)
        {
            this.IsMobil = isMobil;
            this.IpAddress = IpAddressCast(ipAddress);
        }
        private static string? IpAddressCast(object ipAddress)
        {
            if (ipAddress is IPAddress _ip) { return _ip.MapToIPv4().ToString(); }
            if (ipAddress is String _s && IPAddress.TryParse(_s, out _ip)) { return _ip.MapToIPv4().ToString(); }
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
            return value.ToEnumerable().Select(x => x.ToDynamic()).Select(x => new ClientRequestInfo((bool)x.IsMobil, (object)x.IpAddress)).FirstOrDefault();
        }
    }
}