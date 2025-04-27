using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace DanceScore_pontozo
{
    /// <summary>
    /// Interaction logic for Regisztracio.xaml
    /// </summary>
    public partial class Regisztracio : Window
    {
        string kapcsolat = "server=localhost:3307,;uid=adminka;password=1234;database=verseny_db;ssl mode=none";
        public Regisztracio()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void regisztralt_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection con = new MySqlConnection(kapcsolat);
            con.Open();
            //Az adatok átvitele az adatbázisba
            string sql = $"INSERT INTO `regisztralo`(`email`, `jelszo`) VALUES ('{emailcim.Text}','{jelszo.Password}')";
            MySqlCommand msqlc = new MySqlCommand(sql, con);
            msqlc.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Sikeres regisztráció");




        }

        private void regisztracio_Click(object sender, RoutedEventArgs e)
        {
            Bejelentkezes uj = new Bejelentkezes();
            uj.Show();
            this.Close();
        }
    }
}
