using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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
