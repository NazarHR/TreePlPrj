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
    /// Interaction logic for GoalControl.xaml
    /// </summary>
    public partial class GoalControl : UserControl
    {
        public GoalControl()
        {
            InitializeComponent();
        }
        public void addSubgoal(UIElement subgoal)
        {
            this.TasksList.Children.Add(subgoal);
        }
        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
        }
    }
}
