
using Archon.engine.utils;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Archon.engine.graphics
{
    public class Page
    {
        public FilePackage textureFile { get; private set; }
        public Texture2D texture { get; set; }
        public float width { get; private set; }
        public float height { get; private set; }
        public SurfaceFormat format { get; private set; }

        public Page(FilePackage textureFile, float width, float height, SurfaceFormat format) 
        {
            this.textureFile = textureFile;
            this.width = width;
            this.height = height;
            this.format = format;
        } 
    }

    public class Region
    {
        public Page page;
        public int index;
        public string name;
        public float xOff;
        public float yOff;
        public int originalWidth;
        public int originalHeight;
        public bool rotate;
        public int left;
        public int top;
        public int width;
        public int height;
        public bool flip;
        public int[] splits;
        public int[] pads;

    }

    public class AtlasData
    {
        public static readonly string ERROR_TAG = "ATLAS_ERROR";
        public static string[] tee = new string[4];

        public List<Page> pages = new List<Page>();
        public List<Region> regions = new List<Region>();

        public AtlasData(FilePackage atlasFile, FilePackage imageDir, bool flip)
        {
            int lineIndex = 0;
            string[] lines = atlasFile.asString().Split(new[] { "\r\n","\r","\n"},StringSplitOptions.None);
            Page pageImage = null;
            while (lineIndex < lines.Length)
            {
                String line = lines[lineIndex];
                if (line == null)
                    break;
                if (line.Trim().Length == 0)
                    pageImage = null;
                else if (pageImage == null)
                {
                    FilePackage file = imageDir.child(line);

                    float width = 0;
                    float height = 0;

                    lineIndex++;
                    int v = teeUp(lines[lineIndex]);
                    if (v == 2) //compatible with old texture packer
                    {
                        float.TryParse(tee[0], out width);
                        float.TryParse(tee[1], out height);
                        lineIndex++;
                        teeUp(lines[lineIndex]);
                    }

                    SurfaceFormat format = TextureAtlas.formatFromString(tee[0]);

                    lineIndex++;
                    teeUp(lines[lineIndex]);
                    //texture filters here ---- TextureFilter min = tee[0]
                    // - ---------------------- TextureFilter max = tee[1]

                    lineIndex++;
                    string direction = readValue(lines[lineIndex]);
                    //repeat stuff
                    //TextureWrap rx = clampToEdge;
                    //TextureWrap ry = ClampToEdge;
                    //if direction = x then rx = repeat;
                    //if direction = y then ry = repeat;
                    //if direction = xy then rx = ry = repeat;

                    pageImage = new Page(file, width, height, format);
                    pages.Add(pageImage);
                }
                else
                {
                    lineIndex++;
                    bool rotate = bool.Parse(readValue(lines[lineIndex]));

                    lineIndex++;
                    teeUp(lines[lineIndex]);
                    ArchonGame.writeLog("test:",tee[0]);
                    int left = int.Parse(tee[0]);
                    int top = int.Parse(tee[1]);

                    lineIndex++;
                    teeUp(lines[lineIndex]);
                    int width = int.Parse(tee[0]);
                    int height = int.Parse(tee[1]);

                    Region region = new Region();
                    region.page = pageImage;
                    region.left = left;
                    region.top = top;
                    region.width = width;
                    region.height = height;
                    region.name = line;
                    region.rotate = rotate;

                    lineIndex++;
                    int v = teeUp(lines[lineIndex]);
                    if (v == 4) //optional splits included
                    {
                        region.splits = new[] { int.Parse(tee[0]), int.Parse(tee[1]), int.Parse(tee[2]),int.Parse(tee[3]) };
                        lineIndex++;
                        v = teeUp(lines[lineIndex]);
                        if (v == 4)//optional pads included - only happens when there is splits
                        {
                            region.pads = new int[] { int.Parse(tee[0]), int.Parse(tee[1]), int.Parse(tee[2]), int.Parse(tee[3]) };
                            lineIndex++;
                            teeUp(lines[lineIndex]);
                        }
                    }

                    region.originalWidth = int.Parse(tee[0]);
                    region.originalHeight = int.Parse(tee[1]);

                    lineIndex++;
                    teeUp(lines[lineIndex]);

                    region.xOff = int.Parse(tee[0]);
                    region.yOff = int.Parse(tee[1]);

                    lineIndex++;
                    region.index = int.Parse(readValue(lines[lineIndex]));

                    region.flip = flip;

                    regions.Add(region);
                }


                lineIndex++;
            }

            regions.Sort(TextureAtlas.indexComparison);

        }

        public List<Page> getPages()
        {
            return pages;
        }

        public List<Region> getRegions()
        {
            return regions;
        }

        //number of titt values read
        public static int teeUp(String line)
        {
            int colon = line.IndexOf(':');
            if (colon == -1)
            {
                ArchonGame.writeError(ERROR_TAG, "Invalid Line:" + line);
                ArchonGame.exit();
                return -1;
            }
            int i = 0;
            int lastMatch = colon + 1;
            for (i = 0; i < 3; i++)
            {
                int comma = line.IndexOf(',', lastMatch);
                if (comma == -1) break;
                tee[i] = line.Substring(lastMatch,comma-lastMatch).Trim();
                lastMatch = comma + 1;
            }
            tee[i] = line.Substring(lastMatch).Trim();
            return i + 1;
        }

        public static string readValue(String line)
        {
            int colon = line.IndexOf(':');
            if (colon == -1)
            {
                ArchonGame.writeError(ERROR_TAG, "Invalid Line:" + line);
                ArchonGame.exit();
                return null;
            }
            return line.Substring(colon + 1).Trim();
        }
    }

    public class TextureAtlas : IDisposable
    {

        private List<Texture2D> textures = new List<Texture2D>();
        private List<AtlasRegion> regions = new List<AtlasRegion>();

        public TextureAtlas()
        {
        }

        public TextureAtlas(String file) : this(new FilePackage(file))
        {
        }

        public TextureAtlas(FilePackage file) : this (file,file.parent())
        {
        }

        public TextureAtlas(FilePackage file, bool flip): this (file, file.parent(),flip)
        {
        }

        public TextureAtlas(FilePackage file, FilePackage imageDir) : this(file,imageDir,false)
        {
        }

        public TextureAtlas(FilePackage file, FilePackage imageDir, Boolean flip) : this (new AtlasData(file,imageDir,flip))
        {

        }

        public TextureAtlas(AtlasData data)
        {
            if (data != null) load(data);
        }

        private void load(AtlasData data)
        {
            Dictionary<Page, Texture2D> pageToTexture = new Dictionary<Page, Texture2D>();
            foreach (Page page in data.pages)
            {
                Texture2D texture = null;
                if (page.texture == null)
                {
                    // TODO: implemetn minmaps and format and wrapping
                    // texture = new Texture2D(ArchonGame.graphics.GraphicsDevice,(int)page.width,(int)page.height,false,page.format);
                    texture = page.textureFile.asTexture();


                }
                else
                {
                    texture = page.texture;
                 
                }

                textures.Add(texture);
                pageToTexture.Add(page,texture);
            }

            foreach(Region region in data.regions)
            {
                int width = region.width;
                int height = region.height;
                AtlasRegion atlasRegion = new AtlasRegion(pageToTexture[region.page],region.left,region.top,region.rotate ? height : width,region.rotate ? width: height);
                atlasRegion.index = region.index;
                atlasRegion.name = region.name;
                atlasRegion.xOff = region.xOff;
                atlasRegion.yOff = region.yOff;
                atlasRegion.originalWidth = region.originalWidth;
                atlasRegion.originalHeight = region.originalHeight;
                atlasRegion.rotate = region.rotate;
                atlasRegion.splits = region.splits;
                atlasRegion.pads = region.pads;
                if (region.flip) atlasRegion.flip(false,true);
                regions.Add(atlasRegion);
            }
        }

        public List<AtlasRegion> getRegions()
        {
            return regions;
        }

        /// <summary>
        /// returns first region with specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public AtlasRegion findRegion(String name)
        {
            for (int i = 0, n = regions.Count; i < n; i++)
            {
                if (regions[i].name.Equals(name)) return regions[i];
            }
            return null;
        }

        /// <summary>
        /// return region with specified name and index
        /// </summary>
        /// <param name="name"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public AtlasRegion findRegion(String name, int index)
        {
            for (int i = 0, n = regions.Count; i < n; i++)
            {
                AtlasRegion region = regions[i];
                if (!region.name.Equals(name)) continue;
                if (region.index != index) continue;
                return region;
            }
            return null;
        }

        /// <summary>
        /// returns all regions of the specified name ordered small to larger index
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<AtlasRegion> findRegions(String name)
        {
            List<AtlasRegion> matched = new List<AtlasRegion>();
		    for (int i = 0, n = regions.Count; i<n; i++) {
			    AtlasRegion region = regions[i];
			    if (region.name.Equals(name)) matched.Add(new AtlasRegion(region));
		    }
		    return matched;
        }

        public static Comparison<Region> indexComparison = delegate (Region x, Region y) 
        {
            int xx = x.index;
            if (xx == -1) xx = int.MaxValue;
            int yy = y.index;
            if (yy == -1) yy = int.MaxValue;
            return xx - yy;
        };

        /// <summary>
        /// convert libgdx texturepackeer formats to monogame compatible surface format
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static SurfaceFormat formatFromString(String format)
        {
            if (format.Equals("RGBA8888"))
                return SurfaceFormat.Color;
            return SurfaceFormat.Color;//default is color
        }

        public void Dispose()
        {
            ArchonGame.writeDebug("TextureAtlas:","texture atlas disposed");
        }
    }

    public class AtlasRegion : TextureRegion
    {
        public int index;
        public string name;
        public float xOff;
        public float yOff;
        public int packedHeight;
        public int packedWidth;
        public int originalWidth;
        public int originalHeight;
        public bool rotate;
        public int[] splits;
        public int[] pads;

        public AtlasRegion(Texture2D texture, int x, int y, int width, int height) : base(texture,x,y,width,height)
        {
            originalWidth = width;
            originalHeight = height;
            packedWidth = width;
            packedHeight = height;
        }

        public AtlasRegion(AtlasRegion region)
        {
            setRegion(region);
            index = region.index;
            name = region.name;
            xOff = region.xOff;
            yOff = region.yOff;
            packedWidth = region.packedWidth;
            packedHeight = region.packedHeight;
            originalWidth = region.originalWidth;
            originalHeight = region.originalHeight;
            rotate = region.rotate;
            splits = region.splits;
        }

        new public void flip(bool x, bool y)
        {
            base.flip(x,y);
            if (x) xOff = originalWidth - xOff - getRotatedPackedWidth();
            if (y) yOff = originalHeight - yOff - getRotatedPackedHeight(); 
        }

        public float getRotatedPackedWidth()
        {
            return rotate ? packedHeight : packedWidth;
        }

        public float getRotatedPackedHeight()
        {
            return rotate ? packedWidth : packedHeight;
        }

    }
}
