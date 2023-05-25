using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace El_Cautivo.EngineExtentions
{
    public class Button : IDrawable
    {
        public enum ButtonType
        {
            OnUp,
            OnDown
        }
        public Button(Vector2 pos, Texture2D Texture, Action act, int scale = 1, ButtonType type = ButtonType.OnDown)
        {
            action = act;
            Scale = scale;
            Position = pos;
            texture = Texture;
            DetectionType = type;

            IsCovered = false;
            buttonRectangle = new Rectangle((int)pos.X, (int)pos.Y, Texture.Width*scale, Texture.Height*scale);
        }

        ButtonType DetectionType;
        public bool IsCovered { get; private set; }
        public Color CoveringColor = Color.Gray;
        Color overlayColor = Color.White;
        Rectangle buttonRectangle;
        Action action;
        public Vector2 Position;
        Texture2D texture;
        public int Scale = 1;
        bool PrevState;

        /// <summary>
        /// NOTICE - Only works with textures with the same scale props
        /// </summary>
        public void ChangeTexture(Texture2D newTexture)
        {
            if (newTexture.Height == texture.Height && newTexture.Width == texture.Width)
                texture = newTexture;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, Position, new Rectangle(0, 0, texture.Width, texture.Height), overlayColor, 0, 
                Vector2.Zero, Vector2.One*Scale, SpriteEffects.None, 0);
        }

        public void Update()
        {
            var mouseState = Mouse.GetState();
            if (buttonRectangle.Intersects(new Rectangle(mouseState.X, mouseState.Y, 1, 1)))
            {
                if (!IsCovered) OnCover();
                IsCovered = true;
                if ((DetectionType == ButtonType.OnDown && mouseState.LeftButton == ButtonState.Pressed) ||
                    (DetectionType == ButtonType.OnUp && mouseState.LeftButton == ButtonState.Released && PrevState))
                {
                    PrevState = false;
                    action();
                    return;
                }
            }
            else
            { 
                if (IsCovered) OnUncover();
                IsCovered = false;
            }
            PrevState = mouseState.LeftButton == ButtonState.Pressed;
            buttonRectangle.X = (int)Position.X;
            buttonRectangle.Y = (int)Position.Y;
        }

        void OnCover() => overlayColor = CoveringColor;

        void OnUncover() => overlayColor = Color.White;
    }
}
