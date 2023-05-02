using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace El_Cautivo
{
    static class Jessie 
    {
        #region Movement vars
        public enum Directions
        {
            Right,
            Left
        }
        public static Directions direction = Directions.Left;
        public static float speed;
        private static Dictionary<Keys, Vector2> Forces = new Dictionary<Keys, Vector2>()
        {
            { Keys.W, new Vector2(0, -1) },
            { Keys.A, new Vector2(-1, 0) },
            { Keys.S, new Vector2(0, 1) },
            { Keys.D, new Vector2(1, 0) },
        };

        private static Dictionary<Keys, Directions> Dir = new Dictionary<Keys, Directions>()
        {
            {Keys.A, Directions.Left},
            {Keys.D, Directions.Right}
        };
        #endregion


        public static Vector2 Position, Scale;
        public static Animation currentAnimation;

        #region Animation sprites
        public static Texture2D WalkSheet { set { WalkSheet = value; } }
        public static Texture2D RestSheet { set { RestSheet = value; } }
        public static void InitAnimations(Texture2D rest, Texture2D Rrest, Texture2D walk, Texture2D Rwalk)
        {
            WalkAnimation = new Animation(walk, Rwalk, 4, 1, 300);
        }
        static Animation WalkAnimation, RestAnimation;
        #endregion
        public static void Draw(SpriteBatch batch)
        {
            currentAnimation.Draw(batch, Position, Scale);
        }

        public static void Update(ref Game1.GameState State)
        {
            WalkAnimation.Update();
            currentAnimation = WalkAnimation;
            if(State == Game1.GameState.Game) Walk();
        }

        private static void Walk()
        {
            var keyboardStatus = Keyboard.GetState();
            foreach(var i in Forces.Keys)
            {
                if (keyboardStatus.IsKeyDown(i))
                {
                    if (Dir.ContainsKey(i) && direction != Dir[i])
                    {
                        direction = Dir[i];
                        WalkAnimation.Reverce();
                        
                    }
                    Position += Forces[i] * speed;
                }
            }
        }
        
    }
}
