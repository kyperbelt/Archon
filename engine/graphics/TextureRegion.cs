using Microsoft.Xna.Framework.Graphics;
using System;

namespace Archon.engine.graphics
{
    //defines a rectangular region of a texture2D
    public class TextureRegion
    {
        public Texture2D texture { get; set; }
        private float x; //u
        private float y; //v
        private float xx; //u2
        private float yy; //v2
        private int regionWidth;
        private int regionHeight;

        //make an empty region
        public TextureRegion()
        {
        }

        //make a region using whole texture
        public TextureRegion(Texture2D texture)
        {
            if (texture == null) ArchonGame.Log.err("TextureRegion:", "Passed an empty Texture.");
            this.texture = texture;
            setRegion(0,0,texture.Width,texture.Height);
        }

        //texture with width and height
        public TextureRegion(Texture2D texture, int width, int height)
        {
            this.texture = texture;
            setRegion(0,0,width,height);
        }

        public TextureRegion(Texture2D texture, int x, int y, int width, int height)
        {
            this.texture = texture;
            setRegion(x,y,width,height);
        }

        public TextureRegion(Texture2D texture, float x, float y, float xx, float yy)
        {
            this.texture = texture;
            setRegion(x,y,xx,yy);
        }

        public TextureRegion(TextureRegion region)
        {
            setRegion(region);
        }

        public TextureRegion(TextureRegion region, int x, int y, int width, int height)
        {
            setRegion(region, x, y, width, height);
        }

        //sets the texture and coords to texture size
        public void setRegion(Texture2D texture)
        {
            this.texture = texture;
            setRegion(0, 0, texture.Width, texture.Height);
        }

        //se the width and height of the region, negative flips the region
        public void setRegion(int x, int y, int width, int height)
        {
            float invTexWidth = 1f / texture.Width;
            float invTexHeight = 1f / texture.Height;
            setRegion(x * invTexWidth, y * invTexHeight, (x + width) * invTexWidth, (y + height) * invTexHeight);
            regionWidth = Math.Abs(width);
            regionHeight = Math.Abs(height);
        }

        public void setRegion(float x, float y, float xx, float yy)
        {
            int texWidth = texture.Width, texHeight = texture.Height;
            regionWidth = (int)Math.Round(Math.Abs(xx - x) * texWidth);
            regionHeight = (int)Math.Round(Math.Abs(yy - y) * texHeight);

            // For a 1x1 region, adjust UVs toward pixel center to avoid filtering artifacts on AMD GPUs when drawing very stretched.
            if (regionWidth == 1 && regionHeight == 1)
            {
                float adjustX = 0.25f / texWidth;
                x += adjustX;
                xx -= adjustX;
                float adjustY = 0.25f / texHeight;
                y += adjustY;
                yy -= adjustY;
            }

            this.x = x;
            this.y = y;
            this.xx = xx;
            this.yy = yy;
        }

        //set the texture and coords to that of given region
        public void setRegion(TextureRegion region)
        {
            texture = region.texture;
            setRegion(region.x, region.y, region.xx, region.yy);
        }

        public void setRegion(TextureRegion region, int x, int y, int width, int height)
        {
            texture = region.texture;
            setRegion(region.getRegionX() + x, region.getRegionY() + y, width, height);
        }


        public Texture2D getTexture()
        {
            return texture;
        }

        public void setTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public float getX()
        {
            return x;
        }

        public void setX(float x)
        {
            this.x = x;
            regionWidth =(int) Math.Round(Math.Abs(xx - x) * texture.Width);
        }

        public float getY()
        {
            return y;
        }

        public void setY(float y)
        {
            this.y = y;
            regionHeight =(int) Math.Round(Math.Abs(yy - y) * texture.Height);
        }

        public float getXX()
        {
            return xx;
        }

        public void setXX(float xx)
        {
            this.xx = xx;
            regionWidth = (int)Math.Round(Math.Abs(xx - x) * texture.Width);
        }

        public float getYY()
        {
            return yy;
        }

        public void setYY(float yy)
        {
            this.yy = yy;
            regionHeight = (int)Math.Round(Math.Abs(yy - y) * texture.Height);
        }


        public int getRegionX()
        {
            return (int)Math.Round(x * texture.Width);
        }

        public void setRegionX(int x)
        {
            setX(x / (float)texture.Width);
        }

        public int getRegionY()
        {
            return (int) Math.Round(y * texture.Height);
        }

        public void setRegionY(int y)
        {
            setY(y / (float)texture.Height);
        }

        public int getRegionWidth()
        {
            return regionWidth;
        }

        public void setRegionWidth(int width)
        {
            if (isFlipX())
            {
                setX(xx + width / (float)texture.Width);
            }
            else
            {
                setXX(x + width / (float)texture.Width);
            }
        }
        public int getRegionHeight()
        {
            return regionHeight;
        }

        public void setRegionHeight(int height)
        {
            if (isFlipY())
            {
                setY(yy + height / (float)texture.Height);
            }
            else
            {
                setYY(y + height / (float)texture.Height);
            }
        }

        public void flip(bool xAxis, bool yAxis)
        {
            if (xAxis)
            {
                float temp = x;
                x = xx;
                xx = temp;
            }
            if (yAxis)
            {
                float temp = y;
                y = yy;
                yy = temp;
            }
        }

        public bool isFlipX()
        {
            return x > xx;
        }

        public bool isFlipY()
        {
            return y > yy;
        }

        //offset the region relative to the current region. 
        //Generally the regions size should be the entire size of texure in direction scrolled
        public void scroll(float xAmount, float yAmount)
        {
            if (xAmount != 0)
            {
                float width = (xx - x) * texture.Width;
                x = (x + xAmount) % 1;
                xx = x + width / texture.Width;
            }
            if (yAmount != 0)
            {
                float height = (yy - y) * texture.Height;
                y = (y + yAmount) % 1;
                yy = y + height / texture.Height;
            }
        }

        //function to create tiles out of this texture region.
        //starts from top left corner and ending at the bottom right
        //only complete tiles will be returned
        public TextureRegion[,] split(int tileWidth, int tileHeight)
        {
            int x = getRegionX();
            int y = getRegionY();
            int width = regionWidth;
            int height = regionHeight;

            int rows = height / tileHeight;
            int cols = width / tileWidth;

            int startX = x;
            TextureRegion[,] tiles = new TextureRegion[rows,cols];
            for (int row = 0; row < rows; row++, y += tileHeight)
            {
                x = startX;
                for (int col = 0; col < cols; col++, x += tileWidth)
                {
                    tiles[row,col] = new TextureRegion(texture, x, y, tileWidth, tileHeight);
                }
            }

            return tiles;
        }

        public static TextureRegion[,] split(Texture2D texture, int tileWidth, int tileHeight)
        {
            TextureRegion region = new TextureRegion(texture);
            return region.split(tileWidth, tileHeight);
        }

    }
}
