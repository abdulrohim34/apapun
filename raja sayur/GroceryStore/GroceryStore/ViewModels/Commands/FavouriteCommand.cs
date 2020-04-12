using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GroceryStore.ViewModels.Commands
{
    public class FavouriteCommand : ICommand
    {
        public FavouritesVM ViewModel;

        public FavouriteCommand(FavouritesVM favouritesVM)
        {
            ViewModel = favouritesVM;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return false;
        }

        public void Execute(object parameter)
        {

        }
    }
}
