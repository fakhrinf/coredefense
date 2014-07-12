using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace CoreDefense
{
    public class Explosions
    {
        public Vector2 Position { private set; get; }
        public Texture2D ExplodeTexture { private set; get; }

        Point explode_frameSize = new Point(129,129);
        Point explode_currentFrame = new Point(0, 0);
        Point explode_sheetSize = new Point(4,4);

        int timeSinceLastFrame;
        int explode_millisecodsPerFrame = 50;
        int count;

        public bool isEnd;

        public Explosions(Texture2D texture, Vector2 position) 
        {
            this.ExplodeTexture = texture;
            this.Position = position;
        }

        public void Update(GameTime gameTime) 
        {
            AnimateExplode(gameTime);
        }

        private void AnimateExplode(GameTime gameTime)
        {           
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame >= explode_millisecodsPerFrame)
            {
                timeSinceLastFrame -= explode_millisecodsPerFrame;
                ++explode_currentFrame.X;
                count++;
                if (explode_currentFrame.X >= explode_sheetSize.X)
                {
                    explode_currentFrame.X = 0;
                    ++explode_currentFrame.Y;
                    if (explode_currentFrame.Y >= explode_sheetSize.Y)
                        explode_currentFrame.Y = 4;
                                       
                    //if (count >= 1)
                        isEnd = true;
                }                
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(ExplodeTexture, Position, null, new Rectangle(explode_currentFrame.X * explode_frameSize.X, explode_currentFrame.Y * explode_frameSize.Y, explode_frameSize.X, explode_frameSize.Y), new Vector2(explode_frameSize.X / 2, explode_frameSize.Y / 2), 0f, null, Color.White, SpriteEffects.None, 0.6f);
        }
    }
}
