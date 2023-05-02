using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace El_Cautivo
{
    public interface IDrawable : IObject
    {
        public void Draw(SpriteBatch batch);
    }
    public interface IObject
    {
        public void Update();
    }
}
