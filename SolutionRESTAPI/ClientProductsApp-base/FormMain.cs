using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO; //Stream
using System.Linq;
using System.Net; //HttpWebRequest
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using RestSharp;

//JavaScriptSerializer --> necessário criar referencia para System.Web.Extensions caso pretendam usar para serializar objetos em JSON

namespace ClientProductsApp
{
    public partial class FormMain : Form
    {

        string baseURI = @"http://localhost:54290/"; //TODO: needs to be updated!
        RestClient client = null;

        public FormMain()
        {
            InitializeComponent();
            client = new RestClient(baseURI);
        }

        private void buttonGetAll_Click(object sender, EventArgs e)
        {
            var request = new RestRequest("api/products", Method.Get);
            request.RequestFormat = DataFormat.Json;

            var response = client.Execute<List<Product>>(request).Data;

            richTextBoxShowProducts.Clear();

            foreach (Product item in response)
            {
                string prod = $"{item.Id}: {item.Name}\t{item.Category}\n";
                richTextBoxShowProducts.AppendText(prod);
            }

        }

        private void textBoxOutput_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBoxShowProducts_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonGetProductById_Click(object sender, EventArgs e)
        {
            var request = new RestRequest("api/products/{id}", Method.Get);
            request.AddUrlSegment("id", textBoxFilterById.Text);
            request.RequestFormat = DataFormat.Json;

            var response = client.Execute<Product>(request).Data;

            string prod = $"{response.Id}:  {response.Name}\t{response.Category}\t{response.Price}";
            textBoxOutput.Text = prod;
        }

        private void buttonPost_Click(object sender, EventArgs e)
        {
            Product product = new Product
            {
                Name = textBoxName.Text,
                Category = textBoxCategory.Text,
                Price = Convert.ToDecimal(textBoxPrice.Text),   
            };

            var request = new RestRequest("api/products", Method.Post);
            request.RequestFormat = DataFormat.Json;
            request.AddObject(product);

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                MessageBox.Show("Produto adicionado");
            }
            else
                MessageBox.Show("Nao foi possivel adicionar o produto");
        }

        private void buttonPut_Click(object sender, EventArgs e)
        {
            Product product = new Product
            {
                Name = textBoxName.Text,
                Category = textBoxCategory.Text,
                Price = Convert.ToDecimal(textBoxPrice.Text),
            };

            var request = new RestRequest("api/products/{id}", Method.Put);
            request.AddUrlSegment("id", textBoxID.Text);
            request.RequestFormat = DataFormat.Json;
            request.AddObject(product);

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                MessageBox.Show("Produto alterado");
            }
            else
                MessageBox.Show("Nao foi possivel alterar o produto");
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            var request = new RestRequest("api/products/{id}", Method.Delete);
            request.AddUrlSegment("id", textBoxID.Text);

            var response = client.Execute(request);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                MessageBox.Show("Produto Eliminado");
            }
            else
                MessageBox.Show("Produto não foi eliminado");
        }
    }
}
