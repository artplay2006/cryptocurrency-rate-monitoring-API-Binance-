using Newtonsoft.Json;
using System.Globalization;

namespace cryptocheck
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // Установка культуры вывода чисел
            CultureInfo culture = new CultureInfo("en-US");
            culture.NumberFormat.NumberDecimalSeparator = ".";

            // Применение установленной культуры
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
        double price;
        double checkprice;
        string condition = "", message = "";
        private async void button1_Click(object sender, EventArgs e)
        {
            string token = textBox1.Text;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    for (; ; )
                    {
                        HttpResponseMessage response = await client.GetAsync($"https://api.binance.com/api/v3/ticker/price?symbol={token}");
                        response.EnsureSuccessStatusCode();
                        string? json = await response.Content.ReadAsStringAsync();
                        Dictionary<string, object>? data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                        price = double.Parse(data["price"].ToString());

                        if (condition == "<=" && checkprice <= price)
                        {
                            System.Media.SystemSounds.Exclamation.Play();
                            MessageBox.Show($"Цена <= чем {checkprice}\n{message}");
                            condition = "";
                            message = "";
                            //break;
                        }
                        else if (condition == ">=" && checkprice >= price)
                        {
                            MessageBox.Show($"Цена >= чем {checkprice}\n{message}");
                            condition = "";
                            message = "";
                            //break;
                        }

                        textBox2.Text = data["price"].ToString();
                        //Task.Delay(500).Wait();
                    }
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show("Error fetching Bitcoin price: " + ex.Message);
                }
            }
            //});
        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkprice = double.Parse(textBox3.Text);
            condition = "<=";
            message = textBox4.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            checkprice = double.Parse(textBox3.Text);
            condition = ">=";
            message = textBox4.Text;
        }
    }
}