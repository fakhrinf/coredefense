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
    public class CustCursor
    {
        public Texture2D custCursorTexture { private set; get; }
        public Texture2D playCustCursorTexture { private set; get; }
        public Vector2 Position { private set; get; }

        private static CustCursor Instance;

        public static CustCursor Init
        {
            get
            {
                if (Instance == null)                
                    Instance = new CustCursor();

                return Instance;
            }           
        }

        public void LoadContent(ContentManager content)
        {
            custCursorTexture = content.Load<Texture2D>("Image\\custcursor");
            playCustCursorTexture = content.Load<Texture2D>("Image\\custcursor_target");
        }

        public void UnloadContent(ContentManager content)
        {
            content.Unload();
        }

        public void Update(GameTime gameTime) 
        {
            Position = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            switch (Game1.currentGameState)
            {
                case Game1.GameState.MainMenu:
                case Game1.GameState.Option:
                case Game1.GameState.GameOver:
                case Game1.GameState.Lead:
                case Game1.GameState.SubmitScore:
                case Game1.GameState.HowGamePlay:
                case Game1.GameState.Credits:
                    spriteBatch.Draw(custCursorTexture, Position, null, null, Vector2.Zero, 0f, null, Color.White, SpriteEffects.None, 1f);
                    break;
                case Game1.GameState.Play:
                    if (GamePage.Init.isPause)
                        spriteBatch.Draw(custCursorTexture, Position, null, null, Vector2.Zero, 0f, null, Color.White, SpriteEffects.None, 1f);
                    else
                        spriteBatch.Draw(playCustCursorTexture, Position, null, null, new Vector2(playCustCursorTexture.Width / 2, playCustCursorTexture.Height / 2), 0f, null, Color.White, SpriteEffects.None, 1f);
                    break;
                default:
                    break;
            }
        }
    }
}
