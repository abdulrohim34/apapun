using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GroceryStore.ViewModels.Commands
{
    public class NavigationCommand : ICommand
    {
        public ProductListVM ProductListVM
        {
            get;
            set;
        }
        public NavigationCommand(ProductListVM productListVM)
        {
            ProductListVM = productListVM;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            //ProductListVM.Navigate();
        }
    }
}
