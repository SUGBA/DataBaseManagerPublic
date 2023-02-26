using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseManager.Model
{
    public class AttributeItem : INotifyPropertyChanged
    {
        /*
         * IsPrimaryKey Свойство характеризующее является ли аттрибут первичынм ключом
         * TypeItem Свойство характеризующее является ли аттрибут первичынм ключом
         * NameItem Свойство характеризующее является ли аттрибут первичынм ключом
         */

        private bool _IsPrimaryKey;
        public bool IsPrimaryKey
        {
            get { return _IsPrimaryKey; }
            set
            {
                _IsPrimaryKey = value;
                OnPropertyChanged("IsPrimaryKey");
            }
        }

        private string _TypeItem;
        public string TypeItem
        {
            get { return _TypeItem; }
            set
            {
                _TypeItem = value;
                OnPropertyChanged("TypeItem");
            }
        }

        private string _NameItem;
        public string NameItem
        {
            get { return _NameItem; }
            set
            {
                _NameItem = value;
                OnPropertyChanged("NameItem");
            }
        }


        public AttributeItem(string NameItem, string TypeItem, bool IsPrimaryKey)
        {
            this.TypeItem = TypeItem;
            this.NameItem = NameItem;
            this.IsPrimaryKey = IsPrimaryKey;
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
