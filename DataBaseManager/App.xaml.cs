using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace DataBaseManager
{
    public partial class App : Application
    {
        //Метод обрабатывающий исключения и не дающий приложению закрыться из-за exception
        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Process unhandled exception
            MessageBox.Show(e.Exception.Message);
            // Prevent default unhandled exception processing
            e.Handled = true;
        }
    }
}
