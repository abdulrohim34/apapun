using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GroceryStore.Controls
{
    public class CustomElementFrame : Frame
    {
        public new Thickness Padding { get; set; } = 0;
        public int BorderThickness { get; set; }
        public CustomElementFrame()
        {
            base.Padding = this.Padding;
        }
    }
}