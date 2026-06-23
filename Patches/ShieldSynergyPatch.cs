using System;
using Data.ScriptableObject.Terraformation;
using Game.ObjectInfoDataScripts;
using HarmonyLib;

namespace ShieldSynergy.Patches;

[HarmonyPatch(typeof(Facility), nameof(Facility.GetHabitabilityParametersBonus))]
static class ShieldSynergyPatch {
    [HarmonyPostfix]
    static void Postfix(Facility __instance, TerraformationConfig.HabitabilityParametersNew __result) {
        if (!Plugin.ShieldSynergyEnabled.Value) { return; }
        if (__result == null || __result.radiation <= 0.0) { return; } // null/non-shield/partial-build -> skip
        long n = __instance.Enabled;
        if (n <= 1) { return; }                                        // single shield stays vanilla
        double factor = Math.Pow(n, Plugin.ShieldSynergyExponent.Value - 1.0);
        double cap = Plugin.ShieldSynergyMaxMultiplier.Value;
        if (cap > 0.0 && factor > cap) { factor = cap; }
        __result.radiation *= factor;
    }
}
