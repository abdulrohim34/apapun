using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using GroceryStore.Controls;
using GroceryStore.Droid.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomFrame), typeof(CustomElementFrameRenderer))]

namespace GroceryStore.Droid.Controls
{
    public class CustomElementFrameRenderer : Xamarin.Forms.Platform.Android.AppCompat.FrameRenderer
    {
        GradientDrawable _gi;

        public CustomElementFrameRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            var origFrame = e.NewElement as CustomElementFrame;

            if (origFrame != null)
            {
                GradientDrawable gi = new GradientDrawable();

                _gi = gi;

                gi.SetStroke(origFrame.BorderThickness, origFrame.OutlineColor.ToAndroid());
                gi.SetColor(origFrame.BackgroundColor.ToAndroid());
                gi.SetCornerRadius(origFrame.CornerRadius);
#pragma warning disable CS0618 // Type or member is obsolete
                SetBackgroundDrawable(gi);
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ChildCount > 0 && _gi != null)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                SetBackgroundDrawable(_gi);
#pragma warning restore CS0618 // Type or member is obsolete
            }

            base.OnElementPropertyChanged(sender, e);
        }
    }
}