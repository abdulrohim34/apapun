using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GroceryStore.Controls
{
    class CustomLabel : Label
    {
        public static readonly BindableProperty IsUnderlinedProperty = BindableProperty.Create("IsUnderlined", typeof(bool), typeof(CustomLabel), false);

        public bool IsUnderlined
        {
            get { return (bool)GetValue(IsUnderlinedProperty); }
            set { SetValue(IsUnderlinedProperty, value); }
        }
    }
}
