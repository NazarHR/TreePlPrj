using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace TreePlPrj
{
    public class ResizeThumb:Thumb
    {
        private double angle=0;
        private Point transformOrigin;
        private GoalControl designerItem;

        public ResizeThumb()
        {
            DragStarted += new DragStartedEventHandler(this.ResizeThumb_DragStarted);
            DragDelta += new DragDeltaEventHandler(this.ResizeThumb_DragDelta);
        }

        private void ResizeThumb_DragStarted(object sender, DragStartedEventArgs e)
        {

            this.designerItem = this.FindParent<GoalControl>();
            if (this.designerItem != null)
            {
                this.transformOrigin = this.designerItem.RenderTransformOrigin;
            }
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (this.designerItem != null)
            {
                double deltaVertical, deltaHorizontal;

                switch (VerticalAlignment)
                {
                    case System.Windows.VerticalAlignment.Bottom:
                        deltaVertical = Math.Min(-e.VerticalChange, this.designerItem.ActualHeight - this.designerItem.MinHeight);
                        if (designerItem.MinHeight <= designerItem.Height - deltaVertical)
                        {
                            Canvas.SetTop(this.designerItem, Canvas.GetTop(this.designerItem) + (this.transformOrigin.Y * deltaVertical * (1 - Math.Cos(-this.angle))));
                            Canvas.SetLeft(this.designerItem, Canvas.GetLeft(this.designerItem) - deltaVertical * this.transformOrigin.Y * Math.Sin(-this.angle));
                            this.designerItem.Height -= deltaVertical;
                        }
                        break;
                    case System.Windows.VerticalAlignment.Top:
                        deltaVertical = Math.Min(e.VerticalChange, this.designerItem.ActualHeight - this.designerItem.MinHeight);
                        if (designerItem.MinHeight <= designerItem.Height - deltaVertical) { 
                            Canvas.SetTop(this.designerItem, Canvas.GetTop(this.designerItem) + deltaVertical * Math.Cos(-this.angle) + (this.transformOrigin.Y * deltaVertical * (1 - Math.Cos(-this.angle))));
                            Canvas.SetLeft(this.designerItem, Canvas.GetLeft(this.designerItem) + deltaVertical * Math.Sin(-this.angle) - (this.transformOrigin.Y * deltaVertical * Math.Sin(-this.angle)));
                            this.designerItem.Height -= deltaVertical;
                        }
                        break;
                    default:
                        break;
                }

                switch (HorizontalAlignment)
                {
                    case System.Windows.HorizontalAlignment.Left:

                        deltaHorizontal = Math.Min(e.HorizontalChange, this.designerItem.ActualWidth - this.designerItem.MinWidth);
                        if (this.designerItem.MinWidth <= this.designerItem.Width - deltaHorizontal) { 
                            Canvas.SetTop(this.designerItem, Canvas.GetTop(this.designerItem) + deltaHorizontal * Math.Sin(this.angle) - this.transformOrigin.X * deltaHorizontal * Math.Sin(this.angle));
                            Canvas.SetLeft(this.designerItem, Canvas.GetLeft(this.designerItem) + deltaHorizontal * Math.Cos(this.angle) + (this.transformOrigin.X * deltaHorizontal * (1 - Math.Cos(this.angle))));
                            this.designerItem.Width -= deltaHorizontal; 
                        }
                        break;
                    case System.Windows.HorizontalAlignment.Right:
                        deltaHorizontal = Math.Min(-e.HorizontalChange, this.designerItem.ActualWidth - this.designerItem.MinWidth);
                        if (this.designerItem.MinWidth <= this.designerItem.Width - deltaHorizontal)
                        {
                            Canvas.SetTop(this.designerItem, Canvas.GetTop(this.designerItem) - this.transformOrigin.X * deltaHorizontal * Math.Sin(this.angle));
                            Canvas.SetLeft(this.designerItem, Canvas.GetLeft(this.designerItem) + (deltaHorizontal * this.transformOrigin.X * (1 - Math.Cos(this.angle))));
                            this.designerItem.Width -= deltaHorizontal;
                        }
                        break;
                    default:
                        break;
                }
            }
            e.Handled = true;
        }
    }
}
