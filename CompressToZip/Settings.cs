using System.Collections;

internal static class Settings {
//-----------------------------------------------------Customize--------------------------------------------------------
    // ReSharper disable once InconsistentNaming
    public const bool giveMePDBs = false;
    public const bool weave = false;

    public const string pluginName = "MithrixSurprise";
    public const string pluginAuthor = "HDeDeDe";
    public const string pluginVersion = "1.0.5";
    public const string changelog = "";
    public const string readme = "../README.md";

    public const string icon =
        "../Resources/icon.png";

    public const string riskOfRain2Install =
        @"C:\Program Files (x86)\Steam\steamapps\common\Risk of Rain 2\Risk of Rain 2_Data\Managed\";

    public static readonly ArrayList extraFiles = new() {
    };

    public const string manifestWebsiteUrl = "https://github.com/HDeDeDe/MithrixSurprise";

    public const string manifestDescription =
        "Anytime you purchase anything there's a small chance you'll get a visitor.";

    public const string manifestDependencies = "[\n" +
                                               "\t\t\"bbepis-BepInExPack-5.4.2111\",\n" +
                                               "\t\t\"RiskofThunder-HookGenPatcher-1.2.3\"\n" +
                                               "\t]";
}