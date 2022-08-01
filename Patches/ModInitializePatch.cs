using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RF5.HisaCat.AllItemsHere.Patches
{
    [HarmonyPatch]
    internal class ModInitializePatch
    {
        public static bool IsInitialized = false;

        [HarmonyPatch(typeof(SV), nameof(SV.CreateUIRes))]
        [HarmonyPostfix]
        public static void CreateUIResPostfix(SV __instance)
        {
            if (IsInitialized)
                return;

            BepInExLog.Log("Mod initializing...");
            if (BundleLoader.LoadBundle() == false)
            {
                IsInitialized = false;
                BepInExLog.Log("Mod initialize failed.");
                return;
            }

            IsInitialized = true;
            BepInExLog.Log("Mod initialized.");
            return;
        }
    }
}
