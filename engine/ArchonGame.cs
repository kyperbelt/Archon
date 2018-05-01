using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Archon.engine.utils;
using Archon.engine.graphics;

namespace Archon.engine
{
    public abstract class ArchonGame : Game
    {

        public static readonly string ROOT_DIR = "assets";
        public static int ScreenWidth { get; private set; }
        public static int ScreenHeight { get; private set; }

        public static GraphicsDeviceManager graphics;
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
        private static ArchonGame self;

        
        ASpriteBatch spriteBatch;
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
            ScreenWidth = width;
            ScreenHeight = height;
            self = this;

            //we set the content root dir in case we decide to use it for some reason down the line
            Content.RootDirectory = ROOT_DIR;
        }

      
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new ASpriteBatch(GraphicsDevice);

            archonCreate();
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
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            archonUpdate(delta);

            base.Update(gameTime);

            int bwidth = graphics.GraphicsDevice.PresentationParameters.BackBufferWidth;
            int bheight = graphics.GraphicsDevice.PresentationParameters.BackBufferHeight;
            if (c_width != bwidth || c_height != bheight)
            {
                c_height = bheight;
                c_width = bwidth;
                ScreenWidth = c_width;
                ScreenHeight = c_height;
                archonResize(c_width, c_height);
                GraphicsDevice.Viewport = new Viewport(0,0,c_width,c_height);
            }
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            // TODO: Add your drawing code here
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            archonDraw(spriteBatch, delta);

            base.Draw(gameTime);
        }

        
        //UTIL METHODS --------------------------------------------
        //------------------------------------------------------------


       
        public static void writeLog(String tag, String message)
        {
            Log.log(tag,message);
        }

        public static void writeDebug(String tag, String message)
        {
            Log.debug(tag,message);
        }

        public static void writeError(String tag, String message)
        {
            Log.err(tag, message);
        }

        public static void exit()
        {
            self.Exit();
        }

        //OVERRIDES -----------------

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
         * for now using a singular spritebatch but can switch this out 
         * if limitations arise
         */
        public abstract void archonDraw(ASpriteBatch batch,float delta);

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
