using El_Cautivo.EngineExtentions;
using El_Cautivo.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace El_Cautivo.Minigames
{
    public class BoilingMG : IMiniGame
    {
        Vector2 BackPos = new Vector2(10, 814), ActTexture, OnOffPos;
        Texture2D BackTexture, ActionTexture, OnTexture, OffTexture;
        Button BackButton, ActionButton, OnOffButton; 
        Rectangle Collider;
        bool IsColliding = false, IsActive = false, isProcessing = false, OnOffSwich = false;
        string ToDisplay = "F - use boiler";
        public virtual string GetActionTextureName() => "MGContent/Boiling/Heat";
        float state = 1;
        TimeSpan TimeSave, Timer;
        Barrel firstElement, secondElement, Buffer;


        Dictionary<Tuple<Game1.ChemElement, Game1.ChemElement>, Tuple<Game1.ChemElement, TimeSpan>> Recipies;


        Game1.ChemElement[] firstElements => Recipies.Keys.Select(x => x.Item1).ToArray();
        Game1.ChemElement[] secondElements => Recipies.Keys.Select(x => x.Item2).ToArray();

        public BoilingMG(Rectangle collider)
        {
            Collider = collider;
            Recipies = GetRecipies();
        }

        public virtual Dictionary<Tuple<Game1.ChemElement, Game1.ChemElement>, Tuple<Game1.ChemElement, TimeSpan>> GetRecipies()
        {
            return new Dictionary<Tuple<Game1.ChemElement, Game1.ChemElement>, Tuple<Game1.ChemElement, TimeSpan>>()
            {
                {new Tuple<Game1.ChemElement, Game1.ChemElement>(Game1.ChemElement.Glutamic_acid, Game1.ChemElement.Crushed_Glicine),
                    new Tuple<Game1.ChemElement, TimeSpan>(Game1.ChemElement.Glutathionate, new TimeSpan(0, 45, 0))}
            };
        }

        public void Draw(SpriteBatch batch)
        {
            if (IsColliding) Lab.sb.Append(ToDisplay);
            if (Game1.ShowColliders) batch.Draw(Game1.ColliderTexture, new Vector2(Collider.X, Collider.Y), new Rectangle(0, 0, 1, 1),
                 Color.Green, 0, Vector2.Zero, new Vector2(Collider.Width, Collider.Height), SpriteEffects.None, 0);
            if (!IsActive) return;
            BackButton.Draw(batch);
            ActionButton.Draw(batch);
            OnOffButton.Draw(batch);
        }

        public virtual float GetState() => state;

        public virtual void LoadContent(ContentManager content)
        {
            BackTexture = content.Load<Texture2D>("Buttons/BackButton");
            ActionTexture = content.Load<Texture2D>(GetActionTextureName());
            OnTexture = content.Load<Texture2D>("MGContent/OnButton");
            OffTexture = content.Load<Texture2D>("MGContent/OffButton");

            ActTexture = new Vector2((1920/2), (1080-ActionTexture.Height)/2);
            OnOffPos = new Vector2(1920-(OnTexture.Width*2), 1080-(OffTexture.Height*2));

            BackButton = new Button(BackPos / Game1.dScale, BackTexture, OnBackButton, (int)(4 / Game1.dScale), Button.ButtonType.OnUp);
            ActionButton = new Button(ActTexture / Game1.dScale, ActionTexture, OnActionButton, (int)(2 /Game1.dScale), Button.ButtonType.OnUp);
            OnOffButton = new Button(OnOffPos / Game1.dScale, OffTexture, OnOnOffButton, (int)(2 / Game1.dScale), Button.ButtonType.OnUp);
        }

        private void OnBackButton()
        {
            Game1.state = Game1.GameState.Game;
            IsActive = false;
        }

        void OnActionButton()
        {
            if (!isProcessing)
            {
                if (!FindIngredients()) return;
                OnOnOffButton();
                isProcessing = true;
                Buffer = new Barrel(Recipies[new Tuple<Game1.ChemElement, Game1.ChemElement>(firstElement.Content, secondElement.Content)].Item1, firstElement.Position,
                    Math.Min(firstElement.Volume, secondElement.Volume), firstElement.Quality * secondElement.Quality);
                Lab.objects.Remove(firstElement);
                Lab.objects.Remove(secondElement);
                Timer = Lab.InGameTime + Recipies[new Tuple<Game1.ChemElement, Game1.ChemElement>(firstElement.Content, secondElement.Content)].Item2;
            }
            Action();
        }
        public virtual void Action()
        {
            TimeSave = Lab.InGameTime + new TimeSpan(0, 10, 0);
        }
        void OnOnOffButton()
        {
            OnOffSwich = !OnOffSwich;
            OnOffButton.ChangeTexture(OnOffSwich?OnTexture:OffTexture);
            if (!isProcessing) return;
            if (!OnOffSwich)
            {
                var overtime = Math.Abs(Lab.InGameTime.TotalMinutes - Timer.TotalMinutes) /100;
                if (state != 0 && overtime > 0)
                    state -= (float)overtime;
                Buffer.Quality *= state;
                firstElement = null;
                secondElement = null;
                Lab.objects.Add(Buffer);
                OnBackButton();
            }
        }
        public virtual void sedActDescription(string descr) => ToDisplay = descr;
        bool FindIngredients()
        {
            var CollidingBarrels = Lab.objects.Where(x => Collider.Intersects(x.Collider)).ToArray();
            if (CollidingBarrels.Length < 2) return false;
            firstElement = CollidingBarrels.Where(x => firstElements.Contains(x.Content)).FirstOrDefault();
            if (firstElement == null) return false;
            secondElement = CollidingBarrels.Where(x => secondElements.Contains(x.Content)).FirstOrDefault();
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
                ActionButton.Update();
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
