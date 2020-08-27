using System;
using Modules;
namespace Quadcopter
{
    public class QuadEngine : Engine
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
