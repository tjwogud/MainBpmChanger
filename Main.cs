using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

namespace MainBpmChanger
{
    public class Main
    {
        public static Harmony Harmony { get; private set; }
        public static Settings Settings { get; private set; }

        public static Dictionary<SystemLanguage, Dictionary<string, string>> Localizations { get; }
            = new Dictionary<SystemLanguage, Dictionary<string, string>>
            {
                {
                    SystemLanguage.Korean, new Dictionary<string, string>
                    {
                        { "settings.pitch", "피치" },
                        { "settings.applyPitch", "적용" },
                        { "settings.changeMusic", "속도 변경 시 음악 변경" },
                        { "settings.multiplyMusic", "속도 변경 시 음악 배속" }
                    }
                },
                {
                    SystemLanguage.English, new Dictionary<string, string>
                    {
                        { "settings.pitch", "Pitch" },
                        { "settings.applyPitch", "Apply" },
                        { "settings.changeMusic", "Change music when changing speed" },
                        { "settings.multiplyMusic", "Speed up music when changing speed" }
                    }
                }
            };

        public static void Setup(UnityModManager.ModEntry modEntry)
        {
            Harmony = new Harmony(modEntry.Info.Id);
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
            Settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
            bpmCache = Settings.pitch;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
        }

        private static float bpmCache;

        public static void OnGUI(UnityModManager.ModEntry modEntry) {
            Dictionary<string, string> localizations = Localizations.TryGetValue(RDString.language, out Dictionary<string, string> dict) ? dict : Localizations[SystemLanguage.English];
            GUILayout.Label(localizations["settings.pitch"]);
            GUILayout.BeginHorizontal();
            try
            {
                bpmCache = float.Parse(GUILayout.TextField(string.Format("{0:0.0000}", bpmCache), GUILayout.Width(100)));
            }
            catch (Exception) {
            }
            if (bpmCache != Settings.pitch)
            {
                GUILayout.Space(5);
                if (GUILayout.Button(localizations["settings.applyPitch"], GUILayout.Width(70)))
                {
                    Settings.pitch = Mathf.Max(bpmCache, 0.0001f);
                    bpmCache = Settings.pitch;
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            Settings.changeMusic = GUILayout.Toggle(Settings.changeMusic, localizations["settings.changeMusic"]);
            GUILayout.Space(10);
            Settings.multiplyMusic = GUILayout.Toggle(Settings.multiplyMusic, localizations["settings.multiplyMusic"]);
        }

        public static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Settings.Save(modEntry);
        }
    }
}
