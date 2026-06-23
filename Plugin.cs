using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace ShieldSynergy;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin {
    internal static ManualLogSource Log = null!;

    // Master toggle. false = postfix early-returns, byte-identical vanilla.
    internal static ConfigEntry<bool> ShieldSynergyEnabled = null!;

    // p in bonus = perUnit x Enabled^p. 1.0 = vanilla linear; >1 superlinear. Read live.
    internal static ConfigEntry<double> ShieldSynergyExponent = null!;

    // Safety cap on the applied multiplier Enabled^(p-1); 0 = uncapped. Read live.
    internal static ConfigEntry<double> ShieldSynergyMaxMultiplier = null!;

    void Awake() {
        Log = Logger;

        ShieldSynergyEnabled = Config.Bind(
            "ShieldSynergy",
            "ShieldSynergyEnabled",
            true,
            "Master toggle for the feature. false = the postfix early-returns (no field writes), "
            + "so radiation reverts to vanilla linear flat-sum. Read live, so toggling takes effect "
            + "without a rebuild."
        );

        ShieldSynergyExponent = Config.Bind(
            "ShieldSynergy",
            "ShieldSynergyExponent",
            1.5,
            "p in bonus = perUnit x Enabled^p. The radiation term is scaled by Enabled^(p-1) so stacking "
            + "shields is superlinear. 1.0 = vanilla linear; >1 superlinear. 1.5 is a mild starting point "
            + "(the habitability AnimationCurve saturates radiation gains, so aggressive exponents mostly "
            + "waste past the plateau). Read live."
        );

        ShieldSynergyMaxMultiplier = Config.Bind(
            "ShieldSynergy",
            "ShieldSynergyMaxMultiplier",
            0.0,
            "Safety cap on the applied multiplier Enabled^(p-1); the factor is clamped to this value. "
            + "0 = uncapped. Bounds pathological shield stacks. Read live."
        );

        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} loaded.");
        new Harmony(MyPluginInfo.PLUGIN_GUID).PatchAll();
    }
}
