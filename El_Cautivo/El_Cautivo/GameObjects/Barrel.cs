using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using El_Cautivo.EngineExtentions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace El_Cautivo.GameObjects
{
    public class Barrel : IDrawable, IObject
    {
        public Game1.ChemElement Content;
        public Vector2 Position;
        public Rectangle Collider;
        public double Volume;
        Button BButton;
        bool isPressed = false;
        private string Info;
        Texture2D Texture;
        public float Quality;


        public Barrel(Game1.ChemElement cont, Vector2 pos, double vol, float quality = 1)
        {
            Position = pos;
            Content = cont;
            Volume = vol;
            Quality = quality;
            UpdateInfo();
            switch (Content)
            {
                case Game1.ChemElement.Aluminum_dust or Game1.ChemElement.Crushed_Glicine:
                    Texture = Game1.BagTexture;
                    break;
                /*case Game1.ChemElement.Glicine:
                    ;
                    break;*/
                default:
                    Texture = Game1.BarrelTexture;
                    break;
            }
            Collider = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width * 6 / Game1.dScale,
                Texture.Height * 6 / Game1.dScale);
            BButton =  new Button(Position, Texture, new Action(() => isPressed = true), 6 / Game1.dScale) {CoveringColor = Color.White};
        }
        public void Draw(SpriteBatch batch)
        {
            BButton.Draw(batch);
            if(Game1.ShowColliders) batch.Draw(Game1.ColliderTexture, new Vector2(Collider.X, Collider.Y), new Rectangle(0, 0, 1, 1),
                Color.White, 0, Vector2.Zero, new Vector2(Collider.Width, Collider.Height), SpriteEffects.None, 0);
            if (isPressed) Lab.sb.Append(Info);
        }

        public void Update()
        {
            BButton.Position = Position;
            isPressed = false;
            BButton.Update();
            if(Jessie.HoldingObject == this) Jessie.HoldingObject = null;

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && Collider.Intersects(Jessie.Collider))
            {
                if (Jessie.HoldingObject != null) return;
                Jessie.HoldingObject = this;
                Position.X = Jessie.Collider.X;
                Position.Y = Jessie.Collider.Y;
                Collider.Location = Jessie.Collider.Location;
            }
        }

        public void Delete()
        {
            Lab.objects.Remove(this);
        }

        public void UpdateInfo() => Info = Game1.GetName[Content] + " (" + Volume + "L) \n";
    }
}
