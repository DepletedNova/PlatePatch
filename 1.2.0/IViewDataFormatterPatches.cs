using HarmonyLib;
using Kitchen;
using Kitchen.Formatters.Kitchen;
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
        private static readonly FieldInfo _typeToKeyAndJumpMap = typeof(IViewDataFormatter).GetField("typeToKeyAndJumpMap");

        // Serialize
        [HarmonyPatch(nameof(IViewDataFormatter.Serialize)), HarmonyPrefix]
        [HarmonyPriority(Priority.LowerThanNormal)]
        static bool Serialize_Prefix(bool __runOriginal, ref IViewDataFormatter __instance, ref MessagePackWriter writer, ref IViewData value, MessagePackSerializerOptions options)
        {
            if (!__runOriginal || _typeToKeyAndJumpMap.IsNullOrDestroyed() || value == null)
                return true;
            
            Dictionary<RuntimeTypeHandle, KeyValuePair<int, int>> typeToKeyAndJumpMap = (Dictionary<RuntimeTypeHandle, KeyValuePair<int, int>>)_typeToKeyAndJumpMap.GetValue(__instance);
            if (typeToKeyAndJumpMap.ContainsKey(value.GetType().TypeHandle))
                return true;

            writer.WriteArrayHeader(2);
            writer.WriteInt32(784);
            StandardResolver.Instance.GetFormatterWithVerify<IViewData>().Serialize(ref writer, value, options);
            return false;
        }

        // Deserialize
        [HarmonyPatch(nameof(IViewDataFormatter.Deserialize)), HarmonyPrefix]
        [HarmonyPriority(Priority.LowerThanNormal)]
        static bool Deserialize_Prefix(bool __runOriginal, IViewDataFormatter __instance, ref IViewData __result, ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            if (!__runOriginal || reader.TryReadNil() || reader.ReadArrayHeader() != 2 || _typeToKeyAndJumpMap.IsNullOrDestroyed())
                return true;

            var pseudoReader = reader.Clone(reader.Sequence);
            options.Security.DepthStep(ref pseudoReader); // don't want to DepthStep if the reader will have such repeated if it is unmodded
            int modDetector = pseudoReader.ReadInt32();
            if (modDetector != 784)
                return true;

            options.Security.DepthStep(ref reader);
            __result = StandardResolver.Instance.GetFormatterWithVerify<IViewData>().Deserialize(ref pseudoReader, options);
            return false;
        }
    }
}