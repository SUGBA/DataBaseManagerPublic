using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DataBaseManager.ViewModel.Commands
{
    //Класс описывающий команду нажатия RadioButton'a в выборе  первичного ключа в модуле редактирования БД
    internal class RadioButtonIsPrimaryKeyCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        PanelDatabaseViewModel PanelDatabaseVM;

        public RadioButtonIsPrimaryKeyCommand(PanelDatabaseViewModel PanelDatabaseVM)
        {
            this.PanelDatabaseVM = PanelDatabaseVM;
        }

        public bool CanExecute(object? parameter) => true;
        //В зависимоти от передаваемого параметра редактирует информацию о первичном ключе добавляемого элемента
        public void Execute(object? parameter)
            => PanelDatabaseVM.IsPrimaryKey = parameter == null ? false : (bool)parameter;
    }
}
