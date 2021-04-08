using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BreakOutGame
{
    class Ball : GameObject
    {
        private bool inAir = false;
        private Song startSound;
        private Song missSound;
        private Song paddleBounceSound;
        private Song brickSound;
        private Song wallBounceSound;

        /// <summary>
        /// Constructor for ball
        /// </summary>
        public Ball()
        {
            speed = 500;
            velocity = new Vector2(1, -1);
        }

        /// <summary>
        /// Loads content for Ball
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("Ball");

            origin = new Vector2(sprite.Width / 2, sprite.Height);
            position = new Vector2(GameWorld.screenSize.X / 2, GameWorld.screenSize.Y - (sprite.Height * 2));

            startSound = content.Load<Song>("StartSound");
            missSound = content.Load<Song>("MissSound");
            paddleBounceSound = content.Load<Song>("paddleBounceSound");
            brickSound = content.Load<Song>("BrickSound");
            wallBounceSound = content.Load<Song>("WallBounceSound");
        }

        /// <summary>
        /// Updates the ball
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            ScreenBounce();
            CheckHitBottom();

            // Only uses these methods if the ball is not already shot
            if (inAir == false)
            {
                ScreenLimits();
                HandleInput();
            }

            // Removes the ball if all targets have been destroyed or if GameOver is true
            if (GameWorld.Targets == 0 || GameWorld.GameOver == true)
            {
                GameWorld.RemoveObject(this);
            }
        }

        /// <summary>
        /// Checks if the ball hits the bottom of the screen
        /// </summary>
        public void CheckHitBottom()
        {
            if (position.Y > GameWorld.screenSize.Y - sprite.Height / 2)
            {
                MediaPlayer.Play(missSound);
                GameWorld.GameOver = true;
            }
        }

        /// <summary>
        /// Executes whenever the ball collides with another object
        /// </summary>
        /// <param name="other"></param>
        public override void OnCollision(GameObject other)
        {
            MediaPlayer.Stop();
            float x = velocity.X;
            float y = velocity.Y;
            // If ball hits an object going down
            if (y == 1)
            {
                velocity = new Vector2(x, -1);
            }
            // If ball hits a object going up
            if (y == -1)
            {
                velocity = new Vector2(x, 1);
            }

            if (other.GetType().ToString() == "BreakOutGame.Brick")
            {
                MediaPlayer.Play(brickSound);
            }

            if (other.GetType().ToString() == "BreakOutGame.Player")
            {
                MediaPlayer.Play(paddleBounceSound);
            }
        }

        /// <summary>
        /// Makes the ball bounce on the edges of the screen
        /// </summary>
        private void ScreenBounce()
        {
            // If ball hits left screen edge
            if (position.X - sprite.Width / 2 < 0)
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(wallBounceSound);
                float y = velocity.Y;
                velocity = new Vector2(1, y);
            }
            // If ball hits right screen edge
            if (position.X > GameWorld.screenSize.X - sprite.Width / 2)
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(wallBounceSound);
                float y = velocity.Y;
                velocity = new Vector2(-1, y);
            }
            // If ball hits top screen edge
            if (position.Y - sprite.Height < 0)
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(wallBounceSound);
                float x = velocity.X;
                velocity = new Vector2(x, 1);
            }
        }

        /// <summary>
        /// Limits the X movement of the ball before it is shot
        /// </summary>
        private void ScreenLimits()
        {
            // Left side
            if (position.X - sprite.Width / 2 < 25 + sprite.Width / 2)
            {
                position.X = 25 + sprite.Width / 2;
            }
            // Right side
            if (position.X > GameWorld.screenSize.X - (25 + sprite.Width / 2))
            {
                position.X = GameWorld.screenSize.X - (25 + sprite.Width / 2);
            }
        }

        /// <summary>
        /// Handles the input for the ball for it is shot
        /// </summary>
        private void HandleInput()
        {
            velocity = Vector2.Zero;
            KeyboardState keyState = Keyboard.GetState();

            //Move left if A or left key is pressed
            if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
            {
                velocity += new Vector2(-1, 0);
            }

            //Move right if D or right key is pressed
            if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
            {
                velocity += new Vector2(1, 0);
            }

            // Shoots ball when space is pressed and sets inAir to true
            if (keyState.IsKeyDown(Keys.Space))
            {
                speed = 250;
                inAir = true;

                Random rnd = new Random();
                int tmp = rnd.Next(100);

                if (tmp <= 50)
                {
                    velocity = new Vector2(-1, -1);
                }
                if (tmp >= 51)
                {
                    velocity = new Vector2(1, -1);
                }
                MediaPlayer.Play(startSound);
            }

        }
    }
}
