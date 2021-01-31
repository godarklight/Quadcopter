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
        private static bool textureOK = true;

        public static void SetValues(float FL, float FR, float RL, float RR)
        {
            DebugScreen.FL = FL * 100;
            DebugScreen.FR = FR * 100;
            DebugScreen.RL = RL * 100;
            DebugScreen.RR = RR * 100;
            textureOK = false;
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
            drawTexture.Apply();
        }

        private void DrawContent(int windowID)
        {
            GUILayout.BeginVertical();
            GUI.DragWindow(new Rect(0, 0, 10000, 20));
            GUILayout.Box(drawTexture);
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
