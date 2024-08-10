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
    }
}
