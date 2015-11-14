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

using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

using System.Diagnostics;

namespace TopdownShooter
{
    //TODO: attempt to standardize the way the FileManager packages data?
    class FileManager
    {
        //properties
        ContentManager content;

        //constructor
        public FileManager(ContentManager myContent)
        {
            content = myContent;
        }

        //getters and setters

        /*public int[,] LoadMapFromTxt(string filename)
        {
            if (!filename.EndsWith(".txt"))
                filename += ".txt";

            int xdim = 0, ydim = 0, layers = 0, mapID = 0, contentLine = 0;
            int[,] rawMap = null;

            using (StreamReader reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    if (line.Contains('=')) //attribute signal
                    {
                        string attribute = line.Split('=')[0];
                        string value = line.Split('=')[1];

                        switch (attribute)
                        {
                            case "TYPE":
                                if (!value.Equals("MAP"))
                                    throw new InvalidDataException("Not a map file");
                                break;
                            case "ID":
                                if (!int.TryParse(value, out mapID))
                                    throw new InvalidDataException("Couldn't get int from map ID");
                                break;
                            case "XDIM":
                                if (!int.TryParse(value, out xdim))
                                    throw new InvalidDataException("Couldn't get int from xdim");
                                break;
                            case "YDIM":
                                if (!int.TryParse(value, out ydim))
                                    throw new InvalidDataException("Couldn't get int from ydim");
                                break;
                            default:
                                throw new InvalidDataException("Invalid properties");
                        }
                    }
                    else if (line.Equals(string.Empty)) //end of attributes
                    {
                        rawMap = new int[xdim, ydim];
                    }
                    else if (line.StartsWith("#")) //map line signal
                    {
                        string[] tileRow = line.Split(','); //an array of two-digit integers as strings

                        if (tileRow.Length != xdim)
                            throw new InvalidDataException("Line " + (contentLine + 1).ToString() + " wrong length");

                        for (int i = 0; i < tileRow.Length; i++)
                        {
                                if (tileRow[i].Trim('#').Length != 2 || !int.TryParse(tileRow[i].Trim('#'), out rawMap[i, contentLine]))
                                    throw new InvalidDataException("Bad tile ID in row " + (contentLine + 1).ToString() +
                                        ", column " + (i + 1).ToString());
                        }

                        contentLine++;
                    }
                }
            }

            return rawMap;
        } //TODO: implement layers*/

