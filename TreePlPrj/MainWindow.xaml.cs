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
            if(Properties.Settings.Default.CurrentFile!="")
                Load(Properties.Settings.Default.CurrentFile);
        }

        private void mainWindow_Closing(object sender, CancelEventArgs e)
        {
            AskIfWantToSaveProgress();
            canvaScroller.EndThread();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            AskIfWantToSaveProgress();
            Properties.Settings.Default.CurrentFile = "";
            Properties.Settings.Default.Save();
            mainBoard.Clear();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Save(Properties.Settings.Default.CurrentFile);
        }
        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog savePlan = new SaveFileDialog();
            savePlan.Filter = "Tree Plan Files Files (*.plan)|*.plan";
            savePlan.DefaultExt = "plan";

            if ((bool)savePlan.ShowDialog())
            {
                Save(savePlan.FileName);
            }
        }
        private void Save(string path)
        {
            SaveManager.save(mainBoard.Children, path);
        }
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            AskIfWantToSaveProgress();
            OpenFileDialog openPlan = new OpenFileDialog();
            openPlan.Filter = "Tree Plan Files Files (*.plan)|*.plan";
            openPlan.DefaultExt = "plan";
            if ((bool)openPlan.ShowDialog())
            {
                mainBoard.Clear();
                string path = openPlan.FileName;
                if (!File.Exists(path)) return;
                Load(path);
                Properties.Settings.Default.CurrentFile = path;
                Properties.Settings.Default.Save();
            }
        }
        private void Load(string path)
        {
            if (!File.Exists(path)) return;
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
        private void AskIfWantToSaveProgress()
        {
            if (Properties.Settings.Default.CurrentFile == "" || mainBoard.Children.Count==0) return;
            if(MessageBox.Show("Do you want tot save changes before continuing?", "Save", MessageBoxButton.YesNo, MessageBoxImage.Question)==MessageBoxResult.Yes)
            {
                Save(Properties.Settings.Default.CurrentFile);
            }
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
    }
}
