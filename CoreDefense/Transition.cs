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
    public class Transition
    {
        //private static Transition Instance;
        //public static Transition Init 
        //{
        //    get 
        //    {
        //        if (Instance == null)
        //            Instance = new Transition();
        //        return Instance;
        //    }
        //}

        Texture2D transitionTexture;
        Color colorAlpha = new Color(255, 255, 255);

        public Color TransitionColor { get { return colorAlpha; } }
        public byte FadeColor { set { colorAlpha.A = value; } get { return colorAlpha.A; } }

        public Transition(Texture2D texture, bool fadeIn) 
        {
            this.transitionTexture = texture;
            if (fadeIn)
                this.colorAlpha.A = 255;
            else
                this.colorAlpha.A = 0;
        }

        public void LoadContent(ContentManager content) 
        {
            transitionTexture = content.Load<Texture2D>("Image\\transition");
        }

        public void UnloadContent(ContentManager content) 
        {
            content.Unload();
        }

        public void Update(GameTime gameTime)
        {
            if(CheckOut())
                FadeIn();
        }

        public void Reset(bool fadeIn) 
        {
            if (fadeIn)
                colorAlpha.A = 255;
            else
                colorAlpha.A = 0;
        }

        public void FadeIn()
        {
            if (colorAlpha.A != 0)
                colorAlpha.A--;
        }

        public void FadeIn(byte speed)
        {            
            if (colorAlpha.A != 0)
                colorAlpha.A -= speed;
        }

        public void FadeIn(byte speed, byte value)
        {
            if (colorAlpha.A != value)
                colorAlpha.A -= speed;
        }

        public void FadeOut()
        {
            if (colorAlpha.A != 255)
                colorAlpha.A++;
        }

        public void FadeOut(byte speed)
        {
            if (colorAlpha.A != 255)
                colorAlpha.A += speed;
        }

        public void FadeOut(byte speed, byte value ) 
        {
            if (colorAlpha.A != value)
                colorAlpha.A += speed;
        }

        public bool CheckIn() 
        {
            if (colorAlpha.A == 0)
                return true;
            return false;
        }
        public bool CheckOut()
        {
            if (colorAlpha.A == 255)
                return true;
            return false;
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(transitionTexture, Vector2.Zero, null, null, null, 0f, new Vector2(1366, 768),colorAlpha, SpriteEffects.None, 0.8f);
        }
    }
}
