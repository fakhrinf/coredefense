using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace CoreDefense
{
    public class Screen
    {
        //position
        //protected Vector2 Position { get; set; }
        //protected List<Texture2D> ScreenImg = new List<Texture2D>();
        //layering
        //protected int[] layer = new int[9];
        protected Transition transitionIN, transitionOUT;
        public bool isReady = false;

        protected MouseState prevMouseState, mouseState;
        protected KeyboardState prevKeyboardState, keyboardState;

        public virtual void Initialize() { }
        public virtual void Initialize(ContentManager content) { }
        public virtual void LoadContent(ContentManager content) { }
        public virtual void UnloadContent(ContentManager content) { content.Unload(); }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Update(GameTime gameTime, ContentManager content) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}
