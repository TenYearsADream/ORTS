using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ORTS.Core.Graphics.Primatives;
using ORTS.Core.Maths;

namespace ORTS.Core.Graphics.ModelLoaders
{
    public enum STLFormat
    {
        ASCII,
        Binary
    }

    public class STL : IModel<Triangle>
    {
        public STL(string filename, STLFormat format)
        {
            Elements = new List<Triangle>();
            switch (format)
            {
                case STLFormat.ASCII:
                    LoadASCII(filename);
                    break;
                case STLFormat.Binary:
                    LoadBinary(filename);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("format");
            }
        }

        #region IModel<Triangle> Members

        public List<Triangle> Elements { get; private set; }

        #endregion

        private void LoadASCII(string filename)
        {
            Vect3 normal = null;
            var points = new Vect3[3];
            int i = 0;
            const NumberStyles style = NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign | NumberStyles.Number;
            foreach (var split in File.ReadLines(filename).Select(line => line.Trim().ToLower().Split(' ')))
            {
                switch (split[0])
                {
                    case "solid":
                        break;
                    case "facet":
                        normal = new Vect3(Double.Parse(split[2], style), Double.Parse(split[3], style),
                                           Double.Parse(split[4], style));
                        break;
                    case "outer":
                        break;
                    case "vertex":
                        points[i++] = new Vect3(Double.Parse(split[1], style), Double.Parse(split[2], style),
                                                Double.Parse(split[3], style));
                        break;
                    case "endloop":
                        break;
                    case "endfacet":
                        Elements.Add(new Triangle(points[0], points[1], points[2], normal));
                        i = 0;
                        break;
                    case "endsolid":
                        break;
                    default:
                        Console.WriteLine(split[0]);
                        break;
                }
            }
        }

        private void LoadBinary(string filename)
        {
            using (var br = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                br.ReadBytes(80); //header
                var count = (int) br.ReadUInt32();
                for (var i = 0; i < count; i++)
                {
                    var normal = new Vect3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                    var p1 = new Vect3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                    var p2 = new Vect3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                    var p3 = new Vect3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                    br.ReadUInt16(); //attrib
                    Elements.Add(new Triangle(p1, p2, p3, normal));
                }
            }
        }
    }
}