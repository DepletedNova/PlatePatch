using HarmonyLib;
using Kitchen;
using KitchenData;
using KitchenMods;
using System.Reflection;
using Unity.Entities;

namespace KitchenPlatePatch._120
{
    [HarmonyPatch(typeof(EffectHelpers))]
    static class EffectHelpers_Patch
    {
        [HarmonyPatch(nameof(EffectHelpers.AddAttachedEffectComponents))]
        [HarmonyPrefix]
        [HarmonyPriority(int.MinValue)]
        static bool AddAttachedEffectComponents_Prefix(bool __runOriginal, EntityCommandBuffer ecb, Entity e, Effect eff)
        {
            if (!__runOriginal || eff.Properties == null)
                return true;

            foreach (IEffectProperty property in eff.Properties)
            {
                ecb.AddComponent(e, (dynamic)property);
            }

            return false;
        }

        [HarmonyPatch(nameof(EffectHelpers.AddApplianceEffectComponents))]
        [HarmonyPrefix]
        [HarmonyPriority(int.MinValue)]
        static bool AddApplianceEffectComponents_Prefix(bool __runOriginal, EntityCommandBuffer ecb, Entity e, IEffectPropertySource prop)
        {
            if (!__runOriginal || prop.EffectType == null || prop.EffectRange == null)
                return true;

            if (prop.EffectCondition != null)
                ecb.AddComponent(e, (dynamic)prop.EffectCondition);
            else 
                ecb.AddComponent<CEffectAlways>(e);

            ecb.AddComponent(e, (dynamic)prop.EffectType);
            ecb.AddComponent(e, (dynamic)prop.EffectRange);

            return true;
        }
    }
}
