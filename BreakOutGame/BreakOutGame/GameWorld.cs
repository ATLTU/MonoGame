using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BreakOutGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameWorld : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D collisionTexture;
        private Texture2D background;
        private SpriteFont myFont;
        private List<GameObject> gameObjects = new List<GameObject>();
        private static List<GameObject> newObjects = new List<GameObject>();
        private static List<GameObject> deleteObjects = new List<GameObject>();
        public static int Targets;
        public static int TargetsDestroyed = 0;
        public static bool GameOver = false;
        public static Vector2 screenSize;

        public GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // Adjusts the window size and changes screenSize accordingly
            graphics.PreferredBackBufferWidth = 600;
            graphics.PreferredBackBufferHeight = 300;
            graphics.ApplyChanges();
            screenSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            // Instantiates objects
            gameObjects.Add(new Player());
            gameObjects.Add(new Ball());

            // Instantiates all targets/bricks
            int tmpX = 8; // Amount of targets horizontally
            int tmpY = 3; // Amount of targets vertically
            Targets = tmpX * tmpY; // Total amount of targets
            int tmpXOffset = 60;
            int tmpYOffset = 25;
            // Calculates the appropriate offset from the edge to always center the bricks
            float tmpEdgeOffset = ((screenSize.X + tmpXOffset) - (tmpX * tmpXOffset)) / 2;

            for (int i = 0; i < tmpX; i++)
            {
                for (int j = 0; j < tmpY; j++)
                {
                    gameObjects.Add(new Brick(new Vector2(tmpEdgeOffset + i * tmpXOffset, 50 + j * tmpYOffset)));
                }
            }

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            // TODO: use this.Content to load your game content here
            // Loads background
            background = Content.Load<Texture2D>("Background");

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.LoadContent(Content);
            }

            // Loads a font
            myFont = Content.Load<SpriteFont>("MyFont");

            // Loads the texture used for creating the visual hitbox
            collisionTexture = Content.Load<Texture2D>("Pixel");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            // Clears the list of deletedObjects before updating anything else
            deleteObjects.Clear();

            // Adds all the GameObjects
            foreach (GameObject go in gameObjects)
            {
                go.Update(gameTime);
                foreach (GameObject other in gameObjects)
                {
                    go.CheckCollision(other);
                }
            }

            // Removes all objects that should be removed
            foreach (GameObject go in deleteObjects)
            {
                gameObjects.Remove(go);
            }

            // Adds new objects to the gameObjects list if necessary - POWERUPS
            gameObjects.AddRange(newObjects);
            newObjects.Clear();

            base.Update(gameTime);
        }

        /// <summary>
        /// Instantiates new objects if they are added to the game (make method for deletedObjects or make newObjects a public static list)
        /// </summary>
        /// <param name="go"></param>
        public static void Instantiate(GameObject go)
        {
            newObjects.Add(go);
        }

        public static void RemoveObject(GameObject go)
        {
            deleteObjects.Add(go);
        }

        /// <summary>
        /// Creates a red line around each sprite to visualize the hitbox
        /// </summary>
        /// <param name="go">Every GameObject</param>
        private void DrawCollisionBox(GameObject go)
        {
            Rectangle collisionBox = go.CollisionBox;
            Rectangle topLine = new Rectangle(collisionBox.X, collisionBox.Y, collisionBox.Width, 1);
            Rectangle bottomLine = new Rectangle(collisionBox.X, collisionBox.Y + collisionBox.Height, collisionBox.Width, 1);
            Rectangle rightLine = new Rectangle(collisionBox.X + collisionBox.Width, collisionBox.Y, 1, collisionBox.Height);
            Rectangle leftLine = new Rectangle(collisionBox.X, collisionBox.Y, 1, collisionBox.Height);

            spriteBatch.Draw(collisionTexture, topLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(collisionTexture, bottomLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(collisionTexture, rightLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(collisionTexture, leftLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            // Draws the background
            spriteBatch.Draw(background, Vector2.Zero, Color.White);

            // Draws all the game objects
            foreach (GameObject go in gameObjects)
            {
                go.Draw(spriteBatch);
                spriteBatch.DrawString(myFont, $"Bricks destroyed: {TargetsDestroyed}", new Vector2(5, 5), Color.White);
                string bricksLeftMsg = $"Bricks left: {Targets}";
                spriteBatch.DrawString(myFont, bricksLeftMsg, new Vector2(screenSize.X - myFont.MeasureString(bricksLeftMsg).X - 5, 5), Color.White);
                // Shows a red box around each GameObject when run in debug mode
#if DEBUG
                DrawCollisionBox(go);
#endif
            }

            // Win message if all targets have been destroyed 
            if (Targets == 0)
            {
                string winMsg = $"Congratulations, you won. \nBricks destroyed: {TargetsDestroyed}. \n\nPress Space to try again. \n[not yet implemented]";
                Vector2 winMsgOrigin = new Vector2(myFont.MeasureString(winMsg).X / 2, myFont.MeasureString(winMsg).Y / 2);
                spriteBatch.DrawString(myFont, winMsg, new Vector2(screenSize.X / 2, screenSize.Y / 2), Color.White, 0, winMsgOrigin, 1f, SpriteEffects.None, 0);
            }

            // Lose message if GameOver is true
            if (GameOver == true)
            {
                string loseMsg = $"You Lost. \nBricks left: {Targets}. \n\nPress Space to try again. \n[not yet implemented]";
                Vector2 loseMsgOrigin = new Vector2(myFont.MeasureString(loseMsg).X / 2, myFont.MeasureString(loseMsg).Y / 2);
                spriteBatch.DrawString(myFont, loseMsg, new Vector2(screenSize.X / 2, screenSize.Y / 2), Color.White, 0, loseMsgOrigin, 1f, SpriteEffects.None, 0);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
