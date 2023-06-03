using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;

namespace El_Cautivo.Menus
{
    public static class Ending
    {

        public static Action exit;
        static Vector2 position;
        static Texture2D titles;
        static bool hasBegun = false;
        static TimeSpan targetTime, deltaTime = new TimeSpan(0, 0, 0, 0, 500/25), toFinish;
        static SoundEffect bgm;

        public static void LoadContent(ContentManager Content)
        {
            titles = Content.Load<Texture2D>("End");
            bgm = Content.Load<SoundEffect>("Audio/The_end");
        }
        public static void Begin()
        {
            Game1.InitBGM(bgm);
            hasBegun = true;
            toFinish = DateTime.Now.TimeOfDay + bgm.Duration;
            targetTime = DateTime.Now.TimeOfDay + deltaTime;
            position = new Vector2(0, 1080 / Game1.dScale);
        }

        public static void Update()
        {
            if (!hasBegun) return;
            if (DateTime.Now.TimeOfDay > targetTime || targetTime - DateTime.Now.TimeOfDay > deltaTime)
            {
                position.Y -= 0.5f;
            }
            if (DateTime.Now.TimeOfDay > toFinish) exit();
        }

        public static void Draw(SpriteBatch batch)
        {
            batch.Draw(titles, position, new Rectangle(0, 0, titles.Width, titles.Height), Color.White, 0, Vector2.Zero, new Vector2(2, 2) / Game1.dScale, SpriteEffects.None, 0);
        }
    }
}
