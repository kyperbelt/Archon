﻿using System;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Archon.engine.utils
{
    //creates a File package containing the given file path 
    //that can be used to retrieve files in several different formats
    public class FilePackage
    {
        private String file;

        public FilePackage(String file)
        {
            this.file = file;
        }

        //return the name of this file
        public string fileName()
        {
            return file;
        }

        //return the file path relative to the root dir
        public string filePath()
        {
            return Path.Combine(ArchonGame.ROOT_DIR, file);
        }
        
        //returns the file of this package as a texture2d 
        public Texture2D asTexture()
        {
            
    
            Texture2D t = null;
            using (var stream = TitleContainer.OpenStream(filePath()))
            {
                t = Texture2D.FromStream(ArchonGame.graphics.GraphicsDevice, stream);
            }

            return t;
        }

        //returns the file of this package as a String - the entire file is read
        public String asString()
        {
            string s = null;
            using (var stream = TitleContainer.OpenStream(filePath()))
            {
                using (StreamReader reader = new StreamReader(stream, true))
                {
                    s = reader.ReadToEnd();
                }
            }
            return s;
        }

        /// <summary>
        /// create a child of this filepackage if it is a directory.
        /// Note that this does not provide any checks so invalid paths 
        /// or trying to create a child of a file and not dir will explode 
        /// the game
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public FilePackage child(String file)
        {
            return new FilePackage(Path.Combine(this.file, file));
        }

        public FilePackage parent()
        {
            return new FilePackage(Path.GetDirectoryName(fileName()));
        }

        //TODO: asSound, asSong
    }
}
