using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CoreDefense.SubmitScoreReference;

namespace CoreDefense
{
    public class SubmitScore : Screen
    {
        Texture2D btnCancel, btnEnter, background;
        SpriteFont typeYourNameMessage, charCounter;

        bool btnCancelOn, btnEnterOn;

        Vector2 typeYourNameMessage_position = new Vector2(1366 / 2 - 500, 768 / 2 - 300);
        Vector2 charCounter_position = new Vector2(1366 / 2 + 300, 768 / 2 - 300);

        Point btnCancel_frameSize = new Point(314, 100);
        Point btnCancel_currentFrame = new Point(0, 0);
        Point btnCancel_sheetSize = new Point(1, 2);
        Vector2 btnCancel_position = new Vector2((1366 / 2) - 400, (768 / 2) + 70);

        Point btnEnter_frameSize = new Point(257, 98);
        Point btnEnter_currentFrame = new Point(0, 0);
        Point btnEnter_sheetSize = new Point(1, 2);
        Vector2 btnEnter_position = new Vector2((1366 / 2) + 400, (768 / 2) + 70);

        public bool isSubmitted = false;

        private static SubmitScore Instance;
        public static SubmitScore Init
        {
            get 
            {
                if (Instance == null)
                    Instance = new SubmitScore();
                return Instance;
            }
        }


        public override void Initialize(ContentManager content)
        {
            transitionIN = new Transition(content.Load<Texture2D>("Image\\transition"), true);
            transitionOUT = new Transition(content.Load<Texture2D>("Image\\transition"), false);            

            base.Initialize(content);
        }

        public override void LoadContent(ContentManager content)
        {
            typeYourNameMessage = content.Load<SpriteFont>("Fonts\\gamePlayScore");
            charCounter = content.Load<SpriteFont>("Fonts\\gamePlayScore");

            background = content.Load<Texture2D>("Image\\submitScore_BG");
            btnEnter = content.Load<Texture2D>("Image\\btnEnter_animate");
            btnCancel = content.Load<Texture2D>("Image\\btnCancel_animate");
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

            prevKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            transitionIN.FadeIn(5);
            if (transitionIN.CheckIn())
                isReady = true;

            if (isReady)
            {
                btnHover();
                TypeText.Init.Update(gameTime);

                if (btnCancelCollide() && (mouseState.LeftButton.Equals(ButtonState.Pressed) && prevMouseState.LeftButton.Equals(ButtonState.Released)))                
                    doCancel();

                if (btnEnterCollide() && (mouseState.LeftButton.Equals(ButtonState.Pressed) && prevMouseState.LeftButton.Equals(ButtonState.Released)))
                {
                    if(!TypeText.Init.text.Equals(""))
                        doSubmitScore();
                }

                if (keyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape))
                    doCancel();

                if (keyboardState.IsKeyDown(Keys.Enter) && prevKeyboardState.IsKeyUp(Keys.Enter))
                    doSubmitScore();

                if (isSubmitted)
                {
                    Game1.currentGameState = Game1.GameState.GameOver;
                    transitionIN.Reset(true);
                }
            }

            base.Update(gameTime);
        }

        private void doSubmitScore()
        {
            SoundFactory.Init.btnClickPlay();

            //METHOD UNTUK INPUT SCORE KE DATABASE
            Service ss = new Service();
            ss.SubmitScore(TypeText.Init.text, GamePage.Init.score);
            isSubmitted = true;
        }

        private void doCancel()
        {
            SoundFactory.Init.btnClickPlay();

            Game1.currentGameState = Game1.GameState.GameOver;
            transitionIN.Reset(true);
        }

        private void btnHover() 
        {
            if (btnCancelCollide())
            {
                btnCancel_currentFrame.Y = 1;
                if (!btnCancelOn)
                    SoundFactory.Init.btnHoverPlay();
                btnCancelOn = true;
            }
            else
            {
                btnCancel_currentFrame.Y = 0;
                btnCancelOn = false;
            }

            if (btnEnterCollide())
            {
                btnEnter_currentFrame.Y = 1;
                if (!btnEnterOn)
                    SoundFactory.Init.btnHoverPlay();
                btnEnterOn = true;
            }
            else
            {
                btnEnter_currentFrame.Y = 0;
                btnEnterOn = false;
            }
        }

        private bool btnCancelCollide()
        {
            Rectangle btnCancelRec = new Rectangle((int)btnCancel_position.X - 40, (int)btnCancel_position.Y - 20, btnCancel_frameSize.X - 150, btnCancel_frameSize.Y - 40);
            Rectangle mouseRec = new Rectangle((int)CustCursor.Init.Position.X, (int)CustCursor.Init.Position.Y, (int)CustCursor.Init.custCursorTexture.Width, (int)CustCursor.Init.custCursorTexture.Height);

            return mouseRec.Intersects(btnCancelRec);
        }
        private bool btnEnterCollide()
        {
            Rectangle btnEnterRec = new Rectangle((int)btnEnter_position.X - 40, (int)btnEnter_position.Y - 20, btnEnter_frameSize.X - 150, btnEnter_frameSize.Y - 40);
            Rectangle mouseRec = new Rectangle((int)CustCursor.Init.Position.X, (int)CustCursor.Init.Position.Y, (int)CustCursor.Init.custCursorTexture.Width, (int)CustCursor.Init.custCursorTexture.Height);

            return mouseRec.Intersects(btnEnterRec);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {            
            transitionIN.Draw(spriteBatch);
            TypeText.Init.Draw(spriteBatch);

            spriteBatch.DrawString(typeYourNameMessage, "TYPE YOUR NAME", typeYourNameMessage_position, Color.White);
            spriteBatch.DrawString(charCounter, TypeText.Init.limitText.ToString(), charCounter_position, Color.White);

            spriteBatch.Draw(btnCancel, btnCancel_position, null, new Rectangle(btnCancel_currentFrame.X * btnCancel_frameSize.X, btnCancel_currentFrame.Y * btnCancel_frameSize.Y, btnCancel_frameSize.X, btnCancel_frameSize.Y), new Vector2(btnCancel_frameSize.X / 2, btnCancel_frameSize.Y / 2), 0f, null, Color.White, SpriteEffects.None, 0.7f);

            spriteBatch.Draw(btnEnter, btnEnter_position, null, new Rectangle(btnEnter_currentFrame.X * btnEnter_frameSize.X, btnEnter_currentFrame.Y * btnEnter_frameSize.Y, btnEnter_frameSize.X, btnEnter_frameSize.Y), new Vector2(btnEnter_frameSize.X / 2, btnEnter_frameSize.Y / 2), 0f, null, Color.White, SpriteEffects.None, 0.7f);

            spriteBatch.Draw(background, Vector2.Zero, null, null, null, 0f, null, Color.White,SpriteEffects.None, -1f);
            base.Draw(spriteBatch);
        }
    }
}
