using HarmonyLib;
using KitchenMods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KitchenPlatePatch
{
    public class Main : IModInitializer
    {
        public const string MOD_GUID = "DepletedNova.PlatePatch";
        public const string MOD_NAME = "PlatePatch";
        public const string MOD_VERSION = "1.0.4";
        public const string GAME_VERSION = "1.2.0";

        public Main()
        {
            Harmony harmony = new Harmony(MOD_GUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public void PostActivate(Mod mod)
        {
            LogWarning($"{MOD_GUID} (v{MOD_VERSION}) for game version {GAME_VERSION} in use!");
        }

        public void PostInject() { }
        public void PreInject() { }

        #region Logging
        public static void LogInfo(string _log) { Debug.Log($"[{MOD_NAME}] " + _log); }
        public static void LogWarning(string _log) { Debug.LogWarning($"[{MOD_NAME}] " + _log); }
        public static void LogError(string _log) { Debug.LogError($"[{MOD_NAME}] " + _log); }
        public static void LogInfo(object _log) { LogInfo(_log.ToString()); }
        public static void LogWarning(object _log) { LogWarning(_log.ToString()); }
        public static void LogError(object _log) { LogError(_log.ToString()); }
        #endregion
    }
}
