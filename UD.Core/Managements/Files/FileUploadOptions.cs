namespace UD.Core.Managements.Files
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    using UD.Core.Attributes.DataAnnotations;
    using UD.Core.Extensions;
    using UD.Core.Helper;
    using static UD.Core.Helper.GlobalConstants;
    public sealed class FileUploadOptions : IEquatable<FileUploadOptions>
    {
        #region Equals
        public override bool Equals(object other) => this.Equals(other as FileUploadOptions);
        public override int GetHashCode() => HashCode.Combine(this.Accept, this.Size, this.FileCount);
        public bool Equals(FileUploadOptions other) => (other != null && this.Accept.IsUnorderedEqual(other.Accept) && this.Size == other.Size && this.FileCount == other.FileCount);
        #endregion
        [UDRequired]
        [UDArrayMinLength]
        [Display(Name = "Uzantı")]
        public string[] Accept { get; set; } = [];
        [Range(1, Int64.MaxValue, ErrorMessage = ValidationMessageTurkishConstants.Range)]
        [Display(Name = "Belge Boyutu")]
        [DefaultValue(1048576)]
        public long Size { get; set; }
        [Range(1, Byte.MaxValue, ErrorMessage = ValidationMessageTurkishConstants.Range)]
        [Display(Name = "Belge Sayısı")]
        [DefaultValue(1)]
        public byte FileCount { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public string FormattedFileSize => Convert.ToDouble(this.Size).ToFileSizeString();
        public bool HasFileValidationErrors(ICollection<IFormFile> files, out string[] errors) => HasFileValidationErrors(files, this, out errors);
        public FileUploadOptions() : this(default, default, default) { }
        public FileUploadOptions(string[] accept, long size, byte fileCount)
        {
            this.Accept = accept ?? [];
            this.Size = size;
            this.FileCount = fileCount;
        }
        public static bool HasFileValidationErrors(ICollection<IFormFile> files, FileUploadOptions fileRequest, out string[] errors)
        {
            if (files.Count == 0)
            {
                errors = [];
                return false;
            }
            fileRequest ??= new();
            if (TryValidators.TryValidateObject(fileRequest, out errors)) { return false; }
            try
            {
                if (files.Count > fileRequest.FileCount)
                {
                    errors = [
                       "Yüklenecek maksimum dosya sayısını aştınız!",
                       $"Maksimum dosya sayısı: {fileRequest.FileCount}"
                    ];
                    return true;
                }
                var filesArray = files.Select(file => new
                {
                    file,
                    uzn = file.GetFileExtension()
                }).Select(x => new
                {
                    fileName = x.file.FileName,
                    x.uzn,
                    size = x.file.Length,
                    checkExt = fileRequest.Accept.Contains(x.uzn),
                    checkSize = x.file.Length <= fileRequest.Size
                }).ToArray();
                if (filesArray.Any(x => !x.checkExt))
                {
                    errors = [
                       "Yüklenecek dosya uzantıları uyumsuzdur!",
                       $"Uyumsuz olan dosyalar: {String.Join(", ", filesArray.Where(x => !x.checkExt).OrderBy(x => x.fileName).Select(x => x.fileName).ToArray())}",
                       $"İzin verilen uzantı türleri: {String.Join(", ", fileRequest.Accept)}"
                    ];
                    return true;
                }
                if (filesArray.Any(x => !x.checkSize))
                {
                    errors = [
                       "Tek bir dosya için izin verilen yükleme miktarını aştınız!",
                       $"Kapasite miktarı aşan dosyalar: {String.Join(", ", filesArray.Where(x => !x.checkSize).OrderByDescending(x => x.size).ThenBy(x => x.fileName).Select(x => String.Join(": ", x.fileName, Convert.ToDouble(x.size).ToFileSizeString())).ToArray())}",
                       $"Tek bir dosya için izin verilen maksimum boyut miktarı: {fileRequest.FormattedFileSize}"
                    ];
                    return true;
                }
                errors = [];
                return false;
            }
            catch (Exception ex)
            {
                errors = ex.AllExceptionMessage();
                return true;
            }
        }
    }
}