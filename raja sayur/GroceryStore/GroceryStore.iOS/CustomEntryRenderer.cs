using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using GroceryStore.Controls;
using GroceryStore.iOS;
using UIKit;
using CoreAnimation;
using CoreGraphics;

[assembly:ExportRenderer(typeof(CustomEntry),typeof(CustomEntryRenderer))]
namespace GroceryStore.iOS
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            //if (Control != null)
            //{
            //    Control.BorderStyle = UITextBorderStyle.None;
            //    Control.TintColor = UIColor.FromRGB(113, 191, 68);
            //    //Control.Layer.CornerRadius = 10;
            //}

            if (Element != null && Control != null)
            {
                // Create a custom border with square corners
                Control.BorderStyle = UITextBorderStyle.None;
                Control.Layer.CornerRadius = 0;
                Control.Layer.BorderWidth = .5f;
                Control.Layer.BorderColor = UIColor.FromRGB(200, 200, 200).CGColor;

                // Invisible views create padding at the beginning and end
                Control.LeftView = new UIView(new CGRect(0, 0, 8, Control.Frame.Height));
                Control.RightView = new UIView(new CGRect(0, 0, 8, Control.Frame.Height));
                Control.LeftViewMode = UITextFieldViewMode.Always;
                Control.RightViewMode = UITextFieldViewMode.Always;

                // Fixed height creates padding at top and bottom
                Element.HeightRequest = 30;
            }
        }
    }
}
