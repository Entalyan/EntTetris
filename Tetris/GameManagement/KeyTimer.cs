using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Tetris.GameManagement
{
    class KeyTimer
    {
        private Keys key;
        private TimeSpan pressedTime = new TimeSpan();

        public Keys Key
        {
            get
            {
                return key;
            }
        }

        public TimeSpan TimePressed
        {
            get
            {
                return pressedTime;
            }
        }

        public KeyTimer(Keys newKey)
        {
            key = newKey;
        }

        public void Update(TimeSpan elapsedTime)
        {
            pressedTime += elapsedTime;

            //To prevent overflowing, keep pressed time below 1 minute.
            if (pressedTime > TimeSpan.FromMinutes(1))
            {
                pressedTime -= TimeSpan.FromMinutes(1);
            }
        }

        public void ResetTimer()
        {
            pressedTime = TimeSpan.Zero;
        }


    }
}
