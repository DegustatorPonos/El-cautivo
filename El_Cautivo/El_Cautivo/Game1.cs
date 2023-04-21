using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        SpriteFont ArialFont, TitleFont;
        GameState state;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Jessie.Position = new Vector2(512, 512);
            state = GameState.MainMenu;
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            MainMenu.BG = Content.Load<Texture2D>("MenuBG");
            Lab.Surroundings = Content.Load<Texture2D>("Lab");
            ArialFont = Content.Load<SpriteFont>("Font");
            TitleFont = Content.Load<SpriteFont>("TitleFont");
            Jessie.InitAnimations(Content.Load<Texture2D>("WalkSheetBmp"), Content.Load<Texture2D>("WalkSheetBmp"));
            MainMenu.Font = TitleFont;
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (state == GameState.MainMenu && Keyboard.GetState().IsKeyDown(Keys.Enter)) state = GameState.Game;
            // TODO: Add your update logic here
            Jessie.Update(ref state);
            base.Update(gameTime);
            Draw(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            // TODO: Add your drawing code here
            if (state == GameState.MainMenu) MainMenu.Draw(_spriteBatch);
            else
            {
                Lab.Draw(_spriteBatch);
                Jessie.Draw(_spriteBatch);
            }
            _spriteBatch.End();
            base.Draw(gameTime);        
        }
    }
}