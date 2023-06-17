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
        public static Animation currentAnimation = RestAnimation;
        public static Rectangle Collider; //Jessie is roughly 1*4

        public static IObject HoldingObject = null;

        #region Animation sprites
        public static Texture2D WalkSheet { set { WalkSheet = value; } }
        public static Texture2D RestSheet { set { RestSheet = value; } }
        public static void InitAnimations(Texture2D rest, Texture2D Rrest, Texture2D walk, Texture2D Rwalk)
        {
            WalkAnimation = new Animation(walk, Rwalk, 4, 1, 300);
            RestAnimation = new Animation(rest, Rrest, 5, 2, 300);
            Collider = new Rectangle(0, 0, (int)RestAnimation.Scale.X, (int)RestAnimation.Scale.X);
        }
        static Animation WalkAnimation, RestAnimation;
        #endregion
        public static void Draw(SpriteBatch batch)
        {
            currentAnimation.Draw(batch, Position, Scale);
            if (Game1.ShowColliders) batch.Draw(Game1.ColliderTexture, new Vector2(Collider.X,Collider.Y), new Rectangle(0,0,1,1), Color.White, 0, Vector2.Zero, new Vector2(Collider.Width, Collider.Height), SpriteEffects.None, 0);
        }

        public static void Update()
        {
            RestAnimation.Update();
            WalkAnimation.Update();
            Collider.X = (int)Position.X + (int)(Collider.Width * 1.5);
            Collider.Y = (int)(Position.Y + Collider.Height*2.5); //Don't ask pls, this is fucking random
            if (Game1.state == Game1.GameState.Game) Walk();
        }
        private static bool isWalking = false;
        private static void Walk()
        {
            isWalking = false;
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
                    if(CheckForCollision(Forces[i] * speed))
                    Position += Forces[i] * speed;
                    isWalking = true;
                }
            }
            currentAnimation = isWalking ? WalkAnimation : RestAnimation;
        }

        /// <summary>
        /// Returns if it is possible for Jessie to walk there
        /// </summary>
        private static bool CheckForCollision(Vector2 shift)
        {
            shift *= Game1.dScale;
            var ShiftedCollider = new Rectangle((int)(shift.X + Collider.X), (int)(shift.Y + Collider.Y), Collider.Width , Collider.Height);
            foreach(var collider in Lab.Colliders)
                if(ShiftedCollider.Intersects(collider)) return false;
            return true;
        }

        public static void ResizeCollider()
        {
            //if (Game1.dScale == 1) Position *= 2;
            //else Position /= 2;
            Collider = new Rectangle(0, 0, (int)(RestAnimation.Scale.X * (2 / Game1.dScale)), (int)(RestAnimation.Scale.X * (2 / Game1.dScale)));
        }

    }
}
