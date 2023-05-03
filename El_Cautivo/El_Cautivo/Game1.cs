using El_Cautivo.EngineExtentions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace El_Cautivo
{
    public class Game1 : Game
    {
        public enum GameState
        {
            MainMenu,
            Game,
            MiniGame,
            Ending
        }


        Button testButton;
        Slider testSlider;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        SpriteFont ArialFont, TitleFont;
        GameState state;
        public static int dScale = 2;  //TODO Сделать норм поддержку других разрешений, а не вот этот костыль

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Jessie.Scale = new Vector2(8, 8)/dScale;
            Jessie.speed = 2 / dScale;
            Jessie.Position = new Vector2(512, 512) / dScale;
            state = GameState.MainMenu;
            _graphics.PreferredBackBufferHeight = 1080/dScale;
            _graphics.PreferredBackBufferWidth = 1920/dScale;
            _graphics.IsFullScreen = dScale == 1;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            testSlider = new Slider(Content.Load<Texture2D>("SliderBasis"), Content.Load<Texture2D>("SliderPointer"), new Vector2(10, 150));
            testButton = new Button(new Vector2(10, 10), Content.Load<Texture2D>("Lab"), BeginGame);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            MainMenu.BG = Content.Load<Texture2D>("MenuBG");
            Lab.Surroundings = Content.Load<Texture2D>("Lab");
            ArialFont = Content.Load<SpriteFont>("Font");
            TitleFont = Content.Load<SpriteFont>("TitleFont");
            Jessie.InitAnimations(Content.Load<Texture2D>("WalkSheetBmp"), Content.Load<Texture2D>("RevercedWalkSheet"), 
                Content.Load<Texture2D>("WalkSheetBmp"), Content.Load<Texture2D>("RevercedWalkSheet"));
            MainMenu.Font = TitleFont;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            testButton.Update();
            testSlider.Update();
            if (state == GameState.MainMenu && Keyboard.GetState().IsKeyDown(Keys.Enter)) state = GameState.Game;
            Jessie.Update(ref state);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            if (state == GameState.MainMenu)
            {
                MainMenu.Draw(_spriteBatch);
                testButton.Draw(_spriteBatch);
                testSlider.Draw(_spriteBatch);
            }
            else
            {
                Lab.Draw(_spriteBatch);
                Jessie.Draw(_spriteBatch);
            }
            _spriteBatch.End();
            base.Draw(gameTime);        
        }

        void BeginGame() => state = GameState.Game;
    }
}