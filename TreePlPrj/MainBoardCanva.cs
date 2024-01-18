using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;

namespace TreePlPrj
{
    public partial class MainBoardCanva : Canvas
    {
        Point offset;
        public UIElement dragObject = null;
        public Connection currentConnection = null;
 
        public MainBoardCanva() {
            MouseLeftButtonUp += MainBoard_MouseLeftButtonDown;
            PreviewMouseMove += MainBoard_PreviewMouseMove;
           // MouseDown += MainBoard_MouseDown;
            MouseLeftButtonUp += MainBoard_MouseLeftButtonUp;
            ContextMenu = GetContextMenu();
            MouseWheel += CanvaScroller_MouseWheel;
        }

        private void MainBoard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.dragObject = sender as UIElement;
            this.offset = e.GetPosition(this);
            this.offset.Y -= Canvas.GetTop(this.dragObject);
            this.offset.X -= Canvas.GetLeft(this.dragObject);
        }
        private void MainBoard_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(sender as UIElement);
            if (currentConnection != null)
            {
                currentConnection.End = Mouse.GetPosition(this);
            }
            else if (this.dragObject != null)
            {
                Canvas.SetTop(this.dragObject, position.Y - this.offset.Y);
                Canvas.SetLeft(this.dragObject, position.X - this.offset.X);
            }
        }
        //private void MainBoard_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    //Console.WriteLine(sender);
        //    //if (e.Source is Canvas)
        //    //{
        //    //    Connection connection = new Connection();
        //    //    this.Children.Add(connection);
        //    //}
        //}
        private void MainBoard_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.dragObject = null;
            if (currentConnection != null)
            {
                this.Children.Remove(currentConnection);
                currentConnection = null;
            }
        }
        public void Add_Control(GoalControl control, Point position)
        {
            Canvas.SetTop(control, position.Y);
            Canvas.SetLeft(control, position.X);
            Add_Control(control);
        }
        public void Add_Control(GoalControl control)
        {
            Canvas.SetZIndex(control, 1);
            control.MouseLeftButtonDown += MainBoard_MouseLeftButtonDown;
            control.GotFocus += Control_GotFocus;
            control.ImDeleted += Control_Imdeleted;
            this.Children.Add(control);
        }

        private void Control_Imdeleted(object sender, EventArgs e)
        {
            UIElement deletedGoal = sender as UIElement;
            this.Children.Remove(deletedGoal);
        }

        private void Control_GotFocus(object sender, RoutedEventArgs e)
        {
            GoalControl selectedControl = sender as GoalControl;
            this.Children.Remove(selectedControl);
            this.Children.Add(selectedControl);
        }
        private void Create_Goal_Click(object sender, RoutedEventArgs e)
        {
            Point currentPosition = Mouse.GetPosition(this);
            GoalCreator goalCreator = new GoalCreator();
            goalCreator.ShowDialog();
            GoalControl gc = goalCreator.getBuiltGoal();
            if (gc != null)
                Add_Control(gc, currentPosition);
        }
        public void connectionBegin(Connector start)
        {
            this.currentConnection = new Connection(start);
            this.Children.Add(currentConnection);
        }
        public void connectionEnd(Connector sink)
        {
            if (currentConnection == null) return;
            if (!this.currentConnection.Add_Sink(sink))
            {
                this.Children.Remove(currentConnection);
            }
            currentConnection = null;
        }
        public void RemoveElement(UIElement elementToRemove)
        {
            try { this.Children.Remove(elementToRemove); }

            catch (NullReferenceException) { }
        }
        private ContextMenu GetContextMenu()
        {
            ContextMenu menu = new ContextMenu();
            MenuItem create_Goal = new MenuItem();
            create_Goal.Header = "Create Goal";
            create_Goal.Click += Create_Goal_Click;
            menu.Items.Add(create_Goal);
            ResourceDictionary res = (ResourceDictionary)Application.LoadComponent(new Uri("styles/ContextMenu.xaml", UriKind.Relative));
            menu.Resources = res;
            return menu;
        }
        public UIElement[] GetContent()
        {
            UIElement[] elements = this.Children.Cast<UIElement>().ToArray();
            return elements;
        }
        public void SetContent(UIElementCollection content)
        {
            this.Children.Clear();
            foreach (UIElement element in content)
            {
                this.Children.Add(element);
            }
        }
        public void Clear()
        {
            this.Children.Clear();
        }
    }
}
