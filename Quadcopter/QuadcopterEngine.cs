using System;
using Modules;
namespace Quadcopter
{
    public class QuadcopterEngine : Engine
    {
        public bool modOverride = false;
        public new void OnReceiveCtrlState(FSInputState data)
        {
            if (!modOverride)
            {
                SetThrottle(data.throttle);
            }
        }
    }
}
