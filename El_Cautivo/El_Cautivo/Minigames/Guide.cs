using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace El_Cautivo.Minigames
{
    public class Guide : IMiniGame
    {
        bool IsActive = false;

        Rectangle Collider;
        Texture2D Tutorial;
        public void LoadContent(ContentManager Content)
        {
            Tutorial = Content.Load<Texture2D>("MGContent/Guide/Recipe");
        }

        public Guide(Rectangle collider)
        {
            Collider = collider;
        }

        public float GetState() => 1;


        public void Update()
        {
            IsActive = Collider.Intersects(Jessie.Collider);
        }
        Vector2 TextPosition = new Vector2(0, 27);
        public void Draw(SpriteBatch batch)
        {
            if (Game1.ShowColliders) batch.Draw(Game1.ColliderTexture, new Vector2(Collider.X, Collider.Y), new Rectangle(0, 0, 1, 1),
                 Color.Green, 0, Vector2.Zero, new Vector2(Collider.Width, Collider.Height), SpriteEffects.None, 0);
            if (!IsActive) return;
            Lab.sb.Append("F - Show recipe \n");
            if (Keyboard.GetState().IsKeyDown(Keys.F))
                batch.Draw(Tutorial, Vector2.Zero, new Rectangle(0, 0, 192, 168), Color.White, 0, Vector2.Zero, new Vector2(10, 10) / Game1.dScale, SpriteEffects.None, 0);
        }

        public void Refresh() { ; }
    }
}
