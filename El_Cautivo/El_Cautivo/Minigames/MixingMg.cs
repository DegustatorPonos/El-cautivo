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

namespace El_Cautivo.Minigames
{
    public class MixingMg : IMiniGame
    {
        Vector2 BackPos = new Vector2(10, 814),
            DataTextPos = new Vector2(400, 300),
            plusPos, minusPos, UpPos, DownPos;
        Button BackButton, PlusButton, minusButton, UpButton, DownButton;
        Texture2D BackTexture, BG_Texture, Plustexture, MinusTexture, UpTexture, DownTexture;
        Rectangle Collider;
        bool IsColliding = false, IsActive = false, canDoSmth = false;
        string ToDisplay = "F - use mixing table";
        Game1.ChemElement[] inputs;
        Barrel[] toMerge = new Barrel[0];
        int[] values = new int[0];
        int position = 0;
        StringBuilder builder = new StringBuilder();


        Dictionary<Game1.ChemElement[], Game1.ChemElement> Recipes = new Dictionary<Game1.ChemElement[], Game1.ChemElement>()
        {
            {   new Game1.ChemElement[] { Game1.ChemElement.Glutathionate, Game1.ChemElement.Cumene_hydroperoxide},
                Game1.ChemElement.P2P_Wet },
            {new Game1.ChemElement[] { Game1.ChemElement.P2P, Game1.ChemElement.Methilamine, Game1.ChemElement.Aluminum_dust},
                Game1.ChemElement.Liquid_Meth },
        };

        Dictionary<Game1.ChemElement[], int[]> TargetRaios = new Dictionary<Game1.ChemElement[], int[]>();

        public MixingMg(Rectangle collider)
        {
            Collider = collider;
            var temp = Recipes.Keys.ToArray();
            TargetRaios.Add(temp[0], new int[] { 2, 3});
            TargetRaios.Add(temp[1], new int[] {10, 7, 1});
        }

        public void Draw(SpriteBatch batch)
        {
            if (IsColliding) Lab.sb.Append(ToDisplay);
            if (Game1.ShowColliders) batch.Draw(Game1.ColliderTexture, new Vector2(Collider.X, Collider.Y), new Rectangle(0, 0, 1, 1),
                 Color.Green, 0, Vector2.Zero, new Vector2(Collider.Width, Collider.Height), SpriteEffects.None, 0);
            if (!IsActive) return;
            batch.Draw(BG_Texture, Vector2.Zero, new Rectangle(0, 0, 192, 108), Color.White, 0, Vector2.Zero, new Vector2(10, 10) / Game1.dScale, SpriteEffects.None, 0);
            BackButton.Draw(batch);
            PlusButton.Draw(batch);
            minusButton.Draw(batch);
            UpButton.Draw(batch);
            DownButton.Draw(batch);

            if (!canDoSmth) builder.Append("No recipe found");
            else builder.Append(GetInfo());
            batch.DrawString(Game1.TitleFont, builder.ToString(), DataTextPos / Game1.dScale, Color.White);
            builder.Clear();
        }

        public float GetState()
        {
            float outp = 0;
            for (int i = 0; i < toMerge.Length; i++)
            {
                outp += Math.Abs(values[i] - TargetRaios[inputs][i]);
            }
            return outp * 0.01f;
        }

        public void LoadContent(ContentManager content)
        {
            BG_Texture = content.Load<Texture2D>("MGContent/Mixing/BG");
            Plustexture = content.Load<Texture2D>("MGContent/Mixing/plus");
            UpTexture = content.Load<Texture2D>("MGContent/Mixing/Up");
            DownTexture = content.Load<Texture2D>("MGContent/Mixing/Down");
            MinusTexture = content.Load<Texture2D>("MGContent/Mixing/Minus");
            BackTexture = content.Load<Texture2D>("Buttons/BackButton");

            plusPos = new Vector2(1920 - 10 - Plustexture.Width*2, 1080 - 10 - Plustexture.Height*2);
            minusPos = new Vector2(plusPos.X, plusPos.Y - 10 - MinusTexture.Height * 2);
            DownPos = new Vector2(minusPos.X, minusPos.Y - 10 - MinusTexture.Height * 2);
            UpPos = new Vector2(minusPos.X, DownPos.Y - 10 - MinusTexture.Height * 2);


            BackButton = new Button(BackPos / Game1.dScale, BackTexture, OnBackButton, (int)(4 / Game1.dScale), Button.ButtonType.OnUp);
            PlusButton = new Button(plusPos / Game1.dScale, Plustexture, Add, (int)(2 / Game1.dScale), Button.ButtonType.OnUp);
            minusButton = new Button(minusPos/ Game1.dScale, MinusTexture, Subtract, (int)(2 / Game1.dScale), Button.ButtonType.OnUp);
            UpButton = new Button(UpPos / Game1.dScale, UpTexture, Up, (int)(2 / Game1.dScale), Button.ButtonType.OnUp);
            DownButton = new Button(DownPos / Game1.dScale, DownTexture, Down, (int)(2 / Game1.dScale), Button.ButtonType.OnUp);
        }

        private void Up() => position = Math.Max(0, position - 1);

        private void Down() => position = Math.Min(toMerge.Length - 1, position + 1);

        private void Subtract() => values[position] = Math.Max(0, values[position] - 1);

        private void Add() => values[position] = Math.Min(values[position] + 1, (int)toMerge[position].Volume);

        private void OnBackButton()
        {
            if (canDoSmth)
            {
                double targetQuality = 1;
                int TargetVolume = 0;
                var divider = values.Min();
                Vector2 targetPos = toMerge.First().Position;
                for (int i = 0; i < toMerge.Length; i++)
                {
                    targetQuality *= toMerge[i].Quality;
                    TargetVolume += values[i];
                    Lab.objects.Remove(toMerge[i]);
                    values[i] /= divider;
                }
                targetQuality = Math.Max(0, targetQuality - GetState());
                Lab.objects.Add(new Barrel(Recipes[inputs], targetPos, TargetVolume, (float)targetQuality));
                values = null;
                toMerge = null;
                position = 0;
                inputs = null;
            }
            IsActive = false;
            Game1.state = Game1.GameState.Game;
        }

        public void Update()
        {
            IsColliding = Collider.Intersects(Jessie.Collider);
            if (IsColliding && !IsActive && Keyboard.GetState().IsKeyDown(Keys.F))
            {
                IsActive = true;
                canDoSmth = GetComponents();
                Game1.state = Game1.GameState.MiniGame;
            }
            if (!IsActive) return;
            BackButton.Update();
            if (!canDoSmth) return;
            PlusButton.Update();
            minusButton.Update();
            UpButton.Update();
            DownButton.Update();
        }

        bool GetComponents()
        {
            var CollidedBarrels = Lab.objects.Where(b => b.Collider.Intersects(Collider));
            var buffer = new List<Barrel>();
            foreach (var recipe in Recipes.Keys)
            {
                foreach (var element in recipe)
                {
                    var temp = CollidedBarrels.Where(x => x.Content == element).ToArray();
                    if (temp.Length > 0) buffer.Add(temp.First());
                }
                if (buffer.Count() == recipe.Length)
                {
                    inputs = recipe;
                    toMerge = buffer.ToArray();
                    values = new int[recipe.Length];
                    return true;
                }
            }
            return false;
        }
        string GetInfo()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < toMerge.Length; i++)
            {
                sb.Append(i == position ? "> ": "  ");
                sb.Append(Game1.GetName[toMerge[i].Content]);
                sb.Append(" * ");
                sb.Append(values[i]);
                sb.Append("\n");
            }
            return sb.ToString();
        }
    }
}
