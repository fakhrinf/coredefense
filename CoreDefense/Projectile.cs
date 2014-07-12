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
    public class Projectile
    {
                
        public int hp;
        bool isUp, isLeft;
        public bool isHit;
        public int Speed { private set; get; }
        public Vector2 ProjectilePosition { private set; get; }
        Vector2 targetBomb = new Vector2(1366 / 2, 768 / 2);

        public Texture2D smallProjectileTexture, mediumProjectileTexture, bigProjectileTexture, luxuryProjectileTexture;

        Point smallProjectile_frameSize = new Point(132, 132);
        Point smallProjectile_currentFrame = new Point(0, 0);
        Point smallprojectile_sheetSize = new Point(5, 1);

        int timeSinceLastFrame = 0;

        int smallProjectile_milliSecondPerFrame = 40;

        MouseState prevMouseState;
        MouseState mouseState;

        public Projectile(Texture2D texture, int speed, bool isUp, bool isLeft, int hp, Vector2 position) 
        {
            this.Speed = speed;
            this.isUp = isUp;
            this.isLeft = isLeft;
            this.hp = hp;
            this.ProjectilePosition = position;
            this.smallProjectileTexture = texture;
            isHit = false;
        }

        //public void LoadContent(ContentManager content)
        //{
        //    smallProjectileTexture = content.Load<Texture2D>("Image\\smallProjectileMove_animates");
        //}

        public void UnloadContent(ContentManager content) 
        {
            content.Unload();
        }

        public void Update(GameTime gameTime) 
        {            
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();
            AnimateSmallProjectile(gameTime);            

            ProjectileMovement();
            if (projectileCollide() && (mouseState.LeftButton.Equals(ButtonState.Pressed) && prevMouseState.LeftButton.Equals(ButtonState.Released)))
            {
                SoundFactory.Init.explodePlay();
                HIT();
                if(!GamePage.Init.isActive)
                    GamePage.Init.energySourceRect.Width += 20;
            }

            if (GamePage.Init.isActive && mouseState.LeftButton.Equals(ButtonState.Pressed))
            {
                GamePage.Init.energyDrain = true;

                if (projectileCollide() && mouseState.LeftButton.Equals(ButtonState.Pressed))
                {
                    SoundFactory.Init.explodePlay();
                    HIT();
                }
            }
            else
                GamePage.Init.energyDrain = false;               

            if (projectileCoreCollide())
            {
                SoundFactory.Init.explodePlay();
                isHit = true;
                GamePage.Init.gameCore_milliSecondPerFrame += 100;
            }
        }

        private void HIT()
        {
            GamePage.Init.score += hp;
            --hp;
            if (hp == 0)
                isHit = true;
        }

        public bool projectileCollide() 
        {
            Rectangle projectileRec = new Rectangle((int)ProjectilePosition.X, (int)ProjectilePosition.Y, smallProjectile_frameSize.X, smallProjectile_frameSize.Y);

            Rectangle cusorRect = new Rectangle((int)CustCursor.Init.Position.X + 30, (int)CustCursor.Init.Position.Y +30, CustCursor.Init.custCursorTexture.Width - 30, CustCursor.Init.custCursorTexture.Height - 30);

            return cusorRect.Intersects(projectileRec);
        }
        public bool projectileCoreCollide() 
        {
            Rectangle projectileRec = new Rectangle((int)ProjectilePosition.X, (int)ProjectilePosition.Y, smallProjectile_frameSize.X, smallProjectile_frameSize.Y);

            Rectangle coreRect = new Rectangle((int)GamePage.Init.gameCore_position.X + 50, (int)GamePage.Init.gameCore_position.Y + 50, GamePage.Init.gameCore_frameSize.X - 150, GamePage.Init.gameCore_frameSize.Y - 150);

            return coreRect.Intersects(projectileRec);
        }

        private void ProjectileMovement()
        {
            if (isUp)
                ProjectilePosition += new Vector2(0, Speed);
            else
                ProjectilePosition -= new Vector2(0, Speed);

            if (isLeft)
                ProjectilePosition += new Vector2(Speed, 0);
            else
                ProjectilePosition -= new Vector2(Speed, 0);

            if (ProjectilePosition.X >= 1466)
                isLeft = false;
            else if (ProjectilePosition.X <= -150)
                isLeft = true;

            if (ProjectilePosition.Y >= 890)
                isUp = false;
            else if (ProjectilePosition.Y <= -150)
                isUp = true;
        }

        private void AnimateSmallProjectile(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame >= smallProjectile_milliSecondPerFrame)
            {
                timeSinceLastFrame -= smallProjectile_milliSecondPerFrame;
                ++smallProjectile_currentFrame.X;
                if (smallProjectile_currentFrame.X >= smallprojectile_sheetSize.X)
                {
                    smallProjectile_currentFrame.X = 0;
                }
            }
        }
        
        public void Draw(SpriteBatch spriteBatch) 
        {
            switch (hp)
            {
                case 1:
            spriteBatch.Draw(smallProjectileTexture, ProjectilePosition, null,
                new Rectangle(smallProjectile_currentFrame.X * smallProjectile_frameSize.X,
                              smallProjectile_currentFrame.Y * smallProjectile_frameSize.Y,
                              smallProjectile_frameSize.X,
                              smallProjectile_frameSize.Y),
                new Vector2(smallProjectile_frameSize.X / 2, smallProjectile_frameSize.Y / 2), 0f, null, Color.White, SpriteEffects.None, 0.5f);
                    break;
                default:
                    break;
            }
        }
    }
}
