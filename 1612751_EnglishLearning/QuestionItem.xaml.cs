using System;
using System.Collections.Generic;
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

namespace _1612751_EnglishLearning
{
    /// <summary>
    /// Interaction logic for QuestionItem.xaml
    /// </summary>
    public partial class QuestionItem : UserControl
    {
        public QuestionItem()
        {
            InitializeComponent();
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
          
        }

        private void in_border_MouseMove(object sender, MouseEventArgs e)
        {
           
                event_border.Background = new SolidColorBrush(Color.FromArgb(255, 237, 237, 242));

        }

        private void in_border_MouseLeave(object sender, MouseEventArgs e)
        {
           event_border.Background = new SolidColorBrush(Color.FromArgb(0,0,0,0));

        }
    }
}
