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
    static class ApplianceComponentHelpers_Patch
    {
        [HarmonyTargetMethod]
        static MethodBase ApplianceComponentSetDynamic_TargetMethod() =>
            AccessTools.FirstMethod(typeof(ApplianceComponentHelpers), method => method.Name.Contains("SetDynamic") && method.IsGenericMethod).MakeGenericMethod(typeof(IApplianceProperty));

        [HarmonyPrefix]
        [HarmonyPriority(int.MinValue + 100)]
        static bool ApplianceComponentSetDynamic_Prefix(bool __runOriginal, EntityContext ctx, Entity e, IApplianceProperty component)
        {
            if (!__runOriginal || (component is IAttachmentLogic))
                return true;

            ctx.Set(e, (dynamic)component);

            return false;
        }
    }
}
