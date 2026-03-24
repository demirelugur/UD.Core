namespace UD.Core.Helper
{
    public sealed class Files
    {
        /// <summary>Verilen fiziksel dosya yolunda bir dosya varsa onu siler.</summary>
        /// <param name="physicallyPath">Silinecek dosyanın fiziksel yolu.</param>
        public static void FileExistsThenDelete(string physicallyPath) { if (File.Exists(physicallyPath)) { File.Delete(physicallyPath); } }
        /// <summary>Verilen klasör yolunda bir klasör varsa, isteğe bağlı olarak içindekilerle birlikte siler.</summary>
        /// <param name="physicallyPath">Silinecek klasörün fiziksel yolu.</param>
        /// <param name="recursive">Eğer <see langword="true"/> verilirse, dizin ve altındaki tüm dosyalar ve alt dizinler silinir. <see langword="false"/> verilirse, dizin yalnızca boşsa silinir; aksi halde bir <see cref="IOException"/> fırlatılır.</param>
        public static void DirectoryExistsThenDelete(string physicallyPath, bool recursive) { if (Directory.Exists(physicallyPath)) { Directory.Delete(physicallyPath, recursive); } }
        /// <summary>Verilen fiziksel dosya yolunda klasör mevcut değilse, ilgili klasörü ve varsa üst dizinlerini oluşturur.</summary>
        /// <param name="physicallyPath">Oluşturulacak klasörün fiziksel yolu.</param>
        public static void DirectoryCreate(string physicallyPath)
        {
            var di = new DirectoryInfo(physicallyPath);
            if (di.Parent != null) { DirectoryCreate(di.Parent.FullName); }
            if (!di.Exists) { di.Create(); }
        }
    }
}