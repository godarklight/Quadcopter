﻿using System;
using UnityEngine;

namespace Quadcopter
{
    public class QuadPID
    {
        public double kP;
        public double kI;
        public double kD;
        public double rangeMin;
        public double rangeMax;
        public Func<double> input;
        public Func<double> setpoint;
        public Func<double> clockSource;
        public Action<double> output;
        public bool enabled
        {
            get;
            private set;
        }
        public double error
        {
            get;
            private set;
        }
        public double p
        {
            get;
            private set;
        }
        public double i
        {
            get;
            private set;
        }
        public double d
        {
            get;
            private set;
        }
        public double outputValue
        {
            get;
            private set;
        }
        public double lastInput
        {
            get;
            private set;
        }
        public double lastTime
        {
            get;
            private set;
        }

        public QuadPID(double kP, double kI, double kD, double rangeMin, double rangeMax, Func<double> input, Func<double> setpoint, Func<double> clockSource, Action<double> output)
        {
            this.kP = kP;
            this.kI = kI;
            this.kD = kD;
            this.rangeMin = rangeMin;
            this.rangeMax = rangeMax;
            this.input = input;
            this.clockSource = clockSource;
            this.setpoint = setpoint;
            this.output = output;
            this.enabled = true;
        }

        public void FixedUpdate()
        {
            if (!enabled)
            {
                return;
            }
            //Gather inputs
            double currentTime = clockSource();
            double currentInput = input();
            double currentSetpoint = setpoint();
            error = currentSetpoint - currentInput;
            double deltaInput = lastInput - currentInput;
            double deltaTime = currentTime - lastTime;
            //Return if we get called twice on the same frame
            if ((currentTime - lastTime) < double.Epsilon)
            {
                return;
            }
            //PID calculation
            p = error * kP;
            i += error * kI * deltaTime;
            d = (deltaInput * kD) / deltaTime;
            //Clamp I
            if (i < rangeMin)
            {
                i = rangeMin;
            }
            if (i > rangeMax)
            {
                i = rangeMax;
            }
            outputValue = p + i + d;
            //Clamp output
            if (outputValue < rangeMin)
            {
                outputValue = rangeMin;
            }
            if (outputValue > rangeMax)
            {
                outputValue = rangeMax;
            }
            //Call output delegate if it exists
            output?.Invoke(outputValue);
            //Save the state for derivative calculation
            lastTime = currentTime;
            lastInput = currentInput;
        }

        public void Enable()
        {
            //Zero out D
            lastInput = input();
            enabled = true;
        }

        /// <summary>
        /// If switching from manual to automatic control, this will set the PID steady state value
        /// </summary>
        /// <param name="integrator">Integrator.</param>
        public void Enable(double integrator)
        {
            //Set the steady state
            this.i = integrator;
            Enable();
        }

        public void Disable()
        {
            enabled = false;
        }
    }
}
