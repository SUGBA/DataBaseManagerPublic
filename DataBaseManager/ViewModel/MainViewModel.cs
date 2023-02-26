using DataBaseManager.Model;
using DataBaseManager.View;
using DataBaseManager.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DataBaseManager.ViewModel
{
    //Основной ViewModel, в котором описаны локальный ViewModel'и

    public class MainViewModel : INotifyPropertyChanged
    {
        //Инициализация/Редактор БД
        private Page _ActivePage;
        public Page ActivePage
        {
            get { return _ActivePage; }
            set
            {
                _ActivePage = value;
                OnPropertyChanged("ActivePage");
            }
        }

        public InitialiaztionViewModel InitialiaztionVM { get; set; }
        public DataBaseViewModel DataBaseVM { get; set; }

        public ICommand toInitialiaztionPageComamnd { get; set; }

        public MainViewModel()
        {
            InitialiaztionVM = new InitialiaztionViewModel(this);

            ActivePage = new InitializationPage(InitialiaztionVM);

            toInitialiaztionPageComamnd = new ToInitialiaztionPageComamnd(this);
        }

        #region CommandMethods
        public void ToInitialiaztionPage()
        {
            ActivePage = new InitializationPage(InitialiaztionVM);
        }
        public void ToDataBasePage(string path)
        {
            DataBaseVM = new DataBaseViewModel(path);
            ActivePage = new DataBasePage(DataBaseVM);
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
