using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseManager.ViewModel.Core
{
    public enum ListMods : byte
    {
        //Enum описывающий список модификаций 
        ReName,             //Перименование таблицы
        AddColumn,          //Добавление аттрибута
        RemoveTable,        //Удаление таблицы
        Refresh,            //Обновление списка таблицы
        CreateTable,        //Создание таблицы
        RemoveColumn,       //Удаление таблицы
        RemovePrimaryKey,   //Удаление первичного ключа
        AddPrimaryKey       //Добавление первичного ключа
    }
}
