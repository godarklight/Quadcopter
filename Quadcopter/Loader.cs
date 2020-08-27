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

        //Game start
        [BalsaAddonInit]
        public static void BalsaInit()
        {
            //Move to menu load if you want to load later.
            if (go == null)
            {
                go = new GameObject();
                mod = go.AddComponent<QuadMain>();
                QuadLog.Debug("Initialized!");
            }
        }

        //Game exit
        [BalsaAddonFinalize]
        public static void BalsaFinalize()
        {
        }
    }
}
