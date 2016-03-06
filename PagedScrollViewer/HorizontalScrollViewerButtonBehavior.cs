using System;
using Windows.Devices.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PagedScrollViewer
{
    public class HorizontalScrollViewerButtonBehavior : Behavior<Button>
    {
        private bool isTouchInput = false;
        public bool IsRight { get; set; }

        public ScrollViewer HostScrollViewer
        {
            get { return (ScrollViewer)GetValue(HostScrollViewerProperty); }
            set { SetValue(HostScrollViewerProperty, value); }
        }

        public static readonly DependencyProperty HostScrollViewerProperty =
            DependencyProperty.Register("HostScrollViewer", typeof(ScrollViewer), typeof(HorizontalScrollViewerButtonBehavior), new PropertyMetadata(null, HostScrollViewerChanged));

        private static void HostScrollViewerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var hostScrollViewer = e.NewValue as ScrollViewer;
            var buttonBehavior = d as HorizontalScrollViewerButtonBehavior;

            hostScrollViewer.PointerEntered += (sender, arg) =>
            {
                hostScrollViewer.InvalidateScrollInfo();
                if (arg.Pointer.PointerDeviceType == PointerDeviceType.Mouse ||
                    arg.Pointer.PointerDeviceType == PointerDeviceType.Pen)
                    buttonBehavior.CheckVisibility();
                else buttonBehavior.isTouchInput = true;
            };

            hostScrollViewer.PointerExited += (sender, arg) =>
            {
                buttonBehavior.AssociatedObject.Visibility = Visibility.Collapsed;
                buttonBehavior.isTouchInput = false;
            };
        }

        public override void Attach(DependencyObject associatedObject)
        {
            base.Attach(associatedObject);
            AssociatedObject.Click += AssociatedObject_Click;
            AssociatedObject.Visibility = Visibility.Collapsed;
        }

        private void AssociatedObject_Click(object sender, RoutedEventArgs e)
        {
            if (this.IsRight)
            {
                var scrollAmount = HostScrollViewer.HorizontalOffset + HostScrollViewer.ViewportWidth / 2;
                scrollAmount = Math.Min(scrollAmount, Math.Floor(HostScrollViewer.ScrollableWidth));
                HostScrollViewer?.ChangeView(scrollAmount, null, null);
            }
            else
                HostScrollViewer?.ChangeView(HostScrollViewer.HorizontalOffset - HostScrollViewer.ViewportWidth, null, null);
        }

        private void CheckVisibility()
        {
            if (isTouchInput)
                return;

            if (IsRight && HostScrollViewer.HorizontalOffset <= Math.Floor(HostScrollViewer.ScrollableWidth - 5))
                AssociatedObject.Visibility = Visibility.Visible;
            else if (!IsRight && HostScrollViewer.HorizontalOffset != 0)
                AssociatedObject.Visibility = Visibility.Visible;
        }
    }
}