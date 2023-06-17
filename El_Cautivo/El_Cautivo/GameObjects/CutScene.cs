using El_Cautivo.EngineExtentions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace El_Cautivo.GameObjects
{
    public class CutScene : IObject
    {
        Tuple<string, string>[] Plot; //Name, content
        Dictionary<string, Texture2D> CharacterImages;
        int pos;
        Texture2D NextTexture, BG, voice;
        Vector2 NextPos = new Vector2(1614, 780);
        Button NextButton;
        Texture2D[] Chars;

        Action OnEnding;
        public CutScene(ContentManager content, Dictionary<string, Texture2D> characterImages, Tuple<string, string>[] plot, Action End)
        {
            LoadContent(content);
            CharacterImages = characterImages;
            Plot = plot;
            OnEnding = End;

            Chars = CharacterImages.Values.ToArray();
        }

        public void LoadContent(ContentManager content)
        {
            NextTexture = content.Load<Texture2D>("Buttons/BeginButton");
            voice = content.Load<Texture2D>("DialogWindow");
            BG = content.Load<Texture2D>("Lab");

            NextButton = new Button(NextPos / Game1.dScale, NextTexture, new Action(() => pos++), (int)(4 / Game1.dScale), Button.ButtonType.OnUp); 
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(BG, Vector2.Zero, new Rectangle(0, 0, 192, 108), Color.DarkGray, 0, Vector2.Zero, new Vector2(10, 10) / Game1.dScale, SpriteEffects.None, 0);
            batch.Draw(voice, Vector2.Zero, new Rectangle(0, 0, 192, 108), Color.White, 0, Vector2.Zero, new Vector2(10, 10) / Game1.dScale, SpriteEffects.None, 0);
            foreach (var i in Chars)
            {
                if (pos == Plot.Length) return;
                var isActive = CharacterImages[Plot[pos].Item1] == i;
                batch.Draw(i, (Vector2.Zero + (isActive?Vector2.Zero : new Vector2(0, 40))), new Rectangle(0, 0, 192, 108), Color.White, 0, Vector2.Zero, new Vector2(10, 10) / Game1.dScale, SpriteEffects.None, 0);
            }
            batch.DrawString(Game1.TitleFont, Plot[pos].Item2, Vector2.Zero, Color.White);
            NextButton.Draw(batch);
        }

        public void Update()
        {
            NextButton.Update();
            if (pos == Plot.Length) OnEnding();
        }
    }
}
