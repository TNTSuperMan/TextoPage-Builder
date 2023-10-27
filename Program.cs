using System.Net.NetworkInformation;
using System.Xml.Serialization;

internal partial class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("TextoPage Builder");
        #region GetPath
        string? configpath = "";
        if(args.Length == 0)
        {
            Console.Write("設定ファイルをこちらにドラッグ: ");
            configpath = Console.ReadLine();
        }
        else
        {
            configpath = args[0];
        }
        if(!File.Exists(configpath))
        {
            Console.WriteLine("ファイルが存在しませんでした。");
            return;
        }
        #endregion

        #region LoadConfig
        log("-----設定ファイルの読み込み-----");
        var ser = new XmlSerializer(typeof(SiteConfig));
        SiteConfig sc = new SiteConfig();
        try
        {
            sc = (SiteConfig)ser.Deserialize(new FileStream(configpath, FileMode.Open));
        }
        catch
        {
            Console.WriteLine("設定ファイルが認識できませんでした。");
            return;
        }
        if(sc == null)
        {
            Console.WriteLine("設定ファイルが認識できませんでした。");
            return;
        }
        log("--------------完了--------------");
        #endregion

        #region VerifyFile
        log("-----ファイル等の存在の確認-----");
        if (VerifyDir(sc.sd + "\\.advance")) return;
        if (VerifyDir(sc.sd + "\\config")) return;
        if (VerifyDir(sc.sd + "\\page")) return;
        if (VerifyDir(sc.sd + "\\resources")) return;
        if (VerifyDir(sc.od)) return;
        if (VerifyFile(sc.sd + "\\config\\temp.json")) return;
        if (VerifyFile(sc.sd + "\\.advance\\index.html")) return;
        if (VerifyFile(sc.sd + "\\page\\footer.page")) return;
        if (VerifyFile(sc.sd + "\\page\\header.page")) return;
        if (!sc.tisurl) if (VerifyFile(sc.textopage)) return;
        if (!sc.lisurl) if (VerifyFile(sc.launcher)) return;
        log("--------------完了--------------");
        #endregion

        #region CopyFiles
        log("--------ファイルのコピー--------");
        cpdir(sc.sd + "\\page", sc.od + "\\page");
        log("--------------完了--------------");
        #endregion
    }
}
public class SiteConfig
{
    public string textopage;
    public bool tisurl; //true:URL false:File

    public string launcher;
    public bool lisurl;

    public string sd; //No last slash

    public string od;
}