using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
using static _1612751_EnglishLearning.App;

namespace _1612751_EnglishLearning
{
    /// <summary>
    /// Interaction logic for TestScreen.xaml
    /// </summary>
    public partial class TestScreen : UserControl
    {
        CollectionTopic topic;
        Color color;
        public TestScreen(CollectionTopic topic,Color color)
        {
            this.topic = topic;
            this.color = color;
            InitializeComponent();
          
        }
        DirectoryInfo dirInfo;
        FileInfo[] info;
        List<String> name = new List<string>();
        private void getFilesInfo()
        {
             dirInfo = new DirectoryInfo(topic.Path);
            info = dirInfo.GetFiles("*.*", SearchOption.AllDirectories);
            for (int i = 0; i < info.Length; i++)
                name.Add(System.IO.Path.GetFileNameWithoutExtension(info[i].Name));
          
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            getFilesInfo();
           
            for(int i=0;i<info.Length;i++)
            {
                Trace.WriteLine(info[i].Name);
            }
            image.Source =new BitmapImage(new Uri( info[5].FullName));
        }
    }
}
