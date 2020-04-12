using System;
using Xamarin.Forms;

namespace GroceryStore.Controls
{
    public class CustomAppBarFrame : Frame
    {
        public static BindableProperty ElevationProperty = BindableProperty.Create(nameof(Elevation), typeof(float), typeof(CustomFrame), 4.0f);

        public float Elevation
        {
            get
            {
                return (float)GetValue(ElevationProperty);
            }
            set
            {
                SetValue(ElevationProperty, value);
            }
        }

        public static BindableProperty ShadowOpacityProperty = BindableProperty.Create("ShadowOpacity", typeof(float), typeof(CustomFrame), 0.3f);

        public float ShadowOpacity
        {
            get
            {
                return (float)GetValue(ShadowOpacityProperty);
            }
            set
            {
                SetValue(ShadowOpacityProperty, value);
            }
        }
    }
}