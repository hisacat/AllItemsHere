using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RF5.HisaCat.AllItemsHere.Patches
{
    [HarmonyPatch]
    internal class CampMenuPatch
    {
        public delegate void OnPageOpenedDelegate(CampPage page);
        public static event OnPageOpenedDelegate onPageOpened;

        [HarmonyPatch(typeof(CampPageSwitcher), nameof(CampPageSwitcher.OpenPage), new System.Type[] { typeof(CampPage) })]
        [HarmonyPostfix]
        public static void OpenPagePostfix(CampPageSwitcher __instance, int nextPage)
        {
            onPageOpened?.Invoke((CampPage)nextPage);
        }
        [HarmonyPatch(typeof(CampPageSwitcher), nameof(CampPageSwitcher.OpenPage), new System.Type[] { typeof(int) })]
        [HarmonyPostfix]
        public static void OpenPagePostfix(CampPageSwitcher __instance, CampPage nextPage)
        {
            onPageOpened?.Invoke(nextPage);
        }
    }
}
