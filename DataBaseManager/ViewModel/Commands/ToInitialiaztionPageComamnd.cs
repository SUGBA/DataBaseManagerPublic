using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DataBaseManager.ViewModel.Commands
{
    //Класс описывающий команду перехода к авторизационному окну
    class ToInitialiaztionPageComamnd : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        private MainViewModel mainVM;

        public ToInitialiaztionPageComamnd(MainViewModel mainVM) => this.mainVM = mainVM;


        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            mainVM.ToInitialiaztionPage();
        }
    }
}
