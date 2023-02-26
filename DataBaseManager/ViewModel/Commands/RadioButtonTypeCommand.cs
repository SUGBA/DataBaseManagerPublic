using DataBaseManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DataBaseManager.ViewModel.Commands
{
    //Класс описывающий команду нажатия RadioButton'a в выборе типа данных в модуле редактирования БД
    internal class RadioButtonTypeCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        PanelDatabaseViewModel PanelDatabaseVM;

        public RadioButtonTypeCommand(PanelDatabaseViewModel PanelDatabaseVM)
        {
            this.PanelDatabaseVM = PanelDatabaseVM;
        }

        public bool CanExecute(object? parameter) => true;
        //В зависимоти от передаваемого параметра редактирует информацию о типе данных добавляемого элемента
        public void Execute(object? parameter)
            => PanelDatabaseVM.Type = parameter == null ? "integer" : parameter.ToString();
    }
}
