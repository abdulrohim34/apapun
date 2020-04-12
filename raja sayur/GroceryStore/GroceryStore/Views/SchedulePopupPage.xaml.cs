using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using GroceryStore.Logic;
using GroceryStore.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroceryStore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SchedulePopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        Cart _cart;
        public SchedulePopupPage(Cart cart)
        {
            InitializeComponent();
            _cart = cart;
            //FromDate.MinimumDate = DateTime.ParseExact("2019-07-31", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            BindingContext = _cart;
        }

        private void BtnApply_Clicked(object sender, EventArgs e)
        {
            this.updateDate(_cart.id, "from_date", FromDate.Date.ToString(), Application.Current.Properties["user_id"].ToString());
            this.updateDate(_cart.id, "to_date", ToDate.Date.ToString(), Application.Current.Properties["user_id"].ToString());
            MessagingCenter.Send(this, "refreshCartData");
            PopupNavigation.Instance.PopAsync(true);
        }

        public async void updateDate(int id, string type, string date, string user_id)
        {
            await CartLogic.UpdateDate(id, type, date, user_id);
        }
    }
}