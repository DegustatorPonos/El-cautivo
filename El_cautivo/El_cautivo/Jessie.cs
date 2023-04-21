using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace El_Cautivo
{
    static class Jessie
    {
        public static Vector2 Position;
        public static Animation currentAnimation;

        #region Animation sprites
        public static Texture2D WalkSheet { set { WalkSheet = value; } }
        public static Texture2D RestSheet { set { RestSheet = value; } }
        public static void InitAnimations(Texture2D rest, Texture2D walk)
        {
            WalkAnimation = new Animation(walk, 4, 1, 300);
        }
        static Animation WalkAnimation, RestAnimation;
        #endregion
        public static void Draw(SpriteBatch batch)
        {
            currentAnimation.Draw(batch, Position, new Vector2(10, 10));
        }

        public static void Update(ref Game1.GameState State)
        {
            WalkAnimation.Update();
            currentAnimation = WalkAnimation;
        }

        
    }
}
