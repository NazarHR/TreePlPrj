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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static TreePlPrj.Connector;

namespace TreePlPrj
{
    /// <summary>
    /// Interaction logic for GoalControl.xaml
    /// </summary>

    public partial class GoalControl : UserControl
    {
        private int amountOfDoneTasks = 0;
        private int AmoutOfDoneTasks
        {
            set
            {
                amountOfDoneTasks = value;
                UpdateAmounts();
                UpdateProgressbarState();
            }
            get { return amountOfDoneTasks; }
        }
        private int amountOfTasks = 0;
        private int AmountOfTasks
        {
            set
            {
                amountOfTasks = value; 
                UpdateAmounts();
                UpdateProgressBArMaximum();
            }
            get { return amountOfTasks; }
        }
        
        private bool completed = false;
        public event EventHandler ImComplited;
        public event EventHandler ImIncompleted;
        public event EventHandler ImDeleted;
        public bool Completed
        {
            get { return completed; }
            private set {
                if (completed != value)
                {
                    if (value)
                        ImComplited?.Invoke(this, EventArgs.Empty);
                    else
                        ImIncompleted?.Invoke(this, EventArgs.Empty);
                }
                completed = value; }
        }
        public GoalControl()
        {
            InitializeComponent();
        }
        public void addSubgoal(SingleGoalRow subgoal)
        {
            this.TasksList.Children.Add(subgoal);
            subgoal.ImDone += Subgoal_ImDone;
            subgoal.ImUnDone += Subgoal_ImUnDone;
            AmountOfTasks += 1;
            if(subgoal.Comlited)
            {
                Subgoal_ImDone(subgoal, EventArgs.Empty);
            }
        }
        public Point getPosition()
        {
            return new Point(Canvas.GetTop(this), Canvas.GetLeft(this));
        }
        public Connector getConnector(ConnectorOrientation connectorOrientation)
        {
            switch (connectorOrientation)
            {
                case ConnectorOrientation.Left:return LeftConnector;
                case ConnectorOrientation.Right: return RightConnector;
                case ConnectorOrientation.Top: return TopConnector;
                case ConnectorOrientation.Bottom: return BottomConnector;
                default: return null;
            }
        }
        public string getHeading()
        {
            return goalName.Text;
        }
        public List<Tuple<string, bool>> getGoals()
        {
            List<Tuple<string, bool>> goals = new List<Tuple<string, bool>>();
            foreach (SingleGoalRow subgoal in this.TasksList.Children)
            {
                goals.Add(new Tuple<string,bool>( subgoal.GoalName.Text,subgoal.Comlited));
            }
            return goals;
        }
        private void Subgoal_ImDone(object sender, EventArgs e)
        {
            AmoutOfDoneTasks += 1;
        }
        private void Subgoal_ImUnDone(object sender, EventArgs e)
        {
            AmoutOfDoneTasks -= 1;
        }

        private void Goal_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.ContextMenu.IsOpen = true;
        }

        private void Goal_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Focus();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            ImDeleted?.Invoke(this, EventArgs.Empty);
        }
        public void Connect_Goal(Connection goalToConnect)
        {
            ConnectedGoalRow connectedGoal = new ConnectedGoalRow(goalToConnect);
            connectedGoal.ImDeleted += DeleteGoal;
            addSubgoal(connectedGoal);
            
        }
        private void DeleteGoal(SingleGoalRow goal)
        {
            this.TasksList.Children.Remove(goal);
            AmountOfTasks--;
            if (goal.Comlited) AmoutOfDoneTasks--;
            
        }

        private void UpdateAmounts()
        {
            if (amountOfDoneTasks == amountOfTasks)
            {
                Completed = true;
            }
            
            else { Completed = false; }
            goalProgress.Text = $"{amountOfDoneTasks}/{amountOfTasks}";
        }
        private void UpdateProgressbarState()
        {
            DoubleAnimation progressAnimation = new DoubleAnimation(amountOfDoneTasks, TimeSpan.FromSeconds(1));
            goalProgressBar.BeginAnimation(ProgressBar.ValueProperty, progressAnimation);
        }
        private void UpdateProgressBArMaximum()
        {
            DoubleAnimation progressAnimation = new DoubleAnimation(amountOfTasks, TimeSpan.FromSeconds(1));
            goalProgressBar.BeginAnimation(ProgressBar.MaximumProperty, progressAnimation);
        }

        private void Goal_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }
    }
    public class ConnectedGoalRow : SingleGoalRow
    {
        public event Action<SingleGoalRow> ImDeleted;
        GoalControl connectedGoal;
        public ConnectedGoalRow(Connection goalConnection) : base()
        {
            connectedGoal = goalConnection.GetStartGoal();
            setGoalText(connectedGoal.goalName.Text);
            goalConnection.ImDeleted += OnDelition;
            connectedGoal.ImComplited += ConnectedGoal_ImComplited;
            connectedGoal.ImIncompleted += ConnectedGoal_ImIncompleated;
            if(connectedGoal.Completed)
            {
                OnImDone();
            }
        }
        private void ConnectedGoal_ImComplited(object sender, EventArgs e)
        {
            OnImDone();
        }

        private void ConnectedGoal_ImIncompleated(object sender, EventArgs e)
        {
            onImUndone();
        }

        private void OnDelition()
        {
            connectedGoal = null;
            ImDeleted?.Invoke(this);
        }
        protected override void GoalMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            return;
        }
    }
}
