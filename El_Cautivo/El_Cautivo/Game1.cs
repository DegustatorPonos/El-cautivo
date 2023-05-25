using El_Cautivo.EngineExtentions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

using El_Cautivo.Menus;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace El_Cautivo
{
    public class Game1 : Game
    {
        #region Chemistry Stuff
        public enum ChemElement
        {
            Methilamine,
            Aluminum_dust,
            Glutamic_acid,
            Phosphoric_acid,
            Cumeine,
            Glicine,
            Glutathionate,
            Crushed_Glicine,
            Cumene_hydroperoxide,
            P2P_Wet,
            P2P,
            Liquid_Meth
        }
        public static Dictionary<ChemElement, string> GetName = new Dictionary<ChemElement, string>
        {
            { ChemElement.Methilamine, "Mrthilamine" },
            { ChemElement.Aluminum_dust, "Aluminum" },
            { ChemElement.Glutamic_acid, "Glutamic acid" },
            { ChemElement.Phosphoric_acid, "Phosphoric acid" },
            { ChemElement.Cumeine, "Cumeine" },
            { ChemElement.Glicine, "Glicine" },
            { ChemElement.Glutathionate, "Glutathionate " },
            { ChemElement.Crushed_Glicine, "Crushed glicine" },
            { ChemElement.Cumene_hydroperoxide, "Cumene hydroperoxide" },
            { ChemElement.P2P_Wet, "Phenyl-2-propanol + water" },
            { ChemElement.P2P, "Phenyl-2-propanol" },
            { ChemElement.Liquid_Meth, "Liquified methamphetamine" },
        };

        #endregion
        public enum GameState
        {
            MainMenu,
            SettingsMenu,
            ExitMenu,
            Game,
            MiniGame,
            Ending
        }

        public static Texture2D ColliderTexture;
        public static bool ShowColliders = false; //FOR DEBUG PURPOSES ONLY

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static SpriteFont TitleFont;
        public static Texture2D BarrelTexture, BagTexture;
        public static GameState state;
        public static double MusicVolume = 1;
        public static bool IsFSAvaliable;
        public static int dScale = 2;  //TODO Сделать норм поддержку других разрешений, а не вот этот костыль 
        //Я еблан и оставил эту хуйню здесь, теперь всё работает через неё

        SoundEffect MainMenuMusic;
        SoundEffectInstance BGMusicInstance;
        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.Disposed += Game1_Disposed;
        }

        private void Game1_Disposed(object sender, EventArgs e)
        {
         //   _spriteBatch.Dispose();
           // _graphics.Dispose();
        }

        void FSManager()
        {
            BGMusicInstance.Stop();
            dScale = dScale == 1 ? 2 : 1;
            Initialize();
            Lab.InitColliders();
            Jessie.ResizeCollider();
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            IsFSAvaliable = (GraphicsDevice.Adapter.CurrentDisplayMode.Width == 1920 && 
                GraphicsDevice.Adapter.CurrentDisplayMode.Height == 1080);
            MainMenu.Init(Quit);
            Jessie.Scale = new Vector2(8, 8)/dScale;
            Jessie.speed = 2 / dScale;
            Jessie.Position = new Vector2(512, 512) / dScale;
            state = GameState.MainMenu;
            _graphics.PreferredBackBufferHeight = 1080/dScale;
            _graphics.PreferredBackBufferWidth = 1920/dScale;
            _graphics.IsFullScreen = dScale == 1;
            _graphics.ApplyChanges();
            SettingsMenu.FullScreenManager = FSManager;
            Lab.InitColliders();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Lab.LoadContent(Content);
            MainMenu.LoadContent(Content);
            SettingsMenu.LoadContent(Content);
            ExitMenu.LoadContent(Content);
            BarrelTexture = Content.Load<Texture2D>("Items/Barrel");
            BagTexture = Content.Load<Texture2D>("Items/Bag");
            ColliderTexture = Content.Load<Texture2D>("ColliderTexture");
            //InitBGM(Content.Load<SoundEffect>("Audio/Baby_blue"));
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Lab.Surroundings = Content.Load<Texture2D>("Lab");
            TitleFont = Content.Load<SpriteFont>("TitleFont");
            Jessie.InitAnimations(Content.Load<Texture2D>("RestSheet"), Content.Load<Texture2D>("ReversedRestSheet"), 
                Content.Load<Texture2D>("WalkSheetBmp"), Content.Load<Texture2D>("RevercedWalkSheet"));
            MainMenu.Font = TitleFont;
        }

        private void InitBGM(SoundEffect BGMusic)
        {
            MainMenuMusic = BGMusic;
            BGMusicInstance = MainMenuMusic.CreateInstance();
            BGMusicInstance.IsLooped = true;
            BGMusicInstance.Play();
        }

        protected override void Update(GameTime gameTime)
        {
            BGMusicInstance.Volume = (float)MusicVolume;
            
            switch (state)
            {
                case GameState.MainMenu:
                    MainMenu.Update();
                    break;
                case GameState.SettingsMenu:
                    SettingsMenu.Update();
                    break;
                case GameState.ExitMenu:
                    ExitMenu.Update();
                    break;
                case GameState.Game or GameState.MiniGame:
                    Lab.Update();
                    break;
            }
            Jessie.Update();
            //if (state == GameState.MainMenu && Keyboard.GetState().IsKeyDown(Keys.Enter)) state = GameState.Game;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            switch (state)
            {
                case GameState.MainMenu:
                    MainMenu.Draw(_spriteBatch);
                    break;
                case GameState.SettingsMenu:
                    SettingsMenu.Draw(_spriteBatch);
                    break;
                case GameState.Game or GameState.MiniGame:
                    Lab.Draw(_spriteBatch);
                    break;
                case GameState.ExitMenu:
                    ExitMenu.Draw(_spriteBatch);
                    break;
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        void Quit()
        {
            //  GC.Collect();
            //Environment.Exit(0);

            BGMusicInstance.Stop();
           
            Exit();

            
        }
    }
}