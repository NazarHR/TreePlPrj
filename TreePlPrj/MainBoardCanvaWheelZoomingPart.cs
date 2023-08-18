using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TreePlPrj
{
    public partial class MainBoardCanva : Canvas
    {
        private const Double zoomMax = 2.5;
        private const Double zoomMin = 0.1;
        private const Double ZoomFactor = 0.1;
        private Double zoom = 1;
        public Double Zoom { get { return zoom; } }
        private ScrollViewer parentScrollViewer
        {
            get
            {
                return this.FindParent<ScrollViewer>();
            }
        }
        private ScaleTransform scaleTransform = new ScaleTransform();
        private void CanvaScroller_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point mouseCanvasPosition = e.GetPosition(this);
            double scaleFactor = e.Delta > 0
              ? ZoomFactor
              : -1 * ZoomFactor;
            if(zoom + scaleFactor < zoomMax && zoom + scaleFactor > zoomMin)
            {
                
                AdjustScroll(mouseCanvasPosition, scaleFactor);
                ApplyZoom(mouseCanvasPosition, scaleFactor);
            }
            e.Handled = true;
        }
        private void ApplyZoom(Point mouseCanvasPosition, double scaleFactor)
        {
            zoom += scaleFactor;
            scaleTransform.ScaleX = zoom;
            scaleTransform.ScaleY = zoom;
            this.LayoutTransform = scaleTransform;
        }
        private void AdjustScroll(Point mouseCanvasPosition, double scaleFactor)
        {
            parentScrollViewer.ScrollToHorizontalOffset(parentScrollViewer.HorizontalOffset + mouseCanvasPosition.X * scaleFactor);
            parentScrollViewer.ScrollToVerticalOffset(parentScrollViewer.VerticalOffset + mouseCanvasPosition.Y * scaleFactor);
        }
    }
}
