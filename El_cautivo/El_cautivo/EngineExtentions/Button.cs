using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace El_Cautivo.EngineExtentions
{
    public class Button : IDrawable
    {
        public Button(Vector2 pos, Texture2D Texture, Action act, double scale = 1)
        {
            action = act;
            Scale = scale;
            Position = pos;
            texture = Texture;

            IsCovered = false;
            buttonRectangle = new Rectangle((int)pos.X, (int)pos.Y, Texture.Width, Texture.Height);
        }

        public bool IsCovered { get; private set; }
        public Color CoveringColor = Color.Gray;
        Color overlayColor = Color.White;
        Rectangle buttonRectangle;
        Action action;
        Vector2 Position;
        Texture2D texture;
        public double Scale = 1;

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, Position, new Rectangle(0, 0, texture.Width, texture.Height), overlayColor, 0, 
                Vector2.Zero, Vector2.One, SpriteEffects.None, 0);
        }

        public void Update()
        {
            var mouseState = Mouse.GetState();
            if (buttonRectangle.Intersects(new Rectangle(mouseState.X, mouseState.Y, 1, 1)))
            {
                if (!IsCovered) OnCover();
                IsCovered = true;
                if (mouseState.LeftButton == ButtonState.Pressed)
                    action();
            }
            else
            { 
                if (IsCovered) OnUncover();
                IsCovered = false;
            }
        }

        void OnCover() => overlayColor = CoveringColor;

        void OnUncover() => overlayColor = Color.White;
    }
}
