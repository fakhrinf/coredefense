#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.GamerServices;
using System.Windows.Forms;
#endregion

namespace CoreDefense
{
    /// <summary>
    /// This is the main type for your game. 
    /// </summary>
    public class Game1 : Game
    {        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public enum GameState { SplashScreen, MainMenu, Play, Option, GameOver, Lead, Transition, SubmitScore, HowGamePlay, Credits }
        public static GameState currentGameState;

        public bool mainMenuBGMOn = false;
        public bool playBGMOn = false;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //baca resolusi layar monitor
            //yang nanti akan di sesuaikan dengan resolusi game
            System.Drawing.Rectangle screenRect = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            //membaca semua resolusi yang ada di komputer
            System.Windows.Forms.Screen[] allScreenRect = System.Windows.Forms.Screen.AllScreens;
            //mengecek bila terdapat resolusi lebih tinggi dari 1366 dan 768
            // set resolusi menjadi 1366 dan 768
            for (int i = 0; i < allScreenRect.Length; i++)
            {
                if (allScreenRect[i].Bounds.Width > 1366 && allScreenRect[i].Bounds.Height > 768)
                {
                    screenRect.Width = 1366;
                    screenRect.Height = 768;
                }
            }

            Resolution.Init(ref graphics);

            Resolution.SetVirtualResolution(1366, 768);
            Resolution.SetResolution(screenRect.Width, screenRect.Height, true);            
        }

        void Game_LostFocus(object sender, EventArgs e)
        {
            GamePage.Init.isPause = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well. 
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.IsFullScreen = true;
            IsMouseVisible = false;

            currentGameState = GameState.SplashScreen;

            GamePage.Init.Initialize(Content);
            MainMenu.init.Initialize(Content);
            GameOverScreen.Init.Initialize(Content);
            SplashScreen.Init.Initialize(Content);
            SubmitScore.Init.Initialize(Content);
            HowGamePlay.Init.Initialize(Content);
            CreditsScreen.Init.Initialize(Content);
            //transition = new Transition();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            CustCursor.Init.LoadContent(Content);            
            MainMenu.init.LoadContent(Content);
            GamePage.Init.LoadContent(Content);
            GameOverScreen.Init.LoadContent(Content);
            SplashScreen.Init.LoadContent(Content);
            SubmitScore.Init.LoadContent(Content);
            HowGamePlay.Init.LoadContent(Content);
            SoundFactory.Init.LoadContent(Content);
            CreditsScreen.Init.LoadContent(Content);
            //transition.LoadContent(Content);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {            
            switch (currentGameState)
            {
                case GameState.SplashScreen:
                    SplashScreen.Init.Update(gameTime);
                    break;
                case GameState.MainMenu:
                    Deactivated += mainMenu_lostFocus;
                    Disposed += mainMenu_lostFocus;

                    //SoundFactory.Init.mainBGMplay();
                    //SoundFactory.Init.playBGMstop();

                    MainMenu.init.Update(gameTime);
                    CustCursor.Init.Update(gameTime);                    
                    break;
                case GameState.Play:
                    //event saat game hilang fokus
                    Deactivated += Game_LostFocus;
                                        
                    //SoundFactory.Init.playBGMplay();
                    //SoundFactory.Init.mainBGMstop();

                    GamePage.Init.Update(gameTime, Content);                    
                    CustCursor.Init.Update(gameTime);
                    break;
                case GameState.Option:
                    break;
                case GameState.GameOver:
                    GameOverScreen.Init.Update(gameTime);
                    CustCursor.Init.Update(gameTime);
                    break;
                case GameState.Lead:
                    break;
                case GameState.Transition:                    
                    break;
                case GameState.SubmitScore:
                    SubmitScore.Init.Update(gameTime);
                    CustCursor.Init.Update(gameTime);
                    break;                    
                case GameState.HowGamePlay:
                    HowGamePlay.Init.Update(gameTime);
                    CustCursor.Init.Update(gameTime);
                    break;
                case GameState.Credits:
                    CreditsScreen.Init.Update(gameTime);
                    CustCursor.Init.Update(gameTime);
                    break;
                default:
                    mainMenuBGMOn = false;
                    playBGMOn = false;
                    break;
            }
            
            // TODO: Add your update logic here            
            
            base.Update(gameTime);
        }

        void mainMenu_lostFocus(object sender, EventArgs e)
        {
            MainMenu.init.isReady = false;
            MainMenu.init.isExit = false;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            Resolution.BeginDraw();
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());

            switch (currentGameState)
            {
                case GameState.SplashScreen:
                    SplashScreen.Init.Draw(spriteBatch);
                    break;
                case GameState.MainMenu:                    
                    MainMenu.init.Draw(spriteBatch);
                    CustCursor.Init.Draw(spriteBatch); 
                    break;
                case GameState.Play:
                    GamePage.Init.Draw(spriteBatch);
                    CustCursor.Init.Draw(spriteBatch);
                    break;
                case GameState.Option:
                    break;
                case GameState.GameOver:
                    GameOverScreen.Init.Draw(spriteBatch);
                    CustCursor.Init.Draw(spriteBatch);
                    break;
                case GameState.Lead:
                    break;
                case GameState.Transition:                    
                    break;
                case GameState.SubmitScore:
                    SubmitScore.Init.Draw(spriteBatch);
                    CustCursor.Init.Draw(spriteBatch);
                    break;
                case GameState.HowGamePlay:
                    HowGamePlay.Init.Draw(spriteBatch);
                    CustCursor.Init.Draw(spriteBatch);
                    break;
                case GameState.Credits:
                    CreditsScreen.Init.Draw(spriteBatch);
                    CustCursor.Init.Draw(spriteBatch);
                    break;
                default:
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
