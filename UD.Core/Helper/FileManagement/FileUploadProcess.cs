namespace UD.Core.Helper.FileManagement
{
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using UD.Core.Extensions;
    using UD.Core.Extensions.Common;
    using UD.Core.Helper;
    public sealed class FileUploadProcess
    {
        private readonly HashSet<string> toBeDeletedDirectories = [];
        private readonly HashSet<string> toBeDeletedFiles = [];
        private readonly Dictionary<string, object> toBeAddedFiles = [];
        /// <summary>Silinmesi gereken klasörlerin fiziksel yolunu ekler</summary>
        /// <param name="physicallyPaths">Silinmesi gereken klasörün fiziksel yolu.</param>
        public void RemoveDirectory(params string[] physicallyPaths) => this.toBeDeletedDirectories.AddRangeOptimized(physicallyPaths ?? []);
        /// <summary>Silinmesi gereken bir dosyanın fiziksel yolunu ekler.</summary>
        /// <param name="physicallyPaths">Silinmesi gereken dosyanın fiziksel yolu.</param>
        public void RemoveFile(params string[] physicallyPaths) => this.toBeDeletedFiles.AddRangeOptimized(physicallyPaths ?? []);
        /// <summary>Eklenmesi gereken bir dosyanın fiziksel yolunu ve dosya nesnesini ekler.</summary>
        /// <param name="physicallyPath">Eklenmesi gereken dosyanın fiziksel yolu.</param>
        /// <param name="file">Eklenmesi gereken dosya nesnesi.</param>
        public void Add(string physicallyPath, IFormFile file) => this.toBeAddedFiles.AddOrUpdate(physicallyPath, file);
        /// <summary>Eklenmesi gereken bir dosyanın fiziksel yolunu ve bayt dizisini ekler.</summary>
        /// <param name="physicallyPath">Eklenmesi gereken dosyanın fiziksel yolu.</param>
        /// <param name="dataBinary">Eklenmesi gereken dosyanın bayt dizisi.</param>
        public void Add(string physicallyPath, byte[] dataBinary) => this.toBeAddedFiles.AddOrUpdate(physicallyPath, dataBinary);
        /// <summary>Belirtilen dosyalar yüklenmeden önce, varsa önce silinmesi gereken klasörler ve ardından silinmesi gereken dosyalar kaldırılır.</summary>
        public async Task ProcessFileUploadsAndDeletions(CancellationToken cancellationToken = default)
        {
            var hasDeletedFiles = this.toBeDeletedFiles.Count > 0;
            var hasAddedFile = this.toBeAddedFiles.Count > 0;
            if (this.toBeDeletedDirectories.Count > 0)
            {
                foreach (var item in this.toBeDeletedDirectories) { Files.DirectoryExistsThenDelete(item, true); }
                if (hasDeletedFiles || hasAddedFile) { await Task.Delay(300, cancellationToken); }
            }
            if (hasDeletedFiles)
            {
                foreach (var item in this.toBeDeletedFiles) { Files.FileExistsThenDelete(item); }
                if (hasAddedFile) { await Task.Delay(300, cancellationToken); }
            }
            if (hasAddedFile)
            {
                foreach (var item in this.toBeAddedFiles)
                {
                    if (item.Value is IFormFile _f) { await _f.FileUpload(item.Key, cancellationToken); }
                    else { await ((byte[])item.Value).FileUpload(item.Key, cancellationToken); }
                }
            }
        }
    }
}