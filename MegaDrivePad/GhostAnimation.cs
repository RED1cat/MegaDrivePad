using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xna
{
    public class GhostAnimation
    {
        public Vector2 Root = new Vector2(0, 0);
        public Vector2 HandsIdle = new Vector2(161, 98);
        public Vector2 EyesIdle = new Vector2(159, 25);
        public Vector2 PadDropStart = new Vector2(54, 43);

        public Vector2 Hands = new Vector2(161, 98);
        public Vector2 Eyes = new Vector2(159, 25);
        public Vector2 Body = new Vector2(137, 21);
        public Vector2 Pad = new Vector2(211, 234);
        public float PadFloorY = 276;

        public Texture2D HandsSprite;
        public Texture2D EyesSprite;
        public Texture2D BodySprite;
        public Texture2D PadSprite;

        public Rectangle EyesBounds = new Rectangle(0, 0, 207, 183);
        public Rectangle BodyBounds = new Rectangle(0, 0, 241, 342);

        public bool IsLookAway = false;
        public bool IsTalking = false;
        public bool TalkStateUp = true;

        public int EyesFrame = 0;
        public int BodyFrame = 0;
        public int ForcedEyesFrame = 0;

        public int NextBlink = 10;
        public int BlinkState = 0;
        public int IdleTime = 0;
        bool PreviouslyIdle = false;
        public bool EmoteSelectHold = false;
        private int EmoteSelectHoldTime = 0;
        public bool EmoteSelectMode = false;
        bool IsIdle()
        {
            return IdleTime > 20;
        }

        public void PressButton()
        {
            Random RNG = new Random();
            Hands = new Vector2(HandsIdle.X + RNG.Next(-5, 5), HandsIdle.Y + RNG.Next(0, 12));
            IdleTime = 0;
        }


        public GhostAnimation(Texture2D _body, Texture2D _eyes, Texture2D _hands, Texture2D _pad)
        {
            HandsSprite = _hands;
            EyesSprite = _eyes;
            BodySprite = _body;
            PadSprite = _pad;
        }

        public void ProcessAnimation(float dt)
        {
            Vector2 DesiredEyesPosition = Eyes;
            float TargetY = EyesIdle.Y;
            if (!IsLookAway)
            {
                DesiredEyesPosition = EyesIdle;
                TargetY = EyesIdle.Y;
            }else{
                DesiredEyesPosition = new Vector2(140, 32);
                TargetY = 32;
            }

            if (IsTalking)
            {
                if (BlinkState == 0 && ForcedEyesFrame == 0)
                {
                    EyesFrame = 5;
                }

                float MaxUp = 23.5f;
                float MaxDown = 27.5f;

                if (TargetY == EyesIdle.Y)
                {
                    MaxUp = 23.5f;
                    MaxDown = 27.5f;
                }else{
                    MaxUp = 31.5f;
                    MaxDown = 33.5f;
                }

                if (TalkStateUp)
                {
                    if (Eyes.Y > MaxUp)
                    {
                        DesiredEyesPosition.Y = MaxUp - 2;
                    }else{
                        TalkStateUp = false;
                    }
                }
                if (!TalkStateUp)
                {
                    if (Eyes.Y < MaxDown)
                    {
                        DesiredEyesPosition.Y = MaxDown + 2;
                    }else{
                        TalkStateUp = true;
                    }
                }
            }else{
                if (BlinkState == 0)
                {
                    EyesFrame = 0;
                }
            }

            if (ForcedEyesFrame != 0 && BlinkState == 0)
            {
                EyesFrame = ForcedEyesFrame;
            }

            if (Pad.Y < PadFloorY)
            {
                Pad.Y += 5;
            }

            Eyes = Vector2.Lerp(Eyes, DesiredEyesPosition, dt * 8);
            Hands = Vector2.Lerp(Hands, HandsIdle, dt * 8);
        }

        public void Render(SpriteBatch _s)
        {
            _s.Begin();

            BodyBounds.X = BodyFrame * 241;

            _s.Draw(BodySprite, Body, BodyBounds, Color.White);

            EyesBounds.X = EyesFrame * 207;

            _s.Draw(EyesSprite, Eyes, EyesBounds, Color.White);

            if (!IsIdle())
            {
                _s.Draw(HandsSprite, Hands, HandsSprite.Bounds, Color.White);
            }else{
                _s.Draw(PadSprite, Pad, PadSprite.Bounds, Color.White);
            }

            _s.End();
        }

        public void EverySecond()
        {
            if (NextBlink == 0)
            {
                if (ForcedEyesFrame == 0)
                {
                    EyesFrame = 0;
                    BlinkState = 1;
                }
                NextBlink = new Random().Next(10, 20);
            }else{
                NextBlink--;
            }
            IdleTime++;

            if (EmoteSelectHold)
            {
                EmoteSelectHoldTime++;
                if (EmoteSelectHoldTime >= 2)
                {
                    EmoteSelectMode = true;
                }
            }else{
                EmoteSelectHoldTime = 0;
                EmoteSelectMode = false;
            }
        }
        float BlinkPeriod = 0.01f;
        float EverySecondPeriod = 1f;
        float currentTime2 = 0f;
        float currentTime3 = 0f;

        public void Update(float delta, GameTime gameTime)
        {
            currentTime2 += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (currentTime2 >= EverySecondPeriod)
            {
                EverySecond();
                currentTime2 -= EverySecondPeriod;
            }
            currentTime3 += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (currentTime3 >= BlinkPeriod)
            {
                if (BlinkState == 1)
                {
                    if (EyesFrame < 5)
                    {
                        EyesFrame++;
                    }else{
                        BlinkState = 2;
                    }
                }
                if (BlinkState == 2)
                {
                    if (EyesFrame > 0)
                    {
                        EyesFrame--;
                    }else{
                        BlinkState = 0;
                    }
                }
                currentTime3 -= BlinkPeriod;
            }

            bool Idle = IsIdle();

            BodyFrame = Idle ? 1 : 0;
            IsLookAway = IdleTime > 10;

            if (PreviouslyIdle != Idle)
            {
                PreviouslyIdle = Idle;
                Pad = Hands + PadDropStart;
            }
            ProcessAnimation(delta);
        }
    }
}
