using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace El_Cautivo
{
    public interface IDrawable
    {
        public void Draw(SpriteBatch batch);
    }
    public interface IObject : IDrawable
    {

        public void Update();
        
    }

    public interface IMiniGame : IObject
    {
        public void LoadContent(ContentManager content);
        //public void Refresh();
        public float GetState();
    }
}
