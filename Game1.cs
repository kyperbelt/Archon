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
        TextureRegion[] region = new TextureRegion[3];

        public Game1() : base(1280,720){
        }

        public override void archonCreate()
        {
            var atlasPath = new FilePackage("textures/game.atlas");

           

            atlas = new TextureAtlas(atlasPath);
            region[0] = atlas.findRegion("King");
            region[1] = atlas.findRegion("Tjuanfront");
            region[2] = atlas.findRegion("CardFront");
            
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
            float rx = ScreenWidth * .25f - rwidth / 2;
            float ry = ScreenHeight / 2 - rheight / 2;


            //set sampler state to point clamp to disable aa
            batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            for (int i = 0; i < region.Length; i++)
            {
                batch.draw(region[i], rx + (rwidth*3) * i + 20 * i, ry, rwidth, rheight, rwidth / 2, rheight / 2, 2, 2, MathHelper.ToRadians(elapsed));
            }
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
