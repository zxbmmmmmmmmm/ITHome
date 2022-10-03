using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;

namespace RichTextControls
{
    public partial class HtmlTextBlock
    {
        public void NewImagelink_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ImageTapped?.Invoke(this, e);
        }
        public void Hyperlink_Click(Hyperlink sender, HyperlinkClickEventArgs args)
        {

        }
    }
}
