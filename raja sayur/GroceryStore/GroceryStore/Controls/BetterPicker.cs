using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GroceryStore.Controls
{
    public class BetterPicker : Picker
    {
        public static BindableProperty ItemSelectedCommandProperty =
            BindableProperty.Create(nameof(ItemSelectedCommandProperty), typeof(ICommand), typeof(BetterPicker), null);

        public static readonly BindableProperty ImageProperty =
            BindableProperty.Create(nameof(Image), typeof(string), typeof(BetterPicker), string.Empty);


        public ICommand ItemSelectedCommand
        {
            get
            {
                return (ICommand)this.GetValue(ItemSelectedCommandProperty);
            }
            set { this.SetValue(ItemSelectedCommandProperty, value); }
        }

        public BetterPicker()
        {
            this.SelectedIndexChanged += BetterPicker_SelectedIndexChanged;
        }

        private void BetterPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is BetterPicker picker && picker.SelectedItem != null)
            {
                ItemSelectedCommand?.Execute(picker.SelectedItem);
            }
        }

        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
    }
}
