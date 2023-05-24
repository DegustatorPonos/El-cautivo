using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using El_Cautivo.Menus;
using Microsoft.Xna.Framework.Input;

namespace El_Cautivo
{
    static class Lab
    {
        public static Texture2D Surroundings;

        #region Colliders
        static Rectangle LeftBorder, RightBorder,
            NoteTable, SplittingTable, Refigirator, StorageTable,
            CoffeTable, MixingTable, Vaporizer, SmashingTable, DistillingTable;

        public static void InitColliders()
        {
            LeftBorder = new Rectangle(0, 0, 190 / Game1.dScale, 1080 / Game1.dScale);
            RightBorder = new Rectangle((1920 - 190) / Game1.dScale, 0, 190 / Game1.dScale, 1080 / Game1.dScale);

            NoteTable = new Rectangle(190 / Game1.dScale, 0, 290 / Game1.dScale, 250 / Game1.dScale);
            SplittingTable = new Rectangle(480 / Game1.dScale, 0, 440 / Game1.dScale, 240 / Game1.dScale);
            Refigirator = new Rectangle(920 / Game1.dScale, 0, 200 / Game1.dScale, 250 / Game1.dScale);
            StorageTable = new Rectangle(1120 / Game1.dScale, 0, 610 / Game1.dScale, 220 / Game1.dScale);

            CoffeTable = new Rectangle(190 / Game1.dScale, (1080 - 300) / Game1.dScale, 280 / Game1.dScale, 300 / Game1.dScale);
            MixingTable = new Rectangle(470 / Game1.dScale, (1080 - 290) / Game1.dScale, 350 / Game1.dScale, 290 / Game1.dScale);
            Vaporizer = new Rectangle(820 / Game1.dScale, (1080 - 360) / Game1.dScale, 310 / Game1.dScale, 360 / Game1.dScale);
            SmashingTable = new Rectangle(1130 / Game1.dScale, (1080 - 300) / Game1.dScale, 330 / Game1.dScale, 300 / Game1.dScale);
            DistillingTable = new Rectangle(1460 / Game1.dScale, (1080 - 280) / Game1.dScale, 270 / Game1.dScale, 280 / Game1.dScale);
            Colliders = new Rectangle[] { LeftBorder, RightBorder,
                NoteTable, SplittingTable, Refigirator, StorageTable,
                CoffeTable, MixingTable, Vaporizer, SmashingTable, DistillingTable };
            GC.Collect(); //Delete all previous values
        }

        public static Rectangle[] Colliders;

        static void DrawColliders(SpriteBatch batch)
        {
            foreach (var i in Colliders)
            {
                batch.Draw(Game1.ColliderTexture, new Vector2(i.X, i.Y), new Rectangle(0, 0, 1, 1), Color.White, 0, Vector2.Zero, new Vector2(i.Width, i.Height), SpriteEffects.None, 0);
            }
        }
        #endregion
        public static void Draw(SpriteBatch batch)
        {
            
            batch.Draw(Surroundings, Vector2.Zero, new Rectangle(0, 0, 192, 168), Color.White, 0, Vector2.Zero, new Vector2(10, 10)/Game1.dScale, SpriteEffects.None, 0);
            if (Game1.ShowColliders) DrawColliders(batch);
        }
        public static void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.K))
                Game1.ShowColliders = !Game1.ShowColliders;
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Game1.state = Game1.GameState.ExitMenu;
        }
    }
}
