using DataBaseManager.Model;
using DataBaseManager.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Markup;

namespace DataBaseManager.ViewModel
{
    //Класс ViewModel'а для панели ввода пользовательской информации в редакторе
    public class PanelDatabaseViewModel : INotifyPropertyChanged
    {
        //P.S.В зависимости от режима некоторые элементы используются, некоторые нет

        private string _NewTableName;
        public string NewTableName
        {
            get { return _NewTableName; }
            set
            {
                _NewTableName = value;
                OnPropertyChanged("NewTableName");
            }
        }

        private string _NewColumnName;
        public string NewColumnName
        {
            get { return _NewColumnName; }
            set
            {
                _NewColumnName = value;
                OnPropertyChanged("NewColumnName");
            }
        }

        public string Type { get; set; }

        public bool IsPrimaryKey { get; set; }

        public ICommand acceptCommand { get; set; }
        public ICommand _RadioButtonIsPrimaryKeyCommand { get; set; }
        public ICommand _RadioButtonTypeCommand { get; set; }

        public DataBaseViewModel DataBaseVM { get; set; }

        public PanelDatabaseViewModel(DataBaseViewModel DataBaseVM)
        {
            NewTableName = DataBaseVM.ActiveTable == null ? string.Empty : DataBaseVM.ActiveTable.NameTable;
            NewColumnName = DataBaseVM.ActiveColumn == null ? string.Empty : DataBaseVM.ActiveColumn.NameItem;

            this.DataBaseVM = DataBaseVM;

            acceptCommand = new AcceptCommand(this, DataBaseVM);
            _RadioButtonIsPrimaryKeyCommand = new RadioButtonIsPrimaryKeyCommand(this);
            _RadioButtonTypeCommand = new RadioButtonTypeCommand(this);

        }

        #region CommandMethods
        public void ReName()
        {
            try
            {
                DataBase.ChangeNameTable(DataBaseVM.connection, DataBaseVM.ListTables,
                    DataBaseVM.ActiveTable.NameTable, NewTableName);
            }
            catch (Exception) 
            {
                System.Windows.MessageBox.Show("Failed to rename table");
            }
        }

        public void AddColumn()
        {
            try
            {
                var newColumn = new AttributeItem(NewColumnName, Type, IsPrimaryKey);
                if (DataBaseVM.ListTables.Any(x => x.NameTable == NewTableName))
                {
                    DataBase.AddColumns(DataBaseVM.connection, DataBaseVM.ListTables,
                        DataBaseVM.ActiveTable.NameTable, newColumn);
                }
                else
                {
                    var newTable = new TableTemplate(NewTableName, new ObservableCollection<AttributeItem>() { newColumn });
                    DataBase.CreateTable(DataBaseVM.connection, DataBaseVM.ListTables, newTable);
                }
            }
            catch (Exception) 
            {
                System.Windows.MessageBox.Show("Failed to add column");
            }
        }

        public void AddTable()
        {
            try
            {
                DataBase.CreateTable(DataBaseVM.connection, DataBaseVM.ListTables, NewTableName);
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Failed to add table");
            }
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
