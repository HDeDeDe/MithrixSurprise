using System.Runtime.CompilerServices;
using RiskOfOptions;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;

namespace MithrixSurprise {
    internal static class RoO {
        public static bool Enabled =>
            BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions");

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void AddOptions() {
            ModSettingsManager.AddOption(new StepSliderOption(MithrixSurprise.probability, new StepSliderConfig
            {
                min = 0f,
                max = 1f,
                increment = 0.005f
            }));
            ModSettingsManager.SetModDescription("Goodbye Moon Man");
        }
    }
}