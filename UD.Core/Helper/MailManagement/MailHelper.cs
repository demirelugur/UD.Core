namespace UD.Core.Helper.MailManagement
{
    using System.Net.Mail;
    using UD.Core.Extensions;
    using UD.Core.Helper.Validation;
    using static UD.Core.Helper.OrtakTools;
    public sealed class MailHelper
    {
        public string Subject { get; }
        public string Body { get; }
        public bool IsBodyHtml { get; }
        public MailPriority Priority { get; }
        public MailAddress[] Tos { get; }
        public MailAddress[] CCs { get; }
        public MailAddress[] Bccs { get; }
        public Attachment[] Attachments { get; }
        public MailHelper() : this("", "", default, default, default) { }
        public MailHelper(string Subject, string Body, bool IsBodyHtml, MailPriority Priority, MailAddress[] Tos) : this(Subject, Body, IsBodyHtml, Priority, Tos, default, default, default) { }
        public MailHelper(string Subject, string Body, bool IsBodyHtml, MailPriority Priority, MailAddress[] Tos, MailAddress[] CCs, MailAddress[] Bccs, Attachment[] Attachments)
        {
            this.Subject = Subject.ToStringOrEmpty();
            this.Body = Body.ToStringOrEmpty();
            this.IsBodyHtml = IsBodyHtml;
            this.Priority = Priority;
            this.Tos = Tos ?? [];
            this.CCs = CCs ?? [];
            this.Bccs = Bccs ?? [];
            this.Attachments = Attachments ?? [];
        }
        public async Task<(bool hasError, Exception ex)> Send(SmtpClientBasic smtpClientBasic, CancellationToken cancellationToken = default)
        {
            Guard.ThrowIfEmpty(this.Subject, nameof(this.Subject));
            Guard.ThrowIfEmpty(this.Body, nameof(this.Body));
            Guard.ThrowIfEmpty(this.Tos, nameof(this.Tos));
            Guard.ThrowIfNotValidCheckEnumDefined<MailPriority>(this.Priority, nameof(this.Priority));
            try
            {
                smtpClientBasic ??= new();
                if (Validators.TryValidateObject(smtpClientBasic, out string[] _errors)) { throw _errors.ToNestedException(); }
                using (var mm = new MailMessage())
                {
                    mm.Subject = this.Subject;
                    mm.Body = this.Body;
                    mm.IsBodyHtml = this.IsBodyHtml;
                    mm.Priority = this.Priority;
                    mm.From = new(smtpClientBasic.Email);
                    foreach (var item in this.Tos) { mm.To.Add(item); }
                    foreach (var item in this.CCs) { mm.CC.Add(item); }
                    foreach (var item in this.Bccs) { mm.Bcc.Add(item); }
                    foreach (var item in this.Attachments) { mm.Attachments.Add(item); }
                    await smtpClientBasic.toSmtpClient().SendMailAsync(mm, cancellationToken);
                    return (false, default);
                }
            }
            catch (Exception ex) { return (true, ex); }
        }
    }
}