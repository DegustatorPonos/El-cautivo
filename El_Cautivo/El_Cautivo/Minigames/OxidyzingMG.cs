using El_Cautivo.EngineExtentions;
using El_Cautivo.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace El_Cautivo.Minigames
{
    public class OxidyzingMG : BoilingMG
    {
        public override string GetActionTextureName() => "MGContent/Oxidizing/DoAction";
        public OxidyzingMG(Rectangle collider) : base(collider)
        {
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
