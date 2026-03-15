namespace UD.Core.Attributes.DataAnnotations
{
    using System.ComponentModel.DataAnnotations;
    using System.Net.Mail;
    using UD.Core.Extensions;
    using static UD.Core.Helper.GlobalConstants;
    using static UD.Core.Helper.OrtakTools;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDEmailAttribute : ValidationAttribute
    {
        public string[] hosts { get; }
        public UDEmailAttribute(params string[] hosts)
        {
            this.hosts = (hosts ?? []).Select(x => x.ToStringOrEmpty()).Where(x => x.Length > 0).Select(x => x.TrimStart('@').ToLower()).Distinct().ToArray();
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var email = value.ToStringOrEmpty();
            if (email == "" && !validationContext.IsRequiredAttribute())
            {
                validationContext.SetValidatePropertyValue(null);
                return ValidationResult.Success;
            }
            if (Validators.TryMailAddress(email, out MailAddress _ma) && (this.hosts.Length == 0 || this.IsHostAllowed(_ma.Host)))
            {
                validationContext.SetValidatePropertyValue(_ma.Address);
                return ValidationResult.Success;
            }
            if (this.ErrorMessage.IsNullOrEmpty())
            {
                var message = String.Format(ValidationErrorMessageConstants.EMail, validationContext.DisplayName);
                if (this.hosts.Length > 0) { message = $"{message}, Geçerli {(this.hosts.Length == 1 ? "host" : "hostlar")}: {String.Join(", ", this.hosts)}"; }
                this.ErrorMessage = message;
            }
            return new(this.ErrorMessage, [validationContext.MemberName]);
        }
        private bool IsHostAllowed(string host)
        {
            foreach (var pattern in this.hosts)
            {
                if (pattern.StartsWith("*."))
                {
                    var domain = pattern[2..];
                    if (domain.Length > 0 && (host == domain || host.EndsWith($".{domain}"))) { return true; }
                    continue;
                }
                if (host == pattern) { return true; }
            }
            return false;
        }
    }
}