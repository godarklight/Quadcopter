using System;
using BalsaCore;
using UnityEngine;
namespace Quadcopter
{
    public class DebugScreen : MonoBehaviour
    {
        private bool init = false;
        private Rect windowRect;
        private Texture2D drawTexture;
        private static float FL;
        private static float FR;
        private static float RL;
        private static float RR;
        private static double FLpower;
        private static double FRpower;
        private static double RLpower;
        private static double RRpower;
        private static double pitch;
        private static double roll;
        private static double yaw;


        private static bool textureOK = true;

        public static void SetThrottles(float FL, float FR, float RL, float RR)
        {
            DebugScreen.FL = FL * 100;
            DebugScreen.FR = FR * 100;
            DebugScreen.RL = RL * 100;
            DebugScreen.RR = RR * 100;
            textureOK = false;
        }
        public static void SetPower(double FL, double FR, double RL, double RR)
        {
            DebugScreen.FLpower = FL / 320f;
            DebugScreen.FRpower = FR / 320f;
            DebugScreen.RLpower = RL / 320f;
            DebugScreen.RRpower = RR / 320f;
            textureOK = false;
        }

        public static void SetSetting(double pitch, double roll, double yaw)
        {
            DebugScreen.pitch = pitch;
            DebugScreen.roll = roll;
            DebugScreen.yaw = yaw;
        }

        private void OnGUI()
        {
            Draw();
        }

        private void Init()
        {
            init = true;
            drawTexture = new Texture2D(50, 205);
            windowRect = new Rect(70, 250, drawTexture.width + 50, drawTexture.height + 50);
            for (int x = 0; x < drawTexture.width; x++)
            {
                for (int y = 0; y < drawTexture.height; y++)
                {
                    drawTexture.SetPixel(x, y, Color.black);
                }
            }
            drawTexture.Apply();
        }

        public void Draw()
        {
            if (!init)
            {
                Init();
            }
            if (!textureOK)
            {
                DrawTexture();
            }
            windowRect = PreventOffscreenWindow(GUILayout.Window(2348589, windowRect, DrawContent, "Debug"));
        }

        private void DrawTexture()
        {
            textureOK = true;
            //FL RL
            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    if (y < RL)
                    {
                        drawTexture.SetPixel(x, y, Color.green);
                    }
                    else
                    {
                        drawTexture.SetPixel(x, y, Color.black);
                    }
                }
                for (int y = 105; y < 205; y++)
                {
                    if ((y - 105) < FL)
                    {
                        drawTexture.SetPixel(x, y, Color.green);
                    }
                    else
                    {
                        drawTexture.SetPixel(x, y, Color.black);
                    }
                }
            }
            //FR RR
            for (int x = 25; x < 45; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    if (y < RR)
                    {
                        drawTexture.SetPixel(x, y, Color.green);
                    }
                    else
                    {
                        drawTexture.SetPixel(x, y, Color.black);
                    }
                }
                for (int y = 105; y < 205; y++)
                {
                    if ((y - 105) < FR)
                    {
                        drawTexture.SetPixel(x, y, Color.green);
                    }
                    else
                    {
                        drawTexture.SetPixel(x, y, Color.black);
                    }
                }
            }
            //Power
            for (int x = 0; x < 20; x++)
            {
                //FL
                drawTexture.SetPixel(x, 105 + (int)(FLpower * 100), Color.red);
                //FR
                drawTexture.SetPixel(x + 25, 105 + (int)(FRpower * 100), Color.red);
                //RL
                drawTexture.SetPixel(x, (int)(RLpower * 100), Color.red);
                //RR
                drawTexture.SetPixel(x + 25, (int)(RRpower * 100), Color.red);
            }
            drawTexture.Apply();
        }

        private void DrawContent(int windowID)
        {
            GUILayout.BeginVertical();
            GUI.DragWindow(new Rect(0, 0, 10000, 20));
            GUILayout.Box(drawTexture);
            GUILayout.Label("Pitch: " + pitch);
            GUILayout.Label("Roll: " + roll);
            GUILayout.Label("Yaw: " + yaw);
            GUILayout.EndVertical();
        }

        public Rect PreventOffscreenWindow(Rect input)
        {
            int xMax = Screen.width - (int)input.width;
            int yMax = Screen.height - (int)input.height;
            if (input.x < 0)
            {
                input.x = 0;
            }
            if (input.y < 0)
            {
                input.y = 0;
            }
            if (input.x > xMax)
            {
                input.x = xMax;
            }
            if (input.y > yMax)
            {
                input.y = yMax;
            }
            return input;
        }
    }
}
