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
using System.Windows.Media.Animation;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DanceScore_pontozo
{
    public partial class Pontozas : Window
    {
        private readonly string kapcsolat = "server=localhost;port=3307;uid=root;password=;database=versenydb;ssl mode=none;";
        private readonly HttpClient httpClient;
        private readonly string apiBaseUrl = "https://localhost:44333/api/";
        private readonly int biroId;

        public Pontozas(int biroId)
        {
            InitializeComponent();
            this.biroId = biroId;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            ComboVersenyekFel();
        }

        public void ComboVersenyekFel()
        {
            using (MySqlConnection con = new MySqlConnection(kapcsolat))
            {
                con.Open();
                string sql = "SELECT `Nev` FROM `versenyek` ORDER BY `Nev`;";
                using (MySqlCommand msqlc = new MySqlCommand(sql, con))
                {
                    using (MySqlDataReader msdr = msqlc.ExecuteReader())
                    {
                        verseny.Items.Clear();
                        while (msdr.Read())
                        {
                            verseny.Items.Add(msdr.GetString(0));
                        }
                    }
                }
            }
        }

        private async void Verseny_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            kategoria.Items.Clear();
            csapatok.Items.Clear();

            if (verseny.SelectedItem == null)
            {
                return;
            }

            string kivalasztottVerseny = verseny.SelectedItem.ToString();

            try
            {
                var response = await httpClient.GetAsync($"{apiBaseUrl}Nevezes?verseny={Uri.EscapeDataString(kivalasztottVerseny)}");
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var nevezesek = JsonConvert.DeserializeObject<List<NevezesDto>>(responseContent);

                    if (nevezesek == null || !nevezesek.Any())
                    {
                        MessageBox.Show($"Nincsenek nevezések a '{kivalasztottVerseny}' versenyhez!", "Figyelmeztetés", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var kategoriak = nevezesek.Select(n => n.Kategoria).Distinct().Where(k => k != "Nincs kategória").ToList();
                    foreach (var kategoriaNev in kategoriak)
                    {
                        kategoria.Items.Add(kategoriaNev);
                    }

                    if (kategoriak.Count == 0)
                    {
                        MessageBox.Show($"Nincs kategória a '{kivalasztottVerseny}' versenyhez!", "Figyelmeztetés", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Hiba történt a kategóriák lekérdezése közben: {response.StatusCode} - {errorContent}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt a kérés küldése közben: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Kategoria_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            csapatok.Items.Clear();

            if (verseny.SelectedItem == null || kategoria.SelectedItem == null)
            {
                return;
            }

            string kivalasztottVerseny = verseny.SelectedItem.ToString();
            string kivalasztottKategoria = kategoria.SelectedItem.ToString();

            try
            {
                var response = await httpClient.GetAsync($"{apiBaseUrl}Nevezes?verseny={Uri.EscapeDataString(kivalasztottVerseny)}&kategoria={Uri.EscapeDataString(kivalasztottKategoria)}");
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var nevezesek = JsonConvert.DeserializeObject<List<NevezesDto>>(responseContent);

                    if (nevezesek == null || !nevezesek.Any())
                    {
                        MessageBox.Show($"Nincsenek nevezések a '{kivalasztottVerseny}' verseny '{kivalasztottKategoria}' kategóriájához! Válasz: {responseContent}", "Figyelmeztetés", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var csapat = nevezesek.Select(n => n.CsapatNev).Distinct().ToList();
                    foreach (var csapatNev in csapat)
                    {
                        csapatok.Items.Add(csapatNev);
                    }

                    if (csapat.Count == 0)
                    {
                        MessageBox.Show($"Nincs nevezett csapat a '{kivalasztottVerseny}' verseny '{kivalasztottKategoria}' kategóriájához! Válasz: {responseContent}", "Figyelmeztetés", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Hiba történt a csapatok lekérdezése közben: {response.StatusCode} - {errorContent}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt a kérés küldése közben: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void bekuldButton_Click(object sender, RoutedEventArgs e)
        {
            if (verseny.SelectedItem == null || kategoria.SelectedItem == null || csapatok.SelectedItem == null)
            {
                MessageBox.Show("Kérlek, válaszd ki a versenyt, kategóriát és csapatot!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(ruhazat.Text, out int ruhazatPont) || ruhazatPont < 1 || ruhazatPont > 10)
            {
                MessageBox.Show("Kérlek, adj meg érvényes pontszámot a Ruházat szempontnak (1-10 között)!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(szinkron.Text, out int szinkronPont) || szinkronPont < 1 || szinkronPont > 10)
            {
                MessageBox.Show("Kérlek, adj meg érvényes pontszámot a Szinkron szempontnak (1-10 között)!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(muvhatas.Text, out int muvesziHatasPont) || muvesziHatasPont < 1 || muvesziHatasPont > 10)
            {
                MessageBox.Show("Kérlek, adj meg érvényes pontszámot a Művészi hatás szempontnak (1-10 között)!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(eloadasmod.Text, out int eloadasiModPont) || eloadasiModPont < 1 || eloadasiModPont > 10)
            {
                MessageBox.Show("Kérlek, adj meg érvényes pontszámot az Előadási mód szempontnak (1-10 között)!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(technika.Text, out int technikaPont) || technikaPont < 1 || technikaPont > 10)
            {
                MessageBox.Show("Kérlek, adj meg érvényes pontszámot a Technika szempontnak (1-10 között)!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int osszPontszam = ruhazatPont + szinkronPont + muvesziHatasPont + eloadasiModPont + technikaPont;

            int nevezesId = 0;
            try
            {
                string kivalasztottVerseny = verseny.SelectedItem.ToString();
                string kivalasztottKategoria = kategoria.SelectedItem.ToString();
                string kivalasztottCsapat = csapatok.SelectedItem.ToString();

                var response = await httpClient.GetAsync($"{apiBaseUrl}Nevezes?verseny={Uri.EscapeDataString(kivalasztottVerseny)}&kategoria={Uri.EscapeDataString(kivalasztottKategoria)}");
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var nevezesek = JsonConvert.DeserializeObject<List<NevezesDto>>(responseContent);

                    if (nevezesek == null || !nevezesek.Any())
                    {
                        MessageBox.Show($"Nem található nevezés a megadott feltételekkel! Válasz: {responseContent}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var nevezes = nevezesek.FirstOrDefault(n => n.CsapatNev == kivalasztottCsapat);
                    if (nevezes != null)
                    {
                        nevezesId = nevezes.Id;
                    }
                    else
                    {
                        MessageBox.Show($"Nem található nevezés a '{kivalasztottCsapat}' csapathoz! Válasz: {responseContent}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Hiba történt a nevezés azonosítójának lekérdezése közben: {response.StatusCode} - {errorContent}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt a nevezés azonosítójának lekérdezése közben: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (nevezesId == 0)
            {
                MessageBox.Show("Nem található nevezés a megadott feltételekkel!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Hozzunk létre egy EredmenyDto objektumot, amely pontosan megfelel a backend elvárásainak
            var eredmenyDto = new
            {
                NevezesId = nevezesId,
                BiroId = biroId,
                Pontszam = osszPontszam,
                Rogzitve = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") // ISO 8601 formátum
            };

            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(eredmenyDto), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync($"{apiBaseUrl}Eredmeny", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Pontozás sikeresen rögzítve!", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                    // Töröljük a beviteli mezők tartalmát
                    ruhazat.Text = "";
                    szinkron.Text = "";
                    muvhatas.Text = "";
                    eloadasmod.Text = "";
                    technika.Text = "";
                }
                else
                {
                    MessageBox.Show($"Hiba történt a pontozás rögzítése közben: {response.StatusCode} - {responseContent}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt a kérés küldése közben: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class NevezesDto
    {
        public int Id { get; set; }
        public string Verseny { get; set; }
        public DateTime VersenyIdopont { get; set; }
        public string Kategoria { get; set; }
        public int CsapatId { get; set; }
        public string CsapatNev { get; set; }
        public int? Pontszam { get; set; }
        public DateTime? Rogzitve { get; set; }
        public int PontozasokSzama { get; set; }
        public string PontozasHiany { get; set; }
    }
}