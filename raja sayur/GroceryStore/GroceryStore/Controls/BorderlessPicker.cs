using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GroceryStore.Controls
{
    public class BorderlessPicker : Picker
    {

        public static readonly BindableProperty ItemSelectedCommandProperty = BindableProperty.Create(nameof(ItemSelectedCommand), typeof(ICommand), typeof(BetterPicker), null);

        public ICommand ItemSelectedCommand
        {
            get => (ICommand)GetValue(ItemSelectedCommandProperty);
            set => SetValue(ItemSelectedCommandProperty, value);
        }

        public BorderlessPicker()
        {
            this.SelectedIndexChanged += BorderlessPicker_SelectedIndexChanged;
        }

        private void BorderlessPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is Picker picker)
                ItemSelectedCommand?.Execute(picker);
        }
    }
}
