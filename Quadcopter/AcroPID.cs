using System;
using UnityEngine;
using DirectLineRendering;
using FSControl;

namespace Quadcopter
{
    public class AcroPID
    {
        private Vehicle vehicle;
        private double rollRate;
        private QuadPID pidPitch;
        private QuadPID pidRoll;
        private QuadPID pidYaw;

        public AcroPID(double rollRate, Vehicle vehicle)
        {
            this.rollRate = rollRate;
            this.vehicle = vehicle;
            //FSControlUtil.Get* is radians
            pidPitch = new QuadPID(1, 0, .1, -1, 1, () => { return FSControlUtil.GetVehiclePitch(vehicle); }, () => { return 0d; }, () => { return Time.fixedTime; }, null);
            pidRoll = new QuadPID(1, 0, .1, -1, 1, () => { return FSControlUtil.GetVehicleRoll(vehicle); }, () => { return 0d; }, () => { return Time.fixedTime; }, null);
            pidYaw = new QuadPID(1, 0, .1, -1, 1, () => { return FSControlUtil.GetVehicleYaw(vehicle); }, () => { return 0d; }, () => { return Time.fixedTime; }, null);
        }

        public void FixedUpdate(Quaternion targetRotation)
        {
            pidPitch.FixedUpdate();
            pidRoll.FixedUpdate();
            pidYaw.FixedUpdate();
        }

        public void SetMotors(QuadMotor[] motors, float throttle)
        {
            QuadMotor motorFL = motors[0];
            QuadMotor motorFR = motors[1];
            QuadMotor motorRL = motors[2];
            QuadMotor motorRR = motors[3];
            float pitchOffset = 0.5f * (float)pidPitch.outputValue;
            float rollOffset = 0.5f * (float)pidRoll.outputValue;
            float yawOffset = 0f * (float)pidYaw.outputValue;
            float targetFL = throttle * (1f + pitchOffset + rollOffset - yawOffset);
            float targetFR = throttle * (1f + pitchOffset - rollOffset + yawOffset);
            float targetRL = throttle * (1f - pitchOffset + rollOffset + yawOffset);
            float targetRR = throttle * (1f - pitchOffset - rollOffset - yawOffset);
            if (targetFL > 1)
            {
                float divideFactor = targetFL;
                targetFL = targetFL / divideFactor;
                targetFR = targetFR / divideFactor;
                targetRL = targetRL / divideFactor;
                targetRR = targetRR / divideFactor;
            }
            if (targetFR > 1)
            {
                float divideFactor = targetFR;
                targetFL = targetFL / divideFactor;
                targetFR = targetFR / divideFactor;
                targetRL = targetRL / divideFactor;
                targetRR = targetRR / divideFactor;
            }
            if (targetRL > 1)
            {
                float divideFactor = targetRL;
                targetFL = targetFL / divideFactor;
                targetFR = targetFR / divideFactor;
                targetRL = targetRL / divideFactor;
                targetRR = targetRR / divideFactor;
            }
            if (targetRR > 1)
            {
                float divideFactor = targetRR;
                targetFL = targetFL / divideFactor;
                targetFR = targetFR / divideFactor;
                targetRL = targetRL / divideFactor;
                targetRR = targetRR / divideFactor;
            }
            targetFL = Mathf.Clamp01(targetFL);
            targetFR = Mathf.Clamp01(targetFR);
            targetRL = Mathf.Clamp01(targetRL);
            targetRR = Mathf.Clamp01(targetRR);         
            DebugScreen.SetValues(targetFL, targetFR, targetRL, targetRR);
            motorFL.SetThrottle(targetFL);
            motorFR.SetThrottle(targetFR);
            motorRL.SetThrottle(targetRL);
            motorRR.SetThrottle(targetRR);
        }
    }
}
