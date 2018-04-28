
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Archon.engine.graphics
{
    public class ASpriteBatch : SpriteBatch
    {
        Rectangle destRect;
        Rectangle sourceRect;
        Vector2 origin;
        public Color currentColor;

        public ASpriteBatch(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            destRect = new Rectangle(0, 0, 0, 0);
            sourceRect = new Rectangle(0, 0, 0, 0);
            origin = new Vector2(0, 0);
            currentColor = Color.White;
        }

        public void setColor(Color color)
        {
            this.currentColor = color;
        }

        //
        public void draw(TextureRegion region, float x, float y, float width, float height, float originX = 0, float originY = 0, float scaleX = 1, float scaleY = 1, float rotation = 0, bool flipX = false, bool flipY = false)
        {
            sourceRect.X = region.getRegionX();
            sourceRect.Y = region.getRegionY();
            sourceRect.Width = region.getRegionWidth();
            sourceRect.Height = region.getRegionHeight();


            origin.X = originX * (region.getRegionWidth() / width);
            origin.Y = originY * (region.getRegionHeight() / height);

            destRect.X = (int)(x + originX);
            destRect.Y = (int)(y + originY);
            destRect.Width = (int)(width * scaleX);
            destRect.Height = (int)(height * scaleY);


            Draw(region.texture, destRect, sourceRect, currentColor, rotation, origin, (flipX ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (flipY? SpriteEffects.FlipVertically: SpriteEffects.None), 0);
        }
    }
}
