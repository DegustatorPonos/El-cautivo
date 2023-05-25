using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using El_Cautivo.Menus;
using Microsoft.Xna.Framework.Input;
using El_Cautivo.GameObjects;
using El_Cautivo.Minigames;
using Microsoft.Xna.Framework.Content;

namespace El_Cautivo
{
    static class Lab
    {
        public static Texture2D Surroundings, HintDots;

        #region Colliders
        static Rectangle LeftBorder, RightBorder,
            NoteTable, SplittingTable, Refigirator, StorageTable, StorageRoom,
            CoffeTable, MixingTable, Vaporizer, SmashingTable, DistillingTable;

        public static void InitColliders()
        {
            LeftBorder = new Rectangle(0, 0, 190 / Game1.dScale, 1080 / Game1.dScale);
            RightBorder = new Rectangle((1920 - 190) / Game1.dScale, 0, 190 / Game1.dScale, 1080 / Game1.dScale);

            NoteTable = new Rectangle(190 / Game1.dScale, 0, 290 / Game1.dScale, 250 / Game1.dScale);
            SplittingTable = new Rectangle(480 / Game1.dScale, 0, 440 / Game1.dScale, 240 / Game1.dScale);
            Refigirator = new Rectangle(920 / Game1.dScale, 0, 200 / Game1.dScale, 250 / Game1.dScale);
            StorageTable = new Rectangle(1120 / Game1.dScale, 0, 370 / Game1.dScale, 220 / Game1.dScale);
            StorageRoom = new Rectangle(1490 / Game1.dScale, 0, 410 / Game1.dScale, 10 / Game1.dScale);

            CoffeTable = new Rectangle(190 / Game1.dScale, (1080 - 300) / Game1.dScale, 280 / Game1.dScale, 300 / Game1.dScale);
            MixingTable = new Rectangle(470 / Game1.dScale, (1080 - 290) / Game1.dScale, 350 / Game1.dScale, 290 / Game1.dScale);
            Vaporizer = new Rectangle(820 / Game1.dScale, (1080 - 360) / Game1.dScale, 310 / Game1.dScale, 360 / Game1.dScale);
            SmashingTable = new Rectangle(1130 / Game1.dScale, (1080 - 300) / Game1.dScale, 330 / Game1.dScale, 300 / Game1.dScale);
            DistillingTable = new Rectangle(1460 / Game1.dScale, (1080 - 300) / Game1.dScale, 270 / Game1.dScale, 300 / Game1.dScale);
            Colliders = new Rectangle[] { LeftBorder, RightBorder,
                NoteTable, SplittingTable, Refigirator, StorageTable, StorageRoom,
                CoffeTable, MixingTable, Vaporizer, SmashingTable, DistillingTable };
            MGs.Clear();
            InitLab();
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

        public static List<IObject> objects = new List<IObject>();
        public static List<IMiniGame> MGs = new List<IMiniGame>();

        public static void InitLab()
        {
            MGs.Add(new Guide(new Rectangle(190 / Game1.dScale, 250 / Game1.dScale, 290 / Game1.dScale, 250 / Game1.dScale)));
            //MGs.Add(new CrushingMG(new Rectangle(1130 / Game1.dScale, (1080 - 400) / Game1.dScale, 330 / Game1.dScale, 100 / Game1.dScale)));
        }
        
        public static void LoadContent(ContentManager Content)
        {
            HintDots = Content.Load<Texture2D>("Dots");
            foreach (var i in MGs)
                i.LoadContent(Content);
        }

        public static void BeginDay()
        {
            objects.Clear();
            InitObjects();
        }

        static void InitObjects()
        {
            objects.Add(new Barrel(Game1.ChemElement.Glutamic_acid, new Vector2(1580, 200) / Game1.dScale, 50, objects));
            objects.Add(new Barrel(Game1.ChemElement.Phosphoric_acid, new Vector2(1680, 150) / Game1.dScale, 50, objects));
            objects.Add(new Barrel(Game1.ChemElement.Methilamine, new Vector2(1620, 230) / Game1.dScale, 50, objects));
            objects.Add(new Barrel(Game1.ChemElement.Aluminum_dust, new Vector2(1620, 100) / Game1.dScale, 50, objects));
        }

        public static void Draw(SpriteBatch batch)
        {
            batch.Draw(Surroundings, Vector2.Zero, new Rectangle(0, 0, 192, 168), Color.White, 0, Vector2.Zero, new Vector2(10, 10)/Game1.dScale, SpriteEffects.None, 0);
            if (Keyboard.GetState().IsKeyDown(Keys.H))
                batch.Draw(HintDots, Vector2.Zero, new Rectangle(0, 0, 192, 168), Color.White, 0, Vector2.Zero, new Vector2(10, 10) / Game1.dScale, SpriteEffects.None, 0);
            foreach (var i in objects)
                i.Draw(batch);
            Jessie.Draw(batch);
            foreach (var mg in MGs)
                mg.Draw(batch);
            if (Game1.ShowColliders) DrawColliders(batch);
            batch.DrawString(Game1.TitleFont, sb.ToString(), Vector2.Zero, Color.White);
            sb.Clear();
        }
        public static StringBuilder sb = new StringBuilder();
        public static void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.K))
                Game1.ShowColliders = !Game1.ShowColliders;
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Game1.state = Game1.GameState.ExitMenu;
            foreach (var i in objects)
                i.Update();
            foreach (var mg in MGs)
                mg.Update();

        }
    }
}
