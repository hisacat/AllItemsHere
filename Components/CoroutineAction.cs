using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;
using UnityEngine;
using RF5.HisaCat.AllItemsHere.Utils;

namespace RF5.HisaCat.AllItemsHere.Components
{
    [HarmonyPatch]
    internal class CoroutineActionPatcher
    {
        internal static GameObject instance_containerGo = null;
        internal static CoroutineAction instance = null;
        [HarmonyPatch(typeof(Loader.AssetManager), nameof(Loader.AssetManager.Start))]
        [HarmonyPrefix]
        private static void StartPrefix(Loader.AssetManager __instance)
        {
            //use IL2CPPChainloader.AddUnityComponent instead ?
            //https://docs.bepinex.dev/master/api/BepInEx.IL2CPP.IL2CPPChainloader.html

            if (instance != null) return;
            if (instance_containerGo == null)
                instance_containerGo = new GameObject($"#[{BepInExLoader.GUID}]-CoroutineAction#");

            instance = instance_containerGo.AddComponent<CoroutineAction>();
        }
    }

    internal class CoroutineAction : MonoBehaviour
    {
        public CoroutineAction() : base(ClassInjector.DerivedConstructorPointer<CoroutineAction>()) => ClassInjector.DerivedConstructorBody(this);
        public CoroutineAction(IntPtr ptr) : base(ptr) { }

        private static CoroutineAction instance { get { return CoroutineActionPatcher.instance; } }

        public static Coroutine Start(IEnumerator enumerator)
        {
            return instance.StartCoroutine(enumerator);
        }
        public static void Stop(Coroutine coroutine)
        {
            instance.StopCoroutine(coroutine);
        }
        public static void WaitFrame(int frameCount, Action callback)
        {
            instance.StartCoroutine(WaitFrameRoutine(frameCount, callback));
        }
        public static void WaitOneFrame(Action callback)
        {
            instance.StartCoroutine(WaitFrameRoutine(1, callback));
        }
        public static void WaitForSeconds(float seconds, Action callback)
        {
            instance.StartCoroutine(WaitForSecondsRoutine(seconds, callback));
        }
        public static void WaitForEndOfFrame(Action callback)
        {
            instance.StartCoroutine(WaitForEndOfFrameRoutine(callback));
        }
        private static IEnumerator WaitFrameRoutine(int frameCount, Action callback)
        {
            for (int i = 0; i < frameCount; i++) yield return null;
            callback();
        }
        private static IEnumerator WaitForSecondsRoutine(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback();
        }
        private static IEnumerator WaitForEndOfFrameRoutine(Action callback)
        {
            yield return new WaitForEndOfFrame();
            callback();
        }
    }
}