        //specific loading procedures (maps, menus, etc.)
        public Object LoadFromXml(string filename, string filetype)
        {
            //basic document preparation
            if (!filename.EndsWith(".xml"))
                filename += ".xml";

            var doc = XDocument.Load(filename);

            XElement attributes, content;
            if (!CheckFormat(doc, filetype, out attributes, out content))
                throw new InvalidDataException(filename + " failed basic format test");

            //individual 
            switch (filetype)
            {
                #region map
                case "map":
                    var mapData = new Dictionary<string, int[,]>();

                    //try to retrieve dimensions
                    int xdim = 0, ydim = 0;//, layers = 0;
                    if (ChildExists("xdim", attributes) && ChildExists("ydim", attributes))// && ChildExists("layers", attributes))
                    {
                        if (!int.TryParse(attributes.Element("xdim").Value, out xdim) || !int.TryParse(attributes.Element("ydim").Value, out ydim)
                            )//|| int.TryParse(attributes.Element("layers").Value, out layers))
                            throw new InvalidDataException(filename + " (" + filetype + ") attributes missing xdim, ydim, or layers");
                    }

                    //fill in mapData
                    var layers = content.Elements("layer");

                    foreach (XElement layer in layers)
                    {
                        XAttribute layerClass;
                        if (!AttributeExists("class", layer, out layerClass))
                            throw new InvalidDataException("Layers without class attribute");

                        var lines = layer.Elements("line");
                        int[,] newLayer = new int[xdim, ydim];

                        if (lines.Count<XElement>() != ydim)
                            throw new InvalidDataException("Inconsistent ydim");

                        for (int y = 0; y < ydim; y++)
                        {
                            string[] line = lines.ElementAt<XElement>(y).Value.Split(',');

                            if (line.Count<string>() != xdim)
                                throw new InvalidDataException("Inconsistent xdim");

                            for (int x = 0; x < xdim; x++)
                            {
                                if (!int.TryParse(line[x], out newLayer[x, y]))
                                    throw new InvalidDataException("Bad ID at " + x + ", " + y);
                            }
                        }

                        mapData.Add(layerClass.Value, newLayer);
                    }

                    //mapID and songID, when they need to be implemented

                    return new Map(mapData, new Vector2(xdim, ydim));
                #endregion

                #region itemset
                case "itemset":
                    //try to retrieve itemclass
                    string itemTypeName;
                    var itemset = new Dictionary<int, object>(); 

                    if (ChildExists("itemtype", attributes))
                    {
                        itemTypeName = attributes.Element("itemtype").Value;
                    }
                    else
                        throw new InvalidDataException(filename + " (" + filetype + ") missing itemclass node or properties node");

                    //Type ItemType = Type.GetType("TopdownShooter." + itemTypeName, true, true);
                    //PropertyInfo[] itemTypeProperties = ItemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    //instantiate each item and add it to the itemSet dictionary with its ID
                    foreach (XElement item in content.Elements(itemTypeName))
                    {
                        //find ID
                        int id;

                        if (!AttributeExists("id", item) || !int.TryParse(item.Attribute("id").Value, out id))
                            throw new InvalidDataException(filename + " (" + filetype + ") some items with bad or missing IDs");
                        else
                            Debug.WriteLine("Processing item: " + id);

                        #region reflection alternative (not working)
                        //find properties
                        /*
                        var newItem = Activator.CreateInstance(ItemType);
                        
                        foreach (PropertyInfo itemTypeProperty in itemTypeProperties)
                        {
                            XElement itemProperty = item.Element(itemTypeProperty.Name.ToLower());

                            if (itemProperty != null) //for this property, a corresponding xml node exists
                            {
                                Debug.WriteLine("Found " + itemTypeProperty.Name + " in item: " + id);

                                itemTypeProperty.SetValue(newItem, itemProperty.Value, null);
                            }
                            else
                                Debug.WriteLine(id + " in " + filename + " (" + filetype + ") missing " + itemTypeProperty.Name);
                        }*/ 
                        #endregion

                        Object newItem;

                        switch (itemTypeName.ToLower())
                        {
                            case "tile":
                                XElement name, texture, color, cancollide;

                                //TO-DO: reduce ugliness and hard-coding, investigate reflection alternative further
                                if (!ChildExists("name", item, out name) || !ChildExists("texture", item, out texture)
                                    || !ChildExists("color", item, out color) || !ChildExists("cancollide", item, out cancollide))
                                    throw new InvalidDataException(id + " in " + filename + " (" + filetype + ") missing some properties");
                                else
                                    Debug.WriteLine("Got all properties for item " + id);

                                newItem = new Tile(name.Value, texture.Value, color.Value.Split(new char[] { ',', ' ' }),
                                    cancollide.Value, this.content.Load<Texture2D>("tilesheet"));

                                break;
                            default:
                                throw new InvalidDataException(filename + " (" + filetype + ") unrecognized type \"" + itemTypeName + "\"");
                        }

                        Debug.WriteLine(string.Format("Adding id-{0} pair ({1}, {2}) to itemset", itemTypeName, id, newItem.ToString()));
                        itemset.Add(id, newItem);
                    }

                    return itemset;
                #endregion

                default:
                    return null;
            }
        }

        //operations common to all xml files
        private string Capitalize(string s)
        {
            return s.Substring(0, 1).ToUpper() + s.Remove(0, 1);
        }

        private void DisplayPropertyInfo(PropertyInfo[] myPropertyInfo)
        {
            // Display information for all properties. 
            for (int i = 0; i < myPropertyInfo.Length; i++)
            {
                PropertyInfo myPropInfo = (PropertyInfo)myPropertyInfo[i];
                Debug.WriteLine("The property name is {0}.", myPropInfo.Name);
                Debug.WriteLine("The property type is {0}.", myPropInfo.PropertyType);
            }
        }

        private bool ChildExists(string name, XElement parent)
        {
            if (parent.Element(name) != null)
                return true;

            return false;
        }

        private bool ChildExists(string name, XElement parent, out XElement child)
        {
            //return indicates whether retrieval was successful
            child = null;

            if (parent.Element(name) != null)
            {
                child = parent.Element(name);
                return true;
            }
            
            return false;
        }

        private bool AttributeExists(string name, XElement element)
        {
            if (element.Attribute(name) != null)
                return true;

            return false;
        }

        private bool AttributeExists(string name, XElement element, out XAttribute attribute)
        {
            attribute = null;

            if (element.Attribute(name) != null)
            {
                attribute = element.Attribute(name);
                return true;
            }

            return false;
        }

        private bool CheckFormat(XDocument doc, string filetype, out XElement attributes, out XElement content)
        {
            attributes = null; content = null;
            
            if (doc.Root.Name == "load")
            {
                if (doc.Root.Attribute("type") != null && doc.Root.Attribute("type").Value.StartsWith(filetype))
                {
                    if (ChildExists("attributes", doc.Root, out attributes) && ChildExists("content", doc.Root, out content))
                        return true;
                }
            }

            return false;
        }
    }
}
