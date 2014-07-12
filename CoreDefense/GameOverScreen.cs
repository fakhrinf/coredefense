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
    public class GameOverScreen : Screen
    {
        Texture2D GameOverScreenTexture;

        Texture2D btnPlayAgain, btnMenu, btnSubmit, btnView;

        bool btnPlayAgainOn, btnMenuOn, btnSubmitOn, btnViewOn;

        SpriteFont gameResult;
               
        Vector2 gameResultPosition = new Vector2(1366 / 2, (768 / 2) - 75);

        Point btnPlayAgain_frameSize = new Point(243, 98);
        Point btnPlayAgain_currentFrame = new Point(0, 0);
        Point btnPlayAgain_sheetSize = new Point(1, 2);
        Vector2 btnPlayAgain_position = new Vector2(1366 / 2, (768 / 2) + 30);        

        Point btnMenu_frameSize = new Point(275, 99);
        Point btnMenu_currentFrame = new Point(0, 0);
        Point btnMenu_sheetSize = new Point(1, 2);
        Vector2 btnMenu_position = new Vector2((1366 / 2) - 400, (768 / 2) + 30);

        Point btnSubmit_frameSize = new Point(334, 100);
        Point btnSubmit_currentFrame = new Point(0, 0);
        Point btnSubmit_sheetSize = new Point(1, 2);
        Vector2 btnSubmit_position = new Vector2((1366 / 2) + 400, (768 / 2) + 30);

        Point btnView_frameSize = new Point(253, 98);
        Point btnView_currentFrame = new Point(0, 0);
        Point btnView_sheetSize = new Point(1, 2);
        Vector2 btnView_position = new Vector2((1366 / 2) + 400, (768 / 2) + 30);

        private static GameOverScreen Instance;
        public static GameOverScreen Init
        {
            get
            {
                if(Instance == null)
                    Instance = new GameOverScreen();
                return Instance;
            }
        }

        public override void Initialize(ContentManager content)
        {
            transitionIN = new Transition(content.Load<Texture2D>("Image\\transition"), true);
            transitionOUT = new Transition(content.Load<Texture2D>("Image\\transition"), false);
            base.Initialize();
        }

        public override void LoadContent(ContentManager content)
        {
            GameOverScreenTexture = content.Load<Texture2D>("Image\\gameOverScreen");
            btnPlayAgain = content.Load<Texture2D>("Image\\btnplay_animate");
            btnMenu = content.Load<Texture2D>("Image\\btnMenu_animate");
            btnSubmit = content.Load<Texture2D>("Image\\btnSubmit_animate");
            btnView = content.Load<Texture2D>("Image\\btnView_animate");
            gameResult = content.Load<SpriteFont>("Fonts\\gamePlayScore");            

            TypeText.Init.LoadContent(content);
            base.LoadContent(content);
        }

        public override void UnloadContent(ContentManager content)
        {
            base.UnloadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();

            SoundFactory.Init.playBGM(0);

            transitionIN.FadeIn(5);
            if (transitionIN.CheckIn())
                isReady = true;

                if (isReady)
                {
                    ButtonHover();

                    if (btnPlayCollide() && (mouseState.LeftButton.Equals(ButtonState.Pressed) && prevMouseState.LeftButton.Equals(ButtonState.Released)))
                    {
                        SoundFactory.Init.btnClickPlay();
                        SoundFactory.Init.stopBGM();

                        transitionIN.Reset(true);
                        GamePage.Init.ResetGame();
                        Game1.currentGameState = Game1.GameState.Play;
                        SubmitScore.Init.isSubmitted = false;
                    }

                    if (btnMenuCollide() && (mouseState.LeftButton.Equals(ButtonState.Pressed) && prevMouseState.LeftButton.Equals(ButtonState.Released)))
                    {
                        SoundFactory.Init.btnClickPlay();

                        Game1.currentGameState = Game1.GameState.MainMenu;
                        transitionIN.Reset(true);
                        SubmitScore.Init.isSubmitted = false;
                    }

                    if (!SubmitScore.Init.isSubmitted)
                    {
                        if (btnSubmitCollide() && (mouseState.LeftButton.Equals(ButtonState.Pressed) && prevMouseState.LeftButton.Equals(ButtonState.Released)))
                        {
                            SoundFactory.Init.btnClickPlay();

                            Game1.currentGameState = Game1.GameState.SubmitScore;
                            transitionIN.Reset(true);
                            SubmitScore.Init.isSubmitted = false;
                            TypeText.Init.text = "";
                            TypeText.Init.limitText = 15;                            
                        }
                    }
                    else
                    {
                        if (btnSubmitCollide() && (mouseState.LeftButton.Equals(ButtonState.Pressed) && prevMouseState.LeftButton.Equals(ButtonState.Released)))
                            System.Diagnostics.Process.Start("IExplore.exe", "localhost/CoreDefenseWeb/leaderboard.aspx");
                        
                    }
                }            

            base.Update(gameTime);
        }

        private void ButtonHover()
        {
            if (btnPlayCollide())
            {
                btnPlayAgain_currentFrame.Y = 1;
                if (!btnPlayAgainOn)
                    SoundFactory.Init.btnHoverPlay();
                btnPlayAgainOn = true;
            }
            else
            {
                btnPlayAgain_currentFrame.Y = 0;
                btnPlayAgainOn = false;
            }

            if (btnMenuCollide())
            {
                btnMenu_currentFrame.Y = 1;
                if (!btnMenuOn)
                    SoundFactory.Init.btnHoverPlay();
                btnMenuOn = true;
            }
            else
            {
                btnMenu_currentFrame.Y = 0;
                btnMenuOn = false;
            }

            if (!SubmitScore.Init.isSubmitted)
            {
                if (btnSubmitCollide())
                {
                    btnSubmit_currentFrame.Y = 1;
                    if (!btnSubmitOn)
                        SoundFactory.Init.btnHoverPlay();
                    btnSubmitOn = true;
                }
                else
                {
                    btnSubmit_currentFrame.Y = 0;
                    btnSubmitOn = false;
                }
            }
            else
            {
                if (btnViewCollide())
                {
                    btnView_currentFrame.Y = 1;
                    if (!btnViewOn)
                        SoundFactory.Init.btnHoverPlay();
                    btnViewOn = true;
                }
                else
                {
                    btnView_currentFrame.Y = 0;
                    btnViewOn = false;
                }
            }               
        }

        private bool btnPlayCollide() 
        {
            Rectangle btnPlayRec = new Rectangle((int)btnPlayAgain_position.X - 70, (int)btnPlayAgain_position.Y - 20, btnPlayAgain_frameSize.X - 90, btnPlayAgain_frameSize.Y - 40);
            Rectangle mouseRec = new Rectangle((int)CustCursor.Init.Position.X, (int)CustCursor.Init.Position.Y, (int)CustCursor.Init.custCursorTexture.Width, (int)CustCursor.Init.custCursorTexture.Height);

            return mouseRec.Intersects(btnPlayRec);
        }

        private bool btnMenuCollide()
        {
            Rectangle btnMenuRec = new Rectangle((int)btnMenu_position.X - 70, (int)btnMenu_position.Y - 20, btnMenu_frameSize.X - 120, btnMenu_frameSize.Y - 40);
            Rectangle mouseRec = new Rectangle((int)CustCursor.Init.Position.X, (int)CustCursor.Init.Position.Y, (int)CustCursor.Init.custCursorTexture.Width, (int)CustCursor.Init.custCursorTexture.Height);

            return mouseRec.Intersects(btnMenuRec);
        }

        private bool btnSubmitCollide()
        {
            Rectangle btnSubmitRec = new Rectangle((int)btnSubmit_position.X - 40, (int)btnSubmit_position.Y - 20, btnSubmit_frameSize.X - 150, btnSubmit_frameSize.Y - 40);
            Rectangle mouseRec = new Rectangle((int)CustCursor.Init.Position.X, (int)CustCursor.Init.Position.Y, (int)CustCursor.Init.custCursorTexture.Width, (int)CustCursor.Init.custCursorTexture.Height);

            return mouseRec.Intersects(btnSubmitRec);
        }

        private bool btnViewCollide()
        {
            Rectangle btnViewRec = new Rectangle((int)btnView_position.X - 40, (int)btnView_position.Y - 20, btnView_frameSize.X - 150, btnView_frameSize.Y - 40);
            Rectangle mouseRec = new Rectangle((int)CustCursor.Init.Position.X, (int)CustCursor.Init.Position.Y, (int)CustCursor.Init.custCursorTexture.Width, (int)CustCursor.Init.custCursorTexture.Height);

            return mouseRec.Intersects(btnViewRec);
        }
        

        public override void Draw(SpriteBatch spriteBatch)
        {
            string result;
            if (!SubmitScore.Init.isSubmitted)
                result = "YOUR SCORE: " + GamePage.Init.score;
            else
                result = "SCORE SUBMITTED";
            
                transitionIN.Draw(spriteBatch);
                spriteBatch.Draw(GameOverScreenTexture, Vector2.Zero, null, null, null, 0f, null, Color.White, SpriteEffects.None, 0f);

                spriteBatch.DrawString(gameResult, result, gameResultPosition, Color.White, 0f, new Vector2(gameResult.MeasureString(result).X / 2, gameResult.MeasureString(result).Y / 2), 1f, SpriteEffects.None, 0.1f);

                //Button Play Again
                spriteBatch.Draw(btnPlayAgain, btnPlayAgain_position, null,
                    new Rectangle(btnPlayAgain_currentFrame.X * btnPlayAgain_frameSize.X,
                                  btnPlayAgain_currentFrame.Y * btnPlayAgain_frameSize.Y,
                                  btnPlayAgain_frameSize.X,
                                  btnPlayAgain_frameSize.Y),
                    new Vector2(btnPlayAgain_frameSize.X / 2,
                                btnPlayAgain_frameSize.Y / 2), 0f, null, Color.White, SpriteEffects.None, 0.1f);

                //Button Menu
                spriteBatch.Draw(btnMenu, btnMenu_position, null,
                    new Rectangle(btnMenu_currentFrame.X * btnMenu_frameSize.X,
                                  btnMenu_currentFrame.Y * btnMenu_frameSize.Y,
                                  btnMenu_frameSize.X,
                                  btnMenu_frameSize.Y),
                    new Vector2(btnMenu_frameSize.X / 2,
                                btnMenu_frameSize.Y / 2), 0f, null, Color.White, SpriteEffects.None, 0.1f);

                if (!SubmitScore.Init.isSubmitted)
                {
                    //Button Submit
                    spriteBatch.Draw(btnSubmit, btnSubmit_position, null,
                        new Rectangle(btnSubmit_currentFrame.X * btnSubmit_frameSize.X,
                                      btnSubmit_currentFrame.Y * btnSubmit_frameSize.Y,
                                      btnSubmit_frameSize.X,
                                      btnSubmit_frameSize.Y),
                        new Vector2(btnSubmit_frameSize.X / 2, btnSubmit_frameSize.Y / 2), 0f, null, Color.White, SpriteEffects.None, 0.1f);
                }
                else
                {
                    //Button View
                    spriteBatch.Draw(btnView, btnView_position, null,
                        new Rectangle(btnView_currentFrame.X * btnView_frameSize.X,
                                      btnView_currentFrame.Y * btnView_frameSize.Y,
                                      btnView_frameSize.X,
                                      btnView_frameSize.Y),
                        new Vector2(btnView_frameSize.X / 2, btnView_frameSize.Y / 2), 0f, null, Color.White, SpriteEffects.None, 0.1f);
                }                      

            base.Draw(spriteBatch);
        }
    }
}
