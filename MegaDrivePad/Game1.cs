using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using CSCore.CoreAudioAPI;
using System.Linq;
using System.Diagnostics;
using Microsoft.VisualBasic;

namespace xna
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D BG1;
        Texture2D GhostBody;
        Texture2D GhostEyes;
        Texture2D GhostHands;
        Texture2D GhostPad;
        public static GhostAnimation Ghost;

        Texture2D BG2;
        Texture2D CatHandsSprite;
        Texture2D CatPupilsSprite;
        Texture2D CatFaceSprite;
        Texture2D CatMouthSprite;
        Texture2D CatHeadSprite;
        Texture2D CatBodySprite;
        Texture2D CatTailSprite;
        Texture2D CatBagSprite;
        public static CatAnimation Cat;

        Texture2D DebugFont;
        public static SpriteFont DebugText;
        public static Texture2D DummyBG;

        public static VisualPad VisualizedPad1;
        public static VisualPad VisualizedPad2;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 587*2;
            _graphics.PreferredBackBufferHeight = 367 +352;
            _graphics.ApplyChanges();
            Window.IsBorderless = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            InactiveSleepTime = new TimeSpan(0);
        }

        protected override void Initialize()
        {
            AudioInput.StartRecording();
            base.Initialize();
        }

        public static Dictionary<string, Texture2D> Sprites = new Dictionary<string, Texture2D>();
        public void LoadSprite(string Name)
        {
            if (Sprites.ContainsKey(Name))
            {
                return;
            }
            
            Texture2D S = Content.Load<Texture2D>(Name);
            if (S != null)
            {
                Sprites.Add(Name, S);
            }
        }
        public static Texture2D GetSprite(string Name)
        {
            Texture2D S;

            Sprites.TryGetValue(Name, out S);

            return S;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            BG1 = Content.Load<Texture2D>("BG1");
            BG2 = Content.Load<Texture2D>("BG3");
            GhostBody = Content.Load<Texture2D>("GhostBody");
            GhostEyes = Content.Load<Texture2D>("GhostEyes");
            GhostHands = Content.Load<Texture2D>("GhostHands");
            GhostPad = Content.Load<Texture2D>("GhostPad");

            DummyBG = Content.Load<Texture2D>("DummyBG");
            DebugFont = Content.Load<Texture2D>("MyFont");

            CatHandsSprite = Content.Load<Texture2D>("CatPaws");
            CatPupilsSprite = Content.Load<Texture2D>("CatPupils");
            CatFaceSprite = Content.Load<Texture2D>("CatFace");
            CatMouthSprite = Content.Load<Texture2D>("CatMouth");
            CatBodySprite = Content.Load<Texture2D>("CatBody");
            CatHeadSprite = Content.Load<Texture2D>("CatHead");
            CatTailSprite = Content.Load<Texture2D>("CatTail");
            CatBagSprite = Content.Load<Texture2D>("CatBag");

            string G = "Genesis_";
            string D = "_Down";

            // Buttons
            LoadSprite(G + "A");
            LoadSprite(G + "B");
            LoadSprite(G + "C");
            LoadSprite(G + "X");
            LoadSprite(G + "Y");
            LoadSprite(G + "Z");
            LoadSprite(G + "Start");
            //Buttons down
            LoadSprite(G + "Z" + D);
            LoadSprite(G + "A"+ D);
            LoadSprite(G + "B" + D);
            LoadSprite(G + "C" + D);
            LoadSprite(G + "X" + D);
            LoadSprite(G + "Y" + D);
            LoadSprite(G + "Z" + D);
            // D-Pad
            LoadSprite(G + "Up");
            LoadSprite(G + "Down");
            LoadSprite(G + "Left");
            LoadSprite(G + "Right");

            LoadSprite("GenesisPad");

            if (VisualizedPad1 == null)
            {
                VisualizedPad1 = new VisualPad(new Vector2(0, 367));

            }
            if (VisualizedPad2 == null)
            {
                VisualizedPad2 = new VisualPad(new Vector2(587, 367));
            }
        }
        float AudioRatePeriod = 0.1f;
        float currentTime = 0f;


        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            SettingsMenu.KeyboardUpdate();
            GamepadManager.Update(gameTime);
            ComportWorker.PortWorking();

            if(VisualizedPad1 != null)
            {
                VisualizedPad1.ProcessButtons();
            }
            if (VisualizedPad2 != null)
            {
                VisualizedPad2.ProcessButtons();
            }

            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (currentTime >= AudioRatePeriod)
            {
                AudioAppInput.CheckAudioLevels();
                currentTime -= AudioRatePeriod;
            }

            if (Ghost != null)
            {
                float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Ghost.Update(delta, gameTime);
                Ghost.IsLookAway = Keyboard.GetState().IsKeyDown(Keys.L);
            }
            if (Cat != null)
            {
                float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Cat.Update(delta, gameTime);
                Cat.IsLookAway = Keyboard.GetState().IsKeyDown(Keys.L);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //_frameCounter.Update(deltaTime);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(BG1, new Vector2(0, 0), BG1.Bounds, Color.White);
            _spriteBatch.Draw(BG2, new Vector2(587, 0), BG2.Bounds, Color.White);

            _spriteBatch.End();

            if (Ghost == null)
            {
                Ghost = new GhostAnimation(GhostBody, GhostEyes, GhostHands, GhostPad);
            }else{
                Ghost.Render(_spriteBatch);
            }

            if(Cat == null)
            {
                Cat = new CatAnimation(CatBagSprite, CatTailSprite, CatBodySprite, CatHeadSprite, CatMouthSprite, CatFaceSprite, CatPupilsSprite, CatHandsSprite);
            }else{
                Cat.Render(_spriteBatch);
            }

            if (VisualizedPad1 != null)
            {
                VisualizedPad1.Render(_spriteBatch);
            }
            if (VisualizedPad2 != null)
            {
                VisualizedPad2.Render(_spriteBatch);
            }

            if (DebugText == null)
            {
                DebugText = new SpriteFont();
                DebugText.Font = DebugFont;
                DebugText.Position = new Vector2(400,10);
                DebugText.TextColor = Color.Orange;
            }else{

                SettingsMenu.Render(_spriteBatch);
            }

            base.Draw(gameTime);
        }
    }
}