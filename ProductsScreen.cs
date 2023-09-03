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
        private int lastSelectedProductKey;

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

            string query = string.Format(provider, "insert into Products values('{0}','{1}','{2}', {3})", productName, productBrand, productCategory, productPrice);
            ExecuteQuery(query);


            ClearAllFields();
            ShowProducts();

        }


        private void btnProductEdit_Click(object sender, EventArgs e)
        {
            if (lastSelectedProductKey == 0)
            {
                MessageBox.Show("Bitte wähle zuerst ein Produkt aus.");
                return;
            }

            IFormatProvider provider = CultureInfo.CreateSpecificCulture("en-GB");

            string productName = textBoxProductName.Text;
            string productBrand = textBoxProductBrand.Text;
            string productCategory = comboBoxProductCategory.Text;
            float productPrice = float.Parse(textBoxProductPrice.Text.Replace(",", "."), provider);

            string query = string.Format(provider, "update Products set Name ='{0}', Brand = '{1}', Category = '{2}', Price = '{3}' where ID = {4}", 
                productName, productBrand, productCategory, productPrice, lastSelectedProductKey); 
            
            ExecuteQuery(query);

            ShowProducts();
        }


        private void btnProductClear_Click(object sender, EventArgs e)
        {
            ClearAllFields();
        }


        private void btnProductDelete_Click(object sender, EventArgs e)
        {
            if(lastSelectedProductKey == 0) 
            {
                MessageBox.Show("Bitte wähle zuerst ein Produkt aus.");
                return;
            }

            string query = string.Format("delete from Products where Id = {0};", lastSelectedProductKey);
            ExecuteQuery(query);

            ClearAllFields();
            ShowProducts();
        }


        private void ExecuteQuery(string query)
        {
            databaseConnection.Open();
            
            SqlCommand sqlCommand = new SqlCommand(query, databaseConnection);
            sqlCommand.ExecuteNonQuery();
            databaseConnection.Close();
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


        private void productsDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBoxProductName.Text = productsDGV.SelectedRows[0].Cells[1].Value.ToString();
            textBoxProductBrand.Text = productsDGV.SelectedRows[0].Cells[2].Value.ToString();
            comboBoxProductCategory.Text = productsDGV.SelectedRows[0].Cells[3].Value.ToString();
            textBoxProductPrice.Text = productsDGV.SelectedRows[0].Cells[4].Value.ToString();

            lastSelectedProductKey = (int)productsDGV.SelectedRows[0].Cells[0].Value;

        }





        // --------------------------------------------------------------------------------------------------------------
        // Bis hier hin ging das Tut

        private void backToMenu_Click(object sender, EventArgs e)
        {
            MainMenuScreen mainMenuScreen = new MainMenuScreen();
            mainMenuScreen.Show(); 

            this.Close(); // Ich wusste nicht ob es so Funktioniert wie ich es mir Vorstelle, aber anscheinend geht das so ganz gut

            
            
        }

    }
}
