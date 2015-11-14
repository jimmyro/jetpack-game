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
    class InputManager
    {
        //properties and fields
        private KeyboardState oldKeyboardState, newKeyboardState;
        private MouseState oldMouseState, newMouseState;

        //constructor
        public InputManager()
        {
            oldKeyboardState = Keyboard.GetState();
            newKeyboardState = Keyboard.GetState();

            oldMouseState = Mouse.GetState();
            newMouseState = Mouse.GetState();
        }

        //getters and setters
        public KeyboardState OldKeyboardState { get { return oldKeyboardState; } set { oldKeyboardState = value; } }
        public KeyboardState NewKeyboardState { get { return newKeyboardState; } set { newKeyboardState = value; } }

        public MouseState OldMouseState { get { return oldMouseState; } set { oldMouseState = value; } }
        public MouseState NewMouseState { get { return newMouseState; } set { newMouseState = value; } }

        //methods
        public void Update()
        {
            oldKeyboardState = newKeyboardState;
            oldMouseState = newMouseState;

            newKeyboardState = Keyboard.GetState();
            newMouseState = Mouse.GetState();
        }

        public bool KeyPressed(Keys key)
        {
            if (newKeyboardState.IsKeyDown(key) && oldKeyboardState.IsKeyUp(key))
                return true;
            return false;
        }

        public bool KeyPressed(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (newKeyboardState.IsKeyDown(key) && oldKeyboardState.IsKeyUp(key))
                    return true;
            }
            return false;
        }

        public bool KeyReleased(Keys key)
        {
            if (newKeyboardState.IsKeyUp(key) && oldKeyboardState.IsKeyDown(key))
                return true;
            return false;
        }

        public bool KeyReleased(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (newKeyboardState.IsKeyUp(key) && oldKeyboardState.IsKeyDown(key))
                    return true;
            }
            return false;
        }

        public bool KeyDown(Keys key)
        {
            if (newKeyboardState.IsKeyDown(key))
                return true;
            return false;
        }

        public bool KeyDown(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (newKeyboardState.IsKeyDown(key))
                    return true;
            }
            return false;
        }

        public bool KeyUp(Keys key)
        {
            if (newKeyboardState.IsKeyUp(key))
                return true;
            return false;
        }

        public bool KeyUp(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (newKeyboardState.IsKeyUp(key))
                    return true;
            }
            return false;
        }

        public bool LeftClicked()
        {
            if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
                return true;
            return false;
        }

        public bool RightClicked()
        {
            if (newMouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released)
                return true;
            return false;
        }

        public bool LeftReleased()
        {
            if (newMouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed)
                return true;
            return false;
        }

        public bool RightReleased()
        {
            if (newMouseState.RightButton == ButtonState.Released && oldMouseState.RightButton == ButtonState.Pressed)
                return true;
            return false;
        }

        public bool LeftDown()
        {
            if (newMouseState.LeftButton == ButtonState.Pressed)
                return true;
            return false;
        }

        public bool RightDown()
        {
            if (newMouseState.RightButton == ButtonState.Pressed)
                return true;
            return false;
        }

        public bool LeftUp()
        {
            if (newMouseState.LeftButton == ButtonState.Released)
                return true;
            return false;
        }

        public bool RightUp()
        {
            if (newMouseState.RightButton == ButtonState.Released)
                return true;
            return false;
        }
    }
}
