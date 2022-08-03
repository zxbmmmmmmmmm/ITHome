using ITHome.Core.Models;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ITHome.Views
{
    public sealed partial class HomeDetailControl : UserControl
    {
        public SampleOrder SelectedItem
        {
            get { return GetValue(SelectedItemProperty) as SampleOrder; }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(SampleOrder), typeof(HomeDetailControl), new PropertyMetadata(null, OnSelectedItemPropertyChanged));

        public HomeDetailControl()
        {
            InitializeComponent();
        }

        private static void OnSelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as HomeDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
