using DataBaseManager.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DataBaseManager.ViewModel
{
    //Класс ViewModel'а для панели инициализации

    public class InitialiaztionViewModel : INotifyPropertyChanged
    {
        public MainViewModel MainVM { get; set; }

        public ICommand toDataBasePageComamnd { get; set; }

        private string _Host;
        public string Host
        {
            get { return _Host; }
            set
            {
                _Host = value;
                OnPropertyChanged("Host");
            }
        }

        private string _User;
        public string User
        {
            get { return _User; }
            set
            {
                _User = value;
                OnPropertyChanged("User");
            }
        }

        private string _DBname;
        public string DBname
        {
            get { return _DBname; }
            set
            {
                _DBname = value;
                OnPropertyChanged("DBname");
            }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                OnPropertyChanged("Password");
            }
        }

        private int _Port;
        public int Port
        {
            get { return _Port; }
            set
            {
                _Port = value;
                OnPropertyChanged("Port");
            }
        }

        public InitialiaztionViewModel(MainViewModel mainVM)
        {
            Host = string.Empty;
            User = string.Empty;
            DBname = string.Empty;
            Password = string.Empty;

            MainVM = mainVM;
            toDataBasePageComamnd = new ToDataBasePageComamnd(mainVM);
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
