using DataBaseManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataBaseManager.View
{
    /// <summary>
    /// Логика взаимодействия для InputNewName.xaml
    /// </summary>
    public partial class InputNewName : Page
    {
        public InputNewName(PanelDatabaseViewModel VM)
        {
            this.DataContext = VM;
            InitializeComponent();
        }
    }
}
