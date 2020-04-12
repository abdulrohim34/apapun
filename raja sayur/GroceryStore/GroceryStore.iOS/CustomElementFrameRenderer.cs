using System;
using System.ComponentModel;
using System.Drawing;
using CoreGraphics;
using GroceryStore.Controls;
using GroceryStore.iOS.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomFrame), typeof(CustomElementFrameRenderer))]
namespace GroceryStore.iOS.Controls
{
    public class CustomElementFrameRenderer : FrameRenderer
    {
        private CustomElementFrame customElementFrame;

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                customElementFrame = e.NewElement as CustomElementFrame;
                SetupLayer();

            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName ||
                e.PropertyName == Xamarin.Forms.Frame.OutlineColorProperty.PropertyName ||
                e.PropertyName == Xamarin.Forms.Frame.HasShadowProperty.PropertyName ||
                e.PropertyName == Xamarin.Forms.Frame.CornerRadiusProperty.PropertyName)
            {
                SetupLayer();
            }
        }

        void SetupLayer()
        {
            float cornerRadius = customElementFrame.CornerRadius;

            if (cornerRadius == -1f)
                cornerRadius = 5f; // default corner radius

            Layer.CornerRadius = cornerRadius;
            Layer.BackgroundColor = customElementFrame.BackgroundColor.ToCGColor();

            if (customElementFrame.HasShadow)
            {
                Layer.ShadowRadius = 2;
                Layer.ShadowColor = UIColor.Gray.CGColor;
                Layer.ShadowOpacity = 0.2f;
                Layer.ShadowOffset = new SizeF();
            }
            else
                Layer.ShadowOpacity = 0;

            //if (customElementFrame.OutlineColor == Color.Default)
            //    Layer.BorderColor = UIColor.Clear.CGColor;
            //else
            //{
            Layer.BorderColor = customElementFrame.OutlineColor.ToCGColor();
            Layer.BorderWidth = customElementFrame.BorderThickness;
            // }

            Layer.RasterizationScale = UIScreen.MainScreen.Scale;
            Layer.ShouldRasterize = true;
        }
    }
}