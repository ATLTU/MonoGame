using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace BreakOutGame
{
    class Brick : GameObject
    {
        /// <summary>
        /// Constructor for Brick that sets the position
        /// </summary>
        /// <param name="pos"></param>
        public Brick(Vector2 pos) : base(pos)
        {
        }

        /// <summary>
        /// Loads content for Brick
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("brick");

            origin = new Vector2(sprite.Width / 2, sprite.Height);
        }

        /// <summary>
        /// Updates Brick
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (GameWorld.GameOver == true)
            {
                GameWorld.RemoveObject(this);
            }
        }

        /// <summary>
        /// Executes whenever Brick collides with another object
        /// </summary>
        /// <param name="other"></param>
        public override void OnCollision(GameObject other)
        {
            GameWorld.RemoveObject(this);
            GameWorld.Targets -= 1;
            GameWorld.TargetsDestroyed += 1;
        }
    }
}
