# Public Holiday Tracker (Resmi Tatil Ödevi)

Merhaba, bu proje C# dersi için hazırladığım konsol uygulaması ödevidir. Uygulama, internetteki bir API servisini kullanarak Türkiye'deki resmi tatil günlerini çekiyor ve ekranda listeliyor.

## ❓ Proje Ne Yapıyor?
Bu programı çalıştırdığınızda otomatik olarak `nager.at` adresine bağlanıp 2023, 2024 ve 2025 yıllarının tatil verilerini indiriyor. Sonra menüden seçim yaparak şunları yapabiliyorsunuz:

* İstediğiniz yılın tatillerini görebilirsiniz.
* Belirli bir tarihte (mesela 29 Ekim'de) tatil var mı diye bakabilirsiniz.
* İsimle arama yapabilirsiniz (Örneğin "Ramazan" yazınca bayramları buluyor).
* İsterseniz 3 yıllık tüm listeyi tek seferde görebilirsiniz.

## 💻 Nasıl Çalıştırılır?
Projeyi Visual Studio ile açıp "Start" tuşuna basmanız yeterli. Kodlar `Program.cs` dosyasının içindedir. İnternet bağlantısı olması gerekiyor çünkü verileri canlı çekiyor.

---
