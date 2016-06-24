using System.Collections;
using System.Windows.Forms;

namespace SnakeGame
{
    class keyControls
    {
        private static Hashtable inputKeys = new Hashtable();

        public static void ChangeState(Keys key, bool state)
        {
            inputKeys[key] = state;
        }

        public static bool Pressed(Keys key)
        {
            if (inputKeys[key] == null)
                return false;
            return (bool)inputKeys[key];
        }
    }
}
