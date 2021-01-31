using System;
using DirectLineRendering;
using Modules;
using UnityEngine;

namespace Quadcopter
{
    public class QuadMotor
    {
        public Part part;
        public QuadEngine engine;

        public QuadMotor(Part p)
        {
            this.part = p;
            foreach (PartModule pm in p.Modules)
            {
                if (pm is QuadEngine)
                {
                    engine = (QuadEngine)pm;
                    engine.StartEngine();
                    engine.modOverride = true;
                }
            }
        }

        public void SetThrottle(float value)
        {
            engine.SetThrottle(value);
        }
    }
}
