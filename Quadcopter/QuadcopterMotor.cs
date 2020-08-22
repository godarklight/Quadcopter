using System;
using Modules;

namespace Quadcopter
{
    public class QuadcopterMotor
    {
        public Part part;
        public QuadcopterEngine engine;

        public QuadcopterMotor(Part p)
        {
            this.part = p;
            foreach (QuadcopterEngine pm in p.Modules)
            {
                if (pm is QuadcopterEngine)
                {
                    engine = (QuadcopterEngine)pm;
                }
            }
        }

        public void SetThrottle(float value)
        {
            engine.SetThrottle(value);
        }
    }
}
