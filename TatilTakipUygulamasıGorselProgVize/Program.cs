using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PublicHolidayTracker
{
    // Ödev metninde istenen sınıf yapısı birebir korundu.
    class Holiday
    {
        // JSON'dan gelen verileri eşleştirmek için property isimleri korundu
        public string date { get; set; }
        public string localName { get; set; }
        public string name { get; set; }
        public string countryCode { get; set; }
        public bool @fixed { get; set; } // 'fixed' C# keyword'ü olduğu için @ eklendi
        public bool global { get; set; }

        // Listeleme yaparken kolaylık olması için formatlı yazı döndüren metot
        public string GetFormattedString()
        {
            // Tarihi YYYY-MM-DD formatından gün.ay.yıl formatına çeviriyoruz görsellik için
            if (DateTime.TryParse(this.date, out DateTime parsedDate))
            {
                return $"{parsedDate:dd.MM.yyyy} - {this.localName} ({this.name})";
            }
            return $"{this.date} - {this.localName}";
        }
    }

    class Program
    {
        // API bağlantısı için client
        private static readonly HttpClient _client = new HttpClient();
        
        // Verileri hafızada tutacağımız liste
        private static List<Holiday> _holidayCache = new List<Holiday>();
        
        // Çekilecek yıllar
        private static readonly List<int> _yearsToFetch = new List<int> { 2023, 2024, 2025 };

        static async Task Main(string[] args)
        {
            Console.Title = "Public Holiday Tracker - Resmi Tatil Takip Sistemi";
            Console.WriteLine("Veriler API üzerinden yükleniyor, lütfen bekleyiniz...");

            // 1. Adım: Verileri Çek
            await LoadHolidaysAsync();

            if (_holidayCache.Count == 0)
            {
                Console.WriteLine("Hata: Veriler sunucudan çekilemedi. Program sonlandırılıyor.");
                return;
            }

            Console.Clear();
            Console.WriteLine($"Veri tabanı güncellendi. Toplam {_holidayCache.Count} tatil kaydı hafızaya alındı.");
            
            // 2. Adım: Menüyü Başlat
            await RunUserInterface();
        }

        /// <summary>
        /// Belirlenen yıllar için API'den verileri çeker ve listeye ekler.
        /// </summary>
        private static async Task LoadHolidaysAsync()
        {
            foreach (int year in _yearsToFetch)
            {
                try
                {
                    string url = $"https://date.nager.at/api/v3/PublicHolidays/{year}/TR";
                    
                    // JSON verisini string olarak çek
                    string jsonResponse = await _client.GetStringAsync(url);

                    // JSON verisini Holiday listesine dönüştür
                    var holidaysOfYear = JsonSerializer.Deserialize<List<Holiday>>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (holidaysOfYear != null)
                    {
                        _holidayCache.AddRange(holidaysOfYear);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{year} yılı verisi çekilirken hata oluştu: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Kullanıcı menüsünü çalıştırır.
        /// </summary>
        private static async Task RunUserInterface()
        {
            string userChoice = string.Empty;
            do
            {
                PrintMenu();
                userChoice = Console.ReadLine();

                switch (userChoice)
                {
                    case "1":
                        ShowHolidaysByYear();
                        break;
                    case "2":
                        SearchByDate();
                        break;
                    case "3":
                        SearchByName();
                        break;
                    case "4":
                        ShowAllHolidays();
                        break;
                    case "5":
                        Console.WriteLine("Uygulamadan çıkış yapılıyor. İyi günler!");
                        break;
                    default:
                        Console.WriteLine("Geçersiz seçim! Lütfen tekrar deneyiniz.");
                        break;
                }

                if (userChoice != "5")
                {
                    Console.WriteLine("\nAna menüye dönmek için bir tuşa basınız...");
                    Console.ReadKey();
                    Console.Clear();
                }

            } while (userChoice != "5");
        }

        private static void PrintMenu()
        {
            Console.WriteLine("\n===== PublicHolidayTracker =====");
            Console.WriteLine("1. Tatil listesini göster (yıl seçmeli)");
            Console.WriteLine("2. Tarihe göre tatil ara (gg-aa formatı)");
            Console.WriteLine("3. İsme göre tatil ara");
            Console.WriteLine("4. Tüm tatilleri 3 yıl boyunca göster (2023–2025)");
            Console.WriteLine("5. Çıkış");
            Console.Write("Seçiminiz: ");
        }

        // --- Fonksiyonlar ---

        private static void ShowHolidaysByYear()
        {
            Console.Write("Listelemek istediğiniz yılı girin (2023, 2024, 2025): ");
            string inputYear = Console.ReadLine();

            if (int.TryParse(inputYear, out int selectedYear) && _yearsToFetch.Contains(selectedYear))
            {
                var filteredList = _holidayCache
                    .Where(h => h.date.StartsWith(selectedYear.ToString()))
                    .ToList();

                PrintList(filteredList, $"{selectedYear} Yılı Resmi Tatilleri");
            }
            else
            {
                Console.WriteLine("Hatalı giriş! Sadece 2023, 2024 veya 2025 girebilirsiniz.");
            }
        }

        private static void SearchByDate()
        {
            Console.Write("Aramak istediğiniz tarihi girin (Örn: 23-04 veya 29-10): ");
            string inputDate = Console.ReadLine();

            // Girişin formatını kontrol etme (Basit doğrulama)
            if (string.IsNullOrWhiteSpace(inputDate) || !inputDate.Contains("-"))
            {
                Console.WriteLine("Lütfen geçerli formatta (GG-AA) giriş yapınız.");
                return;
            }

            // API formatı YYYY-MM-DD olduğu için tersten kontrol ediyoruz.
            // Kullanıcı 23-04 girdiyse biz MM-DD olarak 04-23 aratmalıyız (DateTime parse ile garantiye alalım)
            string searchPattern = "";
            try
            {
                // Kullanıcının girdiği GG-AA formatını algıla
                var dateParts = inputDate.Split('-');
                string day = dateParts[0].PadLeft(2, '0');
                string month = dateParts[1].PadLeft(2, '0');
                
                // API tarih formatının sonu "-MM-DD" şeklindedir.
                searchPattern = $"-{month}-{day}";
            }
            catch
            {
                Console.WriteLine("Tarih formatı işlenemedi.");
                return;
            }

            var results = _holidayCache
                .Where(h => h.date.EndsWith(searchPattern))
                .ToList();

            if (results.Any())
            {
                PrintList(results, $"{inputDate} Tarihine Denk Gelen Tatiller");
            }
            else
            {
                Console.WriteLine("Bu tarihte kayıtlı bir resmi tatil bulunamadı.");
            }
        }

        private static void SearchByName()
        {
            Console.Write("Tatil adı giriniz (Örn: Ramazan, Zafer): ");
            string keyword = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                Console.WriteLine("Arama metni boş olamaz.");
                return;
            }

            // Büyük/küçük harf duyarsız arama
            var results = _holidayCache
                .Where(h => h.localName.ToLower().Contains(keyword.ToLower()) || 
                            h.name.ToLower().Contains(keyword.ToLower()))
                .OrderBy(h => h.date)
                .ToList();

            if (results.Any())
            {
                PrintList(results, $"'{keyword}' İçeren Tatiller");
            }
            else
            {
                Console.WriteLine($"'{keyword}' ile eşleşen tatil bulunamadı.");
            }
        }

        private static void ShowAllHolidays()
        {
            // Tüm listeyi tarih sırasına göre göster
            var sortedList = _holidayCache.OrderBy(h => h.date).ToList();
            PrintList(sortedList, "Tüm Yılların Resmi Tatil Listesi (2023-2025)");
        }

        // Listeyi ekrana yazdırmak için yardımcı metot (Kod tekrarını önler)
        private static void PrintList(List<Holiday> holidays, string headerTitle)
        {
            Console.WriteLine($"\n--- {headerTitle} ---");
            foreach (var holiday in holidays)
            {
                Console.WriteLine(holiday.GetFormattedString());
            }
            Console.WriteLine("--------------------------------------------");
        }
    }
}