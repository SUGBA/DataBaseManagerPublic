using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseManager.Model
{
    public class TableTemplate : INotifyPropertyChanged
    {
        /*
         * NameTable - Имя таблицы
         * ListAttributes - Список аттрибутов
         */
        private string _NameTable;
        public string NameTable
        {
            get { return _NameTable; }
            set
            {
                _NameTable = value;
                OnPropertyChanged("NameTable");
            }
        }

        private ObservableCollection<AttributeItem> _ListAttributes;
        public ObservableCollection<AttributeItem> ListAttributes
        {
            get { return _ListAttributes; }
            set
            {
                _ListAttributes = value;
                OnPropertyChanged("ListAttributes");
            }
        }

        public TableTemplate(string nameTable, ObservableCollection<AttributeItem> listAttributes)
        {
            NameTable = nameTable;
            ListAttributes = listAttributes;
        }

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
