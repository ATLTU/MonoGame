using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakOutGame
{
    class Player : GameObject
    {
        /// <summary>
        /// Constructor for Player
        /// </summary>
        public Player()
        {
            speed = 500;
        }

        /// <summary>
        /// Loads content for Player
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("Paddle");

            origin = new Vector2(sprite.Width / 2, sprite.Height);
            position = new Vector2(GameWorld.screenSize.X / 2, GameWorld.screenSize.Y - (sprite.Height / 2));
        }

        /// <summary>
        /// Updates the player
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            ScreenLimits();
            HandleInput();

            // Removes the player if all targets have been destroyed or if GameOver is true
            if (GameWorld.Targets == 0 || GameWorld.GameOver == true)
            {
                GameWorld.RemoveObject(this);
            }
        }

        /// <summary>
        /// Executes whenever the player collides with another object
        /// </summary>
        /// <param name="other"></param>
        public override void OnCollision(GameObject other)
        {
        }
        
        /// <summary>
        /// Handles the inputs for the player
        /// </summary>
        private void HandleInput()
        {
            velocity = Vector2.Zero;
            KeyboardState keyState = Keyboard.GetState();

            // Move left if A or left key is pressed
            if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
            {
                velocity += new Vector2(-1, 0);
            }

            // Move right if D or right key is pressed
            if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
            {
                velocity += new Vector2(1, 0);
            }

            // If pressed key is down normalize vector
            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }
        }

        /// <summary>
        /// Stops the player from moving out of the screen
        /// </summary>
        private void ScreenLimits()
        {
            // Left edge
            if (position.X - sprite.Width / 2 < 0)
            {
                position.X = sprite.Width / 2;
            }
            // Right side
            if (position.X > GameWorld.screenSize.X - sprite.Width / 2)
            {
                position.X = GameWorld.screenSize.X - sprite.Width / 2;
            }
        }
    }
}
