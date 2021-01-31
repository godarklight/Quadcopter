using System;
using UnityEngine;

namespace Quadcopter
{
    public class QuadModule : MonoBehaviour
    {
        //Standard configuration
        private Vehicle vehicle;
        private QuadMotor[] motors = new QuadMotor[4];
        private AcroPID acroPID;
        private Quaternion targetRot;

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
            acroPID = new AcroPID(50, vehicle);
            targetRot = Quaternion.identity;
        }

        public void FixedUpdate()
        {
            if (vehicle == null)
            {
                QuadLog.Debug("Fixed update: Vessel is null");
                Destroy(this);
                return;
            }
            if (!vehicle.isActiveAndEnabled)
            {
                QuadLog.Debug("Fixed update: Vessel is not active and enabled");
                Destroy(this);
                return;
            }
            if (vehicle.Autotrim.autoTrimEnabled)
            {
                QuadLog.Debug("Fixed update: Cannot use autotrim with quadcopters");
                ScreenMessages.PostScreenMessage("Cannot use autotrim with quadcopters");
                vehicle.Autotrim.DisableAT();
            }
            acroPID.FixedUpdate(targetRot);
            acroPID.SetMotors(motors, InputSettings.Axis_Throttle.GetAxis());
        }
    }
}
