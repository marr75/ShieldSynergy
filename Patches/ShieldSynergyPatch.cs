using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.ScriptableObject;
using Data.ScriptableObject.Terraformation;
using Game.Info;
using Game.ObjectInfoDataScripts;
using HarmonyLib;
using ShieldSynergy.Core;
using UnityEngine;

namespace ShieldSynergy.Patches;

[HarmonyPatch(typeof(Facility), nameof(Facility.GetHabitabilityParametersBonus))]
static class ShieldSynergyPatch {
    static readonly Dictionary<ObjectInfo, long> PooledEnabledCache = new();
    static int cachedFrame = -1;

    [HarmonyPostfix]
    static void Postfix(Facility __instance, TerraformationConfig.HabitabilityParametersNew __result) {
        if (!Services.Config.Enabled.Value) { return; }
        if (__result == null || __result.radiation >= 0.0) { return; } // non-reduction facility -> vanilla

        var owner = __instance.ObjectInfoData?.ObjectInfo;
        if (owner == null) { return; }
        var groundBody = owner.objectTypes == EObjectTypes.Orbit ? owner.parentObjectInfo : owner;
        if (groundBody == null) { return; }

        var n = GetPooledEnabled(groundBody);
        if (n <= 1) { return; } // single installation total stays vanilla

        var factor = Math.Pow(n, Services.Config.StackingCurveStrength.Value - 1.0);
        var cap = Services.Config.StackingBonusCap.Value;
        if (cap > 0.0 && factor > cap) { factor = cap; }
        __result.radiation *= factor;
    }

    static long GetPooledEnabled(ObjectInfo groundBody) {
        var framesPassed = Time.frameCount - cachedFrame;
        if (framesPassed >= 60) {
            PooledEnabledCache.Clear();
            cachedFrame = Time.frameCount;
        }
        if (PooledEnabledCache.TryGetValue(groundBody, out var cached)) { return cached; }
        var n = SumRadiationEnabled(groundBody, false);
        if (groundBody.lowOrbitCustom) { n += SumRadiationEnabled(groundBody.lowOrbitCustom.GetObjectInfo(), true); }
        PooledEnabledCache[groundBody] = n;
        return n;
    }

    static long SumRadiationEnabled(ObjectInfo body, bool forOrbit) =>
        (from data in body.ObjectsInfoData
            from facility in data.ListFacility
            let placement = facility.facilityDescriptor.PossiblePlacement
            where placement != FacilityBaseDescriptor.EPossiblePlacement.Orbit || forOrbit
            where placement != FacilityBaseDescriptor.EPossiblePlacement.Surface || !forOrbit
            where !(facility.BuildProgress < 1.0)
            where (facility.facilityDescriptor.habitabilityParametersBonus?.radiation ?? 0.0) != 0.0
            select facility.Enabled).Sum();
}
