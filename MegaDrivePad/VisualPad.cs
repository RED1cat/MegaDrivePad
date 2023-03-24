using CSCore.Codecs.FLAC;
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
    public class VisualPad
    {
        public VisualPad(Vector2 pos)
        {
            Position = pos;
        }
        public GamepadUtil Pad = null;

        public Vector2 Position = new Vector2(0, 0);
        public bool Up = false;
        public bool Down = false;
        public bool Left = false;
        public bool Right = false;

        public bool A = false;
        public bool B = false;
        public bool C = false;

        public bool X = false;
        public bool Y = false;
        public bool Z = false;
        public bool Start = false;

        public Texture2D SpA = Game1.GetSprite("Genesis_A");
        public Texture2D SpB = Game1.GetSprite("Genesis_B");
        public Texture2D SpC = Game1.GetSprite("Genesis_C");

        public Texture2D SpADown = Game1.GetSprite("Genesis_A_Down");
        public Texture2D SpBDown = Game1.GetSprite("Genesis_B_Down");
        public Texture2D SpCDown = Game1.GetSprite("Genesis_C_Down");

        public Texture2D SpX = Game1.GetSprite("Genesis_X");
        public Texture2D SpY = Game1.GetSprite("Genesis_Y");
        public Texture2D SpZ = Game1.GetSprite("Genesis_Z");

        public Texture2D SpXDown = Game1.GetSprite("Genesis_X_Down");
        public Texture2D SpYDown = Game1.GetSprite("Genesis_Y_Down");
        public Texture2D SpZDown = Game1.GetSprite("Genesis_Z_Down");

        public Texture2D SpUp = Game1.GetSprite("Genesis_Up");
        public Texture2D SpLeft = Game1.GetSprite("Genesis_Left");
        public Texture2D SpRight = Game1.GetSprite("Genesis_Right");
        public Texture2D SpDown = Game1.GetSprite("Genesis_Down");
        public Texture2D SpPad = Game1.GetSprite("GenesisPad");

        public Texture2D SpStart = Game1.GetSprite("Genesis_Start");

        public void ProcessButtons()
        {
            if(Pad == null)
            {
                return;
            }

            if (Pad.Remapping)
            {
                return;
            }
            
            A = Pad.IsSegaButtonHeldByName("A");
            B = Pad.IsSegaButtonHeldByName("B");
            C = Pad.IsSegaButtonHeldByName("C");
            X = Pad.IsSegaButtonHeldByName("X");
            Y = Pad.IsSegaButtonHeldByName("Y");
            Z = Pad.IsSegaButtonHeldByName("Z");

            Up = Pad.Up;
            Down = Pad.Down;
            Left = Pad.Left;
            Right = Pad.Right;
            Start = Pad.Start;
        }


        public void Render(SpriteBatch _s)
        {
            _s.Begin();

            _s.Draw(SpPad, Position, Color.White);

            _s.Draw(!A ? SpA : SpADown, Position, Color.White);
            _s.Draw(!B ? SpB : SpBDown, Position, Color.White);
            _s.Draw(!C ? SpC : SpCDown, Position, Color.White);

            _s.Draw(!X ? SpX : SpXDown, Position, Color.White);
            _s.Draw(!Y ? SpY : SpYDown, Position, Color.White);
            _s.Draw(!Z ? SpZ : SpZDown, Position, Color.White);

            if (Up)
            {
                _s.Draw(SpUp, Position, Color.White);
            }
            if (Down)
            {
                _s.Draw(SpDown, Position, Color.White);
            }
            if (Left)
            {
                _s.Draw(SpLeft, Position, Color.White);
            }
            if (Right)
            {
                _s.Draw(SpRight, Position, Color.White);
            }
            if (Start)
            {
                _s.Draw(SpStart, Position, Color.White);
            }
            _s.End();
        }
    }
}
