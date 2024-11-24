using System.Runtime.CompilerServices;
using RiskOfOptions;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using UnityEngine;

namespace MithrixSurprise {
    internal static class RoO {
        public static bool Enabled =>
            BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions");

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void AddOptions(string iconPath) {
            StepSliderConfig sharedConfig = new StepSliderConfig {
                min = 0f,
                max = 100f,
                increment = 0.25f,
                FormatString = "{0}%"
            };
            
            ModSettingsManager.AddOption(new StepSliderOption(MithrixSurprise.probability, sharedConfig));
            ModSettingsManager.AddOption(new StepSliderOption(MithrixSurprise.probabilityFalse, sharedConfig));

            if (iconPath != "") {
                Texture2D texture = new Texture2D(256, 256, TextureFormat.ARGB32, 3, linear: false);
                if (texture.LoadImage(System.IO.File.ReadAllBytes(iconPath))) {
                    Sprite icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                    ModSettingsManager.SetModIcon(icon);
                }
            }
            ModSettingsManager.SetModDescription("Goodbye Moon Man");
        }
    }
}