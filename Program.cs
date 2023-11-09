using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Xml;
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
        if (VerifyDir(sc.sd)) return;
        if (VerifyDir(sc.sd + "\\.advance")) return;
        if (VerifyDir(sc.sd + "\\config")) return;
        if (VerifyDir(sc.sd + "\\page")) return;
        if (VerifyFile(sc.sd + "\\config\\temp.json")) return;
        if (VerifyFile(sc.sd + "\\.advance\\index.html")) return;
        if (VerifyFile(sc.sd + "\\page\\footer.page")) return;
        if (VerifyFile(sc.sd + "\\page\\header.page")) return;
        if (VerifyFile(sc.textopage)) return;
        if (VerifyFile(sc.launcher)) return;
        log("--------------完了--------------");
        #endregion

        #region CopyFiles
        log("--------ファイルのコピー--------");
        mkdir(sc.od);
        mkdir(sc.od + "\\script");
        mkdir(sc.od + "\\config");
        cpdir(sc.sd + "\\page", sc.od + "\\page");
        if(Directory.Exists(sc.sd + "\\resources")) cpdir(sc.sd + "\\resources", sc.od + "\\resources");
        if (Directory.Exists(sc.sd + "\\sources")) cpdir(sc.sd + "\\sources", sc.od + "\\sources");
        cp(sc.textopage,sc.od + "\\script\\textopage.js");
        cp(sc.launcher, sc.od + "\\script\\launcher.js");
        cp(sc.sd + "\\.advance\\index.html", sc.od + "\\index.html");
        cp(sc.sd + "\\config\\temp.json", sc.od + "\\config\\temp.json");
        log("--------------完了--------------");
        #endregion

        #region LoadPlugin
        log("------プラグインの読み込み------");
        var plugconf = new Dictionary<string, string[]>();
        var script = new List<string>();
        var module = new List<string>();
        var style = new List<string>();
        script.Add("/script/textopage.js");
        script.Add("https://ajax.googleapis.com/ajax/libs/jqueryui/1.13.2/jquery-ui.min.js");
        style.Add("https://ajax.googleapis.com/ajax/libs/jqueryui/1.13.2/themes/smoothness/jquery-ui.css");
        if (Directory.Exists(sc.sd + "\\sources"))
        {
            if (Directory.Exists(sc.sd + "\\sources\\script")) foreach (string s in Directory.GetFiles(sc.sd + "\\sources\\script")) script.Add("/sources/script/" + Path.GetFileName(s));
            if (Directory.Exists(sc.sd + "\\sources\\module")) foreach (string s in Directory.GetFiles(sc.sd + "\\sources\\module")) module.Add("/sources/module/" + Path.GetFileName(s));
            if (Directory.Exists(sc.sd + "\\sources\\style")) foreach (string s in Directory.GetFiles(sc.sd + "\\sources\\style")) style.Add("/sources/style/" + Path.GetFileName(s));
        }


        plugconf.Add("script",script.ToArray());
        plugconf.Add("module", module.ToArray());
        plugconf.Add("css", style.ToArray());
        string plugjson = JsonSerializer.Serialize(plugconf,new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),WriteIndented = true});
        File.WriteAllText(sc.od + "\\config\\plugin.json",plugjson);
        log("--------------完了--------------");
        #endregion
    }
}
public class SiteConfig
{
    public string? textopage;

    public string? launcher;

    public string? sd;

    public string? od;
}