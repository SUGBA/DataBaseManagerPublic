using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseManager.Model
{
    public static class DataBase
    {
        #region CheckConnection
        //Если удалось связаться с БД возвращает true в противном случае false
        public static bool CheckConnection(string path)    
        {
            try
            {
                using (var connection = new NpgsqlConnection(path))
                {
                    connection.Open();
                    connection.Close();
                }
                return true;
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Failed to connect to the database");
                return false;
            }
        }
        #endregion

        #region AddColumns
        //Добавляет новый аттрибут к выбранной таблице
        public static void AddColumns(NpgsqlConnection connection, ObservableCollection<TableTemplate> ListTables,
           string TableName, AttributeItem NewColumn)
        {
            //Проверяем, что таблица существует и что в таблице еще не существует атрибута с таким же именем
            if (CheckContainsTable(ListTables, TableName) && !CheckContainsTable(ListTables, TableName, NewColumn.NameItem))
            {
                string SQLCommand;
                string PrimaryKeyValue = NewColumn.IsPrimaryKey ? "PRIMARY KEY" : string.Empty;
                //Если мы добавляем новый первичнй ключ и в таблице уже есть первичный ключ
                if (NewColumn.IsPrimaryKey && GetConstraintName(connection, ListTables, TableName) != string.Empty)
                    //Удаляем прошлый первичнйы клю
                    RemovePrimaryKey(connection, ListTables, TableName);

                SQLCommand = $"ALTER TABLE {TableName} " +
                             $"ADD {NewColumn.NameItem} {NewColumn.TypeItem}";

                using (var command = new NpgsqlCommand(SQLCommand, connection))
                {
                    command.ExecuteNonQuery();
                }
                AddPrimaryKey(connection, ListTables, TableName, NewColumn.NameItem);
                //Добавляем новый аттрибут в список
                ListTables.Where(x => x.NameTable == TableName).Single().ListAttributes.Add(NewColumn);
            }
        }
        #endregion

        #region RemoveColumn
        //Метод удаляющий выбранный аттрибут
        public static void RemoveColumn(NpgsqlConnection connection, ObservableCollection<TableTemplate> ListTables,
           string TableName, string ColumnName)
        {
            //Если существует таблица и в ней содержистя выбранный аттриубт
            if (CheckContainsTable(ListTables, TableName, ColumnName))
            {
                string SQLCommand = $"ALTER TABLE {TableName} " +
                                    $"DROP COLUMN {ColumnName}";
                using (var command = new NpgsqlCommand(SQLCommand, connection))
                {
                    command.ExecuteNonQuery();
                }
                //Удаляем аттрибут из списка
                ListTables.Where(x => x.NameTable == TableName).Single().ListAttributes.Remove(
                    ListTables.Where(x => x.NameTable == TableName).Single().ListAttributes.
                    Where(x => x.NameItem == ColumnName).Single());
            }
        }
        #endregion

        #region ChangePrimaryKey
        //Метод удаляющий первичный ключ
        public static void RemovePrimaryKey(NpgsqlConnection connection, ObservableCollection<TableTemplate> ListTables,
            string TableName)
        {
            //Получаем constraint_name выбранной таблицы
            string ConstraintName = GetConstraintName(connection, ListTables, TableName);

            //Если существует таблица и существуют первичные ключи
            if (CheckContainsTable(ListTables, TableName) && ConstraintName != string.Empty)
            {
                string SQLCommand = $"ALTER TABLE {TableName} DROP CONSTRAINT {ConstraintName};";
                using (var command = new NpgsqlCommand(SQLCommand, connection))
                {
                    command.ExecuteNonQuery();
                }
                //Корректируем список убирая сведения о первичном ключе
                ListTables.Where(x => x.NameTable == TableName).Single().ListAttributes.Where(y => y.IsPrimaryKey).Single().IsPrimaryKey = false;
            }
        }
        //Добавление первичного ключа
        public static void AddPrimaryKey(NpgsqlConnection connection, ObservableCollection<TableTemplate> ListTables,
           string TableName, string ColumnName)
        {
            //Получаем constraint_name выбранной таблицы
            string ConstraintName = GetConstraintName(connection, ListTables, TableName);
            //Если существует таблица с ключом и у нее имеется первичный ключ
            if (CheckContainsTable(ListTables, TableName, ColumnName) && ConstraintName == string.Empty)
            {
                ConstraintName = $"{TableName}_pkey";
                string SQLCommand = $"ALTER TABLE {TableName} " +
                                    $"ADD CONSTRAINT {ConstraintName} " +
                                    $"PRIMARY KEY({ColumnName});";
                using (var command = new NpgsqlCommand(SQLCommand, connection))
                {
                    command.ExecuteNonQuery();
                }
                //Записываем, что аттриубт является первичным ключом
                ListTables.Where(x => x.NameTable == TableName).Single().ListAttributes.Where(y => y.NameItem == ColumnName).Single().IsPrimaryKey = true;

            }
        }
        //Метод, получающий constraint_name
        private static string GetConstraintName(NpgsqlConnection connection, ObservableCollection<TableTemplate> ListTables, string TableName)
        {
            //Дефолтное значение возвращаемое в случае, если первичного ключа нету
            string result = string.Empty;
            //Если таблица существует
            if (CheckContainsTable(ListTables, TableName))
            {
                string SQLCommand = $"SELECT constraint_name " +
                                    $"FROM information_schema.table_constraints " +
                                    $"WHERE table_name = '{TableName}' AND constraint_type = 'PRIMARY KEY'";
                using (var command = new NpgsqlCommand(SQLCommand, connection))
                {
                    var reader = command.ExecuteReader();
                    
                    //Записываем список первичных ключей
                    while (reader.Read())
                        result += reader.GetString(0);

                    reader.Close();
                }
            }
            return result;
        }
        #endregion

        #region ChangeNameTable
        //Метод, изменяющий имя таблицы
        public static void ChangeNameTable(NpgsqlConnection connection, ObservableCollection<TableTemplate> ListTables,
            string TableNameBefore, string TableNameAfter)
        {
            //Если таблица существует
            if (CheckContainsTable(ListTables, TableNameBefore))
            {
                string SQLCommand = $"ALTER TABLE {TableNameBefore} RENAME TO {TableNameAfter}";
                using (var command = new NpgsqlCommand(SQLCommand, connection))
                {
                    command.ExecuteNonQuery();
                }
                //Меняем имя таблицы в списке
                ListTables.Where(x => x.NameTable == TableNameBefore).Single().NameTable = TableNameAfter;
            }
        }
        #endregion

        #region RemoveTable
        //Метод, удаляющий таблицу из списка
        public static void RemoveTable(NpgsqlConnection connection, ObservableCollection<TableTemplate> ListTables,
            string NameTable)
        {
            //Если таблица существует
            if (CheckContainsTable(ListTables, NameTable))
            {
                string SQLCommand = $"DROP TABLE {NameTable}";

                using (var command = new NpgsqlCommand(SQLCommand, connection))
                {
                    command.ExecuteNonQuery();
                }
                //Удаляем таблицу из списка
                ListTables.Remove(ListTables.Where(x => x.NameTable == NameTable).Single());
            }
        }
        #endregion

        #region CreateTable
        //Метод, добавляющий пустую таблицу имеющую только имя
        public static void CreateTable(NpgsqlConnection connection, ObservableCollection<TableTemplate> ListTables, string NameTable)
        {
            //Если таблица существуует
            if (!CheckContainsTable(ListTables, NameTable))
            {
                string SQLCommand = $"CREATE TABLE {NameTable}();";

                using (var command = new NpgsqlCommand(SQLCommand, connection))
                {
                    command.ExecuteNonQuery();
                }
                //Добавляем таблицу в список 
                ListTables.Add(new TableTemplate(NameTable, (new ObservableCollection<AttributeItem>())));
            }
        }
        //Метод, добавляющий таблицу с аттрибутами
        public static void CreateTable(NpgsqlConnection connection, ObservableCollection<TableTemplate> ListTables, TableTemplate table)
        {
            if (!CheckContainsTable(ListTables, table.NameTable))
            {
                string SQLCommand = GenerateSQLCommand(table);

                using (var command = new NpgsqlCommand(SQLCommand, connection))
                {
                    command.ExecuteNonQuery();
                }
                ListTables.Add(table);
            }
        }
        //Метод, генерирующий запрос, используется если передается множество аттрибутов
        private static string GenerateSQLCommand(TableTemplate table)
        {
            string result = $"CREATE TABLE {table.NameTable} (";
            foreach (var item in table.ListAttributes)
            {
                if (item.IsPrimaryKey)
                    result += $" {item.NameItem} {item.TypeItem} PRIMARY KEY,";
                else
                    result += $" {item.NameItem} {item.TypeItem},";
            }
            //Удаляем последнюю запятую и закрываем запрос
            result = result.Remove(result.Length - 1) + ");";

            return result;
        }

        #endregion

        #region GetListTables
        // GetListTables -> GetNamesTables -> GetListAttributes -> GetListPrimaryKeys

        //Метод, получающий список таблиц
        public static ObservableCollection<TableTemplate> GetListTables(NpgsqlConnection connection)
        {
            var ListTables = new ObservableCollection<TableTemplate>();
            //Проходимся по списку имен таблиц
            foreach (var item in GetNamesTables(connection))
            {
                ListTables.Add(new TableTemplate(item, GetListAttributes(connection, item)));
            }

            return ListTables;
        }
        //Метод, получающий список доступных пользователю таблицы
        private static List<string> GetNamesTables(NpgsqlConnection connection)
        {
            var NameColumns = new List<string>();
            string SQLCommand = "SELECT table_name " +
                                "FROM information_schema.tables " +
                                "WHERE table_schema='public'";

            using (var Command = new NpgsqlCommand(SQLCommand, connection))
            {
                var reader = Command.ExecuteReader();

                while (reader.Read())
                    NameColumns.Add(reader.GetString(0));

                reader.Close();
            }

            return NameColumns;
        }
        //Метод, получающий список аттрибутов (Отдельно получаем список аттрибутов, являющихся первичным ключом и список всех аттрибутов с типами)
        private static ObservableCollection<AttributeItem> GetListAttributes(NpgsqlConnection connection, string NameTable)
        {
            var listPrimaryKeys = GetListPrimaryKey(connection, NameTable);
            var result = new ObservableCollection<AttributeItem>();
            string SQLCommand = $"SELECT column_name, data_type " +
                                $"FROM information_schema.columns " +
                                $"WHERE table_name = '{NameTable}'";

            using (var Command = new NpgsqlCommand(SQLCommand, connection))
            {
                var reader = Command.ExecuteReader();

                while (reader.Read())
                {
                    //Если элемент содержится в списке первичных ключей записыаем его как первичнй ключ, иначе записываем как обычный аттриубт
                    if (listPrimaryKeys.Contains(reader.GetString(0)))
                        result.Add(new AttributeItem(reader.GetString(0), reader.GetString(1), true));
                    else
                        result.Add(new AttributeItem(reader.GetString(0), reader.GetString(1), false));
                }
                reader.Close();
            }
            return result;
        }
        //Метод, получающий список первичных ключей таблицы
        private static List<string> GetListPrimaryKey(NpgsqlConnection connection, string Nametable)
        {
            var result = new List<string>();
            string SQLCommand = $"SELECT c.column_name " +
                                $"FROM information_schema.key_column_usage AS c " +
                                $"LEFT JOIN information_schema.table_constraints AS t " +
                                $"ON t.constraint_name = c.constraint_name " +
                                $"WHERE t.table_name = '{Nametable}' AND t.constraint_type = 'PRIMARY KEY';";

            using (var Command = new NpgsqlCommand(SQLCommand, connection))
            {
                var reader = Command.ExecuteReader();

                while (reader.Read())
                    result.Add(reader.GetString(0));

                reader.Close();
            }
            return result;
        }
        #endregion

        #region CheckContains
        //Проверка существования таблицы
        private static bool CheckContainsTable(ObservableCollection<TableTemplate> ListTables, string TableName)
            => ListTables.Any(item => item.NameTable == TableName);
        //Проверка существования таблицы и аттрибута в ней
        private static bool CheckContainsTable(ObservableCollection<TableTemplate> ListTables, string TableName, string ColumnName)
            => ListTables.Any(item => item.NameTable == TableName && item.ListAttributes.Any(atr => atr.NameItem == ColumnName));
        #endregion
    }
}
