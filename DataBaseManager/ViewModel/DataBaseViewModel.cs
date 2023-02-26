using DataBaseManager.Model;
using DataBaseManager.View;
using DataBaseManager.ViewModel.Commands;
using DataBaseManager.ViewModel.Core;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DataBaseManager.ViewModel
{
    //Класс ViewModel'а для панели отображения и редактирования БД
    public class DataBaseViewModel : INotifyPropertyChanged
    {
        //Выбранная пользователем таблица
        private TableTemplate _ActiveTable;
        public TableTemplate ActiveTable
        {
            get { return _ActiveTable; }
            set
            {
                _ActiveTable = value;
                OnPropertyChanged("ActiveTable");
            }
        }
        //Выбранный пользователем аттрибут
        private AttributeItem _ActiveColumn;
        public AttributeItem ActiveColumn
        {
            get { return _ActiveColumn; }
            set
            {
                _ActiveColumn = value;
                OnPropertyChanged("ActiveColumn");
            }
        }
        //Список таблиц
        private ObservableCollection<TableTemplate> _ListTables;
        public ObservableCollection<TableTemplate> ListTables
        {
            get { return _ListTables; }
            set
            {
                _ListTables = value;
                OnPropertyChanged("ListTables");
            }
        }
        //Модуль в котором отображается предсталвение для ввода дополнительных данных при модификации
        private Page _ActiveModePage;
        public Page ActiveModePage
        {
            get { return _ActiveModePage; }
            set
            {
                _ActiveModePage = value;
                OnPropertyChanged("ActiveModePage");
            }
        }
        //Имя активного режима модификации
        public ListMods ActiveModeName { get; set; }
        
        public PanelDatabaseViewModel PanelDatabaseVM { get; set; }
        public NpgsqlConnection connection { get; set; }

        public ICommand routeCommand { get; set; }

        public DataBaseViewModel(string path)
        {
            CreateConnection(path);
            ListTables = DataBase.GetListTables(connection);
            PanelDatabaseVM = new PanelDatabaseViewModel(this);

            routeCommand = new RouteCommand(this);
        }
        #region CommandMethods
        private void CreateConnection(string path)
        {
            connection = new NpgsqlConnection(path);
            connection.Open();
        }

        public void ShowNamePanel()
        {
            ActiveModePage = new InputNewName(new PanelDatabaseViewModel(this));
        }
        public void ShowColumnPanel()
        {
            ActiveModePage = new InputNewColumn(new PanelDatabaseViewModel(this));
        }
        public void RemoveTable()
        {
            DataBase.RemoveTable(connection, ListTables, ActiveTable.NameTable);
        }
        public void Refresh()
        {
            ListTables = DataBase.GetListTables(connection);
        }
        public void RemoveColumn()
        {
            DataBase.RemoveColumn(connection, ListTables, ActiveTable.NameTable, ActiveColumn.NameItem);
        }
        public void RemovePrimaryKey()
        {
            DataBase.RemovePrimaryKey(connection, ListTables, ActiveTable.NameTable);
        }
        public void AddPrimaryKey()
        {
            DataBase.AddPrimaryKey(connection, ListTables, ActiveTable.NameTable, ActiveColumn.NameItem);
        }

        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion
    }
}
