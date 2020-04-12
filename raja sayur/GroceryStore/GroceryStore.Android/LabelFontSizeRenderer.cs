using System;
using Android.Content;
using Android.Content.Res;
using Android.Util;
using GroceryStore.Controls;
using GroceryStore.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(LabelFontSize),typeof(LabelFontSizeRenderer))]
namespace GroceryStore.Droid
{
    public class LabelFontSizeRenderer : LabelRenderer
    {
        public LabelFontSizeRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);


            if (Control != null)
            {
                float textSize = Control.TextSize;
                Resources resources = Control.Context.Resources;
                DisplayMetrics metrics = resources.DisplayMetrics;
                int dp = (int)(textSize / metrics.Density);
                if (dp == 10)
                {
                    dp = dp + 5;
                }
                Control.SetTextSize(ComplexUnitType.Dip, dp);
            }
        }
    }
}
