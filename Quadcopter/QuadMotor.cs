using System;
using DirectLineRendering;
using Modules;
using UnityEngine;
using System.Reflection;

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
                    FieldInfo pr = typeof(Engine).GetField("powerResponse", BindingFlags.Instance | BindingFlags.NonPublic);
                    FieldInfo tr = typeof(Engine).GetField("throttleResponsiveness", BindingFlags.Instance | BindingFlags.NonPublic);
                    pr.SetValue(pm, 50f);
                    tr.SetValue(pm, 50f);
                }
            }
        }

        public void SetThrottle(float value)
        {
            engine.SetThrottle(value);
        }

        public double ShaftPower
        {
            get
            {
                return engine.ShaftPower;
            }
        }
    }
}
