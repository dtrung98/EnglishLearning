using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

      
        int columm=4;
        int row = 2;
        int width_item = 200;
        App app;
        int active_topic = 0;
        List<CollectionTopic> topics;
        public MainWindow()
        {
            app = (App)(Application.Current);
            topics = app.topics;
            active_topic = (topics.Count > 7) ? 7 : topics.Count;
            Trace.WriteLine(active_topic);
            InitializeComponent();
            Trace.WriteLine("MainWindow");
          
           int snugContentWidth = width_item*columm;
           int snugContentHeight =50+ width_item*row;

            var horizontalBorderHeight = SystemParameters.ResizeFrameHorizontalBorderHeight;
            var verticalBorderWidth = SystemParameters.ResizeFrameVerticalBorderWidth;
            var captionHeight = SystemParameters.CaptionHeight;

            Width = snugContentWidth + 2 * verticalBorderWidth;
            Height = snugContentHeight + captionHeight + 2 * horizontalBorderHeight;
        }
        public ControlTemplate GetRoundedTextBoxTemplate()
        {
           
            ControlTemplate template = new ControlTemplate(typeof(UserControl));
            FrameworkElementFactory elemFactory = new FrameworkElementFactory(typeof(Border));
            elemFactory.Name = "Border";
            elemFactory.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
            elemFactory.SetValue(Border.BorderBrushProperty, new SolidColorBrush(Colors.Black));
            elemFactory.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(UserControl.BackgroundProperty));
            elemFactory.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(UserControl.BorderThicknessProperty));
            elemFactory.SetValue(Border.SnapsToDevicePixelsProperty, true);
         //   elemFactory.SetValue(Border.PaddingProperty, new Thickness(20));
            elemFactory.SetValue(Border.MarginProperty, new Thickness(20, 20, 20, 20));
            template.VisualTree = elemFactory;
            

            Trigger isEnabledTrigger = new Trigger();
            isEnabledTrigger.Property = UserControl.IsEnabledProperty;
            isEnabledTrigger.Value = false;

            Setter backgroundSetter = new Setter();
            backgroundSetter.TargetName = "Border";
            backgroundSetter.Property = UserControl.BackgroundProperty;
            backgroundSetter.Value = SystemColors.ControlBrush;

            Setter foregroundSetter = new Setter();
            foregroundSetter.Property = TextBox.ForegroundProperty;
            foregroundSetter.Value = SystemColors.GrayTextBrush;

            isEnabledTrigger.Setters.Add(backgroundSetter);
            isEnabledTrigger.Setters.Add(foregroundSetter);


            template.Triggers.Add(isEnabledTrigger);

            return template;
        }
        private static int MinLightness = 1;
        private static int MaxLightness = 10;
        private static float MinLightnessCoef = 1f;
        private static float MaxLightnessCoef = 0.4f;
        private enum State
        {
            COLLECTION_SHOWN,
            TESTING,
            RESULT_SCREEN
        }
        private State state = State.COLLECTION_SHOWN;

        public static Color ChangeLightness( Color color, float lightness)
        {
            if (lightness < MinLightness)
                lightness = MinLightness;
            else if (lightness > MaxLightness)
                lightness = MaxLightness;

            float coef = MinLightnessCoef +
              (
                (lightness - MinLightness) *
                  ((MaxLightnessCoef - MinLightnessCoef) / (MaxLightness - MinLightness))
              );

            return Color.FromArgb(color.A, (byte)(int)(color.R * coef), (byte)(int)(color.G * coef),
                (byte)(int)(color.B * coef));
        }
        QuestionItem[] questionItem;
        Color[] backColor = new Color[]
         {
                Color.FromArgb(255,232, 237, 239),
                Color.FromArgb(255,255, 161, 0),
                Color.FromArgb(255,0, 210, 252),
                Color.FromArgb(255,68, 110, 255),
                Color.FromArgb(255,255, 44, 61),
                Color.FromArgb(255,0, 211, 91),
                 Color.FromArgb(255,0, 143, 122),
                Color.FromArgb(255,214, 93, 177),
                Color.FromArgb(255,255, 128, 102)
         };
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            actionText.Content= "Chọn bộ câu hỏi ("+topics.Count+") ";
            Trace.WriteLine("Write That");
            grid.Height = row * width_item;
            Containner.Height = grid.Height;
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < row; i++)
                grid.RowDefinitions.Add(new RowDefinition());
         
         
            actionText.MouseUp += ActionText_MouseUp;
            questionItem = new QuestionItem[8];
            for (int i = 0; i < 8; i++)
            {
                questionItem[i] = new QuestionItem();
                if (i == 0)
                {
                    questionItem[i].plus_image.Visibility = Visibility.Visible;
                    questionItem[i].in_border.Margin = new Thickness(0,0,0,0);
                    questionItem[i].contentStackPanel.Visibility = Visibility.Collapsed;
                    questionItem[i].MouseDown += QuestionItem_MouseDown_Add_New;
                } else
                {
                    questionItem[i].out_border.Background = new SolidColorBrush(ChangeLightness(backColor[i], 4f));
                    questionItem[i].MouseDown += QuestionItem_MouseDown;

                    if (i<=active_topic)
                    {
                       
                        questionItem[i].Title.Content = topics[i-1].Name;
                        questionItem[i].MaxScore.Content = "High score : " + topics[i-1].HightScore + '/' + topics[i-1].QuestionNumber;
                        questionItem[i].QuestionNumber.Content = "Question Number : " + topics[i-1].QuestionNumber;
                    }
                }

                //  questionItem.SetValue(Label.TemplateProperty, GetRoundedTextBoxTemplate());
                // t1.Content = "item" + (i + 1);
                questionItem[i].in_border.Background = new SolidColorBrush(backColor[i]);
                 int column = i%(4);
                int row = i/4;
                Grid.SetColumn(questionItem[i], column);
                Grid.SetRow(questionItem[i], row);
                grid.Children.Add(questionItem[i]);
            }
         

        }

        private void ActionText_MouseUp(object sender, MouseButtonEventArgs e)
        {
           switch(state)
            {
                case State.COLLECTION_SHOWN: break;
                case State.TESTING:
                    MsgBoxYesNo msgbox = new MsgBoxYesNo("Quit the test will lost everything.\n Comfirm ?");
                    if ((bool)msgbox.ShowDialog())
                    {
                        //do yes stuff
                        state = State.COLLECTION_SHOWN;
                        Containner.Children.Remove(TestScreen);
                        TestScreen = null;
                        grid.Visibility = Visibility.Visible;
                        actionText.Foreground = new SolidColorBrush(Color.FromRgb(161, 161, 161));
                        StackRoot.Background = new SolidColorBrush(Color.FromRgb(255,255,255));
                        actionText.Content = "Chọn bộ câu hỏi (" + topics.Count + ") ";
                    }
                    else
                    {
                       // no
                    }
                   
                    break;
                case State.RESULT_SCREEN: break;
            }
        }

        private void OnClickQuestionItem(int i)
        {
            // i = 1 -> n
            grid.Visibility = Visibility.Collapsed;
            actionText.Content = "Quit";
           actionText.Foreground = new SolidColorBrush(Color.FromRgb(255,255,255));
            StackRoot.Background = new SolidColorBrush(backColor[i]);
               state = State.TESTING;
            TestScreen = new TestScreen(topics[i - 1],backColor[i]);
            Containner.Children.Add(TestScreen);
        }
        TestScreen TestScreen;

        private void QuestionItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(sender is QuestionItem)
            {
                for(int i=1;i<=active_topic;i++)
                {
                    if (sender == questionItem[i])
                    {
                        OnClickQuestionItem(i);
                    }
                }
            }
           
        }

        private void QuestionItem_MouseDown_Add_New(object sender, MouseButtonEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
               
                dialog.SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory;
                dialog.Description = "Choose an icon pack folder";
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if(result==System.Windows.Forms.DialogResult.OK)
                {

                    AddTopic(dialog.SelectedPath);
                }
            }
        }
        private bool AddTopic(String path)
        {
            bool r = app.AddAnTopicFolder(path);
            if(!r)
            {
                MsgBoxYesNo msg = new MsgBoxYesNo("Cann't add this folder as topic. Retry ?");
                if ((bool)msg.ShowDialog())
                {
                    return AddTopic(path);
                }
                else return false;
            }
            else
            {
                active_topic++;
                questionItem[active_topic].Title.Content = topics[active_topic-1].Name;
                questionItem[active_topic].MaxScore.Content = "High score : " + topics[active_topic-1].HightScore + '/' + topics[active_topic-1].QuestionNumber;
                questionItem[active_topic].QuestionNumber.Content = "Question Number : " + topics[active_topic-1].QuestionNumber;

                return true;
            }
        }
    }
}
