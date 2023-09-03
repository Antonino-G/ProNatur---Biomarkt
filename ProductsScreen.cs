using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProNatur_Biomarkt_GmbH
{
    public partial class ProductsScreen : Form
    {
        private SqlConnection databaseConnection = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=C:\Users\nino-\Documents\Pro-Natur Biomarkt GmbH.mdf;Integrated Security = True; Connect Timeout = 30");

        public ProductsScreen()
        {
            InitializeComponent();

            ShowProducts();


        }

        

        private void btnProductSave_Click(object sender, EventArgs e)
        {
 
            if(textBoxProductName.Text == "" 
                || textBoxProductBrand.Text == ""
                || comboBoxProductCategory.Text == ""
                || textBoxProductPrice.Text == "")
            {
                MessageBox.Show("Bitte fülle alle Felder aus");

                return;
            }

            IFormatProvider provider = CultureInfo.CreateSpecificCulture("en-GB");

            // save Produkt Name in database
            string productName = textBoxProductName.Text;
            string productBrand = textBoxProductBrand.Text;
            string productCategory = comboBoxProductCategory.Text;
            // float productPrice = float.Parse(textBoxProductPrice.Text);  Geht nicht , da er das Komma nicht mit nimmt
            // string productPrice = textBoxProductPrice.Text; Kann mir vorstellen das es später dann zum Problem bei der Kalkulation wird
            float productPrice = float.Parse(textBoxProductPrice.Text.Replace(",", "."), provider);

            databaseConnection.Open();
            string query = string.Format(provider, "insert into Products values('{0}','{1}','{2}', {3})", productName, productBrand, productCategory, productPrice);
            SqlCommand sqlCommand = new SqlCommand(query, databaseConnection);
            sqlCommand.ExecuteNonQuery();
            databaseConnection.Close();

            ClearAllFields();
            ShowProducts();

        }

        private void btnProductEdit_Click(object sender, EventArgs e)
        {

            ShowProducts();
        }

        private void btnProductClear_Click(object sender, EventArgs e)
        {
            ClearAllFields();
        }

        private void btnProductDelete_Click(object sender, EventArgs e)
        {

            ShowProducts();
        }

        private void ShowProducts()
        {
            databaseConnection.Open();

            string query = "select * from Products";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, databaseConnection);

            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);

            productsDGV.DataSource = dataSet.Tables[0];

            productsDGV.Columns[0].Visible = false;

            databaseConnection.Close();
        }

        private void ClearAllFields()
        {
            textBoxProductName.Text = "";
            textBoxProductBrand.Text = "";
            textBoxProductPrice.Text = "";
            comboBoxProductCategory.Text = "";
            comboBoxProductCategory.SelectedItem = null;
        }
    }
}
