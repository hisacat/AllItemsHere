using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;

namespace RF5.HisaCat.AllItemsHere
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    internal class BepInExLoader : BepInEx.IL2CPP.BasePlugin
    {
        public const string
            MODNAME = "AllItemsHere",
            AUTHOR = "HisaCat",
            GUID = "RF5." + AUTHOR + "." + MODNAME,
            VERSION = "1.0.0";

        public static BepInEx.Logging.ManualLogSource log;

        public BepInExLoader()
        {
            log = Log;
        }

        public override void Load()
        {
            try
            {
                //Register components
                ClassInjector.RegisterTypeInIl2Cpp<Utils.CoroutineExtension.Wrapper>();
                ClassInjector.RegisterTypeInIl2Cpp<Components.CoroutineAction>();
                ClassInjector.RegisterTypeInIl2Cpp<Components.AllItemsHereWindow>();
                ClassInjector.RegisterTypeInIl2Cpp<Components.SimpleButtonLinker>();
                ClassInjector.RegisterTypeInIl2Cpp<Components.GetItemButtonWork>();
            }
            catch (System.Exception e)
            {
                BepInExLog.LogError($"Harmony - FAILED to Register Il2Cpp Types! {e}");
            }

            try
            {
                //Patches
                Harmony.CreateAndPatchAll(typeof(Components.CoroutineActionPatcher));
                Harmony.CreateAndPatchAll(typeof(Utils.RF5FontHelper.FontLoader));
                Harmony.CreateAndPatchAll(typeof(Patches.ModInitializePatch));
                Harmony.CreateAndPatchAll(typeof(Patches.WindowAttachPatch));
                Harmony.CreateAndPatchAll(typeof(Patches.CampMenuPatch));
            }
            catch (System.Exception e)
            {
                BepInExLog.LogError($"Harmony - FAILED to Apply Patch's! {e}");
            }
        }
    }
}
