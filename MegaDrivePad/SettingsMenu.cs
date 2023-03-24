using CSCore.SoundIn;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xna
{
    internal class SettingsMenu
    {
        private static bool IgnoreTabInput = false;
        private static bool IgnoreRightInput = false;
        private static bool IgnoreLeftInput = false;
        private static bool IgnoreDownInput = false;
        private static bool IgnoreUpInput = false;
        public static int CurrentSetting = 0;
        public static bool SettingSwap = false;
        public static int IgnoreDiscordPid = -1;

        public static void KeyboardUpdate()
        {
            bool HoldTab = Keyboard.GetState().IsKeyDown(Keys.Tab);
            bool HolRight = Keyboard.GetState().IsKeyDown(Keys.Right);
            bool HoldLeft = Keyboard.GetState().IsKeyDown(Keys.Left);
            bool HoldDown = Keyboard.GetState().IsKeyDown(Keys.Down);
            bool HoldUp = Keyboard.GetState().IsKeyDown(Keys.Up);

            if (HoldTab && !IgnoreTabInput)
            {
                IgnoreTabInput = true;
                CurrentSetting = CurrentSetting != 0 ? 0 : 1;
            }
            if (!HoldTab)
            {
                IgnoreTabInput = false;
            }

            if (HoldDown && !IgnoreDownInput)
            {
                IgnoreDownInput = true;
                SettingDown();
            }
            if (!HoldDown)
            {
                IgnoreDownInput = false;
            }
            if (HoldUp && !IgnoreUpInput)
            {
                IgnoreUpInput = true;
                SettingUp();
            }
            if (!HoldUp)
            {
                IgnoreUpInput = false;
            }

            if (HolRight && !IgnoreRightInput)
            {
                IgnoreRightInput = true;
                SettingRight();
            }
            if (!HolRight)
            {
                IgnoreRightInput = false;
            }

            if (HoldLeft && !IgnoreLeftInput)
            {
                IgnoreLeftInput = true;
                SettingLeft();
            }
            if (!HoldLeft)
            {
                IgnoreLeftInput = false;
            }
        }
        public static void SettingDown()
        {
            CurrentSetting++;
            if (CurrentSetting == 10)
            {
                CurrentSetting = 1;
            }
        }
        public static void SettingUp()
        {
            CurrentSetting--;
            if (CurrentSetting == 0)
            {
                CurrentSetting = 1;
            }
        }
        public static void SettingRight()
        {
            if (CurrentSetting == 1)
            {
                SettingSwap = !SettingSwap;
            }
            else if (CurrentSetting == 2)
            {
                AudioInput.MicMinPeak += AudioInput.MicPeakInterval;
            }
            else if (CurrentSetting == 3)
            {
                SetIgnorePid();
            }
        }
        public static void SettingLeft()
        {
            if (CurrentSetting == 1)
            {
                SettingSwap = !SettingSwap;
            }
            else if (CurrentSetting == 2)
            {
                AudioInput.MicMinPeak -= AudioInput.MicPeakInterval;
            }
            else if (CurrentSetting == 3)
            {
                SetIgnorePid();
            }
        }

        public static void SetIgnorePid()
        {
            IgnoreDiscordPid = AudioAppInput.CurrentPID;
            if (AudioAppInput.DiscordSession != null)
            {
                AudioAppInput.DiscordSession.Dispose();
                AudioAppInput.DiscordSession = null;
            }
        }

        public static string IsCurrentSetting(int Setting)
        {
            return Setting == CurrentSetting ? "> " : " ";
        }

        public static void Render(SpriteBatch _spriteBatch)
        {
            string Text = "";

            if (CurrentSetting != 0)
            {
                _spriteBatch.Begin();
                bool Remapping = GamepadManager.GenesisPad.Remapping || GamepadManager.VisualPadOne.Remapping || GamepadManager.VisualPadTwo.Remapping || GamepadManager.GenesisPad2.Remapping;
                int H = 350;
                if (Remapping)
                {
                    H = 450;
                }
                
                
                _spriteBatch.Draw(Game1.DummyBG, new Vector2(Game1.DebugText.Position.X - 20, Game1.DebugText.Position.Y - 5), new Rectangle(0, 0, 400, H), Color.White);

                _spriteBatch.End();

                string AudioMd = !SettingSwap ? "Ghost away; Cat home" : "Cat away; Ghost home";
                Text = "Audio\n\n" +
                       IsCurrentSetting(1) + "Audio Mode: " + AudioMd + "\n" +
                       IsCurrentSetting(2) + "Mic Sensitivity: " + AudioInput.MicMinPeak + "\n" +
                       IsCurrentSetting(3) + "Discord Pid: " + AudioAppInput.CurrentPID + "\n" +
                       "\nGamepads\n\n" +
                       IsCurrentSetting(4) + "Gamepad Ghost: " + (GamepadManager.GhostPad.DeviceID == -1 ? "Non" : GamepadManager.GhostPad.DeviceID) + "\n" +
                       IsCurrentSetting(5) + "Gamepad Cat: " + (GamepadManager.CatPad.DeviceID == -1 ? "Non" : GamepadManager.CatPad.DeviceID) + "\n" +
                       IsCurrentSetting(6) + "Gamepad For Genesis: " + (GamepadManager.GenesisPad.DeviceID == -1 ? "Non" : GamepadManager.GenesisPad.DeviceID) + "\n" +
                       IsCurrentSetting(7) + "Gamepad For Genesis2: " + (GamepadManager.GenesisPad.DeviceID == -1 ? "Non" : GamepadManager.GenesisPad.DeviceID) + "\n" +
                       IsCurrentSetting(8) + "Gamepad Vizualizer 1: " + (GamepadManager.VisualPadOne.DeviceID == -1 ? "Non" : GamepadManager.VisualPadOne.DeviceID) + "\n" +
                       IsCurrentSetting(9) + "Gamepad Vizualizer 2: " + (GamepadManager.VisualPadTwo.DeviceID == -1 ? "Non" : GamepadManager.VisualPadTwo.DeviceID) + "\n" +
                       "\n" +

                "Info\n\n" +
                       "Last Voice Peak: " + AudioInput.LastPeak + "\n" +
                       "SWAP " + GamepadUtil.Conv(GamepadManager.Swapped) +"          UDLRABCXYZSM\n" + 
                       "Genesis PORT 1: " + (!GamepadManager.Swapped ? GamepadManager.GenesisPad.Inputs : GamepadManager.GenesisPad2.Inputs) + "\n" +
                       "Genesis PORT 2: " + (!GamepadManager.Swapped ? GamepadManager.GenesisPad2.Inputs : GamepadManager.GenesisPad.Inputs) + "\n"
                       ;


                if (Remapping)
                {
                    Text = "Mapping Genesis Pad\n\n" + GamepadManager.MappingLog;
                }


                Game1.DebugText.SetText(Text);
                Game1.DebugText.Render(_spriteBatch);
            }
        }
    }
}
