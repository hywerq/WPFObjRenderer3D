using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Windows;

namespace AKG
{
    static class Model
    {
        public static List<Vector3> listV = new List<Vector3>();
        public static List<int[]> listF = new List<int[]>();
        public static List<Vector3> listVn = new List<Vector3>();
        public static List<Vector3> listVt = new List<Vector3>();
        public static List<Vector4> model = new();

        private static string[] verticesTypes = { "v", "vt", "vn", "f"};

        public static void ReadFile(string path)
        {
            try
            {
                using (var sr = new StreamReader(path))
                {
                    var vertices = sr.ReadToEnd().Split('\n').ToList();

                    var temp = vertices
                        .Select(x => Regex.Replace(x.TrimEnd().Replace('.', ','), @"\s+", " ").Split(' '))
                        .Where(x => verticesTypes.Any(x[0].Contains)).ToArray();

                    listV = temp
                        .Where(x => x[0] == "v")
                        .Select(x => x.Skip(1).ToArray())
                        .Select(x => new Vector3(Array.ConvertAll(x, float.Parse))).ToList();
                    /*
                    listVn = temp
                        .Where(x => x[0] == "vn")
                        .Select(x => x.Skip(1).ToArray())
                        .Select(x => new Vector3(Array.ConvertAll(x, float.Parse))).ToList();

                    listVt = temp
                        .Where(x => x[0] == "vt")
                        .Select(x => x.Skip(1).ToArray())
                        .Select(x => new Vector3(Array.ConvertAll(x, float.Parse))).ToList(); 
                    */

                    listF = vertices
                        .Where(x => x.StartsWith('f') == true)
                        .Select(x => x.Remove(0, 2).TrimEnd().Split('/', ' ')).ToArray()
                        .Select(x => Array.ConvertAll(x, int.Parse)).ToList();
                }
            }
            catch (IOException e)
            {
                MessageBox.Show("The file could not be read:\n\n" + e.Message);
            }
        }
    }
}
