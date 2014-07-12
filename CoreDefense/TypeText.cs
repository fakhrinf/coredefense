using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace CoreDefense
{
    public class TypeText
    {
        private static TypeText Instance;

        public static TypeText Init 
        {
            get 
            {
                if (Instance == null)
                    Instance = new TypeText();
                return Instance;
            }
        }

        public SpriteFont textFont;
        public Vector2 textFontPosition;        
        public string text = "";
        public int limitText = 15;

        public Texture2D TextPointer;
        public Vector2 TextPointer_position = new Vector2(1366 / 2 - 450, 768 / 2 - 120);

        KeyboardState keyboardState, prevKeyBoardState;

        public void GetUserInput() 
        {
            prevKeyBoardState = keyboardState;
            keyboardState = Keyboard.GetState();

            if (!limitText.Equals(0))
            {
                if (keyboardState.IsKeyDown(Keys.A) && prevKeyBoardState.IsKeyUp(Keys.A))
                {
                    text += "A";
                    --limitText;
                }
                if (keyboardState.IsKeyDown(Keys.B) && prevKeyBoardState.IsKeyUp(Keys.B))
                {
                    text += "B";
                    --limitText;
                }
                if (keyboardState.IsKeyDown(Keys.C) && prevKeyBoardState.IsKeyUp(Keys.C))
                {
                    text += "C";
                    --limitText;
                }
                if (keyboardState.IsKeyDown(Keys.D) && prevKeyBoardState.IsKeyUp(Keys.D))
                {
                    text += "D";
                    --limitText;
                }
                if (keyboardState.IsKeyDown(Keys.E) && prevKeyBoardState.IsKeyUp(Keys.E))
                {
                    text += "E";
                    --limitText;
                }
                if (keyboardState.IsKeyDown(Keys.F) && prevKeyBoardState.IsKeyUp(Keys.F))
                {
                    text += "F";
                    --limitText;
                }
                if (keyboardState.IsKeyDown(Keys.G) && prevKeyBoardState.IsKeyUp(Keys.G))
                {
                    text += "G";
                    --limitText;
                }
                if (keyboardState.IsKeyDown(Keys.H) && prevKeyBoardState.IsKeyUp(Keys.H))
                {
                    text += "H";
                    --limitText;
                }
                if (keyboardState.IsKeyDown(Keys.I) && prevKeyBoardState.IsKeyUp(Keys.I))
                {
                    text += "I";
                    --limitText;
                }
                if (keyboardState.IsKeyDown(Keys.J) && prevKeyBoardState.IsKeyUp(Keys.J))
                {
                    text += "J";
                    --limitText;
                }
                if (keyboardState.IsKeyDown(Keys.K) && prevKeyBoardState.IsKeyUp(Keys.K))
                {
                    text += "K";
                    --limitText;
                }
                if (keyboardState.IsKeyDown(Keys.L) && prevKeyBoardState.IsKeyUp(Keys.L))
                {
                    text += "L";
                    --limitText;
                }
                if (keyboardState.IsKeyDown(Keys.M) && prevKeyBoardState.IsKeyUp(Keys.M))
                {
                    text += "M";
                    --limitText;
                }
                if (keyboardState.IsKeyDown(Keys.N) && prevKeyBoardState.IsKeyUp(Keys.N))
                {
                    text += "N";
                    --limitText;
                }
                if (keyboardState.IsKeyDown(Keys.O) && prevKeyBoardState.IsKeyUp(Keys.O))
                {
                    text += "O";
                    --limitText;
                }
                if (keyboardState.IsKeyDown(Keys.P) && prevKeyBoardState.IsKeyUp(Keys.P))
                {
                    text += "P";
                    --limitText;
                }
                if (keyboardState.IsKeyDown(Keys.Q) && prevKeyBoardState.IsKeyUp(Keys.Q))
                {
                    text += "Q";
                    --limitText;
                }
                if (keyboardState.IsKeyDown(Keys.R) && prevKeyBoardState.IsKeyUp(Keys.R))
                {
                    text += "R";
                    --limitText;
                }
                if (keyboardState.IsKeyDown(Keys.S) && prevKeyBoardState.IsKeyUp(Keys.S))
                {
                    text += "S";
                    --limitText;
                }
                if (keyboardState.IsKeyDown(Keys.T) && prevKeyBoardState.IsKeyUp(Keys.T))
                {
                    text += "T";
                    --limitText;
                }
                if (keyboardState.IsKeyDown(Keys.U) && prevKeyBoardState.IsKeyUp(Keys.U))
                {
                    text += "U";
                    --limitText;
                }
                if (keyboardState.IsKeyDown(Keys.V) && prevKeyBoardState.IsKeyUp(Keys.V))
                {
                    text += "V";
                    --limitText;
                }
                if (keyboardState.IsKeyDown(Keys.W) && prevKeyBoardState.IsKeyUp(Keys.W))
                {
                    text += "W";
                    --limitText;
                }
                if (keyboardState.IsKeyDown(Keys.X) && prevKeyBoardState.IsKeyUp(Keys.X))
                {
                    text += "X";
                    --limitText;
                }
                if (keyboardState.IsKeyDown(Keys.Y) && prevKeyBoardState.IsKeyUp(Keys.Y))
                {
                    text += "Y";
                    --limitText;
                }
                if (keyboardState.IsKeyDown(Keys.Z) && prevKeyBoardState.IsKeyUp(Keys.Z))
                {
                    text += "Z";
                    --limitText;
                }
                if (keyboardState.IsKeyDown(Keys.Space) && prevKeyBoardState.IsKeyUp(Keys.Space))
                {
                    text += " ";
                    --limitText;
                }
            }

            if (keyboardState.IsKeyDown(Keys.Back) && prevKeyBoardState.IsKeyUp(Keys.Back))
            {
                if (!text.Equals(""))
                {
                    for (int i = text.Length; i >= text.Length; i--)
                    {
                        i--;
                        ++limitText;
                        text = text.Remove(i);
                    }
                }
            }
        }

        public void LoadContent(ContentManager content) 
        {
            TextPointer = content.Load<Texture2D>("Image\\typePointer");
            textFont = content.Load<SpriteFont>("Fonts\\gamePlayScore");
        }

        public void Update(GameTime gameTime) 
        {
            GetUserInput();

            if (!text.Equals(""))
            {
                TextPointer_position = new Vector2(textFont.MeasureString(text).X + 240, textFont.MeasureString(text).Y + 150);
                textFontPosition = new Vector2(1366 / 2 - 450, 768 / 2 - 120);
            }
            else
            {
                TextPointer_position = new Vector2(1366 / 2 - 450, 768 / 2 - 120);
            }
            
        }
        
        public void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.DrawString(textFont, text, textFontPosition, Color.White);
            //spriteBatch.DrawString(textFont, text, textFontPosition, Color.Red);
            spriteBatch.Draw(TextPointer, TextPointer_position, null, null, null, 0f,null, Color.White, SpriteEffects.None, 0.9f);
            
        }
    }
}
