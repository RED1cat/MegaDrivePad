using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xna
{
    public class CatAnimation
    {
        public Vector2 Root = new Vector2(587, 0);
        public Vector2 HeadIdle = new Vector2(587 + 228, 67);
        public Vector2 HandsIdle = new Vector2(587 + 214, 204);
        public Vector2 PupilsIdle = new Vector2(11, 5);
        public Vector2 FaceIdle = new Vector2(3, 72);
        public Vector2 MouthIdle = new Vector2(20, 118);

        public Vector2 Pupils = new Vector2(11, 5);
        public Vector2 Face = new Vector2(3, 72);
        public Vector2 Mouth = new Vector2(20, 118);

        public Vector2 Hands = new Vector2(587 + 214, 204);
        public Vector2 Head = new Vector2(587 + 228, 67);
        public Vector2 Body = new Vector2(587 + 242, 221);
        public Vector2 Tail = new Vector2(587 + 288, 269);
        public Vector2 Bag = new Vector2(587 + 176, 277);

        public Texture2D HandsSprite;
        public Texture2D PupilsSprite;
        public Texture2D FaceSprite;
        public Texture2D MouthSprite;
        public Texture2D HeadSprite;
        public Texture2D BodySprite;
        public Texture2D TailSprite;
        public Texture2D BagSprite;

        public Rectangle FaceBounds = new Rectangle(0, 0, 121, 76);
        public Rectangle MouthBounds = new Rectangle(0, 0, 76, 60);
        public Rectangle PupilsBounds = new Rectangle(0, 0, 96, 62);

        public int FaceFrame = 0;
        public int MouthFrame = 0;
        public int PupilsFrame = 0;

        public int NextBlink = 10;
        public int BlinkState = 0;

        public bool IsLookAway = false;
        public bool IsTalking = false;

        public int IdleTime = 0;

        bool IsIdle()
        {
            return IdleTime > 20;
        }

        public void PressButton()
        {
            Random RNG = new Random();
            Hands = new Vector2(HandsIdle.X + RNG.Next(-3, 3), HandsIdle.Y + RNG.Next(0, 12));
            IdleTime = 0;
        }


        public CatAnimation(Texture2D _bag, Texture2D _tail, Texture2D _body, Texture2D _head,
            Texture2D _mouth, Texture2D _face, Texture2D _pupils, Texture2D _hands)
        {
            HandsSprite = _hands;
            PupilsSprite = _pupils;
            FaceSprite = _face;
            MouthSprite = _mouth;
            HeadSprite = _head;
            BodySprite = _body;
            TailSprite = _tail;
            BagSprite = _bag;
        }

        public void ProcessAnimation(float dt)
        {
            Vector2 DesiredEyesPosition = Pupils;

            if (!IsLookAway)
            {
                DesiredEyesPosition = PupilsIdle;
            }else{
                DesiredEyesPosition = new Vector2(15, 5);
            }

            if (!IsTalking)
            {
                MouthFrame = 0;
            }


            Pupils = Vector2.Lerp(Pupils, DesiredEyesPosition, dt * 8);
            Hands = Vector2.Lerp(Hands, HandsIdle, dt * 8);
        }

        public void Render(SpriteBatch _s)
        {
            _s.Begin();

            //_s.Draw(BagSprite, Bag, BagSprite.Bounds, Color.White);
            _s.Draw(TailSprite, Tail, TailSprite.Bounds, Color.White);
            _s.Draw(BodySprite, Body, BodySprite.Bounds, Color.White);
            _s.Draw(HeadSprite, Head, HeadSprite.Bounds, Color.White);
            MouthBounds.X = MouthFrame * 76;
            _s.Draw(MouthSprite, Head + Mouth, MouthBounds, Color.White);
            FaceBounds.X = FaceFrame * 121;
            _s.Draw(FaceSprite, Head + Face, FaceBounds, Color.White);
            if (FaceFrame == 2)
            {
                PupilsFrame = 1;
            }else{
                PupilsFrame = 0;
            }

            if (FaceFrame != 3)
            {
                PupilsBounds.X = PupilsFrame * 96;
                _s.Draw(PupilsSprite, Head + Face + Pupils, PupilsBounds, Color.White);
            }

            _s.Draw(HandsSprite, Hands, HandsSprite.Bounds, Color.White);

            _s.End();
        }

        float TalkPeriod = 0.24f;
        float BlinkPeriod = 0.03f;
        float EverySecondPeriod = 1f;
        float currentTime = 0f;
        float currentTime2 = 0f;
        float currentTime3 = 0f;
        public void EverySecond()
        {
            if (NextBlink == 0)
            {
                BlinkState = 1;
                NextBlink = new Random().Next(10, 20);
            }else{
                NextBlink--;
            }
            IdleTime++;
        }

        public void Update(float delta, GameTime gameTime)
        {
            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (currentTime >= TalkPeriod)
            {
                if (IsTalking)
                {
                    if (MouthFrame < 4)
                    {
                        MouthFrame++;
                    }else{
                        MouthFrame = 1;
                    }
                }
                currentTime -= TalkPeriod;
            }
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
                    if (FaceFrame < 3)
                    {
                        FaceFrame++;
                    }else{
                        BlinkState = 2;
                    }
                }
                if (BlinkState == 2)
                {
                    if (FaceFrame > 0)
                    {
                        FaceFrame--;
                    }else{
                        BlinkState = 0;
                    }
                }
                currentTime3 -= BlinkPeriod;
            }

            IsLookAway = IdleTime > 10;

            ProcessAnimation(delta);
        }
    }
}
