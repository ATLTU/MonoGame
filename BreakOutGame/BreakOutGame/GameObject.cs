using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakOutGame
{
    abstract public class GameObject
    {
        protected Texture2D sprite;
        protected float speed;
        protected float fps;
        protected Vector2 position;
        protected Vector2 origin;
        protected Vector2 velocity;

        /// <summary>
        /// Creates the collisionbox 
        /// </summary>
        public virtual Rectangle CollisionBox
        {
            get
            {
                return new Rectangle
                    (
                    (int)position.X - (int)origin.X,
                    (int)position.Y - (int)origin.Y,
                    sprite.Width,
                    sprite.Height
                    );
            }
        }

        /// <summary>
        /// Empty constructor for GameObject
        /// </summary>
        public GameObject()
        {
        }

        /// <summary>
        /// Constructor with position for GameObject
        /// </summary>
        /// <param name="pos"></param>
        public GameObject(Vector2 pos)
        {
            this.position = pos;
        }

        /// <summary>
        /// Loads content for GameObject
        /// </summary>
        /// <param name="content"></param>
        public abstract void LoadContent(ContentManager content);

        /// <summary>
        /// Updates the game for GameObject
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Draws the game objects
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Makes the objects move
        /// </summary>
        /// <param name="gameTime"></param>
        protected virtual void Move(GameTime gameTime)
        {
            // Calculates deltaTime based on the gameTime
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Moves the player based on the result from HandleInput, speed and deltaTime
            position += ((velocity * speed) * deltaTime);
        }

        /// <summary>
        /// Is executed whenever a collision occurs
        /// </summary>
        /// <param name="other">the object collided with</param>
        public abstract void OnCollision(GameObject other);

        /// <summary>
        /// Checks if the GameObject has collided with another GameObject
        /// </summary>
        /// <param name="other">the object collided with</param>
        public void CheckCollision(GameObject other)
        {
            if (other != this && CollisionBox.Intersects(other.CollisionBox))
            {
                OnCollision(other);
            }
        }
    }

}