using System;
using System.Collections.Generic;
using System.Drawing;
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
        public static List<int[]> listF2 = new List<int[]>();
        public static List<Vector3> listVn = new List<Vector3>();
        public static List<Vector2> listVt = new List<Vector2>();
        
        public static Vector3[,] fileNormals;
        public static Vector3[,] fileNormalsOrig;
        public static Vector4[] screenVertices;
        public static Vector3[] worldVertices;
        public static Vector3[] worldNormals;
        public static Vector2[] textures;

        public static Bitmap textureFile;
        public static Bitmap mirrorMap;
        public static Bitmap normalMap;
        public static Bitmap mraoMap;

        private static string[] verticesTypes = { "v", "vt", "vn", "f"};

        public static void ReadFile(MainWindow mw, string filePath, string diffuseMapPath, string mirrorMapPath, 
            string normalMapPath, string mraoMapPath)
        {
            try
            {
                using (var sr = new StreamReader(filePath))
                {
                    var vertices = sr.ReadToEnd().Split('\n').ToList();

                    var temp = vertices
                        .Select(x => Regex.Replace(x.TrimEnd()/*.Replace('.', ',')*/, @"\s+", " ").Split(' '))
                        .Where(x => verticesTypes.Any(x[0].Contains)).ToArray();

                    listV = temp
                        .Where(x => x[0] == "v")
                        .Select(x => x.Skip(1).ToArray())
                        .Select(x => new Vector3(Array.ConvertAll(x, float.Parse))).ToList();

                    listVn = temp
                        .Where(x => x[0] == "vn")
                        .Select(x => x.Skip(1).ToArray())
                        .Select(x => new Vector3(Array.ConvertAll(x, float.Parse))).ToList();

                    listVt = temp
                        .Where(x => x[0] == "vt")
                        .Select(x => x.Skip(1).ToArray())
                        .Select(x => new Vector2(Array.ConvertAll(x, float.Parse))).ToList();

                    //listF = vertices
                    //    .Where(x => x.StartsWith('f') == true)
                    //    .Select(x => x.Remove(0, 2).TrimEnd().Split('/', ' ')).ToArray()
                    //    .Select(x => Array.ConvertAll(x, int.Parse)).ToList();

                    var mas_f = vertices.Where(x => x.StartsWith('f') == true);

                    foreach (string str in mas_f)
                    {
                        string pre = str.Remove(0, 1);
                        string[] buf = pre.Trim().Split(new char[] { '/', ' ' });
                        int len = buf.Length;
                        if (buf.Length == 3)
                        {
                            len *= 3;
                        }
                        int[] res = new int[len];
                        for (int i = 0; i < len; i++)
                        {
                            if (buf.Length == len)
                            {
                                if (buf[i] == "")
                                {
                                    res[i] = 0;
                                }
                                else
                                {
                                    res[i] = int.Parse(buf[i]);
                                }
                            }
                            else
                            {
                                if (i % 3 == 0)
                                {
                                    res[i] = int.Parse(buf[i / 3]);
                                }
                                else
                                {
                                    res[i] = 0;
                                }
                            }
                        }

                        listF.Add(res);
                    }

                    var inListButNotInList2 = listF.Except(listF2).ToList();
                    var inList2ButNotInList = listF2.Except(listF).ToList();
                }

                mw.lbVert.Content = listV.Count;
                mw.lbPoly.Content = listF.Count;

                try
                {
                    textureFile = (Bitmap)Bitmap.FromFile(diffuseMapPath);
                    mw.lbTexture.Content = "Yes";
                }
                catch (Exception ex)
                {
                    textureFile = null;
                    mw.lbTexture.Content = "No";
                }

                try
                {
                    mirrorMap = (Bitmap)Bitmap.FromFile(mirrorMapPath);
                    mw.lbSpecular.Content = "Yes";
                }
                catch (Exception ex)
                {
                    mirrorMap = null;
                    mw.lbSpecular.Content = "No";
                }

                try
                {
                    mraoMap = (Bitmap)Bitmap.FromFile(mraoMapPath);
                    mw.lbMRAO.Content = "Yes";
                }
                catch (Exception ex)
                {
                    mraoMap = null;
                    mw.lbMRAO.Content = "No";
                }

                try
                {
                    normalMap = (Bitmap)Bitmap.FromFile(normalMapPath);
                    mw.lbNormal.Content = "Yes";
                    fileNormalsOrig = new Vector3[normalMap.Width, normalMap.Height];

                    for (int i = 0; i < normalMap.Width; i++)
                    {
                        for (int j = 0; j < normalMap.Height; j++)
                        {
                            Color normalColor = normalMap.GetPixel(i, j);
                            Vector3 normal = new Vector3(normalColor.R / 255f, normalColor.G / 255f, normalColor.B / 255f);
                            normal = (normal * 2) - Vector3.One;
                            fileNormalsOrig[i, j] = normal;
                        }
                    }
                }
                catch (Exception ex)
                {
                    normalMap = null;
                    mw.lbNormal.Content = "No";
                }
            }
            catch (IOException e)
            {
                MessageBox.Show("The file could not be read:\n\n" + e.Message);
            }
        }
    }
}
