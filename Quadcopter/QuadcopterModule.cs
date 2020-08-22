using System;
using UnityEngine;

namespace Quadcopter
{
    public class QuadcopterModule : MonoBehaviour
    {
        //Standard configuration
        private Vehicle vehicle;
        private QuadcopterMotor[] motors = new QuadcopterMotor[4];

        public void Setup(Vehicle vehicle, Part motorFL, Part motorFR, Part motorRL, Part motorRR)
        {
            this.vehicle = vehicle;
            if (this.vehicle.Autotrim.autoTrimEnabled)
            {
                this.vehicle.Autotrim.DisableAT();
            }
            motors[0] = new QuadcopterMotor(motorFL);
            motors[1] = new QuadcopterMotor(motorFR);
            motors[2] = new QuadcopterMotor(motorRL);
            motors[3] = new QuadcopterMotor(motorRR);
        }

        public void FixedUpdate()
        {
            if (vehicle == null)
            {
                Destroy(this);
                return;
            }
            if (!vehicle.isActiveAndEnabled)
            {
                Destroy(this);
                return;
            }
            if (vehicle.Autotrim.autoTrimEnabled)
            {
                ScreenMessages.PostScreenMessage("Cannot use autotrim with quadcopters");
                vehicle.Autotrim.DisableAT();
            }
            //Testing
            motors[0].SetThrottle(0.25f);
            motors[1].SetThrottle(0.5f);
            motors[2].SetThrottle(0.75f);
            motors[3].SetThrottle(1f);
        }
    }
}
