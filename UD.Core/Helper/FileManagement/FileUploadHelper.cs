namespace UD.Core.Helper.FileManagement
{
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using UD.Core.Extensions;
    using static UD.Core.Helper.OrtakTools;
    public sealed class FileUploadHelper
    {
        private readonly HashSet<string> deldirectories = new();
        private readonly HashSet<string> delfiles = new();
        private readonly Dictionary<string, object> addfiles = new();
        /// <summary>
        /// Silinmesi gereken klasörlerin fiziksel yolunu ekler
        /// </summary>
        /// <param name="physicallydirectorypaths">Silinmesi gereken klasörün fiziksel yolu.</param>
        public void RemoveDirectory(params string[] physicallydirectorypaths) => this.deldirectories.AddRangeOptimized(physicallydirectorypaths ?? []);
        /// <summary>
        /// Silinmesi gereken bir dosyanın fiziksel yolunu ekler.
        /// </summary>
        /// <param name="physicallypaths">Silinmesi gereken dosyanın fiziksel yolu.</param>
        public void RemoveFile(params string[] physicallypaths) => this.delfiles.AddRangeOptimized(physicallypaths ?? []);
        /// <summary>
        /// Eklenmesi gereken bir dosyanın fiziksel yolunu ve dosya nesnesini ekler.
        /// </summary>
        /// <param name="physicallypath">Eklenmesi gereken dosyanın fiziksel yolu.</param>
        /// <param name="file">Eklenmesi gereken dosya nesnesi.</param>
        public void Add(string physicallypath, IFormFile file) => this.addfiles.AddOrUpdate(physicallypath, file);
        /// <summary>
        /// Eklenmesi gereken bir dosyanın fiziksel yolunu ve bayt dizisini ekler.
        /// </summary>
        /// <param name="physicallypath">Eklenmesi gereken dosyanın fiziksel yolu.</param>
        /// <param name="bytes">Eklenmesi gereken dosyanın bayt dizisi.</param>
        public void Add(string physicallypath, byte[] bytes) => this.addfiles.AddOrUpdate(physicallypath, bytes);
        /// <summary>
        /// Belirtilen dosyalar yüklenmeden önce, varsa önce silinmesi gereken klasörler ve ardından silinmesi gereken dosyalar kaldırılır.
        /// </summary>
        public async Task ProcessFileUploadsAndDeletionsAsync(CancellationToken cancellationtoken = default)
        {
            var delfileany = this.delfiles.Count > 0;
            var addfileany = this.addfiles.Count > 0;
            if (this.deldirectories.Count > 0)
            {
                foreach (var item in this.deldirectories) { _file.DirectoryExistsThenDelete(item, true); }
                if (delfileany || addfileany) { await Task.Delay(300, cancellationtoken); }
            }
            if (delfileany)
            {
                foreach (var item in this.delfiles) { _file.FileExistsThenDelete(item); }
                if (addfileany) { await Task.Delay(300, cancellationtoken); }
            }
            if (addfileany)
            {
                foreach (var item in this.addfiles)
                {
                    if (item.Value is IFormFile _f) { await _f.FileUploadAsync(item.Key, cancellationtoken); }
                    else { await ((byte[])item.Value).FileUploadAsync(item.Key, cancellationtoken); }
                }
            }
        }
    }
}