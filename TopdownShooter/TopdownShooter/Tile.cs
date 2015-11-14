using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System.Diagnostics;

namespace TopdownShooter
{
    class Tile
    {
        //properties
        protected string name;
        protected Texture2D texture;
        protected Color color;
        protected bool cancollide;

        //constructor
        public Tile(string myName, Texture2D myTexture, Color myColor, bool myTruth)
        {
            name = myName;
            texture = myTexture;
            color = myColor;
            cancollide = myTruth;
        }

        public Tile(string myName, string myTextureIDString, string[] myColorString, string myTruthString, Texture2D tileSheet)
        {
            //name is already a string
            name = myName;

            #region handle texture
            byte x, y, tileSize;

            tileSize = (byte)(Math.Floor(tileSheet.Width / 10d)); //this must be a whole number

            if (myTextureIDString.Length != 2)
                throw new Exception("Tile ID string too big or too small for " + name);

            //Debug.WriteLine("Tile \"" + name + "\" with proper ID " + myTextureIDString);
                
            if (!byte.TryParse(myTextureIDString[0].ToString(), out y) || !byte.TryParse(myTextureIDString[1].ToString(), out x))
                throw new Exception("Bad ID for tile \"" + name + "\"");

            texture = Game1.Crop(tileSheet, new Rectangle(x, y, tileSize, tileSize));
            #endregion

            #region handle color
            if (myColorString.Length != 4)
                throw new Exception("Wrong number of components in color string for " + name);

            byte[] colorBytes = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                if (!byte.TryParse(myColorString[i], out colorBytes[i]))
                    colorBytes[i] = 0; //enough with the exceptions D:
            }

            color = new Color(colorBytes[0], colorBytes[1], colorBytes[2], colorBytes[3]);
            #endregion

            //handle bool
            cancollide = Convert.ToBoolean(myTruthString);
        }

        //getters and setters
        public string Name { get { return name; } set { name = value; } }
        public Texture2D Texture { get { return texture; } set { texture = value; } }
        public Color Color { get { return color; } set { color = value; } }
        public bool CanCollide { get { return cancollide; } set { cancollide = value; } }

        //methods
        public override string ToString()
        {
            return string.Format("name: {0} | texture: {1} | color: {2} | cancollide: {3}", name, texture.ToString(),
                color.ToString(), cancollide.ToString());
        }
    }
}
