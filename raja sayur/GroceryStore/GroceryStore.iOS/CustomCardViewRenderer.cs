using System;
using CoreGraphics;
using UIKit;
using GroceryStore.Controls;
using GroceryStore.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CardView), typeof(CustomCardViewRenderer))]
namespace GroceryStore.iOS
{
    public class CustomCardViewRenderer : Xamarin.Forms.Platform.iOS.FrameRenderer
    {
        public override void Draw(CGRect rect)
        {
            SetupShadowLayer();
            base.Draw(rect);
        }

        void SetupShadowLayer()
        {
            Layer.CornerRadius = 3; // 5 Default
            if (Element.BackgroundColor == Xamarin.Forms.Color.Default)
            {
                Layer.BackgroundColor = UIColor.White.CGColor;
            }
            else
            {
                Layer.BackgroundColor = Element.BackgroundColor.ToCGColor();
            }

            Layer.ShadowRadius = 3; // 5 Default
            Layer.ShadowColor = UIColor.Black.CGColor;
            Layer.ShadowOpacity = 0.1f; // 0.8f Default
            Layer.ShadowOffset = new CGSize(0f, 2.5f);

            if (Element.OutlineColor == Xamarin.Forms.Color.Default)
            {
                Layer.BorderColor = UIColor.Clear.CGColor;
            }
            else
            {
                Layer.BorderColor = Element.OutlineColor.ToCGColor();
                Layer.BorderWidth = 0;
            }

            Layer.RasterizationScale = UIScreen.MainScreen.Scale;
            Layer.ShouldRasterize = true;
        }
    }
}