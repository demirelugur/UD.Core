namespace UD.Core.Helper.Managements.Files
{
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using UD.Core.Extensions;
    public sealed class FileOperationBatch
    {
        private readonly HashSet<string> _toBeDeletedDirectories = [];
        private readonly HashSet<string> _toBeDeletedFiles = [];
        private readonly Dictionary<string, object> _toBeAddedFiles = [];
        /// <summary>Silinmesi gereken klasörlerin fiziksel yolunu ekler</summary>
        /// <param name="physicallyPaths">Silinmesi gereken klasörün fiziksel yolu.</param>
        public void RemoveDirectory(params string[] physicallyPaths) => this._toBeDeletedDirectories.UnionWith(physicallyPaths ?? []);
        /// <summary>Silinmesi gereken bir dosyanın fiziksel yolunu ekler.</summary>
        /// <param name="physicallyPaths">Silinmesi gereken dosyanın fiziksel yolu.</param>
        public void RemoveFile(params string[] physicallyPaths) => this._toBeDeletedFiles.UnionWith(physicallyPaths ?? []);
        /// <summary>Eklenmesi gereken bir dosyanın fiziksel yolunu ve dosya nesnesini ekler.</summary>
        /// <param name="physicallyPath">Eklenmesi gereken dosyanın fiziksel yolu.</param>
        /// <param name="file">Eklenmesi gereken dosya nesnesi.</param>
        public void Add(string physicallyPath, IFormFile file) => this._toBeAddedFiles.AddOrUpdate(physicallyPath, file);
        /// <summary>Eklenmesi gereken bir dosyanın fiziksel yolunu ve bayt dizisini ekler.</summary>
        /// <param name="physicallyPath">Eklenmesi gereken dosyanın fiziksel yolu.</param>
        /// <param name="dataBinary">Eklenmesi gereken dosyanın bayt dizisi.</param>
        public void Add(string physicallyPath, byte[] dataBinary) => this._toBeAddedFiles.AddOrUpdate(physicallyPath, dataBinary);
        /// <summary>Belirtilen dosyalar yüklenmeden önce, varsa önce silinmesi gereken klasörler ve ardından silinmesi gereken dosyalar kaldırılır.</summary>
        public async Task ProcessAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in this._toBeDeletedDirectories) { FileHelper.DirectoryExistsThenDelete(item, true); }
            foreach (var item in this._toBeDeletedFiles) { FileHelper.FileExistsThenDelete(item); }
            foreach (var item in this._toBeAddedFiles)
            {
                if (item.Value is IFormFile _f) { await _f.FileUploadAsync(item.Key, cancellationToken); }
                else { await ((byte[])item.Value).FileUploadAsync(item.Key, cancellationToken); }
            }
        }
    }
}