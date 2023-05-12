using El_Cautivo.EngineExtentions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

using El_Cautivo.Menus;
using Microsoft.Xna.Framework.Audio;

namespace El_Cautivo
{
    public class Game1 : Game
    {
        public enum GameState
        {
            MainMenu,
            SettingsMenu,
            ExitMenu,
            Game,
            MiniGame,
            Ending
        }

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        SpriteFont TitleFont;
        public static GameState state;
        public static double MusicVolume = 1;
        public static bool IsFSAvaliable;
        public static int dScale = 2;  //TODO Сделать норм поддержку других разрешений, а не вот этот костыль

        SoundEffect MainMenuMusic;
        SoundEffectInstance BGMusicInstance;
        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        void FSManager()
        {
            BGMusicInstance.Stop();
            dScale = dScale == 1 ? 2 : 1;
            Initialize();
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            IsFSAvaliable = (GraphicsDevice.Adapter.CurrentDisplayMode.Width == 1920 && 
                GraphicsDevice.Adapter.CurrentDisplayMode.Height == 1080);
            MainMenu.Init(Exit);
            Jessie.Scale = new Vector2(8, 8)/dScale;
            Jessie.speed = 2 / dScale;
            Jessie.Position = new Vector2(512, 512) / dScale;
            state = GameState.MainMenu;
            _graphics.PreferredBackBufferHeight = 1080/dScale;
            _graphics.PreferredBackBufferWidth = 1920/dScale;
            _graphics.IsFullScreen = dScale == 1;
            _graphics.ApplyChanges();
            SettingsMenu.FullScreenManager = FSManager;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            MainMenu.LoadContent(Content);
            SettingsMenu.LoadContent(Content);
            ExitMenu.LoadContent(Content);
            InitBGM(Content.Load<SoundEffect>("Audio/Baby_blue"));
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Lab.Surroundings = Content.Load<Texture2D>("Lab");
            TitleFont = Content.Load<SpriteFont>("TitleFont");
            Jessie.InitAnimations(Content.Load<Texture2D>("RestSheet"), Content.Load<Texture2D>("RestSheet"), 
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
                case GameState.Game:
                    Lab.Update();
                    break;
            }
            Jessie.Update(ref state);
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
                case GameState.Game:
                    Lab.Draw(_spriteBatch);
                    Jessie.Draw(_spriteBatch);
                    break;
                case GameState.ExitMenu:
                    ExitMenu.Draw(_spriteBatch);
                    break;
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}