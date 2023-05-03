using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace El_Cautivo.EngineExtentions
{
    public class Slider : IDrawable
    {
        /// <summary>
        /// Creates the slider - GUI element which you can slide
        /// </summary>
        /// <param name="basis">Texture of a background</param>
        /// <param name="pointer">Testure of a point</param>
        public Slider(Texture2D basis, Texture2D pointer, Vector2 pos, double scale = 1)
        {
            Position = pos;
            slideTesture = basis;
            pointerTexture = pointer;

            shiftPointer = new Vector2();
            deltaPointerPos = new Vector2(-(pointer.Width/2), -((pointer.Height-basis.Height) / 2));
            Basis = new Button(pos, slideTesture, OnMove);
            Basis.CoveringColor = Color.Red; //for it not to change its color
            Value = 1f;
        }
        public Vector2 Position { get; private set; }
        public Texture2D slideTesture, pointerTexture;
        Button Basis;
        double scale;

        //basically this piece of shit is needed to centre the pointer
        public Vector2 deltaPointerPos; 
        //This MF makes ball shifty =D
        public Vector2 shiftPointer;

        /// <summary>
        /// Value of a slider (0-1.0f)
        /// </summary>
        public double Value { get; private set; }
        public void Draw(SpriteBatch batch)
        {
            Basis.Draw(batch);
            batch.Draw(pointerTexture, Position+deltaPointerPos+shiftPointer, new Rectangle(0, 0, pointerTexture.Width, pointerTexture.Height), Color.White, 0,
                Vector2.Zero, Vector2.One, SpriteEffects.None, 0);
        }

        public void Update()
        {
            Basis.Update();
            shiftPointer.X = (int)(slideTesture.Width*Value);
        }

        void OnMove()
        {
            var MouseX = Mouse.GetState().X;
            var dX = MouseX - Position.X;
            Value = dX / slideTesture.Width;
        }
    }
}
