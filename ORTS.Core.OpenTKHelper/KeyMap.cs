using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK.Input;
namespace ORTS.Core.OpenTKHelper
{
    public class KeyMap
    {
        public Dictionary<Key, Keys> keys = new Dictionary<Key, Keys>();
        public KeyMap()
        {
            keys.Add(Key.A, Keys.A);
            keys.Add(Key.B, Keys.B);
            keys.Add(Key.C, Keys.C);
            keys.Add(Key.D, Keys.D);
            keys.Add(Key.E, Keys.E);

            keys.Add(Key.W, Keys.W);
            keys.Add(Key.S, Keys.S);
            keys.Add(Key.Escape, Keys.Escape);
            keys.Add(Key.Space, Keys.Space);
        }
        public Keys Do(Key key)
        {
            if (keys.ContainsKey(key))
            {
                return keys[key];
            }
            else
            {
                return Keys.OemPipe;
            }
        }
    }
}
