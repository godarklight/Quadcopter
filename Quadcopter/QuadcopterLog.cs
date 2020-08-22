using System;
using UnityEngine;

namespace Quadcopter
{
    public static class QuadcopterLog
    {
        public static void Debug(string text)
        {
            UnityEngine.Debug.Log($"{Time.realtimeSinceStartup} [Quadcopter] {text}");
        }
    }
}
