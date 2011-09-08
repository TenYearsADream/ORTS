using System.Collections.Generic;
using System.Windows.Forms;
using OpenTK.Input;
namespace ORTS.Core.OpenTKHelper
{
    public class KeyMap
    {
        private readonly Dictionary<Key, Keys> _keys = new Dictionary<Key, Keys>();
        public KeyMap()
        {
            _keys.Add(Key.A, Keys.A);
            _keys.Add(Key.B, Keys.B);
            _keys.Add(Key.C, Keys.C);
            _keys.Add(Key.D, Keys.D);
            _keys.Add(Key.E, Keys.E);
            _keys.Add(Key.F, Keys.F);
            _keys.Add(Key.G, Keys.G);
            _keys.Add(Key.H, Keys.H);
            _keys.Add(Key.I, Keys.I);
            _keys.Add(Key.J, Keys.J);
            _keys.Add(Key.K, Keys.K);
            _keys.Add(Key.L, Keys.L);
            _keys.Add(Key.M, Keys.M);
            _keys.Add(Key.N, Keys.N);
            _keys.Add(Key.O, Keys.O);
            _keys.Add(Key.P, Keys.P);
            _keys.Add(Key.Q, Keys.Q);
            _keys.Add(Key.R, Keys.R);
            _keys.Add(Key.S, Keys.S);
            _keys.Add(Key.T, Keys.T);
            _keys.Add(Key.U, Keys.U);
            _keys.Add(Key.V, Keys.V);
            _keys.Add(Key.W, Keys.W);
            _keys.Add(Key.X, Keys.X);
            _keys.Add(Key.Y, Keys.Y);
            _keys.Add(Key.Z, Keys.Z);

            _keys.Add(Key.Enter,Keys.Enter);
            _keys.Add(Key.Escape, Keys.Escape);
            _keys.Add(Key.Space, Keys.Space);
        }
        public Keys Match(Key key)
        {
            return _keys.ContainsKey(key) ? _keys[key] : Keys.OemQuestion;
        }
    }
}
