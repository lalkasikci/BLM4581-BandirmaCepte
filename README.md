# BLM4581 - Bandırma Cepte 🌊

**Bandırma Cepte**, Bandırma’da yaşayanların ve ilçeyi ziyaret edenlerin günlük yaşamda ihtiyaç duyduğu yerel bilgilere (haberler, nöbetçi eczaneler, otobüs saatleri, depremler ve acil durum numaraları) tek bir platform üzerinden ulaşmasını sağlayan bir yerel bilgi portalı ve Web API projesidir.

> **Ders:** BLM4581 Ağ Tabanlı Teknolojiler ve Uygulamaları  
> **Öğrenci:** Zeynep Lal Kaşıkçı (22290070)  
> **1. Hafta Teslimi:** [Google Drive Bağlantısı](https://drive.google.com/file/d/1bnuVva1_aneg7ZNEorloHewDkUVMJnBj/view?usp=drive_link)
> **2. Hafta Teslimi:** [Google Drive Bağlantısı](https://drive.google.com/file/d/1E2LQXYvk5l5oFwKakm0hPz8iuuig6xtC/view?usp=sharing)

## 🚀 Özellikler

- **Yerel Haberler & Duyurular:** Bandırma ile ilgili güncel gelişmeler. Admin rolü ile haber ekleme ve silme.
- **Otobüs Hatları ve Hareket Saatleri:** Şehir içi hatlar (H1, H2 vb.) ve kalkış saatleri.
- **Nöbetçi Eczaneler:** Günlük nöbetçi eczane bilgisi, adres ve tek tıkla arama imkanı.
- **Son Depremler:** Bandırma ve Marmara çevresindeki son sarsıntılar (büyüklük, derinlik ve saat bilgisi).
- **Acil Durum & Önemli Numaralar:** 112 Acil, Devlet Hastanesi, Belediye vb. hızlı arama bağlantıları.
- **Kullanıcı & Rol Yönetimi (JWT & BCrypt):**
  - **Admin:** İçerik yönetimi (haber ekleme/silme).
  - **User:** İçerikleri görüntüleme.
- **Etkileşimli Web Arayüzü:** `wwwroot` üzerinden sunulan modern, mobil uyumlu kullanıcı portalı.
- **Swagger / OpenAPI:** API uç noktalarını doğrudan test edebilme (`/swagger`).

---

## 🛠️ Kullanılan Teknolojiler

- **Backend:** ASP.NET Core Web API (.NET 10)
- **Veritabanı & ORM:** PostgreSQL & Entity Framework Core
- **Kimlik Doğrulama & Yetkilendirme:** JWT (JSON Web Tokens) & BCrypt.Net
- **Dokümantasyon:** Swagger / OpenAPI (Swashbuckle)
- **Arayüz:** HTML5, CSS3, JavaScript (Fetch API), Bootstrap 5, FontAwesome

---

## 💻 Kurulum ve Çalıştırma

### 1. Depoyu Klonlayın
```bash
git clone https://github.com/lalkasikci/BLM4581-BandirmaCepte.git
cd BLM4581-BandirmaCepte/BandirmaCepte
```

### 2. Veritabanı Bağlantısı (İsteğe Bağlı)
`appsettings.json` dosyasında PostgreSQL bağlantı dizesini kontrol edin:
```json
"ConnectionStrings": {
  "BandirmaDb": "Host=localhost;Port=5432;Database=BandirmaCepteDb;Username=postgres;Password=postgres"
}
```

### 3. Projeyi Çalıştırın
```bash
dotnet run
```

### 4. Tarayıcıda Açın
- **Web Portalı:** `http://localhost:5000` (veya `http://localhost:5032`)
- **Swagger API Dokümantasyonu:** `http://localhost:5000/swagger`

---

## 🔑 Varsayılan Kullanıcı Hesapları

| Rol | Kullanıcı Adı | Şifre | Yetkiler |
|---|---|---|---|
| **Admin** | `admin` | `Admin123!` | Haber Ekleme / Silme, Tüm Listeleri Görüntüleme |
| **User** | `user` | `User123!` | Tüm Listeleri Görüntüleme |
