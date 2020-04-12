using System;
using Xamarin.Forms;

namespace GroceryStore.Controls
{
    public class CardView : Frame
    {
        public float ShadowRadius { get; set; }
        public CardView()
        {
            Padding = 0;
            //if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.iOS)
            //{
            //    HasShadow = false;
            //    OutlineColor = Color.Transparent;
            //    BackgroundColor = Color.Transparent;
            //}
        }
    }
}
