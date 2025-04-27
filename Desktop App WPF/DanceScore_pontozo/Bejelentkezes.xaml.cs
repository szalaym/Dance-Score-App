using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;

namespace DanceScore_pontozo
{
    public partial class Bejelentkezes : Window
    {
        private readonly string kapcsolat = "server=localhost;port=3307;uid=root;password=;database=versenydb;ssl mode=none;";

        public Bejelentkezes()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void BejelentkezesButton_Click(object sender, RoutedEventArgs e)
        {
            string email = emailcim.Text;
            string jelszo = jelszok.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(jelszo))
            {
                MessageBox.Show("Kérlek, add meg az email címet és a jelszót!");
                return;
            }

            // Jelszó titkosítása (ugyanazt a titkosítási logikát használjuk, mint a backend)
            string titkosítottJelszo = JelszoTitkositas(jelszo);

            // Bíró azonosítójának lekérdezése az adatbázisból
            int biroId = 0;
            using (MySqlConnection con = new MySqlConnection(kapcsolat))
            {
                con.Open();
                string sql = "SELECT `Id` FROM `birok` WHERE `Email` = @email AND `Jelszo` = @jelszo";
                using (MySqlCommand msqlc = new MySqlCommand(sql, con))
                {
                    msqlc.Parameters.AddWithValue("@email", email);
                    msqlc.Parameters.AddWithValue("@jelszo", titkosítottJelszo);
                    using (MySqlDataReader msdr = msqlc.ExecuteReader())
                    {
                        if (msdr.Read())
                        {
                            biroId = msdr.GetInt32(0);
                        }
                    }
                }
            }

            if (biroId == 0)
            {
                MessageBox.Show("Hibás email cím vagy jelszó!");
                return;
            }

            // Sikeres bejelentkezés, megnyitjuk a Pontozas ablakot
            Pontozas pontozasAblak = new Pontozas(biroId);
            pontozasAblak.Show();
            this.Close();
        }

        private string JelszoTitkositas(string jelszo)
        {
            if (string.IsNullOrWhiteSpace(jelszo))
            {
                throw new ArgumentException("A jelszó nem lehet üres!");
            }

            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(jelszo));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}