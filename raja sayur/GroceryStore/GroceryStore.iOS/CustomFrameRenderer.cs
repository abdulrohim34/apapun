using System;
using System.ComponentModel;
using CoreGraphics;
using GroceryStore.Controls;
using GroceryStore.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomFrame), typeof(CustomFrameRenderer))]
namespace GroceryStore.iOS
{
    public class CustomFrameRenderer : FrameRenderer
    {
        public static void Initialize()
        {
            // empty, but used for beating the linker
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
                return;
            UpdateShadow();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "Elevation")
            {
                UpdateShadow();
            }
        }

        private void UpdateShadow()
        {

            //var materialFrame = (MaterialFrame)Element;

            // Update shadow to match better material design standards of elevation
            //Layer.ShadowRadius = materialFrame.Elevation;

            //Layer.BorderWidth = 0.1f;
            //Layer.BorderColor = UIColor.LightGray.CGColor;

            //Layer.ShadowColor = UIColor.Gray.CGColor;
            //Layer.ShadowOffset = new CGSize(2, 2);
            //Layer.ShadowOpacity = 0.80f;
            //Layer.ShadowPath = UIBezierPath.FromRect(Layer.Bounds).CGPath;
            //Layer.MasksToBounds = false;
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            var materialFrame = (CustomFrame)Element;
            // Update shadow to match better material design standards of elevation
            //Layer.CornerRadius = materialFrame.CornerRadius;
            //Layer.BorderWidth = 0.1f;
            if (materialFrame.ShadowOpacity == 0.11f)
            {
                Layer.BorderColor = UIColor.LightGray.CGColor;
            }
            else if (materialFrame.ShadowOpacity == 0.12f)
            {
                Layer.BorderColor = UIColor.White.CGColor;
            }
            else
            {
                Layer.BorderColor = UIColor.White.CGColor;
            }
            if (materialFrame.ShadowOpacity == 0.11f)
            {
                Layer.ShadowRadius = materialFrame.Elevation;
            }
            else
            {
                Layer.ShadowRadius = (materialFrame.Elevation == 1f) ? 6 : materialFrame.Elevation;
            }
            if (materialFrame.ShadowOpacity == 0.12f)
            {
                Layer.ShadowRadius = 0;
            }

            Layer.ShadowColor = UIColor.Gray.CGColor;
            Layer.ShadowOffset = new CGSize(2, 2);
            Layer.ShadowOpacity = materialFrame.ShadowOpacity;
            Layer.ShadowPath = UIBezierPath.FromRect(Layer.Bounds).CGPath;
            Layer.MasksToBounds = false;

            // Other
            //Layer.BackgroundColor = UIColor.LightGray.CGColor;
            //// Shadow and Radius
            //Layer.ShadowColor = UIColor.Gray.CGColor;
            //Layer.ShadowOffset = new CGSize(0, 2);
            //Layer.ShadowOpacity = 1;
            //Layer.ShadowRadius = 0;
            //Layer.MasksToBounds = false;
            //Layer.CornerRadius = 0;

        }
    }
}