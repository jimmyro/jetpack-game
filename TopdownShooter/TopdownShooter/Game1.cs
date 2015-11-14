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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        /*      test mouse mechanics
         * player moves left and right
         * player jumps up and down w/physics
         * player fires projectile in direction of the mouse
         * 
         */

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        InputManager inputManager;
        FileManager fileManager;
        //KeyboardState oldKeyboardState, newKeyboardState;
        //MouseState oldMouseState, newMouseState;

        Player player;

        Keys[] rightKeys, leftKeys, upKeys, downKeys;

        Map testMap;
        Dictionary<int, Tile> testTileSet;

        SpriteFont font;
        Texture2D playerTexture;

        int SCREEN_WIDTH, SCREEN_HEIGHT;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = 480;
            graphics.PreferredBackBufferWidth = 800;
            this.IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            LoadContent();

            inputManager = new InputManager();

            SCREEN_WIDTH = graphics.GraphicsDevice.Viewport.Width;
            SCREEN_HEIGHT = graphics.GraphicsDevice.Viewport.Height;

            fileManager = new FileManager(Content);
            testMap = fileManager.LoadFromXml("test_level", "map") as Map;
            testTileSet = fileManager.LoadFromXml("tileset", "itemset") as Dictionary<int, Tile>;

            player = new Player(playerTexture, new Vector2(0, SCREEN_HEIGHT - playerTexture.Height), 400f, 1500f);

            leftKeys = new Keys[] { Keys.A, Keys.F, Keys.J, Keys.Left };
            rightKeys = new Keys[] { Keys.D, Keys.H, Keys.L, Keys.Right };
            upKeys = new Keys[] { Keys.W, Keys.T, Keys.I, Keys.Up };
            downKeys = new Keys[] { Keys.S, Keys.G, Keys.K, Keys.Down };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("font");
            playerTexture = Content.Load<Texture2D>("player");
        }

        protected override void UnloadContent()
        {   
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            inputManager.Update();

            PlayerMovement(gameTime);

            player.Update(gameTime);

            base.Update(gameTime);
        }

        private void PlayerMovement(GameTime gameTime)
        {
            //left-right movement
            if (inputManager.KeyDown(leftKeys)) 
            {
                player.SetVelocityX(-player.MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                //multiply by ElapsedGameTime so that the player can't move quickly while the game is running slowly
            }
            else if (inputManager.KeyDown(rightKeys))
            {
                player.SetVelocityX(player.MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            else
            {
                player.SetVelocityX(0);
            }
            
            //start a jump
            if (inputManager.KeyPressed(upKeys)) //player tries to jump
            {
                if (player.CanJump) //if a jump is not already in progress
                {
                    player.CanJump = false; //in the air now
                    player.SetVelocityY(-player.JumpPower * (float)gameTime.ElapsedGameTime.TotalSeconds);
                }
            }
        }

        private void MouseInput()
        {

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            player.Draw(spriteBatch);

            spriteBatch.DrawString(font, "Vx: " + Math.Round(player.Velocity.X, 3).ToString(), new Vector2(5, 5), Color.Red);
            spriteBatch.DrawString(font, "Vy: " + Math.Round(player.Velocity.Y, 3).ToString(), new Vector2(5, 22), Color.Blue);

            //test:
            for (int y = 0; y < 18; y++)
            {
                for (int x = 0; x < 24; x++)
                {
                    spriteBatch.DrawString(font, testMap.MapData["tile"][x, y].ToString(), new Vector2(20 * x + 325, 20 * y), Color.Black);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static Texture2D Crop(Texture2D source, Rectangle area)
        {
            if (source == null)
                return null;

            Texture2D cropped = new Texture2D(source.GraphicsDevice, area.Width, area.Height);
            Color[] data = new Color[source.Width * source.Height];
            Color[] cropData = new Color[cropped.Width * cropped.Height];

            source.GetData<Color>(data);

            int index = 0;
            for (int y = area.Y; y < area.Y + area.Height; y++)
            {
                for (int x = area.X; x < area.X + area.Width; x++)
                {
                    cropData[index] = data[x + (y * source.Width)];
                    index++;
                }
            }

            cropped.SetData<Color>(cropData);

            return cropped;
        }  
    }
}
