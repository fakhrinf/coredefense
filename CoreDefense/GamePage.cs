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
    public class GamePage : Screen
    {
        private static GamePage Instance;
        public static GamePage Init 
        { 
            get 
            {
                if (Instance == null)                
                    Instance = new GamePage();
                return Instance;
            }
        }

        Random rand;
        int randX, randY;

        public bool isPlay = false;
        public bool isOver = false;
        public bool isPause = false;
        public bool isActive = false;
        public bool energyDrain = false;

        bool isActiveOn = false;

        Texture2D gameScreen, gameCore;
        SpriteFont scoreFont;
        Vector2 scoreFont_position = new Vector2(1366 / 2, 768 / 15);

        Texture2D btnMenuThin, btnResumeThin, btnMusic, btnSoundFX, pauseTexture, btnPause, energyBG_texture, energy_texture;

        bool btnMenuThinOn, btnResumeThinOn, btnPauseOn;

        Point btnMenuThin_frameSize = new Point(252, 99);
        Point btnMenuThin_currentFrame = new Point(0, 0);
        Point btnMenuThin_sheetSize = new Point(1, 2);
        Vector2 btnMenuThin_position = new Vector2(1366 / 2, 768 / 1.2f);

        Point btnResumeThin_frameSize = new Point(314, 100);
        Point btnResumeThin_currentFrame = new Point(0, 0);
        Point btnResumeThin_sheetSize = new Point(1, 2);
        Vector2 btnResumeThin_position = new Vector2(1366 / 2, 768 / 2.92f);

        Point btnMusic_frameSize = new Point(116, 116);
        Point btnMusic_currentFrame = new Point(0, 0);
        Point btnMusic_sheetSize = new Point(2, 1);
        Vector2 btnMusic_position = new Vector2((1366 / 2) - 90, (768 / 2) + 40);

        Point btnSoundFX_frameSize = new Point(116,116);
        Point btnSoundFX_currentFrame = new Point(0, 0);
        Point btnSoundFX_sheetSize = new Point(2, 1);
        Vector2 btnSoundFX_position = new Vector2((1366 / 2) + 90, (768 / 2) + 40);

        int musicFlag, sfxFlag = 0;

        Point btnPause_frameSize = new Point(57, 57);
        Point btnPause_currentFrame = new Point(0, 0);
        Point btnPause_sheetSize = new Point(2, 1);
        Vector2 btnPause_position = new Vector2(1366 - 50, 50);

        Point energyBG_frameSize = new Point(394, 38);
        Point energyBG_currentFrame = new Point(0, 0);
        Point energyBG_sheetSize = new Point(1, 2);
        Vector2 energyBG_position = new Vector2(250, 50);
        Vector2 energy_position = new Vector2(255, 45);
        public Rectangle energySourceRect;

        Vector2 pauseTexture_position = new Vector2(1366 / 2, 768 / 5);

        public int score = 0;

        public Point gameCore_frameSize = new Point(192, 192);
        Point gameCore_currentFrame = new Point(0, 0);
        Point gameCore_sheetSize = new Point(4, 2);
        public Vector2 gameCore_position = new Vector2(1366 / 2, 768 / 2);
        int gameCore_timeSinceLastFrame = 0;
        public int gameCore_milliSecondPerFrame = 30;        

        List<Projectile> smallProjectilesUpperLeft = new List<Projectile>();
        List<Projectile> smallProjectilesBottomLeft = new List<Projectile>();
        List<Projectile> smallProjectilesUpperRight = new List<Projectile>();
        List<Projectile> smallProjectilesBottomRight = new List<Projectile>();
        List<Explosions> explode = new List<Explosions>();        

        int projectileAmount, projectileSpeed;               

        public override void Initialize(ContentManager content) 
        {
            rand = new Random();
            
            projectileAmount = rand.Next(2, 4);
            projectileSpeed = rand.Next(3, 5);
            transitionIN = new Transition(content.Load<Texture2D>("Image\\transition"), true);
            transitionOUT = new Transition(content.Load<Texture2D>("Image\\transition"), false);

            energySourceRect = new Rectangle((int)energy_position.X, (int)energy_position.Y, 5, 5);            
        }

        public override void LoadContent(ContentManager content)
        {
            gameScreen = content.Load<Texture2D>("Image\\gameField");
            gameCore = content.Load<Texture2D>("Image\\gameCore_animate");

            scoreFont = content.Load<SpriteFont>("Fonts\\gamePlayScore");

            btnMenuThin = content.Load<Texture2D>("Image\\btnMenuThin_animate");
            btnResumeThin = content.Load<Texture2D>("Image\\btnResume_animate");
            pauseTexture = content.Load<Texture2D>("Image\\pausedTexture");
            btnPause = content.Load<Texture2D>("Image\\btnPause_animate");
            btnMusic = content.Load<Texture2D>("Image\\btnMusic_animate");
            btnSoundFX = content.Load<Texture2D>("Image\\btnSFX_animate");

            energyBG_texture = content.Load<Texture2D>("Image\\energi_BG");
            energy_texture = content.Load<Texture2D>("Image\\energiBar");

            base.LoadContent(content);
        }

        public override void UnloadContent(ContentManager content)
        {
            base.UnloadContent(content);
        }

        public override void Update(GameTime gameTime, ContentManager content)
        {
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();

            prevKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            SoundFactory.Init.playBGM(1);

            transitionIN.FadeIn();
            if (transitionIN.CheckIn())
                isPlay = true;

            if (!isPause)
            {
                if (isPlay && !isOver)
                {
                    btnHover(isPause);                                        

                    if (gameCore_milliSecondPerFrame < 1000)
                        AnimateGameCore(gameTime);
                    if (gameCore_milliSecondPerFrame >= 1000)
                    {
                        gameCore_milliSecondPerFrame = 1000;
                        isPlay = false;
                        isOver = true;
                    }

                    if (energySourceRect.Width >= energy_texture.Width - 20) 
                    {
                        if(!isActiveOn)
                            SoundFactory.Init.powerUpPlay();
                        isActiveOn = true;
                        energySourceRect.Width.Equals(energy_texture.Width - 20);
                        isActive = true;
                        energyBG_currentFrame.Y = 1;
                    }

                    if (energySourceRect.Width <= 5)
                    {
                        isActive = false;
                        isActiveOn = false;
                        energyBG_currentFrame.Y = 0;
                    }

                    if (energyDrain)
                        --energySourceRect.Width;

                    spawnUpperLeft += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    spawnButtomLeft += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    spawnUpperRight += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    SpawnBottomRight += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    foreach (Projectile projectile in smallProjectilesUpperLeft)
                        projectile.Update(gameTime);
                    foreach (Projectile projectile in smallProjectilesBottomLeft)
                        projectile.Update(gameTime);
                    foreach (Projectile projectile in smallProjectilesUpperRight)
                        projectile.Update(gameTime);
                    foreach (Projectile projectile in smallProjectilesBottomRight)
                        projectile.Update(gameTime);

                    foreach (Explosions bomb in explode)
                        bomb.Update(gameTime);

                    loadProjectileUpperLeft(content);
                    loadProjectileBottomLeft(content);
                    loadProjectileUpperRight(content);
                    loadProjectileBottomRight(content);
                }

                if (btnPauseCollide() && (mouseState.LeftButton.Equals(ButtonState.Pressed) && prevMouseState.LeftButton.Equals(ButtonState.Released)))                
                    doPause();                

                if (keyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space))                
                    doPause();                
            }
            else
            {
                transitionOUT.FadeOut(5, 240);
                if(transitionOUT.FadeColor.Equals(240))    
                    btnHover(isPause);

                if (btnResumeCollide() && (mouseState.LeftButton.Equals(ButtonState.Pressed) && prevMouseState.LeftButton.Equals(ButtonState.Released)))
                {
                    SoundFactory.Init.btnClickPlay();
                    isPause = false;
                    isPlay = false;
                    transitionIN.Reset(true);
                    if (transitionIN.CheckIn())                                           
                        isPlay = true;                                            
                    transitionOUT.Reset(false);
                }
                
                if (btnMenuThinCollide() && (mouseState.LeftButton.Equals(ButtonState.Pressed) && prevMouseState.LeftButton.Equals(ButtonState.Released)))
                {
                    SoundFactory.Init.btnClickPlay();
                    SoundFactory.Init.stopBGM();
                    Game1.currentGameState = Game1.GameState.MainMenu;
                    smallProjectilesBottomLeft.Clear();
                    smallProjectilesUpperLeft.Clear();
                    smallProjectilesBottomRight.Clear();
                    smallProjectilesUpperRight.Clear();
                    explode.Clear();
                    ResetGame();
                }
                
                if (btnSFXCollide() && (mouseState.LeftButton.Equals(ButtonState.Pressed) && prevMouseState.LeftButton.Equals(ButtonState.Released)))
                {
                    SoundFactory.Init.btnClickPlay();

                    if (sfxFlag.Equals(0))
                    {
                        sfxFlag++;
                        btnSoundFX_currentFrame.X = 1;
                        SoundFactory.Init.SoundFXOn = false;
                    }
                    else
                    {
                        sfxFlag--;
                        btnSoundFX_currentFrame.X = 0;
                        SoundFactory.Init.SoundFXOn = true;
                    }
                }
                
                if (btnMusicCollide() && (mouseState.LeftButton.Equals(ButtonState.Pressed) && prevMouseState.LeftButton.Equals(ButtonState.Released)))
                {
                    SoundFactory.Init.btnClickPlay();

                    if (musicFlag.Equals(0))
                    {
                        musicFlag++;
                        btnMusic_currentFrame.X = 1;
                        SoundFactory.Init.BGMOn = false;
                    }
                    else
                    {
                        musicFlag--;
                        btnMusic_currentFrame.X = 0;
                        SoundFactory.Init.BGMOn = true;
                    }
                }
            }

            if (isOver)
            {
                smallProjectilesBottomLeft.Clear();
                smallProjectilesUpperLeft.Clear();
                smallProjectilesUpperRight.Clear();
                smallProjectilesBottomRight.Clear();
                explode.Clear();

                transitionOUT.FadeOut(5);
                if (transitionOUT.CheckOut()) 
                {
                    //ResetGame();                    
                    Game1.currentGameState = Game1.GameState.GameOver;
                    SoundFactory.Init.stopBGM();
                }
            }

            base.Update(gameTime);
        }

        private void doPause()
        {
            SoundFactory.Init.btnClickPlay();
            isPause = true;
            isPlay = false;
            btnResumeThin_currentFrame.Y = 0;
            btnMenuThin_currentFrame.Y = 0;
        }

        bool btnPauseCollide() 
        {
            Rectangle btnPauseRec = new Rectangle((int)btnPause_position.X - btnPause_frameSize.X / 2, (int)btnPause_position.Y, btnPause_frameSize.X, btnPause_frameSize.Y);
            Rectangle mouseRec = new Rectangle((int)CustCursor.Init.Position.X, (int)CustCursor.Init.Position.Y, (int)CustCursor.Init.custCursorTexture.Width, (int)CustCursor.Init.custCursorTexture.Height);

            return mouseRec.Intersects(btnPauseRec);
        }

        bool btnResumeCollide() 
        {
            Rectangle btnResumeRec = new Rectangle((int)btnResumeThin_position.X - btnResumeThin_frameSize.X / 2, (int)btnResumeThin_position.Y, btnResumeThin_frameSize.X, btnResumeThin_frameSize.Y - 70);
            Rectangle mouseRec = new Rectangle((int)CustCursor.Init.Position.X, (int)CustCursor.Init.Position.Y, (int)CustCursor.Init.custCursorTexture.Width, (int)CustCursor.Init.custCursorTexture.Height);

            return mouseRec.Intersects(btnResumeRec);
        }

        bool btnMenuThinCollide() 
        {
            Rectangle btnMenuThinRec = new Rectangle((int)btnMenuThin_position.X - btnMenuThin_frameSize.X / 2, (int)btnMenuThin_position.Y, btnMenuThin_frameSize.X, btnMenuThin_frameSize.Y - 70);
            Rectangle mouseRec = new Rectangle((int)CustCursor.Init.Position.X, (int)CustCursor.Init.Position.Y, (int)CustCursor.Init.custCursorTexture.Width, (int)CustCursor.Init.custCursorTexture.Height);

            return mouseRec.Intersects(btnMenuThinRec);
        }

        bool btnMusicCollide() 
        {
            Rectangle btnMusicRec = new Rectangle((int)btnMusic_position.X - btnMusic_frameSize.X / 2, (int)btnMusic_position.Y, btnMusic_frameSize.X, btnMusic_frameSize.Y - 90);
            Rectangle mouseRec = new Rectangle((int)CustCursor.Init.Position.X, (int)CustCursor.Init.Position.Y, (int)CustCursor.Init.custCursorTexture.Width, (int)CustCursor.Init.custCursorTexture.Height);

            return mouseRec.Intersects(btnMusicRec);
        }

        bool btnSFXCollide() 
        {
            Rectangle btnSFXRec = new Rectangle((int)btnSoundFX_position.X - btnSoundFX_frameSize.X / 2, (int)btnSoundFX_position.Y, btnSoundFX_frameSize.X, btnSoundFX_frameSize.Y - 90);
            Rectangle mouseRec = new Rectangle((int)CustCursor.Init.Position.X, (int)CustCursor.Init.Position.Y, (int)CustCursor.Init.custCursorTexture.Width, (int)CustCursor.Init.custCursorTexture.Height);

            return mouseRec.Intersects(btnSFXRec);
        }

        void btnHover(bool isPause) 
        {
            if (!isPause)
            {
                if (btnPauseCollide())
                {
                    btnPause_currentFrame.X = 1;
                    if (!btnPauseOn)
                        SoundFactory.Init.btnHoverPlay();
                    btnPauseOn = true;                    
                }
                else
                {
                    btnPause_currentFrame.X = 0;
                    btnPauseOn = false;
                }
            }
            else
            {
                if (btnResumeCollide())
                {
                    btnResumeThin_currentFrame.Y = 1;
                    if (!btnResumeThinOn)
                        SoundFactory.Init.btnHoverPlay();
                    btnResumeThinOn = true;                    
                }
                else
                {
                    btnResumeThin_currentFrame.Y = 0;
                    btnResumeThinOn = false;
                }

                if (btnMenuThinCollide())
                {
                    btnMenuThin_currentFrame.Y = 1;
                    if (!btnMenuThinOn)
                        SoundFactory.Init.btnHoverPlay();
                    btnMenuThinOn = true;
                }
                else
                {
                    btnMenuThin_currentFrame.Y = 0;
                    btnMenuThinOn = false;
                }
            }
        }

        float spawnUpperLeft, spawnButtomLeft, spawnUpperRight, SpawnBottomRight = 0;

        private void loadProjectileUpperLeft(ContentManager content)
        {
            if (spawnUpperLeft >= 1)
            {
                spawnUpperLeft = 0;
                if (smallProjectilesUpperLeft.Count < projectileAmount)
                {
                smallProjectilesUpperLeft.Add(new Projectile(content.Load<Texture2D>("Image\\smallProjectileMove_animates"), projectileSpeed, true, true, 1, new Vector2(rand.Next(-150, 683), rand.Next(-150, 0))));
                }                
            }

            for (int i = 0; i < smallProjectilesUpperLeft.Count; i++)
            {
                if (smallProjectilesUpperLeft[i].isHit)
                {
                    
                    explode.Add(new Explosions(content.Load<Texture2D>("Image\\explotionsblury"), new Vector2(smallProjectilesUpperLeft[i].ProjectilePosition.X, smallProjectilesUpperLeft[i].ProjectilePosition.Y)));
                    for (int j = 0; j < explode.Count; j++)
                    {
                        if (explode[j].isEnd)
                        {
                            explode.RemoveAt(j);
                            j--;
                        }
                    }
                    //score += smallProjectilesUpperLeft[i].hp;
                    smallProjectilesUpperLeft.RemoveAt(i);
                    i--;              
                       
                }                
            }
        }
        private void loadProjectileBottomLeft(ContentManager content)
        {
            if (spawnButtomLeft >= 1)
            {
                spawnButtomLeft = 0;
                if (smallProjectilesBottomLeft.Count < projectileAmount)
                {
                    smallProjectilesBottomLeft.Add(new Projectile(content.Load<Texture2D>("Image\\smallProjectileMove_animates"), projectileSpeed, false, true, 1, new Vector2(rand.Next(-150, 683), rand.Next(768, 818))));
                }
            }

            for (int i = 0; i < smallProjectilesBottomLeft.Count; i++)
            {
                if (smallProjectilesBottomLeft[i].isHit)
                {
                    
                    explode.Add(new Explosions(content.Load<Texture2D>("Image\\explotionsblury"), new Vector2(smallProjectilesBottomLeft[i].ProjectilePosition.X, smallProjectilesBottomLeft[i].ProjectilePosition.Y)));
                    for (int j = 0; j < explode.Count; j++)
                    {
                        if (explode[j].isEnd)
                        {
                            explode.RemoveAt(j);
                            j--;
                        }
                    }
                    //score += smallProjectilesBottomLeft[i].hp;
                    smallProjectilesBottomLeft.RemoveAt(i);
                    i--;                                     
                }
            }
        }

        private void loadProjectileUpperRight(ContentManager content)
        {
            if (spawnUpperRight >= 1)
            {
                spawnUpperRight = 0;
                if (smallProjectilesUpperRight.Count < projectileAmount)
                {
                    smallProjectilesUpperRight.Add(new Projectile(content.Load<Texture2D>("Image\\smallProjectileMove_animates"), projectileSpeed, true, false, 1, new Vector2(rand.Next(1380 / 2, 1516), rand.Next(-150, 0))));
                }
            }

            for (int i = 0; i < smallProjectilesUpperRight.Count; i++)
            {
                if (smallProjectilesUpperRight[i].isHit)
                {

                    explode.Add(new Explosions(content.Load<Texture2D>("Image\\explotionsblury"), new Vector2(smallProjectilesUpperRight[i].ProjectilePosition.X, smallProjectilesUpperRight[i].ProjectilePosition.Y)));
                    for (int j = 0; j < explode.Count; j++)
                    {
                        if (explode[j].isEnd)
                        {
                            explode.RemoveAt(j);
                            j--;
                        }
                    }
                    //score += smallProjectilesBottomLeft[i].hp;
                    smallProjectilesUpperRight.RemoveAt(i);
                    i--;
                }
            }
        }
        private void loadProjectileBottomRight(ContentManager content)
        {
            if (SpawnBottomRight >= 1)
            {
                SpawnBottomRight = 0;
                if (smallProjectilesBottomRight.Count < projectileAmount)
                {
                    smallProjectilesBottomRight.Add(new Projectile(content.Load<Texture2D>("Image\\smallProjectileMove_animates"), projectileSpeed, false, false, 1, new Vector2(rand.Next(1370 / 2, 1516), rand.Next(768, 918))));
                }
            }

            for (int i = 0; i < smallProjectilesBottomRight.Count; i++)
            {
                if (smallProjectilesBottomRight[i].isHit)
                {

                    explode.Add(new Explosions(content.Load<Texture2D>("Image\\explotionsblury"), new Vector2(smallProjectilesBottomRight[i].ProjectilePosition.X, smallProjectilesBottomRight[i].ProjectilePosition.Y)));
                    for (int j = 0; j < explode.Count; j++)
                    {
                        if (explode[j].isEnd)
                        {
                            explode.RemoveAt(j);
                            j--;
                        }
                    }
                    //score += smallProjectilesBottomLeft[i].hp;
                    smallProjectilesBottomRight.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// ini adalah game sbeuha jalan yang berliku untuk 
        /// </summary>
        /// <param name="gameTime"></param>
        private void AnimateGameCore(GameTime gameTime)
        {
            gameCore_timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (gameCore_timeSinceLastFrame >= gameCore_milliSecondPerFrame)
            {
                gameCore_timeSinceLastFrame -= gameCore_milliSecondPerFrame;
                ++gameCore_currentFrame.X;
                if (gameCore_currentFrame.X >= gameCore_sheetSize.X)
                {
                    gameCore_currentFrame.X = 0;
                    ++gameCore_currentFrame.Y;
                    if (gameCore_currentFrame.Y >= gameCore_sheetSize.Y)
                        gameCore_currentFrame.Y = 0;
                }
            }
        }

        public void ResetGame() 
        {           
            isPlay = false;
            isOver = false;
            isPause = false;
            isActive = false;
            energySourceRect.Width = 5;

            gameCore_milliSecondPerFrame = 30;
            score = 0;

            transitionIN.Reset(true);
            transitionOUT.Reset(false);

            List<Projectile> smallProjectilesUpperLeft = new List<Projectile>();
            List<Projectile> smallProjectilesBottomLeft = new List<Projectile>();
            List<Projectile> smallProjectilesUpperRight = new List<Projectile>();
            List<Projectile> smallProjectilesBottomRight = new List<Projectile>();

            List<Explosions> explode = new List<Explosions>();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {           
            transitionIN.Draw(spriteBatch);
            spriteBatch.Draw(gameScreen, Vector2.Zero, null, null, Vector2.Zero, 0f, null, Color.White,SpriteEffects.None, 0f);
            spriteBatch.DrawString(scoreFont, score.ToString(), scoreFont_position, Color.White, 0f, new Vector2(scoreFont.MeasureString(score.ToString()).X / 2, scoreFont.MeasureString(score.ToString()).Y / 2), 1f, SpriteEffects.None, 0.7f);
            spriteBatch.Draw(btnPause, btnPause_position, null,
                new Rectangle(btnPause_currentFrame.X * btnPause_frameSize.X,
                              btnPause_currentFrame.Y * btnPause_frameSize.Y,
                              btnPause_frameSize.X,
                              btnPause_frameSize.Y),
                              new Vector2(btnPause_frameSize.X / 2, btnPause_frameSize.Y / 2), 0f, null, Color.White, SpriteEffects.None, 0.7f);

            spriteBatch.Draw(gameCore, gameCore_position, null, 
                new Rectangle(gameCore_currentFrame.X * gameCore_frameSize.X, 
                              gameCore_currentFrame.Y * gameCore_frameSize.Y, 
                              gameCore_frameSize.X, 
                              gameCore_frameSize.Y), new Vector2(gameCore_frameSize.X / 2, gameCore_frameSize.Y / 2), 0f, null, Color.White, SpriteEffects.None, 0.1f);

            foreach (Projectile projectile in smallProjectilesUpperLeft)
                projectile.Draw(spriteBatch);

            foreach (Projectile projectile in smallProjectilesBottomLeft)
                projectile.Draw(spriteBatch);

            foreach (Projectile projectile in smallProjectilesUpperRight)
                projectile.Draw(spriteBatch);

            foreach (Projectile projectile in smallProjectilesBottomRight)
                projectile.Draw(spriteBatch);

            foreach (Explosions bomb in explode)
                bomb.Draw(spriteBatch);

            spriteBatch.Draw(energyBG_texture, energyBG_position, null, new Rectangle(energyBG_currentFrame.X * energyBG_frameSize.X, energyBG_currentFrame.Y * energyBG_frameSize.Y, energyBG_frameSize.X, energyBG_frameSize.Y), new Vector2(energyBG_frameSize.X / 2, energyBG_frameSize.Y / 2), 0f, null, Color.White, SpriteEffects.None, 0.6f);

            spriteBatch.Draw(energy_texture, energy_position, null, energySourceRect, new Vector2(energy_texture.Width / 2, energy_texture.Height / 2), 0f, null, Color.White, SpriteEffects.None, 0.7f);

            if (isPause) 
            {
                transitionOUT.Draw(spriteBatch);
                spriteBatch.Draw(btnMenuThin, btnMenuThin_position, null,
                    new Rectangle(btnMenuThin_currentFrame.X * btnMenuThin_frameSize.X,
                                  btnMenuThin_currentFrame.Y * btnMenuThin_frameSize.Y,
                                  btnMenuThin_frameSize.X,
                                  btnMenuThin_frameSize.Y), new Vector2(btnMenuThin_frameSize.X / 2, btnMenuThin_frameSize.Y / 2), 0f, null, transitionOUT.TransitionColor, SpriteEffects.None, 0.9f);

                spriteBatch.Draw(btnResumeThin, btnResumeThin_position, null,
    new Rectangle(btnResumeThin_currentFrame.X * btnResumeThin_frameSize.X,
                  btnResumeThin_currentFrame.Y * btnResumeThin_frameSize.Y,
                  btnResumeThin_frameSize.X,
                  btnResumeThin_frameSize.Y), new Vector2(btnResumeThin_frameSize.X / 2, btnResumeThin_frameSize.Y / 2), 0f, null, transitionOUT.TransitionColor, SpriteEffects.None, 0.9f);

                spriteBatch.Draw(pauseTexture, pauseTexture_position, null, null, new Vector2(pauseTexture.Width / 2, pauseTexture.Height / 2), 0f, null, transitionOUT.TransitionColor, SpriteEffects.None, 0.9f);

                spriteBatch.Draw(btnMusic, btnMusic_position, null,
    new Rectangle(btnMusic_currentFrame.X * btnMusic_frameSize.X,
                  btnMusic_currentFrame.Y * btnMusic_frameSize.Y,
                  btnMusic_frameSize.X,
                  btnMusic_frameSize.Y), new Vector2(btnMusic_frameSize.X / 2, btnMusic_frameSize.Y / 2), 0f, null, transitionOUT.TransitionColor, SpriteEffects.None, 0.9f);

                spriteBatch.Draw(btnSoundFX, btnSoundFX_position, null,
    new Rectangle(btnSoundFX_currentFrame.X * btnSoundFX_frameSize.X,
                  btnSoundFX_currentFrame.Y * btnSoundFX_frameSize.Y,
                  btnSoundFX_frameSize.X,
                  btnSoundFX_frameSize.Y), new Vector2(btnSoundFX_frameSize.X / 2, btnSoundFX_frameSize.Y / 2), 0f, null, transitionOUT.TransitionColor, SpriteEffects.None, 0.9f);
            }

            //if (isActive)
            //    transitionIN.Draw(spriteBatch);

            if (isOver)
                transitionOUT.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
