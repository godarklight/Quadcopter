using System;
using Modules;

namespace Quadcopter
{
    public class QuadMotor
    {
        public Part part;
        public QuadEngine engine;

        public QuadMotor(Part p)
        {
            this.part = p;
            foreach (QuadEngine pm in p.Modules)
            {
                if (pm is QuadEngine)
                {
                    engine = (QuadEngine)pm;
                }
            }
        }

        public void SetThrottle(float value)
        {
            engine.SetThrottle(value);
        }
    }
}
