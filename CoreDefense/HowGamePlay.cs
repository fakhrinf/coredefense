using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CoreDefense
{
    public class HowGamePlay : Screen
    {
        Texture2D HowGamePlayTexture, btnBack;

        bool btnBackOn = false;

        Point btnBack_frameSize = new Point(255, 100);
        Point btnBack_currentFrame = new Point(0, 0);
        Point btnBack_sheetSize = new Point(1, 2);
        Vector2 btnBack_position = new Vector2(1366 / 2 + 500, 768 / 2 + 300);

        private static HowGamePlay Instance;
        public static HowGamePlay Init 
        {
            get 
            {
                if (Instance == null)
                    Instance = new HowGamePlay();
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
            btnBack = content.Load<Texture2D>("Image\\btnBack_animate");
            HowGamePlayTexture = content.Load<Texture2D>("Image\\HowGamePlay");
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
                if (btnBackCollide())
                {
                    btnBack_currentFrame.Y = 1;
                    if (!btnBackOn)
                        SoundFactory.Init.btnHoverPlay();
                    btnBackOn = true;
                    SoundFactory.Init.btnHoverStop();
                }
                else
                {
                    btnBack_currentFrame.Y = 0;
                    btnBackOn = false;
                }

                if (btnBackCollide() && (mouseState.LeftButton.Equals(ButtonState.Pressed) && prevMouseState.LeftButton.Equals(ButtonState.Released)))                
                    doBack();                

                if (keyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape))
                    doBack();
            }
            base.Update(gameTime);
        }

        private void doBack()
        {
            Game1.currentGameState = Game1.GameState.MainMenu;
            transitionIN.Reset(true);
            SoundFactory.Init.btnClickPlay();
        }

        private bool btnBackCollide()
        {
            Rectangle btnBackRec = new Rectangle((int)btnBack_position.X - btnBack_frameSize.X / 2, (int)btnBack_position.Y, btnBack_frameSize.X, btnBack_frameSize.Y);
            Rectangle mouseRec = new Rectangle((int)CustCursor.Init.Position.X, (int)CustCursor.Init.Position.Y, (int)CustCursor.Init.custCursorTexture.Width, (int)CustCursor.Init.custCursorTexture.Height);

            return mouseRec.Intersects(btnBackRec);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            transitionIN.Draw(spriteBatch);
            spriteBatch.Draw(HowGamePlayTexture, Vector2.Zero, Color.White);
            spriteBatch.Draw(btnBack, btnBack_position, null, new Rectangle(btnBack_currentFrame.X * btnBack_frameSize.X, btnBack_currentFrame.Y * btnBack_frameSize.Y, btnBack_frameSize.X, btnBack_frameSize.Y), new Vector2(btnBack_frameSize.X / 2, btnBack_frameSize.Y / 2), 0f, null, Color.White, SpriteEffects.None, 0.1f);
            base.Draw(spriteBatch);
        }
    }
}
