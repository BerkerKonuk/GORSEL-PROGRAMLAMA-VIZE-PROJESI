# Public Holiday Tracker (Resmi Tatil Takip Projesi)

Bu proje, **Görsel Programlama** dersi kapsamında geliştirilmiş bir C# konsol uygulamasıdır. `date.nager.at` API servisini kullanarak Türkiye'nin 2023, 2024 ve 2025 yıllarındaki resmi tatil verilerini anlık olarak çeker ve listeler.

## 🎓 Öğrenci Bilgileri

| Bilgi | Detay |
| :--- | :--- |
| **Ad Soyad** | **Berker Konuk** |
| **Numara** | **20230108038** |
| **Ders** | Görsel Programlama (BIP2033) |
| **Öğretim Görevlisi** | Emrah SARIÇİÇEK |
| **Teslim Tarihi** | 05.12.2025 |

## 🛠 Projenin Özellikleri

Uygulama açıldığında verileri internetten indirir ve şu işlemleri yapmanızı sağlar:

* **Yıl Bazlı Listeleme:** Sadece seçtiğiniz yılın (Örn: 2024) tatillerini gösterir.
* **Tarih Kontrolü:** Girdiğiniz günde (Örn: `29-10`) bir tatil olup olmadığını sorgular.
* **Kelime ile Arama:** Tatil ismine göre (Örn: "Ramazan", "Zafer") arama yapar.
* **Tam Liste:** Hafızadaki 3 yıllık tüm tatil listesini tarih sırasına göre döker.

## 💻 Teknik Detaylar

* **Platform:** .NET Core / .NET 8.0
* **Dil:** C#
* **Kullanılan Kütüphaneler:** `System.Net.Http` (Veri çekmek için), `System.Text.Json` (JSON işlemek için), `System.Linq` (Sorgulama için).

## ▶️ Kurulum ve Çalıştırma

1.  Projeyi indirin ve **Visual Studio 2025** ile açın.
2.  Bilgisayarınızın internete bağlı olduğundan emin olun.
3.  'program.cs' adlı dosyayı çalıştırarak program çalışır

---
*Not: Veriler `https://date.nager.at/api/v3/PublicHolidays/{yil}/TR` adresinden dinamik olarak çekilmektedir.*
