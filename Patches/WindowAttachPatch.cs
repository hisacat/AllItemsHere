using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RF5.HisaCat.AllItemsHere.Components;

namespace RF5.HisaCat.AllItemsHere.Patches
{
    [HarmonyPatch]
    internal class WindowAttachPatch
    {
        [HarmonyPatch(typeof(CampMenuMain), nameof(CampMenuMain.StartCamp))]
        [HarmonyPostfix]
        public static void StartCampPostfix(CampMenuMain __instance)
        {
            if (AllItemsHereWindow.Instance != null)
                return;

            if (ModInitializePatch.IsInitialized == false)
            {
                BepInExLog.LogError("InstantiateAndAttach: Mod does not initialized");
                return;
            }

            AllItemsHereWindow.InstantiateAndAttach();
        }
    }
}
