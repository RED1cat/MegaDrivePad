using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace xna
{
    public enum GamepadMode
    {
        Ghost,
        Cat,
        Genesis,
    }
    
    
    public class GamepadUtil
    {
        public int DeviceID = -1;
        GamePadState buttons;
        GamePadState buttonsPrevious;
        GamepadMode Mode;


        public string Inputs = "000000000000";
        string SegaABind = "X";
        string SegaBBind = "B";
        string SegaCBind = "A";
        string SegaXBind = "L";
        string SegaYBind = "Y";
        string SegaZBind = "R";
        string SegaModeBind = "LT";
        string SwapButton = "BACK";

        public int MappingID = 0;
        public bool Remapping = false;

        public GamepadUtil(GamepadMode _Mode)
        {
            Mode = _Mode;
        }

        bool A = false;
        bool B = false;
        bool X = false;
        bool Y = false;
        bool L = false;
        bool R = false;
        bool LT = false;
        bool RT = false;
        bool BACK = false;

        public bool Start = false;

        public bool Left = false;
        public bool Right = false;
        public bool Up = false;
        public bool Down = false;

        bool PreviousStickLeft = false;
        bool PreviousStickRight = false;
        bool PreviousStickUp = false;
        bool PreviousStickDown = false;
        public TimeSpan SwapLastRelease = TimeSpan.Zero;

        public void AnyButtonPressed()
        {
            if(Mode == GamepadMode.Ghost)
            {
                Game1.Ghost.PressButton();
            }else if(Mode == GamepadMode.Cat)
            {
                Game1.Cat.PressButton();
            }
        }

        public bool IsSegaButtonHeldByName(string Button)
        {
            if(Button == "A")
            {
                return IsButtonHeldByName(SegaABind);
            }else if(Button == "B")
            {
                return IsButtonHeldByName(SegaBBind);
            }else if(Button == "C")
            {
                return IsButtonHeldByName(SegaCBind);
            }else if(Button == "X")
            {
                return IsButtonHeldByName(SegaXBind);
            }
            else if (Button == "Y")
            {
                return IsButtonHeldByName(SegaYBind);
            }
            else if (Button == "Z")
            {
                return IsButtonHeldByName(SegaZBind);
            } else if (Button == "MODE")
            {
                return IsButtonHeldByName(SegaModeBind);
            } else if (Button == "SWAP")
            {
                return IsButtonHeldByName(SwapButton);
            }
            return false;
        }

        public bool IsButtonHeldByName(string Button)
        {
            if (Button == "A")
            {
                return A;
            }
            else if (Button == "B")
            {
                return B;
            }
            else if (Button == "X")
            {
                return X;
            }
            else if (Button == "Y")
            {
                return Y;
            }
            else if (Button == "L")
            {
                return L;
            }
            else if (Button == "R")
            {
                return R;
            } else if (Button == "LT")
            {
                return LT;
            } else if (Button == "RT")
            {
                return RT;
            } else if (Button == "BACK")
            {
                return BACK;
            }
            return false;
        }

        public void Map(string Button)
        {
            if (!Remapping)
            {
                return;
            }

            if (MappingID == 0)
            {
                SegaABind = Button;
                GamepadManager.AddMapingLog("Genesis A mapped to " + Button);
                GamepadManager.AddMapingLog("\nPress button for B");
            }
            else if (MappingID == 1)
            {
                SegaBBind = Button;
                GamepadManager.AddMapingLog("Genesis B mapped to " + Button);
                GamepadManager.AddMapingLog("\nPress button for C");
            }
            else if (MappingID == 2)
            {
                SegaCBind = Button;
                GamepadManager.AddMapingLog("Genesis C mapped to " + Button);
                GamepadManager.AddMapingLog("\nPress button for X");
            }
            else if (MappingID == 3)
            {
                SegaXBind = Button;
                GamepadManager.AddMapingLog("Genesis X mapped to " + Button);
                GamepadManager.AddMapingLog("\nPress button for Y");
            }
            else if (MappingID == 4)
            {
                SegaYBind = Button;
                GamepadManager.AddMapingLog("Genesis Y mapped to " + Button);
                GamepadManager.AddMapingLog("\nPress button for Z");
            }
            else if (MappingID == 5)
            {
                SegaZBind = Button;
                GamepadManager.AddMapingLog("Genesis Z mapped to " + Button);
                GamepadManager.AddMapingLog("\nPress button for MODE");
            } else if (MappingID == 6)
            {
                SegaModeBind = Button;
                GamepadManager.AddMapingLog("Genesis MODE mapped to " + Button);
                GamepadManager.AddMapingLog("\nPress button for SWAP");
            } else if (MappingID == 7)
            {
                SwapButton = Button;
                GamepadManager.AddMapingLog("SWAP mapped to " + Button);
            }
            MappingID++;

            if (MappingID == 8)
            {
                MappingID = 0;
                Remapping = false;
                GamepadManager.FinishMapping();
            }
        }

        void ProcessButtonsTriggers()
        {
            if (A && buttonsPrevious.IsButtonUp(Buttons.A))
            {
                AnyButtonPressed();
                Map("A");
            }
            if (B && buttonsPrevious.IsButtonUp(Buttons.B))
            {
                AnyButtonPressed();
                Map("B");
            }
            if (X && buttonsPrevious.IsButtonUp(Buttons.X))
            {
                AnyButtonPressed();
                Map("X");
            }
            if (Y && buttonsPrevious.IsButtonUp(Buttons.Y))
            {
                AnyButtonPressed();
                Map("Y");
            }
            if (R && buttonsPrevious.IsButtonUp(Buttons.RightShoulder))
            {
                AnyButtonPressed();
                Map("R");
            }
            if (L && buttonsPrevious.IsButtonUp(Buttons.LeftShoulder))
            {
                AnyButtonPressed();
                Map("L");
            }
            if (RT && buttonsPrevious.IsButtonUp(Buttons.RightTrigger))
            {
                AnyButtonPressed();
                Map("RT");
            }
            if (LT && buttonsPrevious.IsButtonUp(Buttons.LeftTrigger))
            {
                AnyButtonPressed();
                Map("LT");
            }
            if (Start && buttonsPrevious.IsButtonUp(Buttons.Start))
            {
                AnyButtonPressed();
            }
            if (BACK && buttonsPrevious.IsButtonUp(Buttons.Back))
            {
                AnyButtonPressed();
                Map("BACK");
            }
            if (Up && buttonsPrevious.IsButtonUp(Buttons.DPadUp) && !PreviousStickUp)
            {
                AnyButtonPressed();
            }
            if (Down && buttonsPrevious.IsButtonUp(Buttons.DPadDown) && !PreviousStickDown)
            {
                AnyButtonPressed();
            }
            if (Left && buttonsPrevious.IsButtonUp(Buttons.DPadLeft) && !PreviousStickLeft)
            {
                AnyButtonPressed();
            }
            if (Right && buttonsPrevious.IsButtonUp(Buttons.DPadRight) && !PreviousStickRight)
            {
                AnyButtonPressed();
            }
        }

        public void SetInputs()
        {
            bool SegaA = IsButtonHeldByName(SegaABind);
            bool SegaB = IsButtonHeldByName(SegaBBind);
            bool SegaC = IsButtonHeldByName(SegaCBind);
            bool SegaX = IsButtonHeldByName(SegaXBind);
            bool SegaY = IsButtonHeldByName(SegaYBind);
            bool SegaZ = IsButtonHeldByName(SegaZBind);
            bool SegaMode = IsButtonHeldByName(SegaModeBind);

            Inputs = Conv(Up) + Conv(Down) + Conv(Left) + Conv(Right) + Conv(SegaA) + Conv(SegaB) + Conv(SegaC) + Conv(SegaX) + Conv(SegaY) + Conv(SegaZ) + Conv(Start) + Conv(SegaMode);
        }

        public static string Conv(bool b)
        {
            if (b)
            {
                return "1";
            }else{
                return "0";
            }
        }

        public bool CanSwap = true;

        public void SwapProcess(GameTime gameTime)
        {
            if (IsSegaButtonHeldByName("SWAP"))
            {
                if (CanSwap)
                {
                    if (gameTime.TotalGameTime >= SwapLastRelease + TimeSpan.FromSeconds(3))
                    {
                        CanSwap = false;
                        GamepadManager.Swap();
                    }
                }
            } else
            {
                SwapLastRelease = gameTime.TotalGameTime;
                CanSwap = true;
            }
        }

        public void Update(GameTime gameTime)
        {
            if(DeviceID != -1)
            {
                buttons = GamePad.GetState(DeviceID);

                A = buttons.IsButtonDown(Buttons.A);
                B = buttons.IsButtonDown(Buttons.B);
                X = buttons.IsButtonDown(Buttons.X);
                Y = buttons.IsButtonDown(Buttons.Y);
                L = buttons.IsButtonDown(Buttons.LeftShoulder);
                R = buttons.IsButtonDown(Buttons.RightShoulder);
                LT = buttons.IsButtonDown(Buttons.LeftTrigger);
                RT = buttons.IsButtonDown(Buttons.RightTrigger);

                Start = buttons.IsButtonDown(Buttons.Start);
                BACK = buttons.IsButtonDown(Buttons.Back);

                bool StickUp = buttons.ThumbSticks.Left.Y > 0.3;
                bool StickDown = buttons.ThumbSticks.Left.Y < -0.3;
                bool StickLeft = buttons.ThumbSticks.Left.X < -0.3;
                bool StickRight = buttons.ThumbSticks.Left.X > 0.3;

                Up = buttons.IsButtonDown(Buttons.DPadUp) || StickUp;
                Down = buttons.IsButtonDown(Buttons.DPadDown) || StickDown;
                Left = buttons.IsButtonDown(Buttons.DPadLeft) || StickLeft;
                Right = buttons.IsButtonDown(Buttons.DPadRight) || StickRight;

                ProcessButtonsTriggers();

                if(Mode == GamepadMode.Genesis)
                {
                    SetInputs();
                    SwapProcess(gameTime);
                }

                buttonsPrevious = buttons;
                PreviousStickUp = StickUp;
                PreviousStickDown = StickDown;
                PreviousStickLeft = StickLeft;
                PreviousStickRight = StickRight;
            }
        }
    }
}
