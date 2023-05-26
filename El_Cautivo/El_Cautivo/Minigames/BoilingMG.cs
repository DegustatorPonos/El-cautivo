using El_Cautivo.EngineExtentions;
using El_Cautivo.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace El_Cautivo.Minigames
{
    public class BoilingMG : IMiniGame
    {
        Vector2 BackPos = new Vector2(10, 814), MixPos, OnOffPos;
        Texture2D BackTexture, MixTexture, OnTexture, OffTexture;
        Button BackButton, MixButton, OnOffButton; 
        Rectangle Collider;
        bool IsColliding = false, IsActive = false, isProcessing = false, OnOffSwich = false;
        string ToDisplay = "F - boil";
        float state = 1;
        TimeSpan TimeSave, Timer;
        Barrel firstElement, secondElement, Buffer;


        Dictionary<Tuple<Game1.ChemElement, Game1.ChemElement>, Game1.ChemElement> Recipies = new Dictionary<Tuple<Game1.ChemElement, Game1.ChemElement>, Game1.ChemElement>()
        {
            {new Tuple<Game1.ChemElement, Game1.ChemElement>(Game1.ChemElement.Glutamic_acid, Game1.ChemElement.Crushed_Glicine), Game1.ChemElement.Glutathionate}
        };

        public BoilingMG(Rectangle collider)
        {
            Collider = collider;
        }

        public void Draw(SpriteBatch batch)
        {
            if (IsColliding) Lab.sb.Append(ToDisplay);
            if (Game1.ShowColliders) batch.Draw(Game1.ColliderTexture, new Vector2(Collider.X, Collider.Y), new Rectangle(0, 0, 1, 1),
                 Color.Green, 0, Vector2.Zero, new Vector2(Collider.Width, Collider.Height), SpriteEffects.None, 0);
            if (!IsActive) return;
            BackButton.Draw(batch);
            MixButton.Draw(batch);
            OnOffButton.Draw(batch);
        }

        public float GetState() => state;

        public void LoadContent(ContentManager content)
        {
            BackTexture = content.Load<Texture2D>("Buttons/BackButton");
            MixTexture = content.Load<Texture2D>("MGContent/Boiling/MixButton");
            OnTexture = content.Load<Texture2D>("MGContent/OnButton");
            OffTexture = content.Load<Texture2D>("MGContent/OffButton");

            MixPos = new Vector2((1920/2), (1080-MixTexture.Height)/2);
            OnOffPos = new Vector2(1920-(OnTexture.Width*2), 1080-(OffTexture.Height*2));

            BackButton = new Button(BackPos / Game1.dScale, BackTexture, OnBackButton, 4 / Game1.dScale, Button.ButtonType.OnUp);
            MixButton = new Button(MixPos / Game1.dScale, MixTexture, OnMixButton, 2/Game1.dScale, Button.ButtonType.OnUp);
            OnOffButton = new Button(OnOffPos / Game1.dScale, OffTexture, OnOnOffButton, 2 / Game1.dScale, Button.ButtonType.OnUp);
        }

        private void OnBackButton()
        {
            Game1.state = Game1.GameState.Game;
            IsActive = false;
        }

        private void OnMixButton()
        {
            if (!isProcessing)
            {
                if (!FindIngredients()) return;
                OnOnOffButton();
                isProcessing = true;
                Buffer = new Barrel(Recipies[new Tuple<Game1.ChemElement, Game1.ChemElement>(firstElement.Content, secondElement.Content)], firstElement.Position,
                    Math.Min(firstElement.Volume, secondElement.Volume), firstElement.Quality * secondElement.Quality);
                Lab.objects.Remove(firstElement);
                Lab.objects.Remove(secondElement);
                Timer = Lab.InGameTime + new TimeSpan(0, 45, 0);
                TimeSave = Lab.InGameTime + new TimeSpan(0, 10, 0); //To mix
                return;
            }
            TimeSave = Lab.InGameTime + new TimeSpan(0, 10, 0);
        }

        void OnOnOffButton()
        {
            OnOffSwich = !OnOffSwich;
            OnOffButton.ChangeTexture(OnOffSwich?OnTexture:OffTexture);
            if (!isProcessing) return;
            if (!OnOffSwich)
            {
                var overtime = Math.Abs(Lab.InGameTime.TotalMinutes - TimeSave.TotalMinutes);
                if (state != 0 && overtime > 0)
                    state /=(float)overtime;
                Buffer.Quality *= state;
                Lab.objects.Add(Buffer);
                OnBackButton();
            }
        }

        bool FindIngredients()
        {
            var CollidingBarrels = Lab.objects.Where(x => Collider.Intersects(x.Collider)).ToArray();
            if (CollidingBarrels.Length < 2) return false;
            firstElement = CollidingBarrels.Where(x => x.Content == Game1.ChemElement.Glutamic_acid).FirstOrDefault();
            if (firstElement == null) return false;
            secondElement = CollidingBarrels.Where(x => x.Content == Game1.ChemElement.Crushed_Glicine).FirstOrDefault();
            if (secondElement != null)
                return true;
            firstElement = null;
            return false;
        }

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
                MixButton.Update();
                BackButton.Update();
                OnOffButton.Update();
            }
            if (isProcessing)
            {
                if (Lab.InGameTime > TimeSave)
                    state = 0;
            }
        }
    }
}
