using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace El_Cautivo
{
    class Animation
    {
        public float Delay;
        private List<Rectangle> Frames = new List<Rectangle>();
        DateTime currentTime;
        int CurrentFrame = 0;
        Texture2D sheet, reverceSheet;
        static bool isReverced;
        public Vector2 Scale => new Vector2(Frames[CurrentFrame].Width, Frames[CurrentFrame].Height);

        /// <summary>
        /// Creates an animation
        /// </summary>
        /// <param name="SpriteSheet">Texture2D with all of the frames in a row</param>
        /// <param name="Reverce">Same as previous but reverced</param>
        /// <param name="xIterations">The ammount of sprites placed horizontally</param>
        /// <param name="yIterations">The ammount of sprites placed vertically</param>
        /// <param name="delay">The delay between frames(in ms)</param>
        public Animation(Texture2D SpriteSheet, Texture2D Reverce, int xIterations, int yIterations, float delay)
        {
            sheet = SpriteSheet;
            reverceSheet = Reverce;
            Delay = delay;
            currentTime = DateTime.Now;

            
            var dX = SpriteSheet.Width / xIterations;
            var dY = SpriteSheet.Height / yIterations;

            for (var y = 0; y < yIterations; y++)
            {
                for (var x = 0; x < xIterations; x++)
                {
                    Frames.Add(new Rectangle(dX*x, dY*y, dX, dY));
                }
            }
        }

        public void Reverce() => isReverced = !isReverced;

        public void Update()
        {
            var DeltaTime = (DateTime.Now - currentTime).TotalMilliseconds;
            if (DeltaTime > Delay)
            {
                currentTime = DateTime.Now;
                CurrentFrame = CurrentFrame + 1 == Frames.Count() ? 0 : CurrentFrame + 1;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 scale)
        {
            spriteBatch.Draw(!isReverced?sheet:reverceSheet, position, Frames[CurrentFrame], Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
    }
}
