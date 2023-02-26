using DataBaseManager.Model;
using DataBaseManager.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DataBaseManager.ViewModel.Commands
{
    //Класс описывающий команду нажатия кнопки Accept в модуле редактирования БД
    internal class AcceptCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private PanelDatabaseViewModel panelDatabaseVM;
        private DataBaseViewModel dataBaseVM;

        public AcceptCommand(PanelDatabaseViewModel panelDatabaseVM, DataBaseViewModel dataBaseVM)
        {
            this.panelDatabaseVM = panelDatabaseVM;
            this.dataBaseVM = dataBaseVM;
        }

        public bool CanExecute(object? parameter)
        {
            //В зависимости от выбранного режима корректировки (Описаны в enum'e ListMods) будут разные ограничение
            switch (dataBaseVM.ActiveModeName)
            {
                //Если переименовывание
                case ListMods.ReName:
                    //Должна быть выбранна активная таблица
                    return dataBaseVM.ActiveTable != null;
                //Если добавление аттрибута
                case ListMods.AddColumn:
                    //Должна быть выбранна активная таблица
                    return dataBaseVM.ActiveTable != null;
                //Если добавление аттрибута
                case ListMods.CreateTable:
                    //Доступна всегда
                    return true;
                default:
                    return false;
            }
        }

        public void Execute(object? parameter)
        {
            //Методы, вызывающиеся в заивисимости от выбранного режима корректировки
            switch (dataBaseVM.ActiveModeName)
            {
                case ListMods.ReName:
                    panelDatabaseVM.ReName();
                    break;
                case ListMods.AddColumn:
                    panelDatabaseVM.AddColumn();
                    break;
                case ListMods.CreateTable:
                    panelDatabaseVM.AddTable();
                    break;
                default:
                    break;
            }
        }
    }
}
