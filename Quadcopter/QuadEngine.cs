// Modules.Engine
using BalsaCore.FX;
using CfgFields;
using Dev;
using FSResources;
using Modules;
using Network;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Vehicles.Logging;

namespace Quadcopter
{

    public class QuadEngine : Engine, ICtrlReceiver, IPart, IActiveFlow, INetworkFlow, IEngine
    {
        public bool modOverride;
        public new void OnReceiveCtrlState(FSInputState data)
        {
            if (!modOverride)
            {
                SetThrottle(data.throttle);
            }
        }
    }
}