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

        
        ASpriteBatch spriteBatch;
        Texture2D texture;
        TextureRegion region;
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

            var filePath = new FilePackage("textures/Tjuanfront.png");
            var atlasPath = new FilePackage("textures/game.atlas");



            texture = filePath.asTexture();


            region = new TextureRegion(texture,20,20);

            string atlasText = null;

            atlasText = atlasPath.asString();

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
                GraphicsDevice.Viewport = new Viewport(0,0,c_width,c_height);
            }
        }

        float elapsed = 0;
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //elapased used to rotate region
            elapsed += delta*100;
            //test region scrolling
            region.scroll(delta,delta);

            float rwidth = 100;
            float rheight = 100;
            float rx = c_width / 2 - rwidth/2;
            float ry = c_height / 2 - rheight/2;
            

            //set sampler state to point clamp to disable aa
            spriteBatch.Begin(SpriteSortMode.Deferred,null,SamplerState.PointClamp);

            spriteBatch.draw(region,rx,ry, rwidth,rheight,rwidth/2,rheight/2,2,2,MathHelper.ToRadians(elapsed),true);
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
