using System;
using UnityEngine;

namespace Quadcopter
{
    public class QuadModule : MonoBehaviour
    {
        //Standard configuration
        private Vehicle vehicle;
        private QuadMotor[] motors = new QuadMotor[4];

        public void Setup(Vehicle vehicle, Part motorFL, Part motorFR, Part motorRL, Part motorRR)
        {
            this.vehicle = vehicle;
            if (this.vehicle.Autotrim.autoTrimEnabled)
            {
                this.vehicle.Autotrim.DisableAT();
            }
            motors[0] = new QuadMotor(motorFL);
            motors[1] = new QuadMotor(motorFR);
            motors[2] = new QuadMotor(motorRL);
            motors[3] = new QuadMotor(motorRR);
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
