using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TreePlPrj
{
    public partial class CanvaScroller : ScrollViewer
    {
        Thread border_scroller;
        private static double desired_border_move_trigger = 100;
        private MainBoardCanva ChildrenCanva = null;
        private MainBoardCanva childrenCanva
        {
            get
            {
                if (ChildrenCanva == null)
                    ChildrenCanva = Dispatcher.Invoke(new Func<MainBoardCanva>(() =>
                    {
                        return this.Content as MainBoardCanva;
                    }));
                return ChildrenCanva;
            }
        }

        public CanvaScroller() {
            
            InitializeIvents();
            StartSideScrollingThread();

        }
        private void StartSideScrollingThread()
        {
            border_scroller = new Thread(SideScroll);
            border_scroller.Start();
        }
        private void SideScroll()
        {
            
            while (true)
            {
                if (childrenCanva.dragObject == null && childrenCanva.currentConnection == null) continue;
                Point point = Dispatcher.Invoke(new Func<Point>(() => { return Mouse.GetPosition(this); }));
                double new_horizontal_offset;
                double new_vertical_offset;
                int speed = 1;
                if (point.X > ActualWidth - desired_border_move_trigger)
                {
                    new_horizontal_offset = ((point.X - ActualWidth) / desired_border_move_trigger + 1) * speed;
                    Dispatcher.Invoke(new Action(() => this.ScrollToHorizontalOffset(this.HorizontalOffset + new_horizontal_offset)));
                }
                else if (point.X < desired_border_move_trigger)
                {
                    new_horizontal_offset = (point.X - desired_border_move_trigger) / desired_border_move_trigger * speed;
                    Dispatcher.Invoke(new Action(() => this.ScrollToHorizontalOffset(this.HorizontalOffset + new_horizontal_offset)));
                }
                if (point.Y > ActualHeight - desired_border_move_trigger)
                {
                    new_vertical_offset = ((point.Y - ActualHeight) / desired_border_move_trigger + 1) * speed;
                    Dispatcher.Invoke(new Action(() => this.ScrollToVerticalOffset(this.VerticalOffset + new_vertical_offset)));
                }
                else if (point.Y < desired_border_move_trigger)
                {
                    new_vertical_offset = (point.Y - desired_border_move_trigger) / desired_border_move_trigger * speed;
                    Dispatcher.Invoke(new Action(() => this.ScrollToVerticalOffset(this.VerticalOffset + new_vertical_offset)));
                }
            }
            
        }
        private void CanvaScroller_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            childrenCanva.Width = (this.ActualWidth + this.HorizontalOffset) / childrenCanva.Zoom + 10;
            childrenCanva.Height = (this.ActualHeight + this.VerticalOffset) / childrenCanva.Zoom + 10;
        }

        public void EndThread()
        {
            border_scroller.Abort();
        }
    }
}
