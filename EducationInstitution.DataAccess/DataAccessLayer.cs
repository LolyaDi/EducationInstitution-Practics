using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace EducationInstitution.DataAccess
{
    public class DataAccessLayer<T>
    {
        private readonly string _connectionString;
        private readonly string _providerName;
        private readonly DbProviderFactory _providerFactory;

        private DbConnection _connection;
        private DbDataAdapter _dataAdapter;

        public DataSet Dataset { get; set; }

        public DataAccessLayer()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["appConnectionString"].ConnectionString;
            _providerName = ConfigurationManager.ConnectionStrings["appConnectionString"].ProviderName;
            _providerFactory = DbProviderFactories.GetFactory(_providerName);
        }

        public void Connect()
        {
            _connection = _providerFactory.CreateConnection();
            _connection.ConnectionString = _connectionString;
            _connection.Open();
        }

        public void GainAccess(string tableName)
        {
            _dataAdapter = _providerFactory.CreateDataAdapter();

            var command = _connection.CreateCommand();
            command.CommandText = "select * from " + tableName;

            _dataAdapter.SelectCommand = command;

            var commandBuilder = _providerFactory.CreateCommandBuilder();
            commandBuilder.DataAdapter = _dataAdapter;

            _dataAdapter.Fill(Dataset, tableName);
        }

        public void UpdateData(string tableName)
        {
            _dataAdapter.Update(Dataset, tableName);
        }

        public void Disconnect()
        {
            _dataAdapter.Dispose();
            Dataset.Dispose();
            _connection.Dispose();
        }

        public void InsertRow(string tableName, T instance)
        {
            DataTable dataTable = Dataset.Tables[tableName];
            DataRow dataRow = dataTable.NewRow();

            Type type = instance.GetType();
            PropertyInfo[] propertyInfo = type.GetProperties();

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                dataRow[dataTable.Columns.Count - 1 - i] = propertyInfo[i].GetValue(instance);
            }

            dataTable.Rows.Add(dataRow);
        }

        public void UpdateRow(string tableName, T instance, int id)
        {
            DataTable dataTable = Dataset.Tables[tableName];
            DataRow dataRow = dataTable.Rows[id];

            Type type = instance.GetType();
            PropertyInfo[] propertyInfo = type.GetProperties();

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                dataRow[dataTable.Columns.Count - 1 - i] = propertyInfo[i].GetValue(instance);
            }
        }

        public void DeleteRow(string tableName, int id)
        {
            DataTable dataTable = Dataset.Tables[tableName];
            dataTable.Rows[id].Delete();
        }

        public List<T> SelectAllRows(string tableName)
        {
            List<T> instances = new List<T>();

            DataTable dataTable = Dataset.Tables[tableName];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                var instance = (T)Activator.CreateInstance(typeof(T));

                Type type = instance.GetType();
                PropertyInfo[] propertyInfo = type.GetProperties();

                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    propertyInfo[dataTable.Columns.Count - 1 - j].SetValue(instance, dataTable.Rows[i].ItemArray[j]);
                }

                instances.Add(instance);

            }

            return instances;
        }
    }
}