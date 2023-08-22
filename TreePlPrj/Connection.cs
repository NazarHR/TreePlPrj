using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TreePlPrj
{
    public class Connection : Shape
    {
        public static readonly DependencyProperty BeginingProperty =
            DependencyProperty.Register("Begining",
            typeof(Point), typeof(Connection),
            new FrameworkPropertyMetadata(new Point(0, 0),
            FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty EndProperty =
            DependencyProperty.Register("End",
            typeof(Point), typeof(Connection),
            new FrameworkPropertyMetadata(new Point(0, 0),
            FrameworkPropertyMetadataOptions.AffectsRender));

        private Point begining;
        public Point Begining
        {
            set { SetValue(BeginingProperty, value); begining = value; }
        }
        private Point end;
        public Point End
        {
            set { SetValue(EndProperty, value); end = value; }
        }
        private LineGeometry line;
        protected override Geometry DefiningGeometry
        {
            get
            {
                LineGeometry line = new LineGeometry(begining, end);
                this.line = line;
                return this.line;
            }
        }
        private Connector start;
        private Connector sink;
        private MainBoardCanva ParentCanva
        {
            get { return this.FindParent<MainBoardCanva>(); }
        }
        public Connection()
        {
            Stroke = Brushes.Red;
            StrokeThickness = 5;
        }
        public Connection(Connector start) : this()
        {
            this.start = start;
            begining = start.Position;
            start.PropertyChanged += Start_PropertyChanged;
            end = start.Position;
            start.ParentGoal.ImDeleted += OnDeletion;
            start.ParentGoal.ImComplited += ParentGoal_ImComplited;
            start.ParentGoal.ImIncompleted += ParentGoal_ImIncompleted;
            if(start.ParentGoal.Completed)Stroke=Brushes.Green;
        }

        private void ParentGoal_ImIncompleted(object sender, EventArgs e)
        {
            Stroke = Brushes.Red;
        }

        private void ParentGoal_ImComplited(object sender, EventArgs e)
        {
            Stroke = Brushes.Green;
        }

        private void Start_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Position")
            {
                Begining = start.Position;
            }
        }

        public bool Add_Sink(Connector sink)
        {
            if (sink.ParentGoal == start.ParentGoal) return false;
            this.sink = sink;
            End = sink.Position;
            sink.PropertyChanged += Sink_PropertyChanged;
            sink.ParentGoal.ImDeleted += OnDeletion;
            sink.ParentGoal.Connect_Goal(this);
            return true;
        }
        
        private void Sink_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Position")
            {
                End = sink.Position;
            }
        }
        public Connector getStart()
        {
            return start;
        }
        public Connector getSink() 
        {
            return sink;
        }
        public GoalControl GetStartGoal()
        {
            return start.ParentGoal;
        }
        public event Action ImDeleted;
        private void OnDeletion(object sender, EventArgs e)
        {
            if (ParentCanva != null)
            {
                ParentCanva.RemoveElement(this);
                ImDeleted?.Invoke();
            }
            start.PropertyChanged -= Start_PropertyChanged;
            start.ParentGoal.ImDeleted -= OnDeletion;
            start.ParentGoal.ImComplited -= ParentGoal_ImComplited;
            start.ParentGoal.ImIncompleted -= ParentGoal_ImIncompleted;
            start = null;
            if (sink != null)
            {
                sink.PropertyChanged -= Sink_PropertyChanged;
                sink.ParentGoal.ImDeleted -= OnDeletion;
                sink = null;
            }
            ImDeleted = null;
        }
    }
}
