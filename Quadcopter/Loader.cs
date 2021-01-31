using System;
using BalsaCore;
using UnityEngine;

namespace Quadcopter
{
    [BalsaAddon]
    public class Loader
    {
        private static GameObject go;
        private static MonoBehaviour mod;
        private static DebugScreen debugScreen;

        //Game start
        [BalsaAddonInit]
        public static void BalsaInit()
        {
            //Move to menu load if you want to load later.
            if (go == null)
            {
                go = new GameObject();
                mod = go.AddComponent<QuadMain>();
                debugScreen = go.AddComponent<DebugScreen>();
                QuadLog.Debug($"Initialized {typeof(QuadMain).FullName}!");
            }
        }

        //Game exit
        [BalsaAddonFinalize]
        public static void BalsaFinalize()
        {
        }
    }
}
