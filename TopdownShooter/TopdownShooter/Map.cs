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

namespace TopdownShooter
{
    class Map
    {
        //properties
        private Dictionary<string, int[,]> mapData;
        private Vector2 dimensions;
        //private Texture2D background;

        //constructor
        public Map(Dictionary<string, int[,]> myMapData, Vector2 myDimensions)
        {
            mapData = myMapData;
            dimensions = myDimensions;
        }

        //getters and setters
        public Dictionary<string, int[,]> MapData { get { return mapData; } set { mapData = value; } }
        public Vector2 Dimensions { get { return dimensions; } set { dimensions = value; } }

        //methods
        public void Update()
        {
        }

        public void Draw(SpriteBatch myBatch)
        {
            
        }

    }
}
