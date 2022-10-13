using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace KaelsToolbox.Input
{
    public static class KeyboardButtons
    {
        static KeyboardState previosState, currentState;

        public static void Setup()
        {
            previosState = Keyboard.GetState();
            currentState = Keyboard.GetState();
        }
        public static bool ButtonClicked(Keys key)
        {
            bool ans;
            if (currentState.IsKeyUp(key) && !previosState.IsKeyUp(key)) 
                ans = true; else ans = false;

            previosState = currentState;
            currentState = Keyboard.GetState();

            return ans;
        }
    }
}
