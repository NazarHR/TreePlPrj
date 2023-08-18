using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Connector.xaml
    /// </summary>
    public partial class Connector : UserControl, INotifyPropertyChanged
    {
        private Point position;
        public Point Position
        {
            get { return position; }
            set
            {
                if (position != value)
                {
                    position = value;
                    OnPropertyChanged("Position");
                }
            }
        }
        public ConnectorOrientation Orientation { get; set; }

        private GoalControl parentGoal;
        public GoalControl ParentGoal
        {
            get
            {
                if (parentGoal == null)
                {
                    parentGoal = this.FindParent<GoalControl>();
                }
                return parentGoal;
            }
        }
        private MainBoardCanva mainCanva;
        private MainBoardCanva MainCanva
        {
            get
            {
                if(mainCanva==null)
                {
                    mainCanva = this.FindParent<MainBoardCanva>();
                }
                return mainCanva;
            }
        }
        private List<Connection> connections;
        public List<Connection> Connections
        {
            get
            {
                if (connections == null)
                {
                    connections = new List<Connection>();
                }
                return connections;
            }
        }
        public Connector()
        {
            InitializeComponent();
            base.LayoutUpdated += new EventHandler( Connector_LayoutUpdated);
        }

        void Connector_LayoutUpdated(object sender, EventArgs e)
        {
            Canvas designer = this.FindParent<Canvas>();
            if (designer != null)
            {
                this.Position = this.TransformToAncestor(designer).Transform(new Point(this.Width / 2, this.Height / 2));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private void Connector_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainCanva.connectionBegin(this);
        }

        private void Connector_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainCanva.connectionEnd(this);
        }
        //internal ConnectorInfo GetInfo()
        //{
        //    ConnectorInfo info = new ConnectorInfo();
        //    info.DesignerItemLeft = Canvas.GetLeft(this.ParentGoal);
        //    info.DesignerItemTop = Canvas.GetTop(this.ParentGoal);
        //    info.DesignerItemSize = new Size(this.ParentGoal.ActualWidth, this.ParentGoal.ActualHeight);
        //    info.Orientation = this.Orientation;
        //    info.Position = this.Position;
        //    return info;
        //}
        //internal struct ConnectorInfo
        //{
        //    public double DesignerItemLeft { get; set; }
        //    public double DesignerItemTop { get; set; }
        //    public Size DesignerItemSize { get; set; }
        //    public Point Position { get; set; }
        //    public ConnectorOrientation Orientation { get; set; }
        //}

        public enum ConnectorOrientation
        {
            None,
            Left,
            Top,
            Right,
            Bottom
        }
    }
}
