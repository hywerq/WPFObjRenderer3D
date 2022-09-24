using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AKG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] verticesTypes = { "v", "vt", "vn", "f"};
        public MainWindow()
        {
            InitializeComponent();

            ReadFile("lego.obj");
        }

        private void ReadFile(string path)
        {
            try
            {
                using (var sr = new StreamReader(path))
                {
                    var vertices = sr.ReadToEnd().Split('\n').ToList()
                        .Select(x => Regex.Replace(x.TrimEnd(), @"\s+", " ").Split(' '))
                        .Where(x => verticesTypes.Any(x[0].Contains)).ToList(); 

                    var listV = vertices.Where(x => x[0] == "v").Select(x => x.Skip(1)).ToList();
                }
            }
            catch (IOException e)
            {
                MessageBox.Show("The file could not be read:\n" + e.Message);
            }
        }
    }
}
