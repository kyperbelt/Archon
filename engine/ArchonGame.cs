using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Archon.engine.utils;

namespace Archon.engine
{
    public abstract class ArchonGame : Game
    {

        private static ALogger log;
        public static ALogger Log
        {
            get
            {
                if (log == null) log = new DesktopConsoleLogger();
                return log;
            }
            set
            {
                log = value;
            }
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D texture;
        private int c_width; //current width
        private int c_height; //current height

        public ArchonGame(int width, int height)
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            Window.AllowUserResizing = true;
            this.c_width = width;
            this.c_height = height;

            Content.RootDirectory = "assets";
        }

      
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            var filePath = Path.Combine(Content.RootDirectory, "textures/WeinerCon.png");
            var atlasPath = Path.Combine(Content.RootDirectory, "textures/game.atlas");

            using (var stream = TitleContainer.OpenStream(filePath))
            {
                texture = Texture2D.FromStream(graphics.GraphicsDevice, stream);
            }

            string atlasText = null;

            using (var stream = TitleContainer.OpenStream(atlasPath))
            {
                using (StreamReader reader = new StreamReader(stream, true))
                {
                    atlasText = reader.ReadToEnd();
                }

            }

            Console.WriteLine(atlasText);
            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

       
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);

            int bwidth = graphics.GraphicsDevice.PresentationParameters.BackBufferWidth;
            int bheight = graphics.GraphicsDevice.PresentationParameters.BackBufferHeight;
            if (c_width != bwidth || c_height != bheight)
            {
                c_height = bheight;
                c_width = bwidth;
                archonResize(c_width, c_height);
            }
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(texture, new Vector2(0, 0), Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public virtual void archonPaused() { }
        public virtual void archonResumed() { }

        /**
         * <summary>
         * archon game should be created here
         * </summary>
         */
        public abstract void archonCreate();
        /**
         * archon game window was resized
         */
        public abstract void archonResize(int width, int height);
        /**
         * archon rendering all logic and rendering is done in here
         */
        public abstract void archonDraw(float delta);

        /**
        *
        * archon update
        */
        public abstract void archonUpdate(float delta);
        /**
         * 
         *dispose of all your archon assets 
         */
        public abstract void archonDispose();
    }

    public static class LogLevel {
        public const int NONE = 0;
        public const int LOG = 1;
        public const int DEBUG = 2;
        public const int ERROR = 3;
    }
}
