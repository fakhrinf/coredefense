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
    public class SplashScreen : Screen
    {                
        Texture2D splashScreen1, splashScreen2;        
        int index = 1;

        List<Texture2D> splashScreen = new List<Texture2D>();

        private static SplashScreen Instance;
        public static SplashScreen Init 
        {
            get 
            {
                if (Instance == null)
                    Instance = new SplashScreen();
                return Instance;
            }
        }

        public override void Initialize(ContentManager content)
        {
            transitionIN = new Transition(content.Load<Texture2D>("Image\\transition"), true);
            splashScreen.Add(splashScreen1);
            splashScreen.Add(splashScreen2);
            base.Initialize(content);
        }

        public override void LoadContent(ContentManager content)
        {                       
            splashScreen1 = content.Load<Texture2D>("Image\\cyberline_splashscreen");
            splashScreen2 = content.Load<Texture2D>("Image\\ngegame_wallpaper");

            base.LoadContent(content);
        }

        public override void UnloadContent(ContentManager content)
        {
            base.UnloadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            transitionIN.FadeIn();
            if (transitionIN.CheckIn())
            {
                index += 1;
                transitionIN.Reset(true);
            }

            if (index.Equals(splashScreen.Count))
            {
                transitionIN.FadeIn();
                if (transitionIN.CheckIn())
                    isReady = true;
            }

            if (isReady)
            {
               Game1.currentGameState = Game1.GameState.MainMenu;
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            transitionIN.Draw(spriteBatch);
            switch (index)
            {
                case 1:
                    spriteBatch.Draw(splashScreen1, Vector2.Zero, Color.White);
                    break;
                case 2:
                    spriteBatch.Draw(splashScreen2, Vector2.Zero, Color.White);
                    break;
                default:
                    break;
            }

            base.Draw(spriteBatch);
        }
    }
}
