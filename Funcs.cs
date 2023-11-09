internal partial class Program
{
    private static void mkdir(string path) => Directory.CreateDirectory(path);
    private static void cp(string src, string dest)
    {
        log("\"" + src + "\"から\n\"" + dest + "\"にコピー\n");
        File.Copy(src, dest, true);
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
    private static void cpdir(string src, string dest)
    {
        log("\"" + src + "\"から\n\"" + dest + "\"にコピー\n");
        if (!Directory.Exists(dest)) mkdir(dest);


        //コピー先のディレクトリ名の末尾に"\"をつける
        if (dest[dest.Length - 1] !=
                Path.DirectorySeparatorChar)
            dest = dest + Path.DirectorySeparatorChar;

        string[] files = Directory.GetFiles(src);
        foreach (string file in files)
            cp(file, dest + Path.GetFileName(file));

        string[] dirs = Directory.GetDirectories(src);
        foreach (string dir in dirs)
            cpdir(dir, dest + Path.GetFileName(dir));
    }
    private static void log(string message) => Console.WriteLine(message);
}
