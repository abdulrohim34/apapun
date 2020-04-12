using GroceryStore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroceryStore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        //public List<MasterPageItem> menuList { get; set; }
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();

        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;
            //menuList = new List<MasterPageItem>();
            //menuList.Add(new MasterPageItem()
            //{
            //    Title = "Home",
            //    Icon = "",
            //    Type = typeof(Home)
            //});
            //menuList.Add(new MasterPageItem()
            //{
            //    Title = "LoginPage",
            //    Icon = "",
            //    Type = typeof(LoginPage)
            //});
            //menuList.Add(new MasterPageItem()
            //{
            //    Title = "Sign Up",
            //    Icon = "",
            //    Type = typeof(RegisterPage)
            //});
            //menuList.Add(new MasterPageItem()
            //{
            //    Title = "Product Category",
            //    Icon = "",
            //    Type = typeof(LoginPage)
            //});
            //menuList.Add(new MasterPageItem()
            //{
            //    Title = "Help",
            //    Icon = "",
            //    Type = typeof(LoginPage)
            //});
            //navigationDrawerList.ItemsSource = menuList;
            //var page = (Page)Activator.CreateInstance(typeof(LoginPage));
            //page.Title = "Grocery Store";
            //Detail = new NavigationPage(page);

            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(LoginPage)));
            IsPresented = false;
            // MenuPages.Add((int)MenuItemType.Home, (NavigationPage)Detail);
        }

        //private void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    var item = (MasterPageItem)e.SelectedItem;
        //    Type page = item.Type;
        //    if (page == typeof(LoginPage))
        //    {
        //        //INavigation.PushAsync(new MyPage2());
        //        //await Navigation.PushAsync(new MyPage2());
        //        //Detail = new NavigationPage((Page)Activator.CreateInstance(page));
        //        Detail.Navigation.PushAsync(new LoginPage());
        //    }
        //    else
        //    {
        //        Detail = new NavigationPage((Page)Activator.CreateInstance(page));
        //    }
        //    IsPresented = false;
        //}

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Home:
                        MenuPages.Add(id, new NavigationPage(new Home()));
                        break;
                    case (int)MenuItemType.Login:
                        MenuPages.Add(id, new NavigationPage(new LoginPage()));
                        break;
                    case (int)MenuItemType.Register:
                        MenuPages.Add(id, new NavigationPage(new RegisterPage()));
                        break;
                    case (int)MenuItemType.ProductCategory:
                        MenuPages.Add(id, new NavigationPage(new ProductsPage()));
                        break;
                    case (int)MenuItemType.ProductDetail:
                        MenuPages.Add(id, new NavigationPage(new ProductDetailPage()));
                        break;
                    case (int)MenuItemType.Cart:
                        MenuPages.Add(id, new NavigationPage(new CartPage()));
                        break;
                    case (int)MenuItemType.Checkout:
                        MenuPages.Add(id, new NavigationPage(new CheckoutPage()));
                        break;
                    case (int)MenuItemType.ManageAddress:
                        MenuPages.Add(id, new NavigationPage(new ManageAddressPage()));
                        break;
                    case (int)MenuItemType.AddAddress:
                        MenuPages.Add(id, new NavigationPage(new AddAddressPage()));
                        break;
                    case (int)MenuItemType.ChangePassword:
                        MenuPages.Add(id, new NavigationPage(new ChangePasswordPage()));
                        break;
                    case (int)MenuItemType.VerifyCutomerInfo:
                        MenuPages.Add(id, new NavigationPage(new VerifyCustomerInfoPage()));
                        break;
                    case (int)MenuItemType.Notifications:
                        MenuPages.Add(id, new NavigationPage(new NotificationsPage()));
                        break;
                    case (int)MenuItemType.MyProfile:
                        MenuPages.Add(id, new NavigationPage(new MyProfilePage()));
                        break;
                    case (int)MenuItemType.Orders:
                        MenuPages.Add(id, new NavigationPage(new OrdersPage()
                        {
                            Title = "Order 1"
                        }));
                        break;
                    case (int)MenuItemType.Favourites:
                        MenuPages.Add(id, new NavigationPage(new FavouritesPage()));
                        break;
                    case (int)MenuItemType.Help:
                        MenuPages.Add(id, new NavigationPage(new HelpPage()));
                        break;
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                if (id == 1)
                {
                    this.ToolbarItems.Clear();
                    View view = new StackLayout()
                    {
                        HeightRequest = 40,
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };
                    (view as StackLayout).Children.Add(new Image()
                    {
                        Aspect = Aspect.AspectFill,
                        HorizontalOptions = LayoutOptions.Center,
                        //Source = ImageSource.FromResource("Helix.PassengerPortal.Images.helipass_logo_white.png")
                    });

                    await Detail.Navigation.PushAsync(new LoginPage());
                }
                else if (id == 13)
                {
                    this.ToolbarItems.Clear();
                    var page = (Page)Activator.CreateInstance(typeof(OrdersPage));
                    page.Title = "Orders";
                    //Detail.Title = "Order";
                    //Detail = new NavigationPage(page);
                    //Detail = new NavigationPage(new OrdersPage());
                    await Detail.Navigation.PushAsync(page, false);
                    //Application.Current.MainPage = new NavigationPage(new OrdersPage());
                }
                else
                {
                    this.ToolbarItems.Clear();
                    this.ToolbarItems.Add(new ToolbarItem()
                    {
                        Text = "Search",
                        Icon = "search.png",
                    });
                    this.ToolbarItems.Add(new ToolbarItem()
                    {
                        Text = "Favourites",
                        Icon = "Favourites_1.png",
                    });
                    this.ToolbarItems.Add(new ToolbarItem()
                    {
                        Text = "Cart",
                        Icon = "cart.png",
                    });
                    Detail = newPage;
                }

                //if (Device.RuntimePlatform == Device.Android)
                //    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}