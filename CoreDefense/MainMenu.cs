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
    public class MainMenu : Screen
    {
        private static MainMenu Instance;

        public static MainMenu init
        {
            get 
            {
                if (Instance == null)
                    Instance = new MainMenu();

                return Instance;
            }
        }

        public bool isExit = false;
        public bool btnPlayOn, btnHelpOn, btnExitOn, btnOptionOn, btnLeadOn, btnCoreOn = false;

        Texture2D screenBase, screenCore, screenTop;
        Texture2D btnCore, btnPlay, btnOption, btnLead, btnExit, btnHelp;

        Point frameSize_screenCore = new Point(314,308);
        Point currentFrame_screenCore = new Point(0,0);
        Point sheetSize_screenCore = new Point(4,1);

        Point btnCore_framesize = new Point(583, 62);
        Point btnCore_currentFrame = new Point(0, 0);
        Point btnCore_sheetSize = new Point(1, 2);
        Vector2 btnCore_position = new Vector2(1366 / 1.98f, 768 / 2.9f);   

        Point btnPlay_framesize = new Point(243, 98);
        Point btnPlay_currentFrame = new Point(0, 0);
        Point btnPlay_sheetSize = new Point(1, 2);
        Vector2 btnPlay_position = new Vector2(1366 / 2, 768 / 1.53f);

        Point btnOption_framesize = new Point(89, 83);
        Point btnOption_currentFrame = new Point(0, 0);
        Point btnOption_sheetSize = new Point(2, 1);
        Vector2 btnOption_position = new Vector2(1366 / 2.9f, 768 / 1.53f);

        Point btnLead_framesize = new Point(88, 77);
        Point btnLead_currentFrame = new Point(0, 0);
        Point btnLead_sheetSize = new Point(2, 1);
        Vector2 btnLead_position = new Vector2(1366 / 1.48f, 768 / 1.54f);

        Point btnExit_framesize = new Point(215, 98);
        Point btnExit_currentFrame = new Point(0, 0);
        Point btnExit_sheetSize = new Point(1, 2);
        Vector2 btnExit_position = new Vector2(1366 / 2, 768 / 1.13f);

        Point btnHelp_framesize = new Point(242, 98);
        Point btnHelp_currentFrame = new Point(0, 0);
        Point btnHelp_sheetSize = new Point(1, 2);
        Vector2 btnHelp_position = new Vector2(1366 / 2, 768 / 1.3f);

        int coreTimeSinceLastFrame = 0;
        int coreMillisecondPerFrame = 90;

        Color mainMenuColor = new Color(255, 255, 255);           

        public override void Initialize(ContentManager content)
        {
            transitionIN = new Transition(content.Load<Texture2D>("Image\\transition"), true);
            transitionOUT = new Transition(content.Load<Texture2D>("Image\\transition"), false);
            base.Initialize();
        }

        public override void LoadContent(ContentManager content)
        {            
            screenBase = content.Load<Texture2D>("Image\\mainMenu_base");
            screenCore = content.Load<Texture2D>("Image\\core_animate");
            screenTop = content.Load<Texture2D>("Image\\mainMenu_topNoLogo");

            btnCore = content.Load<Texture2D>("Image\\btnCore_animate");
            btnPlay = content.Load<Texture2D>("Image\\btnplay_animate");
            btnOption = content.Load<Texture2D>("Image\\btnOptionIcon_animate");
            btnLead = content.Load<Texture2D>("Image\\btnLeaderBoardIcon_animate");
            btnExit = content.Load<Texture2D>("Image\\btnExit_animate");
            btnHelp = content.Load<Texture2D>("Image\\btnHelp_animate");

            base.LoadContent(content);
        }

        public override void UnloadContent(ContentManager content)
        {            
            base.UnloadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            AnimateCore(gameTime);

            SoundFactory.Init.playBGM(0);

            transitionIN.FadeIn(5);
            if (transitionIN.CheckIn())
                isReady = true;
            
            if (isReady && !isExit)
            {                
                btnHover();

                prevMouseState = mouseState;
                mouseState = Mouse.GetState();

                //Button Play Click
                if (btnPlayCollide() && (mouseState.LeftButton.Equals(ButtonState.Pressed) && prevMouseState.LeftButton.Equals(ButtonState.Released)))
                {
                    transitionIN.Reset(true);
                    GamePage.Init.ResetGame();
                    Game1.currentGameState = Game1.GameState.Play;
                    SoundFactory.Init.btnClickPlay();

                    SoundFactory.Init.stopBGM();
                }

                if (btnHelpCollide() && (mouseState.LeftButton.Equals(ButtonState.Pressed) && prevMouseState.LeftButton.Equals(ButtonState.Released)))
                {
                    transitionIN.Reset(true);                    
                    Game1.currentGameState = Game1.GameState.HowGamePlay;
                    SoundFactory.Init.btnClickPlay();
                }

                //Button Exit Click
                if (btnExitCollide() && (mouseState.LeftButton.Equals(ButtonState.Pressed) && prevMouseState.LeftButton.Equals(ButtonState.Released)))
                {
                    isExit = true;
                    SoundFactory.Init.btnClickPlay();
                }

                if (btnLeadCollide() && (mouseState.LeftButton.Equals(ButtonState.Pressed) && prevMouseState.LeftButton.Equals(ButtonState.Released)))
                {                    
                    SoundFactory.Init.btnClickPlay();
                    //OPEN WEB BROWSER DAN TAMPILKAN HALAMAN LEADERBOARD PADA WEBSITE
                    System.Diagnostics.Process.Start("IExplore.exe", "localhost/CoreDefenseWeb/leaderboard.aspx");
                }

                if (btnCoreCollide() && (mouseState.LeftButton.Equals(ButtonState.Pressed) && prevMouseState.LeftButton.Equals(ButtonState.Released)))
                {
                    transitionIN.Reset(true);  
                    SoundFactory.Init.btnClickPlay();
                    Game1.currentGameState = Game1.GameState.Credits;
                }
            }

            if(isExit)
            {                
                transitionOUT.FadeOut(5);
                if (transitionOUT.CheckOut())
                    Environment.Exit(0);
            }

            base.Update(gameTime);
        }

        private void btnHover()
        {            
            if (btnPlayCollide())
            {
                btnPlay_currentFrame.Y = 1;
                if (!btnPlayOn)
                    SoundFactory.Init.btnHoverPlay();
                btnPlayOn = true;
                //SoundFactory.Init.btnHoverStop();
            }
            else
            {
                btnPlay_currentFrame.Y = 0;
                btnPlayOn = false;
            }

            if (btnCoreCollide())
            {
                btnCore_currentFrame.Y = 1;
                if (!btnCoreOn)
                    SoundFactory.Init.btnHoverPlay();
                btnCoreOn = true;
                //SoundFactory.Init.btnHoverStop();
            }
            else
            {
                btnCore_currentFrame.Y = 0;
                btnCoreOn = false;
            }

            if (btnOptionCollide())
            {
                btnOption_currentFrame.X = 1;
                if (!btnOptionOn)
                    SoundFactory.Init.btnHoverPlay();
                btnOptionOn = true;
                //SoundFactory.Init.btnHoverStop();
            }
            else
            {
                btnOption_currentFrame.X = 0;
                btnOptionOn = false;
            }

            if (btnLeadCollide())
            {
                btnLead_currentFrame.X = 1;
                if (!btnLeadOn)
                    SoundFactory.Init.btnHoverPlay();
                btnLeadOn = true;
                //SoundFactory.Init.btnHoverStop();
            }
            else
            {
                btnLead_currentFrame.X = 0;
                btnLeadOn = false;
            }

            if (btnExitCollide())
            {
                btnExit_currentFrame.Y = 1;
                if (!btnExitOn)
                    SoundFactory.Init.btnHoverPlay();
                btnExitOn = true;
                //SoundFactory.Init.btnHoverStop();
            }
            else
            {
                btnExit_currentFrame.Y = 0;
                btnExitOn = false;
            }

            if (btnHelpCollide())
            {
                btnHelp_currentFrame.Y = 1;
                if (!btnHelpOn)
                    SoundFactory.Init.btnHoverPlay();
                btnHelpOn = true;
                //SoundFactory.Init.btnHoverStop();
            }
            else
            {
                btnHelp_currentFrame.Y = 0;
                btnHelpOn = false;
            }            
        }

        private void AnimateCore(GameTime gameTime)
        {
            coreTimeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (coreTimeSinceLastFrame > coreMillisecondPerFrame)
            {
                coreTimeSinceLastFrame -= coreMillisecondPerFrame;
                ++currentFrame_screenCore.X;
                if (currentFrame_screenCore.X >= sheetSize_screenCore.X)
                {
                    currentFrame_screenCore.X = 0;
                }
            }
        }

        private bool btnPlayCollide() 
        {
            Rectangle btnPlayRec = new Rectangle((int)btnPlay_position.X - 40, (int)btnPlay_position.Y - 20, btnPlay_framesize.X - 90, btnPlay_framesize.Y - 40);
            Rectangle mouseRec = new Rectangle((int)CustCursor.Init.Position.X, (int)CustCursor.Init.Position.Y, (int)CustCursor.Init.custCursorTexture.Width, (int)CustCursor.Init.custCursorTexture.Height);

            return mouseRec.Intersects(btnPlayRec);
        }
        private bool btnCoreCollide() 
        {
            Rectangle btnCoreRec = new Rectangle((int)btnCore_position.X - btnCore_framesize.X/2, (int)btnCore_position.Y, btnCore_framesize.X, btnCore_framesize.Y);
            Rectangle mouseRec = new Rectangle((int)CustCursor.Init.Position.X, (int)CustCursor.Init.Position.Y, (int)CustCursor.Init.custCursorTexture.Width, (int)CustCursor.Init.custCursorTexture.Height);

            return mouseRec.Intersects(btnCoreRec);
        }
        private bool btnOptionCollide() 
        {
            Rectangle btnOptionRec = new Rectangle((int)btnOption_position.X - btnOption_framesize.X / 2, (int)btnOption_position.Y, btnOption_framesize.X, btnOption_framesize.Y - 30);
            Rectangle mouseRec = new Rectangle((int)CustCursor.Init.Position.X, (int)CustCursor.Init.Position.Y, (int)CustCursor.Init.custCursorTexture.Width, (int)CustCursor.Init.custCursorTexture.Height);

            return mouseRec.Intersects(btnOptionRec);
        }
        private bool btnLeadCollide() 
        {
            Rectangle btnLeadRec = new Rectangle((int)btnLead_position.X, (int)btnLead_position.Y, btnLead_framesize.X - 30, btnLead_framesize.Y - 30);
            Rectangle mouseRec = new Rectangle((int)CustCursor.Init.Position.X, (int)CustCursor.Init.Position.Y, (int)CustCursor.Init.custCursorTexture.Width, (int)CustCursor.Init.custCursorTexture.Height);

            return mouseRec.Intersects(btnLeadRec);
        }
        private bool btnExitCollide()
        {
            Rectangle btnExitRec = new Rectangle((int)btnExit_position.X - 40, (int)btnExit_position.Y, btnExit_framesize.X - 90, btnExit_framesize.Y - 40);
            Rectangle mouseRec = new Rectangle((int)CustCursor.Init.Position.X, (int)CustCursor.Init.Position.Y, (int)CustCursor.Init.custCursorTexture.Width, (int)CustCursor.Init.custCursorTexture.Height);

            return mouseRec.Intersects(btnExitRec);
        }
        private bool btnHelpCollide()
        {
            Rectangle btnHelpRec = new Rectangle((int)btnHelp_position.X - 40, (int)btnHelp_position.Y, btnHelp_framesize.X - 90, btnHelp_framesize.Y - 60);
            Rectangle mouseRec = new Rectangle((int)CustCursor.Init.Position.X, (int)CustCursor.Init.Position.Y, (int)CustCursor.Init.custCursorTexture.Width, (int)CustCursor.Init.custCursorTexture.Height);

            return mouseRec.Intersects(btnHelpRec);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            transitionIN.Draw(spriteBatch);
            if(isExit)
                transitionOUT.Draw(spriteBatch);

            //bottom layer
            spriteBatch.Draw(screenBase, Vector2.Zero, null, null, null, 0f, null, mainMenuColor, SpriteEffects.None, 0f);

            //core layer
            spriteBatch.Draw(screenCore, new Vector2(1366 / 1.98f, 768 / 2), null, 
                new Rectangle(currentFrame_screenCore.X * frameSize_screenCore.X, 
                              currentFrame_screenCore.Y * frameSize_screenCore.Y, 
                              frameSize_screenCore.X, 
                              frameSize_screenCore.Y), new Vector2(frameSize_screenCore.X/2, frameSize_screenCore.Y/2), 0f, null, mainMenuColor, SpriteEffects.None, 0.1f); 
            
            //top layer
            spriteBatch.Draw(screenTop, Vector2.Zero, null,null, null, 0f, null, mainMenuColor, SpriteEffects.None, 0.2f);

            //Button core           
            spriteBatch.Draw(btnCore, btnCore_position, null , 
                new Rectangle(btnCore_currentFrame.X * btnCore_framesize.X, 
                              btnCore_currentFrame.Y * btnCore_framesize.Y, 
                              btnCore_framesize.X, 
                              btnCore_framesize.Y), new Vector2(btnCore_framesize.X/2,btnCore_framesize.Y/2), 0f, null,mainMenuColor, SpriteEffects.None, 0.3f);

            //Button Play
            spriteBatch.Draw(btnPlay, btnPlay_position, null,
                new Rectangle(btnPlay_currentFrame.X * btnPlay_framesize.X,
                              btnPlay_currentFrame.Y * btnPlay_framesize.Y,
                              btnPlay_framesize.X,
                              btnPlay_framesize.Y), new Vector2(btnPlay_framesize.X / 2, btnPlay_framesize.Y / 2), 0f, null, mainMenuColor, SpriteEffects.None, 0.3f);

            //Button Option
            spriteBatch.Draw(btnOption, btnOption_position, null,
                new Rectangle(btnOption_currentFrame.X * btnOption_framesize.X,
                              btnOption_currentFrame.Y * btnOption_framesize.Y,
                              btnOption_framesize.X,
                              btnOption_framesize.Y), new Vector2(btnOption_framesize.X / 2, btnOption_framesize.Y / 2), 0f, null, mainMenuColor, SpriteEffects.None, 0.3f);

            //Button Lead
            spriteBatch.Draw(btnLead, btnLead_position, null,
                new Rectangle(btnLead_currentFrame.X * btnLead_framesize.X,
                              btnLead_currentFrame.Y * btnLead_framesize.Y,
                              btnLead_framesize.X,
                              btnLead_framesize.Y), new Vector2(btnLead_framesize.X / 2, btnLead_framesize.Y / 2), 0f, null, mainMenuColor, SpriteEffects.None, 0.3f);

            //Button Exit
            spriteBatch.Draw(btnExit, btnExit_position, null,
    new Rectangle(btnExit_currentFrame.X * btnExit_framesize.X,
                  btnExit_currentFrame.Y * btnExit_framesize.Y,
                  btnExit_framesize.X,
                  btnExit_framesize.Y), new Vector2(btnExit_framesize.X / 2, btnExit_framesize.Y / 2), 0f, null, mainMenuColor, SpriteEffects.None, 0.3f);

            spriteBatch.Draw(btnHelp, btnHelp_position, null,
    new Rectangle(btnHelp_currentFrame.X * btnHelp_framesize.X,
                  btnHelp_currentFrame.Y * btnHelp_framesize.Y,
                  btnHelp_framesize.X,
                  btnHelp_framesize.Y), new Vector2(btnHelp_framesize.X / 2, btnHelp_framesize.Y / 2), 0f, null, mainMenuColor, SpriteEffects.None, 0.3f);

            base.Draw(spriteBatch);
        }
    }
}
