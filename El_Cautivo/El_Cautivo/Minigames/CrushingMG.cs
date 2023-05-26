using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using El_Cautivo.EngineExtentions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using El_Cautivo.GameObjects;

namespace El_Cautivo.Minigames
{
    public class CrushingMG : IMiniGame
    {
        Button BackButton, HitButton;
        Texture2D BackTexture, HitTexture;

        Dictionary<Game1.ChemElement, Game1.ChemElement> Recipes = new Dictionary<Game1.ChemElement, Game1.ChemElement>()
        {
            {Game1.ChemElement.Glicine, Game1.ChemElement.Crushed_Glicine},
            {Game1.ChemElement.Solid_meth, Game1.ChemElement.Methilamine}
        };

        Vector2 BackPos = new Vector2(10, 814), HitPos;

        Rectangle Collider;
        bool IsActive = false, IsColliding = false;
        int Hits = 0, losses = 0;
        Barrel toCrush;
        string ToDisplay = "F - use crushing table \n";

        public CrushingMG(Rectangle collider)
        {
            Collider = collider;
        }

        public void Draw(SpriteBatch batch)
        {
            if (IsActive)
            {
                BackButton.Draw(batch);
                HitButton.Draw(batch);
            }
            if (IsColliding) Lab.sb.Append(ToDisplay);
            if(Game1.ShowColliders) batch.Draw(Game1.ColliderTexture, new Vector2(Collider.X, Collider.Y), new Rectangle(0, 0, 1, 1),
                 Color.Green, 0, Vector2.Zero, new Vector2(Collider.Width, Collider.Height), SpriteEffects.None, 0);
        }

        public float GetState()
        {
            if(Hits >= 15) return (float)15/Hits;
            losses = (Hits - 15);
            return 1;
        }

        public void LoadContent(ContentManager content)
        {
            BackTexture = content.Load<Texture2D>("Buttons/BackButton");
            HitTexture = content.Load<Texture2D>("MGContent/Crushing/HitButton");

            HitPos = new Vector2((1920 - (HitTexture.Width * 2)), 1080 - (HitTexture.Height * 2)) / 2;

            BackButton = new Button(BackPos / Game1.dScale, BackTexture, OnBackButton, 4 / Game1.dScale, Button.ButtonType.OnUp);
            HitButton = new Button(HitPos / Game1.dScale, HitTexture, OnHitButton, 2 / Game1.dScale, Button.ButtonType.OnUp);
        }

        void OnBackButton()
        {
            if (toCrush != null && Hits != 0)
            {
                var state = GetState();
                Lab.objects.Add(new Barrel(Recipes[toCrush.Content], toCrush.Position, toCrush.Volume - losses, state));
                toCrush.Delete();
            }
            ToDisplay = "F - use crushing table \n";
            toCrush = null;
            Hits = 0;
            Game1.state = Game1.GameState.Game;
            IsActive = false;
        }

        void OnHitButton()
        {
            if (toCrush != null) Hits++;
        }

        public void Update()
        {
            IsColliding = Collider.Intersects(Jessie.Collider);
            if (IsColliding && !IsActive && Keyboard.GetState().IsKeyDown(Keys.F))
            {
                IsActive = true;
                Game1.state = Game1.GameState.MiniGame;
                if (toCrush == null) toCrush = Lab.objects.Where(b => (b.Content == Game1.ChemElement.Glicine || b.Content == Game1.ChemElement.Methilamine))
                        .Where(e => e.Collider.Intersects(this.Collider))
                        .FirstOrDefault();
                if (toCrush != null) ToDisplay = Game1.GetName[toCrush.Content] + " -> " + Game1.GetName[Recipes[toCrush.Content]];
                else ToDisplay = "Nothing to crush";
            }
            if (IsActive)
            {
                BackButton.Update();
                HitButton.Update();
            }
        }
    }
}
