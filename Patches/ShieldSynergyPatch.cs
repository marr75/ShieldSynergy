using System;
using Data.ScriptableObject.Terraformation;
using Game.ObjectInfoDataScripts;
using HarmonyLib;
using ShieldSynergy.Core;

namespace ShieldSynergy.Patches;

[HarmonyPatch(typeof(Facility), nameof(Facility.GetHabitabilityParametersBonus))]
static class ShieldSynergyPatch {
    [HarmonyPostfix]
    static void Postfix(Facility __instance, TerraformationConfig.HabitabilityParametersNew __result) {
        if (!Services.Config.Enabled.Value) { return; }
        if (__result == null || __result.radiation >= 0.0) { return; } // null/non-shield/partial-build -> skip
        var n = __instance.Enabled;
        if (n <= 1) { return; } // single shield stays vanilla
        var factor = Math.Pow(n, Services.Config.StackingCurveStrength.Value - 1.0);
        var cap = Services.Config.StackingBonusCap.Value;
        if (cap > 0.0 && factor > cap) { factor = cap; }
        __result.radiation *= factor;
    }
}
