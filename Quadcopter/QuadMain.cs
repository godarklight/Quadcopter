using System;
using System.Collections.Generic;
using Modules;
using UnityEngine;
using GameEventsData;


namespace Quadcopter
{
    public class QuadMain : MonoBehaviour
    {
        public void Start()
        {
            DontDestroyOnLoad(this);
            GameEvents.Vehicles.OnVehicleSpawned.AddListener(AttachModuleToSelf);
        }

        private void AttachModuleToSelf(Vehicle v)
        {
            QuadLog.Debug($"{v.name} spawned");
            if (v.IsLocalPlayerVehicle && !v.IsEditing)
            {
                QuadLog.Debug($"{v.name} is our vechile, checking motor setup");
                List<Part> motorParts = new List<Part>();
                foreach (Part p in v.parts)
                {
                    bool partIsMotor = false;
                    foreach (PartModule pm in p.Modules)
                    {
                        if (pm is QuadEngine)
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
                    QuadModule qm = CheckMotorSetup(motorParts, v);
                    if (qm != null)
                    {
                        ScreenMessages.PostScreenMessage("Quadcopter OK!");
                        QuadLog.Debug($"Quadcopter OK!");
                    }
                    else
                    {
                        QuadLog.Debug($"Vechile has 4 motors but an incorrect configuration");
                    }
                }
                else
                {
                    QuadLog.Debug($"Vessel does not have 4 motors");
                }
            }
        }

        private QuadModule CheckMotorSetup(List<Part> motorParts, Vehicle v)
        {
            Vector3d CoM = FindVehicleCoM(v);
            QuadLog.Debug($"Vehicle CoM is: {CoM}");
            Part motorFL = null;
            Part motorFR = null;
            Part motorRL = null;
            Part motorRR = null;
            foreach (Part p in motorParts)
            {
                if (p.physics == null)
                {
                    QuadLog.Debug("Motor is physicless, not valid.");
                    return null;
                }
                Vector3 partCoM = p.p0;
                QuadLog.Debug($"Part CoM is: {partCoM}");
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
                        QuadLog.Debug("Front left motor already assigned, not valid.");
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
                        QuadLog.Debug("Front right motor already assigned, not valid.");
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
                        QuadLog.Debug("Rear left motor already assigned, not valid.");
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
                        QuadLog.Debug("Rear right motor already assigned, not valid.");
                        return null;
                    }
                }
            }
            GameObject go = new GameObject();
            QuadModule qm = go.AddComponent<QuadModule>();
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
