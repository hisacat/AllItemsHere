using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RF5.HisaCat.AllItemsHere
{
    internal static class BepInExLog
    {
        public static void Log(string msg)
        {
            BepInExLoader.log.LogMessage($"[{BepInExLoader.GUID}] {msg}");
        }
        public static void Log(object obj)
        {
            BepInExLoader.log.LogMessage($"[{BepInExLoader.GUID}] {obj.ToString()}");
        }
        public static void LogError(string msg)
        {
            BepInExLoader.log.LogError($"[{BepInExLoader.GUID}] {msg}");
        }
        public static void LogError(object obj)
        {
            BepInExLoader.log.LogError($"[{BepInExLoader.GUID}] {obj.ToString()}");
        }
        public static void LogWarning(string msg)
        {
            BepInExLoader.log.LogWarning($"[{BepInExLoader.GUID}] {msg}");
        }
        public static void LogWarning(object obj)
        {
            BepInExLoader.log.LogWarning($"[{BepInExLoader.GUID}] {obj.ToString()}");
        }
    }
}
