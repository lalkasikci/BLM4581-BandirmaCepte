# BLM4581 - Bandırma Cepte 📱🌊

**Bandırma Cepte**, Bandırma'da yaşayanların ve ilçeyi ziyaret edenlerin günlük yaşamda ihtiyaç duyduğu yerel bilgilere (haberler, nöbetçi eczaneler, otobüs saatleri, depremler ve acil durum numaraları) tek bir platform üzerinden ulaşmasını sağlayan bir yerel bilgi portalı ve Web API projesidir.

> **Ders:** BLM4581 Ağ Tabanlı Teknolojiler ve Uygulamaları  
> **Öğrenci:** Zeynep Lal Kaşıkçı
> **1. Hafta Teslimi:** [Google Drive Bağlantısı](https://drive.google.com/file/d/1bnuVva1_aneg7ZNEorloHewDkUVMJnBj/view?usp=drive_link)
> **2. Hafta Teslimi:** [Google Drive Bağlantısı](https://drive.google.com/file/d/1E2LQXYvk5l5oFwKakm0hPz8iuuig6xtC/view?usp=sharing)
> **3. Hafta Teslimi:** [Google Drive Bağlantısı](https://drive.google.com/file/d/18FHCMDOwulYwNRgUvXIgytxFjj_9uXC8/view?usp=sharing)
> **4. Hafta Teslimi:** [Google Drive Bağlantısı](https://drive.google.com/file/d/1f2Ngev0Fo--6OHNcDvhMILgiU-1-MN2x/view?usp=sharing)
> Dosya Drive Bağlantısı:[Google Drive Bağlantısı](https://drive.google.com/drive/folders/1yvspIT4KBqQWabtEsUVhYYAO2PwjQfHD?usp=sharing)
> **GitHub Deposu:** [lalkasikci/BLM4581-BandirmaCepte](https://github.com/lalkasikci/BLM4581-BandirmaCepte)  
> **Google Drive Bağlantısı:** [Dönem Projesi Klasörü](https://drive.google.com/drive/folders/1yvspIT4KBqQWabtEsUVhYYAO2PwjQfHD)


## 🛠️ Kullanılan Teknolojiler

- **Backend:** ASP.NET Core Web API (.NET 10)
- **ORM / Veritabanı:** Entity Framework Core & PostgreSQL (Npgsql)
- **Kimlik Doğrulama & Yetkilendirme:** JWT Bearer & BCrypt.Net
- **API Dokümantasyonu:** Swagger / OpenAPI (Swashbuckle)
- **Ön Yüz:** HTML5, CSS3, Vanilla JS (Fetch API), Bootstrap 5, FontAwesome

---

## 🚀 Kurulum ve Çalıştırma

### 1. Depoyu Klonlayın
```bash
git clone https://github.com/lalkasikci/BLM4581-BandirmaCepte.git
cd BLM4581-BandirmaCepte/BandirmaCepte
```

### 2. Veritabanı Yapılandırması
`appsettings.json` dosyasında PostgreSQL bağlantı dizesini kontrol edin:
```json
"ConnectionStrings": {
  "BandirmaDb": "Host=localhost;Port=5432;Database=BandirmaCepteDb;Username=postgres;Password=postgres"
}
```

### 3. Projeyi Derleyin ve Çalıştırın
```bash
dotnet build
dotnet run
```

### 4. Tarayıcıdan Erişin
- **Web Portalı:** `http://localhost:5000` (veya port bilgisine göre `http://localhost:5032`)
- **Swagger API Dokümantasyonu:** `http://localhost:5000/swagger`

---

## 👥 Varsayılan Kullanıcı Hesapları

| Rol | Kullanıcı Adı | Şifre | Yetkiler |
|---|---|---|---|
<<<<<<< Updated upstream
| **Admin** | `admin` | `Admin123!` | Haber Ekleme / Silme, Tüm Listeleri Görüntüleme |
=======
| **Admin** | `admin` | `Admin123!` | Haber/Hat/Deprem/Acil Numara Ekleme, Silme ve Yönetim |
>>>>>>> Stashed changes
| **User** | `user` | `User123!` | Tüm Listeleri Görüntüleme |
