using System.Runtime.CompilerServices;
using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;

namespace MithrixSurprise {
    public static class RoO {
        public static bool Enabled =>
            BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions");

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void AddOptions(ConfigEntry<float> configEntry) {
            ModSettingsManager.AddOption(new StepSliderOption(configEntry, new StepSliderConfig
            {
                min = 0.005f,
                max = 1f,
                increment = 0.005f
            }));
            ModSettingsManager.SetModDescription("Goodbye Moon Man");
        }
    }
}