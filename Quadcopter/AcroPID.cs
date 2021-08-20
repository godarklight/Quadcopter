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
        //private QuadPID pidYaw;
        private double pitchSetting;
        private double rollSetting;
        //private double yawSetting;

        public AcroPID(double rollRate, Vehicle vehicle)
        {
            this.rollRate = rollRate * Mathf.Deg2Rad;
            this.vehicle = vehicle;
            //FSControlUtil.Get* is radians
            pidPitch = new QuadPID(1, 0.1, .5, -1, 1, () => { return FSControlUtil.GetVehiclePitch(vehicle); }, () => { return pitchSetting; }, () => { return Time.fixedTime; }, null);
            pidRoll = new QuadPID(1, 0.1, .5, -1, 1, () => { return FSControlUtil.GetVehicleRoll(vehicle); }, () => { return rollSetting; }, () => { return Time.fixedTime; }, null);
            //pidYaw = new QuadPID(1, 0, 0, -1, 1, () => { return FSControlUtil.GetVehicleYaw(vehicle); }, () => { return yawSetting; }, () => { return Time.fixedTime; }, null);
        }

        public void FixedUpdate(Quaternion targetRotation)
        {
            pitchSetting = pitchSetting + (InputSettings.Axis_Pitch.GetAxis() * rollRate * Time.deltaTime);
            rollSetting = rollSetting + (InputSettings.Axis_Roll.GetAxis() * rollRate * Time.deltaTime);
            //yawSetting = yawSetting + (InputSettings.Axis_Yaw.GetAxis() * rollRate * Time.deltaTime);
            pitchSetting = Mathf.Clamp((float)pitchSetting, -1, 1);
            rollSetting = Mathf.Clamp((float)rollSetting, -1, 1);
            //yawSetting = Mathf.Clamp((float)yawSetting, -1, 1);
            DebugScreen.SetSetting(pitchSetting, rollSetting, InputSettings.Axis_Yaw.GetAxis());
            pidPitch.FixedUpdate();
            pidRoll.FixedUpdate();
            //pidYaw.FixedUpdate();
        }

        public void SetMotors(QuadMotor[] motors, float throttle)
        {
            QuadMotor motorFL = motors[0];
            QuadMotor motorFR = motors[1];
            QuadMotor motorRL = motors[2];
            QuadMotor motorRR = motors[3];
            float pitchOffset = 0.2f * (float)pidPitch.outputValue;
            float rollOffset = 0.2f * (float)pidRoll.outputValue;
            //float yawOffset = 0.2f * (float)pidYaw.outputValue;
            float yawOffset = 0.2f * InputSettings.Axis_Yaw.GetAxis();
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
            DebugScreen.SetThrottles(targetFL, targetFR, targetRL, targetRR);
            DebugScreen.SetPower(motorFL.ShaftPower, motorFR.ShaftPower, motorRL.ShaftPower, motorRR.ShaftPower);
            motorFL.SetThrottle(targetFL);
            motorFR.SetThrottle(targetFR);
            motorRL.SetThrottle(targetRL);
            motorRR.SetThrottle(targetRR);
        }
    }
}
