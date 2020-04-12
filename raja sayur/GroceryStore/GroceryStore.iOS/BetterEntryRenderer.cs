using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using GroceryStore.Controls;
using GroceryStore.iOS;
using UIKit;

[assembly: ExportRenderer(typeof(BetterEntry), typeof(BetterEntryRenderer))]
namespace GroceryStore.iOS
{
    public class BetterEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.BorderStyle = UIKit.UITextBorderStyle.None;
                if (Element != null)
                {
                    Element.HeightRequest = 35;
                    Element.Margin = new Thickness(5, 0, 5, 0);
                }

                UITextField textField = (UITextField)Control;
                textField.Font = UIFont.FromName("AvenirLTStd Roman", 13);
            }
        }
    }
}
