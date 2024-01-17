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
    /// Interaction logic for SingleGoalRow.xaml
    /// </summary>
    public partial class SingleGoalRow : UserControl
    {
        private Brush default_colour;
        private Brush complition_colour=Brushes.DarkGreen;
        private bool complited = false;
        public bool Comlited{
            get {return complited;} 
        }
        public event EventHandler ImDone;
        public event EventHandler ImUnDone;
        public SingleGoalRow()
        {
            InitializeComponent();
        }
        public SingleGoalRow(string goal)
        {
            InitializeComponent();
            GoalName.Text = goal;
        }

        public void setGoalText(string goal)
        {
            GoalName.Text = goal;
        }
        protected virtual void GoalMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(!complited)
            {
                OnImDone();
            }
            else
            {
                onImUndone();
            }
        }
        public void setDone()
        {
            OnImDone();
        }
        protected virtual void OnImDone()
        {
            complited = true;
            default_colour = GoalName.Background;
            GoalName.Background = complition_colour;
            ImDone?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void onImUndone()
        {
            complited = false;
            GoalName.Background = default_colour;
            ImUnDone?.Invoke(this, EventArgs.Empty);
        }
    }
}
