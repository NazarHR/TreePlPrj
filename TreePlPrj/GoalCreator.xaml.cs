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
using System.Windows.Shapes;

namespace TreePlPrj
{
    /// <summary>
    /// Interaction logic for GoalCreator.xaml
    /// </summary>
    public partial class GoalCreator : Window
    {
        private GoalControl goalControl;
        private List<UIElement> tsklst;
        public GoalCreator()
        {
            goalControl = new GoalControl();
            tsklst = new List<UIElement>();
            InitializeComponent();
        }
        private void AddNewSubgoalButton_Click(object sender, RoutedEventArgs e)
        {
            TextBox textBox = new TextBox();
            textBox.Text = "im a goal";
            SubgoalPreview.Children.Add(textBox);
        }

        private void CreateNewGoalTree_Click(object sender, RoutedEventArgs e)
        {
            goalControl.goalName.Text = this.goalNamePreview.Text;
            goalControl.goalProgress.Text = this.goalDescriptionPreview.Text;
            foreach (var subgoal in SubgoalPreview.Children)
            {
                TextBox subgoalTextHolder= subgoal as TextBox;
                SingleGoalRow goalRow = new SingleGoalRow(subgoalTextHolder.Text);
                goalControl.addSubgoal(goalRow);
            }
            this.Close();
        }
        public GoalControl getBuiltGoal()
        {
            if (goalControl.TasksList.Children.Count == 0)
                return null;
            return goalControl;
        }
        public static GoalControl CreateGoalFromParams(string heading, List<Tuple<string, bool>> subgoals, double top = 0, double left = 0, double width = 150, double height = 200)
        {
            GoalControl goalControl = new GoalControl();
            goalControl.goalName.Text=heading;
            Canvas.SetTop(goalControl, top);
            Canvas.SetLeft(goalControl, left);
            goalControl.Width = width;
            goalControl.Height = height;
            foreach (var subgoal in subgoals)
            {
                SingleGoalRow singleGoalRow = new SingleGoalRow(subgoal.Item1);
                goalControl.addSubgoal(singleGoalRow);
                if(subgoal.Item2)
                {
                    singleGoalRow.setDone();
                }
            }
            
            return goalControl;
        }
    }
}
