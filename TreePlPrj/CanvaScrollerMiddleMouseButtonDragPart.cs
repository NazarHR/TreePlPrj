using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TreePlPrj
{
    public partial class CanvaScroller : ScrollViewer
    {
        private bool isMoving = false;
        private bool isDeferredMovingStarted = false;
        private Point? startPosition = null;
        private double slowdown = 200;

        private void InitializeIvents()
        {
            ScrollChanged += CanvaScroller_ScrollChanged;
            MouseDown += CanvaScroller_MouseDown;
            MouseUp += CanvaScroller_MouseUp;
            MouseMove += CanvaScroller_MouseMove;
        }
        private void CanvaScroller_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(this.isMoving==true)
                this.CancelScrolling();
            else if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Pressed)
            {
                if (this.isMoving == false)
                {
                    this.isMoving = true;
                    this.startPosition = e.GetPosition(sender as IInputElement);
                    this.isDeferredMovingStarted = true;
                    this.AddScrollSign(e.GetPosition(this.childrenCanva).X, e.GetPosition(this.childrenCanva).Y);
                }
            }
        }

        private void CanvaScroller_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Released && this.isDeferredMovingStarted != true)
                this.CancelScrolling();
        }
        private void CancelScrolling()
        {
            this.isMoving = false;
            this.startPosition = null;
            this.isDeferredMovingStarted = false;
            this.RemoveScrollSign();
        }

        private void CanvaScroller_MouseMove(object sender, MouseEventArgs e)
        {
            var sv = sender as ScrollViewer;

            if (this.isMoving && sv != null)
            {
                this.isDeferredMovingStarted = false; 

                var currentPosition = e.GetPosition(sv);
                var offset = currentPosition - startPosition.Value;
                offset.Y /= slowdown;
                offset.X /= slowdown;
                sv.ScrollToVerticalOffset(sv.VerticalOffset + offset.Y);
                sv.ScrollToHorizontalOffset(sv.HorizontalOffset + offset.X);
            }
        }

        private void AddScrollSign(double x,double y)
        {
            Cursor = Cursors.ScrollAll;
        }
        private void RemoveScrollSign()
        {
            Cursor=Cursors.Arrow;
        }
        
    }
}
