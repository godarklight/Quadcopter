using System;
using System.Collections.Generic;
using Modules;
using UnityEngine;
using GameEventsData;


namespace Quadcopter
{
    public class QuadcopterMain : MonoBehaviour
    {
        public void Start()
        {
            DontDestroyOnLoad(this);
            GameEvents.Game.OnSceneTransitionComplete.AddListener(PowerOnSelfTest);
            GameEvents.Vehicles.OnVehicleSpawned.AddListener(AttachModuleToSelf);
        }

        private void PowerOnSelfTest(FromToAction<GameScenes> scene)
        {
            string t1String = "Modules.Engine";
            string t2String = "Modules.Engine, Assembly-CSharp";
            Type t1 = Type.GetType(t1String);
            Type t2 = Type.GetType(t2String);
            QuadcopterLog.Debug($"{t1String} OK? {(t1 != null).ToString()}");
            QuadcopterLog.Debug($"{t1String} OK? {(t2 != null).ToString()}");
        }

        private void AttachModuleToSelf(Vehicle v)
        {
            QuadcopterLog.Debug($"{v.name} spawned");
            if (v.IsLocalPlayerVehicle && !v.IsEditing)
            {
                QuadcopterLog.Debug($"{v.name} is our vechile, checking motor setup");
                List<Part> motorParts = new List<Part>();
                foreach (Part p in v.parts)
                {
                    bool partIsMotor = false;
                    foreach (PartModule pm in p.Modules)
                    {
                        if (pm is QuadcopterEngine)
                        {
                            partIsMotor = true;
                            break;
                        }
                    }
                    if (partIsMotor)
                    {
                        motorParts.Add(p);
                    }
                }
                if (motorParts.Count == 4)
                {
                    QuadcopterModule qm = CheckMotorSetup(motorParts, v);
                    if (qm != null)
                    {
                        ScreenMessages.PostScreenMessage("Quadcopter OK!");
                        QuadcopterLog.Debug($"Quadcopter OK!");
                    }
                    else
                    {
                        QuadcopterLog.Debug($"Vechile has 4 motors but an incorrect configuration");
                    }
                }
                else
                {
                    QuadcopterLog.Debug($"Vessel does not have 4 motors");
                }
            }
        }

        private QuadcopterModule CheckMotorSetup(List<Part> motorParts, Vehicle v)
        {
            Vector3d CoM = FindVehicleCoM(v);
            QuadcopterLog.Debug($"Vehicle CoM is: {CoM}");
            Part motorFL = null;
            Part motorFR = null;
            Part motorRL = null;
            Part motorRR = null;
            foreach (Part p in motorParts)
            {
                if (p.physics == null)
                {
                    QuadcopterLog.Debug("Motor is physicless, not valid.");
                    return null;
                }
                Vector3 partCoM = p.p0;
                QuadcopterLog.Debug($"Part CoM is: {partCoM}");
                bool negativeX = false;
                bool negativeZ = false;
                if (partCoM.x < CoM.x)
                {
                    negativeX = true;
                }
                if (partCoM.z < CoM.z)
                {
                    negativeZ = true;
                }
                //Front left
                if (negativeX && !negativeZ)
                {
                    if (motorFL == null)
                    {
                        motorFL = p;
                    }
                    else
                    {
                        QuadcopterLog.Debug("Front left motor already assigned, not valid.");
                        return null;
                    }
                }
                //Front right
                if (!negativeX && !negativeZ)
                {
                    if (motorFR == null)
                    {
                        motorFR = p;
                    }
                    else
                    {
                        QuadcopterLog.Debug("Front right motor already assigned, not valid.");
                        return null;
                    }
                }
                //Rear left
                if (negativeX && negativeZ)
                {
                    if (motorRL == null)
                    {
                        motorRL = p;
                    }
                    else
                    {
                        QuadcopterLog.Debug("Rear left motor already assigned, not valid.");
                        return null;
                    }
                }
                //Rear right
                if (!negativeX && negativeZ)
                {
                    if (motorRR == null)
                    {
                        motorRR = p;
                    }
                    else
                    {
                        QuadcopterLog.Debug("Rear right motor already assigned, not valid.");
                        return null;
                    }
                }
            }
            GameObject go = new GameObject();
            QuadcopterModule qm = go.AddComponent<QuadcopterModule>();
            DontDestroyOnLoad(qm);
            qm.Setup(v, motorFL, motorFR, motorRL, motorRR);
            return qm;
        }

        private Vector3 FindVehicleCoM(Vehicle v)
        {
            if (v.Physics != null)
            {
                return v.Physics.GetCoM();
            }
            return Vector3.zero;
        }
    }
}
