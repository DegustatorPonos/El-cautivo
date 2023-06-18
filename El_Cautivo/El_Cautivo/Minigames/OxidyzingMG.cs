using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace El_Cautivo.Minigames
{
    public class OxidyzingMG : BoilingMG
    {
        public override string GetActionTextureName() => "MGContent/Oxidizing/OxidyzeButton";
        public OxidyzingMG(Rectangle collider) : base(collider)
        {
            base.sedActDescription("Use oxiyzer");
        }
        public override Dictionary<Tuple<Game1.ChemElement, Game1.ChemElement>, Tuple<Game1.ChemElement, TimeSpan>> GetRecipies()
        {
            return new Dictionary<Tuple<Game1.ChemElement, Game1.ChemElement>, Tuple<Game1.ChemElement, TimeSpan>>()
            {
                {new Tuple<Game1.ChemElement, Game1.ChemElement>(Game1.ChemElement.Phosphoric_acid, Game1.ChemElement.Cumeine),
                    new Tuple<Game1.ChemElement, TimeSpan>(Game1.ChemElement.Cumene_hydroperoxide, new TimeSpan(2, 0, 0))}
            };
        }

        public override void Action()
        {
            return;
        }
    }
}
