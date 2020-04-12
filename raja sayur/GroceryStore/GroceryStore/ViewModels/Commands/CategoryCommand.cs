using GroceryStore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GroceryStore.ViewModels.Commands
{
    public class CategoryCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public HomeVM ViewModel { get; set; }

        public CategoryCommand(HomeVM viewModel)
        {
            ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var category = (Category)parameter;
            ViewModel.GetProduct();
        }
    }
}
