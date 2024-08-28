using BibGen.App.Viewmodel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BibGen.App.UserControls
{
    /// <summary>
    /// Interaction logic for PreviewOverlayControl.xaml
    /// </summary>
    public partial class PreviewOverlayControl : UserControl
    {
        private MainWindowVM MyDataContext =>
            (MainWindowVM)DataContext;

        public PreviewOverlayControl()
        {
            InitializeComponent();
            SizeChanged += PreviewOverlayControl_SizeChanged;
        }

        private void PreviewOverlayControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            HandleTextBlocks();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (MyDataContext != null)
            {
                MyDataContext.StripeItems.CollectionChanged += StripeItems_CollectionChanged;
                MyDataContext.PaginationVM.PropertyChanged += PaginationVM_PropertyChanged;
            }

        }

        private void StripeItems_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            HandleTextBlocks();

            var oldItems = e.OldItems?.OfType<StripeItemVM>();
            if (oldItems != null)
            {
                foreach (var item in oldItems)
                {
                    item.PropertyChanged -= StripeItem_PropertyChanged;
                }
            }

            var newItems = e.NewItems?.OfType<StripeItemVM>();
            if (newItems != null)
            {
                foreach (var item in newItems)
                {
                    item.PropertyChanged += StripeItem_PropertyChanged;
                }
            }
        }

        private void PaginationVM_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PaginationVM.ItemCount) ||
                e.PropertyName == nameof(PaginationVM.CurrentItem))
            {
                HandleTextBlocks();
                InvalidateVisual();
            }
        }

        private void StripeItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(StripeItemVM.FontSize) ||
                e.PropertyName == nameof(StripeItemVM.FontName) ||
                e.PropertyName == nameof(StripeItemVM.Color) ||
                e.PropertyName == nameof(StripeItemVM.Baseline) ||
                e.PropertyName == nameof(StripeItemVM.ExcelColumnName))
            {
                HandleTextBlocks();
                InvalidateVisual();
            }
        }

        private void HandleTextBlocks()
        {
            var foreground = Brushes.Gray;

            OverlayCanvas.Children.RemoveRange(0, OverlayCanvas.Children.Count);

            if (!MyDataContext.BibEntries.Any())
                return;

            foreach (var stripe in MyDataContext.StripeItems)
            {
                var itemIdx = MyDataContext.PaginationVM.CurrentItem;
                var bibEntry = MyDataContext.BibEntries[itemIdx];

                bibEntry.DataItems.TryGetValue(stripe.ExcelColumnName, out string overlayText);
                var textBlock = new TextBlock
                {
                    Text = overlayText ?? "???",
                    FontFamily = new FontFamily(stripe.FontName),
                    FontSize = stripe.FontSize,
                    Foreground = new SolidColorBrush(stripe.Color),
                };

                textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                var left = (ActualWidth - textBlock.DesiredSize.Width) / 2;
                var top = -textBlock.DesiredSize.Height + (double)stripe.Baseline * this.ActualHeight;
                Canvas.SetLeft(textBlock, left);
                Canvas.SetTop(textBlock, top);

                OverlayCanvas.Children.Add(textBlock);
            }
        }
    }
}
