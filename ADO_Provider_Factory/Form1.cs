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
        DataTable DataViewManagerDT = null;
        string providerName = string.Empty;
        public Form1()
        {
            InitializeComponent();
            button2.Enabled = false;
            dataGridView1.AllowUserToAddRows = false;
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
            
                fact = DbProviderFactories.GetFactory(comboBox1.SelectedItem.ToString());
                conn = fact.CreateConnection();
                providerName = GetConnectionStringByProvider(comboBox1.SelectedItem.ToString());
                textBox1.Text = providerName;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            conn.ConnectionString = textBox1.Text;
            DbDataAdapter adapter = fact.CreateDataAdapter();
            adapter.SelectCommand = conn.CreateCommand();
            adapter.SelectCommand.CommandText = textBox2.Text;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt;
            DataViewManagerDT = dt;
           

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 6)
            {
                button2.Enabled = true;
            }
            else
                button2.Enabled = false;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var sign = (sender as DataGridView).CurrentRow.Cells[2].Value as string;
            if(sign!=null && sign.Contains("Data"))
            {
                fact = DbProviderFactories.GetFactory(sign);
                conn = fact.CreateConnection();
                providerName = GetConnectionStringByProvider(sign);
                textBox1.Text = providerName;
                textBox2.Text = "SELECT * FROM Persons";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(DataViewManagerDT);
            dataSet.Tables[0].TableName = "Persons";
            DataViewManager dvm = new DataViewManager(dataSet);
          //  dvm.DataViewSettings["Persons"].RowFilter = "ID < 3";
            dvm.DataViewSettings["Persons"].Sort = "age ASC";
            DataView dw = dvm.CreateDataView(dataSet.Tables["Persons"]);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dw;
        }
    }
}
