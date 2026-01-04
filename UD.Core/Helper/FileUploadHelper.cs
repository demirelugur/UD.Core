namespace UD.Core.Helper
{
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using UD.Core.Extensions;
    using static UD.Core.Helper.OrtakTools;
    public sealed class FileUploadHelper
    {
        private readonly HashSet<string> _deldirectories = new();
        private readonly HashSet<string> _delfiles = new();
        private readonly Dictionary<string, object> _addfiles = new();
        /// <summary>
        /// Silinmesi gereken klasörlerin fiziksel yolunu ekler
        /// </summary>
        /// <param name="physicallydirectorypaths">Silinmesi gereken klasörün fiziksel yolu.</param>
        public void RemoveDirectory(params string[] physicallydirectorypaths) => this._deldirectories.AddRangeOptimized(physicallydirectorypaths ?? Array.Empty<string>());
        /// <summary>
        /// Silinmesi gereken bir dosyanın fiziksel yolunu ekler.
        /// </summary>
        /// <param name="physicallypaths">Silinmesi gereken dosyanın fiziksel yolu.</param>
        public void RemoveFile(params string[] physicallypaths) => this._delfiles.AddRangeOptimized(physicallypaths ?? Array.Empty<string>());
        /// <summary>
        /// Eklenmesi gereken bir dosyanın fiziksel yolunu ve dosya nesnesini ekler.
        /// </summary>
        /// <param name="physicallypath">Eklenmesi gereken dosyanın fiziksel yolu.</param>
        /// <param name="file">Eklenmesi gereken dosya nesnesi.</param>
        public void Add(string physicallypath, IFormFile file) => this._addfiles.AddOrUpdate(physicallypath, file);
        /// <summary>
        /// Eklenmesi gereken bir dosyanın fiziksel yolunu ve bayt dizisini ekler.
        /// </summary>
        /// <param name="physicallypath">Eklenmesi gereken dosyanın fiziksel yolu.</param>
        /// <param name="bytes">Eklenmesi gereken dosyanın bayt dizisi.</param>
        public void Add(string physicallypath, byte[] bytes) => this._addfiles.AddOrUpdate(physicallypath, bytes);
        /// <summary>
        /// Belirtilen dosyalar yüklenmeden önce, varsa önce silinmesi gereken klasörler ve ardından silinmesi gereken dosyalar kaldırılır.
        /// </summary>
        public async Task ProcessFileUploadsAndDeletionsAsync(CancellationToken cancellationtoken = default)
        {
            var _delfileany = this._delfiles.Count > 0;
            var _addfileany = this._addfiles.Count > 0;
            if (this._deldirectories.Count > 0)
            {
                foreach (var item in this._deldirectories) { _file.DirectoryExiststhenDelete(item, true); }
                if (_delfileany || _addfileany) { await Task.Delay(300, cancellationtoken); }
            }
            if (_delfileany)
            {
                foreach (var item in this._delfiles) { _file.FileExiststhenDelete(item); }
                if (_addfileany) { await Task.Delay(300, cancellationtoken); }
            }
            if (_addfileany)
            {
                foreach (var item in this._addfiles)
                {
                    if (item.Value is IFormFile _f) { await _f.FileUploadAsync(item.Key, cancellationtoken); }
                    else { await ((byte[])item.Value).FileUploadAsync(item.Key, cancellationtoken); }
                }
            }
        }
    }
}