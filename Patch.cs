using HarmonyLib;
using UnityEngine;

namespace MainBpmChanger
{
    [HarmonyPatch(typeof(ffxMenuPlanetSpeedChange), "Awake")]
    public static class Patch1
    {
        public static bool Prefix(ffxMenuPlanetSpeedChange __instance)
        {
            __instance.gameObject.AddComponent<ffxMenuPlanetSpeedChange2>();
            Object.DestroyImmediate(__instance);
            return false;
        }
    }

    //[HarmonyPatch(typeof(AudioSource), "pitch", MethodType.Getter)]
    public static class Patch2
    {
        public static void Postfix(ref float __result, AudioSource __instance)
        {
            if (__instance == scrConductor.instance.song && ADOBase.isLevelSelect)
                __result = 0.8f;
        }
    }
}
