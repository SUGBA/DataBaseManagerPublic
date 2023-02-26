using DataBaseManager.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace DataBaseManager.ViewModel.Commands
{
    //Класс описывающий команду нажатия на один из элементов редактирование БД
    internal class RouteCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        private DataBaseViewModel dataBaseVM;

        public RouteCommand(DataBaseViewModel dataBaseVM) => this.dataBaseVM = dataBaseVM;
        //Режим редактирования передается через параметры описанные в XAML
        public bool CanExecute(object parameter)
        {
            ListMods param;
            Enum.TryParse(parameter.ToString(), out param);
            //Для каждой модификации свои условия доступа
            switch (param)
            {
                case ListMods.ReName:
                    return dataBaseVM.ActiveTable != null;
                case ListMods.AddColumn:
                    return dataBaseVM.ActiveTable != null;
                case ListMods.RemoveTable:
                    return dataBaseVM.ActiveTable != null;
                case ListMods.Refresh:
                    return true;
                case ListMods.CreateTable:
                    return true;
                case ListMods.RemoveColumn:
                    return dataBaseVM.ActiveColumn != null;
                case ListMods.RemovePrimaryKey:
                    return dataBaseVM.ActiveColumn != null;
                case ListMods.AddPrimaryKey:
                    return dataBaseVM.ActiveColumn != null;
                default:
                    return false;
            }
        }
        //В зависимости от режима либо вызывается метод (в случаях не требуются дополнительный пользовательский ввод)
        //Либо отображается панель ввода 
        public void Execute(object parameter)
        {
            ListMods param;
            Enum.TryParse(parameter.ToString(), out param);

            dataBaseVM.ActiveModeName = param;
            switch (param)
            {
                case ListMods.ReName:
                    dataBaseVM.ShowNamePanel();
                    break;
                case ListMods.AddColumn:
                    dataBaseVM.ShowColumnPanel();
                    break;
                case ListMods.RemoveTable:
                    dataBaseVM.RemoveTable();
                    dataBaseVM.ActiveModePage = null;
                    break;
                case ListMods.Refresh:
                    dataBaseVM.Refresh();
                    dataBaseVM.ActiveModePage = null;
                    break;
                case ListMods.CreateTable:
                    dataBaseVM.ShowNamePanel();
                    break;
                case ListMods.RemoveColumn:
                    dataBaseVM.RemoveColumn();
                    dataBaseVM.ActiveModePage = null;
                    break;
                case ListMods.RemovePrimaryKey:
                    dataBaseVM.RemovePrimaryKey();
                    dataBaseVM.ActiveModePage = null;
                    break;
                case ListMods.AddPrimaryKey:
                    dataBaseVM.AddPrimaryKey();
                    dataBaseVM.ActiveModePage = null;
                    break;
                default:
                    break;
            }
        }
    }
}
