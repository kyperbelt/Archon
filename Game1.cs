using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Archon.engine;
using Archon.engine.graphics;
using Archon.engine.utils;

namespace Archon
{
    /// <summary>
    /// this is the main type for your game.
    /// </summary>
    public class Game1 : ArchonGame
    {


        
        TextureAtlas atlas;
        TextureRegion region;

        public Game1() : base(320,320){
        }

        public override void archonCreate()
        {
            var atlasPath = new FilePackage("textures/game.atlas");

           

            atlas = new TextureAtlas(atlasPath);
            region = atlas.findRegion("King");
            
        }

        public override void archonResize(int width, int height)
        {
            writeLog("Info:","window size changed [width:"+width+" height:"+height+"]");
        }

        float elapsed = 0;
        public override void archonDraw(ASpriteBatch batch,float delta)
        {
           
            //elapased used to rotate region
            elapsed += delta * 100;
            //test region scrolling

            float rwidth = 100;
            float rheight = 100;
            float rx = ScreenWidth / 2 - rwidth / 2;
            float ry = ScreenHeight / 2 - rheight / 2;


            //set sampler state to point clamp to disable aa
            batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            batch.draw(region, rx, ry, rwidth, rheight, rwidth / 2, rheight / 2, 2, 2, MathHelper.ToRadians(elapsed));
            batch.End();
        }

        public override void archonUpdate(float delta)
        {

        }

        public override void archonDispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
