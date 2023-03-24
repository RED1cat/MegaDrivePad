using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xna
{
    public static class GamepadManager
    {
        public static GamepadUtil GhostPad = new GamepadUtil(GamepadMode.Ghost);
        public static GamepadUtil CatPad = new GamepadUtil(GamepadMode.Cat);
        public static GamepadUtil GenesisPad = new GamepadUtil(GamepadMode.Genesis);
        public static GamepadUtil GenesisPad2 = new GamepadUtil(GamepadMode.Genesis);
        public static GamepadUtil VisualPadOne = new GamepadUtil(GamepadMode.Genesis);
        public static GamepadUtil VisualPadTwo = new GamepadUtil(GamepadMode.Genesis);
        public static string MappingLog = "";
        public static bool Swapped = false;

        public static void FinishMapping()
        {
            MappingLog = "";
        }
        public static void AddMapingLog(string Msg)
        {
            MappingLog += "\n" + Msg;
        }

        public static void Search()
        {
            for (int i = 0; i < GamePad.MaximumGamePadCount; i++)
            {
                if (GamePad.GetState(i).IsConnected && GamePad.GetState(i).Buttons.Start == ButtonState.Pressed)
                {
                    if(SettingsMenu.CurrentSetting == 4)
                    {
                        GhostPad.DeviceID = i;
                    }
                    else if (SettingsMenu.CurrentSetting == 5)
                    {
                        CatPad.DeviceID = i;
                    }
                    else if (SettingsMenu.CurrentSetting == 6)
                    {
                        GenesisPad.DeviceID = i;
                        
                        if (!GenesisPad.Remapping)
                        {
                            GenesisPad.Remapping = true;
                            AddMapingLog("Press button for A");
                        }
                    } else if (SettingsMenu.CurrentSetting == 7)
                    {
                        GenesisPad2.DeviceID = i;

                        if (!GenesisPad2.Remapping)
                        {
                            GenesisPad2.Remapping = true;
                            AddMapingLog("Press button for A");
                        }
                    } else if (SettingsMenu.CurrentSetting == 8)
                    {
                        VisualPadOne.DeviceID = i;

                        if (!VisualPadOne.Remapping)
                        {
                            VisualPadOne.Remapping = true;
                            AddMapingLog("Press button for A");
                        }
                        Game1.VisualizedPad1.Pad = VisualPadOne;
                    }
                    else if (SettingsMenu.CurrentSetting == 9)
                    {
                        VisualPadTwo.DeviceID = i;

                        if (!VisualPadTwo.Remapping)
                        {
                            VisualPadTwo.Remapping = true;
                            AddMapingLog("Press button for A");
                        }
                        Game1.VisualizedPad2.Pad = VisualPadTwo;
                    }
                }
            }
        }

        public static void Swap()
        {
            Swapped = !Swapped;
        }
        public static void Update(GameTime gameTime)
        {
            Search();

            GhostPad.Update(gameTime);
            CatPad.Update(gameTime);
            GenesisPad.Update(gameTime);
            GenesisPad2.Update(gameTime);
            VisualPadOne.Update(gameTime);
            VisualPadTwo.Update(gameTime);
        }
    }
}
