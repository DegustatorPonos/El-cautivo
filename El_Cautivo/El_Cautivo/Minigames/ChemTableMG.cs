using El_Cautivo.EngineExtentions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using El_Cautivo.GameObjects;

namespace El_Cautivo.Minigames
{
    public class ChemTableMG : IMiniGame
    {
        Vector2 BackPos = new Vector2(10, 814), CumPos, GlicPos;
        Button BackButton, GetCumeine, GetGlicine;
        Texture2D BackTexture, GetCumeineTexture, GetGlicineTexture;
        Rectangle Collider;
        bool IsColliding = false, IsActive = false;
        string ToDisplay = "F - take chemicals";

        public ChemTableMG(Rectangle collider)
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
                GetGlicine.Draw(batch);
                GetCumeine.Draw(batch);
            }
        }

        public float GetState()
        {
            return 1;
        }

        public void LoadContent(ContentManager content)
        {
            BackTexture = content.Load<Texture2D>("Buttons/BackButton");
            GetCumeineTexture = content.Load<Texture2D>("MGContent/ChemTable/Take_Cumeine");
            GetGlicineTexture = content.Load<Texture2D>("MGContent/ChemTable/Take_Glicine");

            CumPos = new Vector2(10, (1080 - (GetCumeineTexture.Height * 2)) / 2);
            GlicPos = new Vector2((1920 - GetGlicineTexture.Width*2) - 10, (1080 - (GetCumeineTexture.Height * 2)) / 2);

            BackButton = new Button(BackPos / Game1.dScale, BackTexture, OnBackButton, 4 / Game1.dScale, Button.ButtonType.OnUp);
            GetGlicine = new Button(GlicPos / Game1.dScale, GetGlicineTexture, SummonGlicine, 2 / Game1.dScale, Button.ButtonType.OnUp);
            GetCumeine = new Button(CumPos / Game1.dScale, GetCumeineTexture, SummonCumeine, 2 / Game1.dScale, Button.ButtonType.OnUp);
        }

        void OnBackButton()
        {
            Game1.state = Game1.GameState.Game;
            IsActive = false;
        }

        void SummonGlicine() => Lab.objects.Add(new Barrel(Game1.ChemElement.Glicine, new Vector2(Collider.X, Collider.Y), 10));

        void SummonCumeine() => Lab.objects.Add(new Barrel(Game1.ChemElement.Cumeine, new Vector2(Collider.X, Collider.Y), 12));
        public void Update()
        {
            IsColliding = Collider.Intersects(Jessie.Collider);
            if (IsColliding && !IsActive && Keyboard.GetState().IsKeyDown(Keys.F))
            {
                IsActive = true;
                Game1.state = Game1.GameState.MiniGame;
            }
            if (IsActive)
            {
                BackButton.Update();
                GetCumeine.Update();
                GetGlicine.Update();
            }
        }
    }
}
