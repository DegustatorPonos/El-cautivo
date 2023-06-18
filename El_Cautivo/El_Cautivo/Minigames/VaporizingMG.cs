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
    public  class VaporizingMG : IMiniGame
    {
        int temperature;
        Vector2 BackPos = new Vector2(10, 814), ActTexture, OnOffPos, TSliderPos = new Vector2(10, 1080/2);
        Texture2D BackTexture, ActionTexture, OnTexture, OffTexture, SliderTexture, SliderBasisTexture;
        Slider tempSlider;
        Button BackButton, ActionButton, OnOffButton;
        Rectangle Collider;
        bool IsColliding = false, IsActive = false, isProcessing = false, OnOffSwich = false;
        string ToDisplay = "F - use boiler";
        int maxTemperature = 250;
        public virtual string GetActionTextureName() => "MGContent/Vaporizer/Boil";
        float state = 1;
        TimeSpan Timer;
        Barrel Input, Buffer;


        Dictionary<Game1.ChemElement, Tuple<Game1.ChemElement, TimeSpan, int>> Recipies;


        Game1.ChemElement[] AvaliableInputs => Recipies.Keys.ToArray();

        public VaporizingMG(Rectangle collider)
        {
            Collider = collider;
            Recipies = GetRecipies();
        }

        public virtual Dictionary<Game1.ChemElement, Tuple<Game1.ChemElement, TimeSpan, int>> GetRecipies()
        {
            return new Dictionary<Game1.ChemElement, Tuple<Game1.ChemElement, TimeSpan, int>>()
            {
                {Game1.ChemElement.P2P_Wet,
                    new Tuple<Game1.ChemElement, TimeSpan, int>(Game1.ChemElement.P2P, new TimeSpan(1, 45, 0), 145)}
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
            tempSlider.Draw(batch);
        }

        public virtual float GetState() => (float) (0.01 * Math.Abs(Recipies[Input.Content].Item3 - temperature));

        public virtual void LoadContent(ContentManager content)
        {
            BackTexture = content.Load<Texture2D>("Buttons/BackButton");
            ActionTexture = content.Load<Texture2D>(GetActionTextureName());
            OnTexture = content.Load<Texture2D>("MGContent/OnButton");
            OffTexture = content.Load<Texture2D>("MGContent/OffButton");
            SliderTexture = content.Load<Texture2D>("SliderPointer");
            SliderBasisTexture = content.Load<Texture2D>("SliderBasis");

            ActTexture = new Vector2((1920 / 2), (1080 - ActionTexture.Height) / 2);
            OnOffPos = new Vector2(1920 - (OnTexture.Width * 2), 1080 - (OffTexture.Height * 2));

            tempSlider = new Slider(SliderBasisTexture, SliderTexture, TSliderPos / Game1.dScale);
            BackButton = new Button(BackPos / Game1.dScale, BackTexture, OnBackButton, (int)(4 / Game1.dScale), Button.ButtonType.OnUp);
            ActionButton = new Button(ActTexture / Game1.dScale, ActionTexture, OnActionButton, (int)(2 / Game1.dScale), Button.ButtonType.OnUp);
            OnOffButton = new Button(OnOffPos / Game1.dScale, OffTexture, OnOnOffButton, (int)(2 / Game1.dScale), Button.ButtonType.OnUp);
        }

        private void OnBackButton()
        {
            IsActive = false;
            ToDisplay = "F - use boiler";
            Game1.state = Game1.GameState.Game;
        }

        void OnActionButton()
        {
            if (isProcessing) return;
                if (!FindIngredients()) return;
                OnOnOffButton();
                isProcessing = true;
                Buffer = new Barrel(Recipies[Input.Content].Item1, Input.Position,
                    (int)(Input.Volume * 2 / 3), Input.Quality);
                Lab.objects.Remove(Input);
                Timer = Lab.InGameTime + Recipies[Input.Content].Item2;
        }
        void OnOnOffButton()
        {
            OnOffSwich = !OnOffSwich;
            OnOffButton.ChangeTexture(OnOffSwich ? OnTexture : OffTexture);
            if (!isProcessing) return;
            if (!OnOffSwich)
            {
                var overtime = Math.Abs(Lab.InGameTime.TotalMinutes - Timer.TotalMinutes) / 100;
                if (state != 0 && overtime > 0)
                    state -= (float)overtime;
                Buffer.Quality *= GetState();
                Input = null;
                Lab.objects.Add(Buffer);
                OnBackButton();
            }
        }

        bool FindIngredients()
        {
            var CollidingBarrels = Lab.objects.Where(x => Collider.Intersects(x.Collider)).ToArray();
            if (CollidingBarrels.Length < 1) return false;
            Input = CollidingBarrels.Where(x => AvaliableInputs.Contains(x.Content)).FirstOrDefault();
            if (Input == null) return false;
            return true;
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
                OnOffButton.Update();
                tempSlider.Update();
                temperature = (int)(maxTemperature * tempSlider.Value);
                ToDisplay = "Temperature = " + temperature;
                BackButton.Update();
            }
        }
    }
}
