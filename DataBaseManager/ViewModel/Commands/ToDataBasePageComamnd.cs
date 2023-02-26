using DataBaseManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace DataBaseManager.ViewModel.Commands
{
    //Класс описывающий команду нажатия перехода от авторизации к редактированию БД

    class ToDataBasePageComamnd : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        private MainViewModel mainVM;

        public ToDataBasePageComamnd(MainViewModel mainVM) => this.mainVM = mainVM;


        public bool CanExecute(object? parameter)
        {
            return true;
        }
        //Условие поместил внутрь Execute, поскольку оно самостоятельно проверяет условие, а проверка занимает порядка секунды
        public void Execute(object? parameter)
        {
            string path = $"Host={mainVM.InitialiaztionVM.Host};" +
               $"Port={mainVM.InitialiaztionVM.Port};" +
               $"Database={mainVM.InitialiaztionVM.DBname};" +
               $"Username={mainVM.InitialiaztionVM.User};" +
               $"Password={mainVM.InitialiaztionVM.Password}";

            if (DataBase.CheckConnection(path))
            {
                mainVM.ToDataBasePage(path);
            }

        }
    }
}
