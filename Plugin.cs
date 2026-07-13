using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using ShieldSynergy.Config;
using ShieldSynergy.Core;

namespace ShieldSynergy;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin {
    internal static ManualLogSource Log = null!;

    void Awake() {
        Log = Logger;
        Services.Init(new Configuration(Config)); // must precede patching: patches read Services.Config

        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} loaded.");
        new Harmony(MyPluginInfo.PLUGIN_GUID).PatchAll();
    }
}
