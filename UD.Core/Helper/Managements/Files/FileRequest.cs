namespace UD.Core.Helper.Managements.Files
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
    using UD.Core.Helper.Resources;
    public sealed class FileRequest : IEquatable<FileRequest>
    {
        #region Equals
        public override bool Equals(object other) => this.Equals(other as FileRequest);
        public override int GetHashCode() => HashCode.Combine(this.accept, this.size, this.fileCount);
        public bool Equals(FileRequest other) => (other != null && this.accept.IsUnorderedEqual(other.accept) && this.size == other.size && this.fileCount == other.fileCount);
        #endregion
        private string[] _Accept;
        [UDRequired]
        [UDArrayMinLength]
        [Display(Name = nameof(DisplayNames.FileSettingsHelperAccept), ResourceType = typeof(DisplayNames))]
        public string[] accept { get { return _Accept; } set { _Accept = (value ?? []); } }
        [Range(1, Int64.MaxValue, ErrorMessageResourceName = nameof(DisplayNames.RangeValidationError), ErrorMessageResourceType = typeof(DisplayNames))]
        [Display(Name = nameof(DisplayNames.FileSettingsHelperSize), ResourceType = typeof(DisplayNames))]
        [DefaultValue(1048576)]
        public long size { get; set; }
        [Range(1, Byte.MaxValue, ErrorMessageResourceName = nameof(DisplayNames.RangeValidationError), ErrorMessageResourceType = typeof(DisplayNames))]
        [Display(Name = nameof(DisplayNames.FileSettingsHelperFileCount), ResourceType = typeof(DisplayNames))]
        [DefaultValue(1)]
        public byte fileCount { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public string getFileSizeString => Convert.ToDouble(this.size).ToFileSizeString();
        public bool getTryFileIsException(ICollection<IFormFile> files, out string[] errors) => TryFileisException(files, this, out errors);
        public FileRequest() : this(default, default, default) { }
        public FileRequest(string[] accept, long size, byte fileCount)
        {
            this.accept = accept;
            this.size = size;
            this.fileCount = fileCount;
        }
        public static bool TryFileisException(ICollection<IFormFile> files, FileRequest fileRequest, out string[] errors)
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
                var filesArray = files.Select(file => new
                {
                    file,
                    uzn = file.GetFileExtension()
                }).Select(x => new
                {
                    fileName = x.file.FileName,
                    x.uzn,
                    size = x.file.Length,
                    checkExt = fileRequest.accept.Contains(x.uzn),
                    checkSize = x.file.Length <= fileRequest.size
                }).ToArray();
                if (filesArray.Length > fileRequest.fileCount)
                {
                    errors = [
                       "Yüklenecek maksimum dosya sayısını aştınız!",
                       $"Maksimum dosya sayısı: {fileRequest.fileCount}"
                    ];
                    if (Checks.IsEnglishCurrentUICulture)
                    {
                        errors = [
                           "You have exceeded the maximum number of files allowed to upload!",
                           $"Maximum file count: {fileRequest.fileCount}"
                        ];
                    }
                    return true;
                }
                if (filesArray.Any(x => !x.checkExt))
                {
                    errors = [
                       "Yüklenecek dosya uzantıları uyumsuzdur!",
                       $"Uyumsuz olan dosyalar: {String.Join(", ", filesArray.Where(x => !x.checkExt).OrderBy(x => x.fileName).Select(x => x.fileName).ToArray())}",
                       $"İzin verilen uzantı türleri: {String.Join(", ", fileRequest.accept)}"
                    ];
                    if (Checks.IsEnglishCurrentUICulture)
                    {
                        errors = [
                            "The file extensions are not compatible!",
                            $"Incompatible files: {String.Join(", ", filesArray.Where(x => !x.checkExt).OrderBy(x => x.fileName).Select(x => x.fileName).ToArray())}",
                            $"Allowed extension types: {String.Join(", ", fileRequest.accept)}"
                        ];
                    }
                    return true;
                }
                if (filesArray.Any(x => !x.checkSize))
                {
                    errors = [
                       "Tek bir dosya için izin verilen yükleme miktarını aştınız!",
                       $"Kapasite miktarı aşan dosyalar: {String.Join(", ", filesArray.Where(x => !x.checkSize).OrderByDescending(x => x.size).ThenBy(x => x.fileName).Select(x => String.Join(": ", x.fileName, Convert.ToDouble(x.size).ToFileSizeString())).ToArray())}",
                       $"Tek bir dosya için izin verilen maksimum boyut miktarı: {fileRequest.getFileSizeString}"
                    ];
                    if (Checks.IsEnglishCurrentUICulture)
                    {
                        errors = [
                            "You have exceeded the allowed upload size for a single file!",
                            $"Files exceeding the size limit: {String.Join(", ", filesArray.Where(x => !x.checkSize).OrderByDescending(x => x.size).ThenBy(x => x.fileName).Select(x => String.Join(": ", x.fileName, Convert.ToDouble(x.size).ToFileSizeString())).ToArray())}",
                            $"Maximum allowed size for a single file: {fileRequest.getFileSizeString}"
                        ];
                    }
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