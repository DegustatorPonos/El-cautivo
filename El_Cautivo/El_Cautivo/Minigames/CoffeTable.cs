using El_Cautivo.EngineExtentions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace El_Cautivo.Minigames
{
    public class CoffeTable : IMiniGame
    {
        Vector2 BackPos = new Vector2(10, 814), DrikPos, MixPos;
        Button BackButton, Drink, Mix;
        Texture2D BackTexture, DrinkTexture, MixTexture;
        Rectangle Collider;
        bool IsColliding = false, IsActive = false;
        string ToDisplay = "F - take chemicals";

        public CoffeTable(Rectangle collider)
        {
            Collider = collider;
        }
        public void Draw(SpriteBatch batch)
        {
            if (IsColliding) Lab.sb.Append(ToDisplay);
            if (Game1.ShowColliders) batch.Draw(Game1.ColliderTexture, new Vector2(Collider.X, Collider.Y), new Rectangle(0, 0, 1, 1),
                 Color.Green, 0, Vector2.Zero, new Vector2(Collider.Width, Collider.Height), SpriteEffects.None, 0);
            if (IsActive)
            {
                BackButton.Draw(batch);
                Mix.Draw(batch);
                Drink.Draw(batch);
            }
        }

        public float GetState()
        {
            return 1;
        }

        public void LoadContent(ContentManager content)
        {
            BackTexture = content.Load<Texture2D>("Buttons/BackButton");
            DrinkTexture = content.Load<Texture2D>("MGContent/Oxidizing/DoAction");
            MixTexture = content.Load<Texture2D>("MGContent/Boiling/MixButton");

            DrikPos = new Vector2(10, (1080 - (DrinkTexture.Height * 2)) / 2);
            MixPos = new Vector2((1920 - MixTexture.Width * 2) - 10, (1080 - (MixTexture.Height * 2)) / 2);

            BackButton = new Button(BackPos / Game1.dScale, BackTexture, OnBackButton, (int)(4 / Game1.dScale), Button.ButtonType.OnUp);
            Drink = new Button(DrikPos / Game1.dScale, DrinkTexture, DrinkAction, (int)(2 / Game1.dScale), Button.ButtonType.OnUp);
            Mix = new Button(MixPos / Game1.dScale, MixTexture, Mixing, (int)(2 / Game1.dScale), Button.ButtonType.OnUp);
        }

        void OnBackButton()
        {
            Game1.state = Game1.GameState.Game;
            IsActive = false;
        }

        void DrinkAction() => Lab.EndDay();

        void Mixing()
        {
            Lab.isPoisoned = true;
            Lab.EndDay();
        }
        public void Update()
        {
            var toMix = Lab.objects.Where(x => x.Content == Game1.ChemElement.P2P_Wet)
                .Where(x => Collider.Intersects(x.Collider)).FirstOrDefault() != null;
            IsColliding = Collider.Intersects(Jessie.Collider);
            if (IsColliding && !IsActive && Keyboard.GetState().IsKeyDown(Keys.F))
            {
                IsActive = true;
                Game1.state = Game1.GameState.MiniGame;
            }
            if (IsActive)
            {
                BackButton.Update();
                Drink.Update();
                if(toMix) Mix.Update();
            }
        }
    }
}
