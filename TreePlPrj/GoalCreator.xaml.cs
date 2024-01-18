using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TextBox = System.Windows.Controls.TextBox;

namespace TreePlPrj
{
    /// <summary>
    /// Interaction logic for GoalCreator.xaml
    /// </summary>
    public partial class GoalCreator : Window
    {
        private GoalControl goalControl;
        private TextBoxWithPlaceholder goalNamePreview = new TextBoxWithPlaceholder("Enter heading here...");
        public GoalCreator()
        {
            goalControl = new GoalControl();
            InitializeComponent();
            HeadingPlace.Children.Add(goalNamePreview);
        }
        private void AddNewSubgoalButton_Click(object sender, RoutedEventArgs e)
        {
            TextBoxWithPlaceholder textBox = new TextBoxWithPlaceholder("Enter goal here...");
            SubgoalPreview.Children.Add(textBox);
        }
        private void RemoveLastSubgoalButton_Click(object sender, RoutedEventArgs e)
        {
            SubgoalPreview.Children.RemoveAt(SubgoalPreview.Children.Count - 1);
        }
        private void CreateNewGoalTree_Click(object sender, RoutedEventArgs e)
        {
            goalControl.goalName.Text = this.goalNamePreview.Text;
            foreach (TextBoxWithPlaceholder subgoal in SubgoalPreview.Children)
            {
                string subgoalTextHolder = subgoal.Text;
                SingleGoalRow goalRow = new SingleGoalRow(subgoalTextHolder);
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
        public class TextBoxWithPlaceholder: TextBox
        {
            private bool isPlaceholder = true;
            private string placeholderText;

            public new string Text
            {
                get => isPlaceholder ? string.Empty : base.Text;
                set => base.Text = value;
            }

            public TextBoxWithPlaceholder(string text)
            {
                this.SetResourceReference(StyleProperty, typeof(TextBox));
                this.GotFocus += RemoveText;
                this.LostFocus += AddText;
                this.placeholderText = text; 
                this.Text = placeholderText;
            }
            
            public void RemoveText(object sender, EventArgs e)
            {
                if (isPlaceholder)
                {
                    this.Text = "";
                    isPlaceholder = false;
                }
            }
            public void AddText(object sender, EventArgs e)
            {
                if (this.Text == "")
                {
                    this.Text = placeholderText;
                    isPlaceholder = true;
                }
            }
        }

        private void CancleNewGoaldTreeCreation_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
