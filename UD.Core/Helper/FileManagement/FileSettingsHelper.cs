namespace UD.Core.Helper.FileManagement
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
    using UD.Core.Helper.Validation;
    using static UD.Core.Helper.GlobalConstants;
    using static UD.Core.Helper.OrtakTools;
    public sealed class FileSettingsHelper : IEquatable<FileSettingsHelper>
    {
        #region Equals
        public override bool Equals(object other) => this.Equals(other as FileSettingsHelper);
        public override int GetHashCode() => HashCode.Combine(this.accept, this.size, this.fileCount);
        public bool Equals(FileSettingsHelper other) => (other != null && this.accept.IsEqual(other.accept) && this.size == other.size && this.fileCount == other.fileCount);
        #endregion
        private string[] _Accept;
        [UDRequired]
        [UDArrayMinLength]
        [Display(Name = "Uzantı")]
        public string[] accept { get { return _Accept; } set { _Accept = value ?? []; } }
        [Range(1, Int64.MaxValue, ErrorMessage = ValidationErrorMessageConstants.Range)]
        [Display(Name = "Limit Belge Boyutu")]
        [DefaultValue(1048576)]
        public long size { get; set; }
        [Range(1, Byte.MaxValue, ErrorMessage = ValidationErrorMessageConstants.Range)]
        [Display(Name = "Limit Belge Sayısı")]
        [DefaultValue(1)]
        public byte fileCount { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public string getformatsize => FormatSize(Convert.ToDouble(this.size));
        public bool gettryfileisexception(ICollection<IFormFile> files, string dil, out string[] errors) => TryFileisException(files, this, dil, out errors);
        public FileSettingsHelper() : this(default, default, default) { }
        public FileSettingsHelper(string[] accept, long size, byte fileCount)
        {
            this.accept = accept;
            this.size = size;
            this.fileCount = fileCount;
        }
        public static string FormatSize(double value)
        {
            if (value < 0 || Double.IsNaN(value)) { value = 0; }
            var j = 0;
            var sz = FileSizeUnits.Length - 1;
            while (value > 1024 && j < sz) { value /= 1024; j++; }
            return String.Join(" ", (Math.Ceiling(value * 100) / 100).ToString(), FileSizeUnits[j]);
        }
        public static string FormatSizeDetail(double value)
        {
            if (value < 0 || Double.IsNaN(value)) { return ""; }
            var fs = FormatSize(value);
            if (value < 1024 || value % 1024 == 0) { return fs; }
            return $"{value.ToString()} {FileSizeUnits[0]} (~ {fs})";
        }
        public static bool TryFileisException(ICollection<IFormFile> files, FileSettingsHelper fileSettingsHelper, string dil, out string[] errors)
        {
            if (files.Count == 0)
            {
                errors = [];
                return false;
            }
            fileSettingsHelper ??= new();
            if (Validators.TryValidateObject(fileSettingsHelper, out errors)) { return false; }
            Guard.UnSupportLanguage(dil, nameof(dil));
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
                    checkExt = fileSettingsHelper.accept.Contains(x.uzn),
                    checkSize = x.file.Length <= fileSettingsHelper.size
                }).ToArray();
                if (filesArray.Length > fileSettingsHelper.fileCount)
                {
                    if (dil == "en")
                    {
                        errors = new[] {
                           "You have exceeded the maximum number of files allowed to upload!",
                           $"Maximum file count: {fileSettingsHelper.fileCount}"
                        };
                    }
                    else
                    {
                        errors = new[] {
                           "Yüklenecek maksimum dosya sayısını aştınız!",
                           $"Maksimum dosya sayısı: {fileSettingsHelper.fileCount}"
                        };
                    }
                    return true;
                }
                if (filesArray.Any(x => !x.checkExt))
                {
                    if (dil == "en")
                    {
                        errors = new[] {
                            "The file extensions are not compatible!",
                            $"Incompatible files: {String.Join(", ", filesArray.Where(x => !x.checkExt).OrderBy(x => x.fileName).Select(x => x.fileName).ToArray())}",
                            $"Allowed extension types: {String.Join(", ", fileSettingsHelper.accept)}"
                        };
                    }
                    else
                    {
                        errors = new[] {
                           "Yüklenecek dosya uzantıları uyumsuzdur!",
                           $"Uyumsuz olan dosyalar: {String.Join(", ", filesArray.Where(x => !x.checkExt).OrderBy(x => x.fileName).Select(x => x.fileName).ToArray())}",
                           $"İzin verilen uzantı türleri: {String.Join(", ", fileSettingsHelper.accept)}"
                        };
                    }
                    return true;
                }
                if (filesArray.Any(x => !x.checkSize))
                {
                    if (dil == "en")
                    {
                        errors = new[] {
                            "You have exceeded the allowed upload size for a single file!",
                            $"Files exceeding the size limit: {String.Join(", ", filesArray.Where(x => !x.checkSize).OrderByDescending(x => x.size).ThenBy(x => x.fileName).Select(x => String.Join(", ", x.fileName, FormatSize(x.size))).ToArray())}",
                            $"Maximum allowed size for a single file: {fileSettingsHelper.getformatsize}"
                        };
                    }
                    else
                    {
                        errors = new[] {
                            "Tek bir dosya için izin verilen yükleme miktarını aştınız!",
                            $"Kapasite miktarı aşan dosyalar: {String.Join(", ", filesArray.Where(x => !x.checkSize).OrderByDescending(x => x.size).ThenBy(x => x.fileName).Select(x => String.Join(", ", x.fileName, FormatSize(x.size))).ToArray())}",
                            $"Tek bir dosya için izin verilen maksimum boyut miktarı: {fileSettingsHelper.getformatsize}"
                        };
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