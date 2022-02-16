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

namespace TreePlPrj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UIElement dragObject = null;
        Point offset;
        public MainWindow()
        {
            InitializeComponent();
            //GoalControl uc = new GoalControl();
            //Add_Control(uc);
        }
        private void MainBoard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.dragObject = sender as UIElement;
            this.offset = e.GetPosition(this.MainBoard);
            this.offset.Y -= Canvas.GetTop(this.dragObject);
            this.offset.X -= Canvas.GetLeft(this.dragObject);
            this.MainBoard.CaptureMouse();
        }
        private void MainBoard_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (this.dragObject == null) return;
            var position = e.GetPosition(sender as UIElement);
            Canvas.SetTop(this.dragObject, position.Y - this.offset.Y);
            Canvas.SetLeft(this.dragObject, position.X - this.offset.X);
        }

        private void MainBoard_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.dragObject = null;
            this.MainBoard.ReleaseMouseCapture();
        }
        public void Add_Control(UIElement control)
        {
            Canvas.SetTop(control, 20);
            Canvas.SetLeft(control, 20);
            control.MouseLeftButtonDown += MainBoard_MouseLeftButtonDown;
            MainBoard.Children.Add(control);
        }

        private void Create_Goal_Click(object sender, RoutedEventArgs e)
        {
            GoalCreator goalCreator = new GoalCreator();
            goalCreator.ShowDialog();
            GoalControl gc = goalCreator.getBuiltGoal();
            if(gc !=null)
                Add_Control(gc);
        }
    }
}
