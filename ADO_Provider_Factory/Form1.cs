using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;

namespace ADO_Provider_Factory
{
    public partial class Form1 : Form
    {
        DbConnection conn = null;
        DbProviderFactory fact = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = DbProviderFactories.GetFactoryClasses();
            dataGridView1.DataSource = dt;
            comboBox1.Items.Clear();
            dataGridView1.AutoResizeColumn(0);
            foreach (DataRow dr in dt.Rows)
            {
                comboBox1.Items.Add(dr["InvariantName"]);
            }
        }
        static string GetConnectionStringByProvider(string providerName)
        {
            string returnValue = null;
            // читаем все строки подключения из App.config
            ConnectionStringSettingsCollection settings =
            ConfigurationManager.ConnectionStrings;
            // ищем и возвращаем строку подключения 
            // для providerName
            if (settings != null)
            {
                foreach (ConnectionStringSettings cs in settings)
                {
                    if (cs.ProviderName == providerName)
                    {
                        returnValue = cs.ConnectionString;
                        break;
                    }
                }
            }
            return returnValue;
        }
        /// <summary>
        /// имея в свом распоряжении фабрику для выбранного
        /// поставщика выполняем стандартные действия с БД
        /// для демонстрации работоспособности кода
        /// </summary>
        /// <param name="sender"></param name>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            {
                fact = DbProviderFactories.GetFactory(comboBox1.
                SelectedItem.ToString());
                conn = fact.CreateConnection();
                providerName =
                GetConnectionStringByProvider(comboBox1.
                SelectedItem.ToString());
                textBox1.Text = providerName;
        }
    }
}
