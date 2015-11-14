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
    class Player
    {
        //fields
        private bool enabled;
        private Texture2D texture;
        private Vector2 position, velocity;
        private Rectangle hitbox;
        private Color color;
        
            //physics stuff
        private float moveSpeed;
        private bool canJump;
        private float jumpPower;
        private const float gravity = 50f;

        //constructor
        public Player(Texture2D myTexture, Vector2 myPosition, float myMoveSpeed, float myJumpPower)
        {
            enabled = true;

            texture = myTexture;
            position = myPosition;
            velocity = Vector2.Zero;
            color = Color.White;

            moveSpeed = myMoveSpeed;
            jumpPower = myJumpPower;

            canJump = true;
        }

        //getters and setters
        public bool Enabled { get { return enabled; } set { enabled = value; } }
        public Texture2D Texture { get { return texture; } }
        public Rectangle Hitbox { get { return hitbox; } set { hitbox = value; } }
        public Color Color { get { return color; } set { color = value; } }
        
        public Vector2 Position { get { return position; } set { position = value; } }
        public void SetPositionX(float x) { position.X = x; }
        public void SetPositionY(float y) { position.Y = y; }
        
        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }
        public void SetVelocityX(float x) { velocity.X = x; }
        public void SetVelocityY(float y) { velocity.Y = y; }

        public bool CanJump { get { return canJump; } set { canJump = value; } }
        public float JumpPower { get { return jumpPower; } set { jumpPower = value; } }
        public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

        //methods
        public void Update(GameTime gameTime)
        {
            if (enabled)
            {
                    //float dt = (float)(gameTime.TotalGameTime.TotalSeconds - initialTime);
                if (!canJump) //in the air
                    velocity.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                {
                    velocity.Y = 0;
                    position.Y = Helper.SCREEN_HEIGHT - texture.Height;
                }

                position += velocity;

                canJump = position.Y >= Helper.SCREEN_HEIGHT - texture.Height;
            }
        }

        public void Draw(SpriteBatch myBatch)
        {
            if (enabled)
            {
                if (canJump)
                    color = Color.LightGreen;
                else
                    color = Color.LightGray;

                myBatch.Draw(texture, position, color);
            }
        }
    }
}
