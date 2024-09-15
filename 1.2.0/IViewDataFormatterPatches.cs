using HarmonyLib;
using Kitchen;
using Kitchen.Formatters.Kitchen;
using KitchenLib.Utils;
using MessagePack;
using MessagePack.Resolvers;
using System;
using System.Collections.Generic;
using System.Reflection;
using UniverseLib.Utility;
using Priority = HarmonyLib.Priority;

namespace KitchenPlatePatch._120
{
    /*
     * Thanks to StarFluxGames for creating the patch itself
     */

    [HarmonyPatch(typeof(IViewDataFormatter))]
    static class IViewDataFormatter_Patch
    {
        private static FieldInfo _typeToKeyAndJumpMap = ReflectionUtils.GetField<IViewDataFormatter>("typeToKeyAndJumpMap");

        // Serialize
        [HarmonyPatch(nameof(IViewDataFormatter.Serialize)), HarmonyPrefix]
        [HarmonyPriority(Priority.LowerThanNormal)]
        static bool Serialize_Prefix(bool __runOriginal, IViewDataFormatter __instance, ref MessagePackWriter writer, IViewData value, MessagePackSerializerOptions options)
        {
            if (!__runOriginal || _typeToKeyAndJumpMap.IsNullOrDestroyed() || value == null)
                return true;
            
            Dictionary<RuntimeTypeHandle, KeyValuePair<int, int>> typeToKeyAndJumpMap = (Dictionary<RuntimeTypeHandle, KeyValuePair<int, int>>)_typeToKeyAndJumpMap.GetValue(__instance);
            if (typeToKeyAndJumpMap.ContainsKey(value.GetType().TypeHandle))
                return true;

            writer.WriteInt32(784);
            StandardResolver.Instance.GetFormatterWithVerify<IViewData>().Serialize(ref writer, value, options);
            return false;
        }

        // Deserialize
        [HarmonyPatch(nameof(IViewDataFormatter.Deserialize)), HarmonyPrefix]
        [HarmonyPriority(Priority.LowerThanNormal)]
        static bool Deserialize_Prefix(bool __runOriginal, IViewDataFormatter __instance, ref IViewData __result, MessagePackReader reader, MessagePackSerializerOptions options)
        {
            if (!__runOriginal || reader.TryReadNil() || reader.ReadArrayHeader() != 2 || _typeToKeyAndJumpMap.IsNullOrDestroyed())
                return true;

            int modDetector = reader.ReadInt32();
            if (modDetector != 784)
                return true;

            __result = StandardResolver.Instance.GetFormatterWithVerify<IViewData>().Deserialize(ref reader, options);
            return false;
        }
    }
}