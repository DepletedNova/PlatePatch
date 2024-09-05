using HarmonyLib;
using Kitchen;
using KitchenData;
using KitchenMods;
using System.Reflection;
using Unity.Entities;

namespace KitchenPlatePatch._120
{
    /*
     * Thanks to IcedMilo for creating the patch itself
     */

    [HarmonyPatch]
    static class ItemComponentHelpers_Patch
    {
        [HarmonyTargetMethod]
        static MethodBase ItemComponentSetDynamic_TargetMethod() =>
            AccessTools.FirstMethod(typeof(ItemComponentHelpers), method => method.Name.Contains("SetDynamic") && method.IsGenericMethod).MakeGenericMethod(typeof(IComponentData));

        [HarmonyPrefix]
        [HarmonyPriority(int.MinValue + 100)]
        static bool ItemComponentSetDynamic_Prefix(bool __runOriginal, EntityContext ctx, Entity e, IComponentData component)
        {
            if (!__runOriginal)
                return true;

            ctx.Set(e, (dynamic)component);

            return false;
        }
    }
}
