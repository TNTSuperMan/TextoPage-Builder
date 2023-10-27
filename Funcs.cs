internal partial class Program
{
    private static void mkdir(string path) => Directory.CreateDirectory(path);
    private static void cp(string src, string dest)
    {
        if (File.Exists(dest))
        {
            if (isNotEquals(src, dest))
            {
                log("\"" + src + "\"から\n\"" + dest + "\"にコピー\n");
                File.Copy(src, dest, true);
            }
        }
        else
        {
            log("\"" + src + "\"から\n\"" + dest + "\"にコピー\n");
            File.Copy(src, dest, true);
        }
    }
    private static bool VerifyFile(string filename)
    {
        log("確認：ファイル \"" + filename + "\"");
        if (!File.Exists(filename))
        {
            log("ファイル \"" + filename + "\" が存在しません。");
            return true;
        }
        return false;
    }
    private static bool VerifyDir(string filename)
    {
        log("確認：ディレクトリ \"" + filename + "\"");
        if (!Directory.Exists(filename))
        {
            log("ディレクトリ \"" + filename + "\" が存在しません。");
            return true;
        }
        return false;
    }
    private static void cpdir(string src,string dest)
    {
        log("\"" + src + "\"から\n\"" + dest + "\"にコピー\n");
        if (!Directory.Exists(dest)) mkdir(dest);

        string[] srcFiles = Directory.GetFiles(src);
        foreach (string srcf in srcFiles)
        {
            cp(srcf, dest + "\\" + Path.GetFileName(srcf));
        }
    }
    private static bool isNotEquals(string path1,string path2)
    {
        var md5 = System.Security.Cryptography.MD5.Create();
        byte[] hash1 = md5.ComputeHash(new FileStream(path1, FileMode.Open));
        byte[] hash2 = md5.ComputeHash(new FileStream(path2, FileMode.Open));
        return (hash1.Length != hash2.Length);
    }
    private static void log(string message) => Console.WriteLine(message);
}
