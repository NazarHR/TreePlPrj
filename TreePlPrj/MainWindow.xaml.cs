using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.Windows.Markup;
using System.Text;
using System.Xml;
using System.Linq;
using System.Net.Http.Headers;

namespace TreePlPrj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void mainWindow_Closing(object sender, CancelEventArgs e)
        {
            canvaScroller.EndThread();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.CurrentFIle = null;
            Properties.Settings.Default.Save();
            mainBoard.Clear();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openPlan = new OpenFileDialog();
            openPlan.Filter = "Tree Plan Files Files (*.plan)|*.plan";
            openPlan.DefaultExt = "plan";
            if ((bool)openPlan.ShowDialog())
            {
                string openedCanva = File.ReadAllText(openPlan.FileName);
                StringReader stringReader = new StringReader(openedCanva);
                XmlReader xmlReader = XmlReader.Create(stringReader);
                CanvaScroller readedScroller = (CanvaScroller)XamlReader.Load(xmlReader);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            //SaveManager.save(mainBoard.Children);
        }
        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog savePlan = new SaveFileDialog();
            savePlan.Filter = "Tree Plan Files Files (*.plan)|*.plan";
            savePlan.DefaultExt = "plan";

            if ((bool)savePlan.ShowDialog())
            {
                SaveManager.save(mainBoard.Children, savePlan.FileName);
            }
        }
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            string path = " C:\\Users\\nazar\\Desktop\\plan.plan";
            OpenFileDialog openPlan = new OpenFileDialog();
            openPlan.Filter = "Tree Plan Files Files (*.plan)|*.plan";
            openPlan.DefaultExt = "plan";
            if ((bool)openPlan.ShowDialog())
            {
                mainBoard.Clear();
                path = openPlan.FileName;
                Load(path);
            }
        }
        private void Load(string path)
        {
            List<GoalControl> loadedGoals;
            List<Connection> loadedConnections;
            SaveManager.Load(path, out loadedGoals, out loadedConnections);
            foreach (GoalControl goalControl in loadedGoals)
            {
                mainBoard.Add_Control(goalControl);
            }
            foreach (Connection connection in loadedConnections)
            {
                mainBoard.Children.Add(connection);
            }
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ChangeBackground_Click(object sender, RoutedEventArgs e)
        {
            MenuItem backgoundChanger = sender as MenuItem;
            string changerName = (string)backgoundChanger.Header;

            switch (changerName)
            {
                case "Gray":
                    Properties.Settings.Default.BackgoundPath = "backgrounds/gray.jpg";
                    break;
                case "Stardust":
                    Properties.Settings.Default.BackgoundPath = "backgrounds/stardust.png";
                    break;
                case "Motley":
                    Properties.Settings.Default.BackgoundPath = "backgrounds/motley.png";
                    break;
                case "Green":
                    Properties.Settings.Default.BackgoundPath = "backgrounds/green.jpg";
                    break;
                default:
                    string path;
                    if (SelectBackground(out path))
                        Properties.Settings.Default.BackgoundPath = path;
                    break;

            }
            Properties.Settings.Default.Save();
        }
        private bool SelectBackground(out string backgroundPath)
        {
            backgroundPath = null;
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";
            fileDialog.Title = "Select background";
            if((bool)fileDialog.ShowDialog())
            {
                backgroundPath = fileDialog.FileName;
                return true;
            }
            return false;
            
        }
        private void Save()
        {

        }


    }
}
