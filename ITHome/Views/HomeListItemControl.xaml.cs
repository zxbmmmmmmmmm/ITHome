using System;

using ITHome.Core.Models;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ITHome.Views
{
    public sealed partial class HomeListItemControl : UserControl
    {
        public News Item
        {
            get { return (News)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(Item), typeof(News), typeof(HomeListItemControl), new PropertyMetadata(null, OnItemPropertyChanged));

        public HomeListItemControl()
        {
            InitializeComponent();
            this.DataContextChanged += (s, e) => Bindings.Update();
        }

        private static void OnItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
    }
}
