# BLM4581 - Bandırma Cepte 📱🌊

**Bandırma Cepte**, Bandırma'da yaşayanların ve ilçeyi ziyaret edenlerin günlük yaşamda ihtiyaç duyduğu yerel bilgilere (haberler, nöbetçi eczaneler, otobüs saatleri, depremler ve acil durum numaraları) tek bir platform üzerinden ulaşmasını sağlayan bir yerel bilgi portalı ve Web API projesidir.

> **Ders:** BLM4581 Ağ Tabanlı Teknolojiler ve Uygulamaları  
> **Öğrenci:** Zeynep Lal Kaşıkçı
> **1. Hafta Teslimi:** [Google Drive Bağlantısı](https://drive.google.com/file/d/1bnuVva1_aneg7ZNEorloHewDkUVMJnBj/view?usp=drive_link)
> **2. Hafta Teslimi:** [Google Drive Bağlantısı](https://drive.google.com/file/d/1E2LQXYvk5l5oFwKakm0hPz8iuuig6xtC/view?usp=sharing)
> **3. Hafta Teslimi:** [Google Drive Bağlantısı](https://drive.google.com/file/d/18FHCMDOwulYwNRgUvXIgytxFjj_9uXC8/view?usp=sharing)
> Dosya Drive Bağlantısı:[Google Drive Bağlantısı](https://drive.google.com/drive/folders/1yvspIT4KBqQWabtEsUVhYYAO2PwjQfHD?usp=sharing)
> **GitHub Deposu:** [lalkasikci/BLM4581-BandirmaCepte](https://github.com/lalkasikci/BLM4581-BandirmaCepte)  
> **Google Drive Bağlantısı:** [Dönem Projesi Klasörü](https://drive.google.com/drive/folders/1yvspIT4KBqQWabtEsUVhYYAO2PwjQfHD)

---
## 📅 Hafta Bazlı İlerleme Durumu

| Hafta | Planlanan İşler | Durum | Açıklama |
|---|---|---|---|
| **1. Hafta** | Gereksinimleri belirleme, GitHub deposu, geliştirme ortamı ve ASP.NET Core projesini kurma | ✅ Tamamlandı | .NET 10 Web API, GitHub & README başlangıcı yapıldı. |
| **2. Hafta** | PostgreSQL kurulumu, EF Core bağlantısı, ilk modeller, kullanıcı girişi ve Admin/User rol yapısı | ✅ Tamamlandı | JWT kimlik doğrulama, BCrypt hashleme, rol tabanlı yetkilendirme eklendi. |
| **3. Hafta** | Haber, deprem, otobüs hattı/saatleri ve acil numara modellerini oluşturma; veritabanı ilişkilerini kurma; örnek verileri ekleme | ✅ Tamamlandı | Varlık modelleri (News, Earthquake, BusLine, BusSchedule, EmergencyContact, User, Role), EF Core 1:N ilişkileri ve kapsamlı tohumlama (SeedData) tamamlandı. |
| **4. Hafta** | Haber varlığı için tam CRUD Web API uç noktalarını geliştirme; Swagger testleri; form doğrulama; ilk çalışan ekranlar | ⏳ Hazırlık | CRUD Web API ve Swagger dokümantasyonu hazırlandı. |

---

## 🗄️ Veritabanı Modelleri ve İlişkiler (3. Hafta)

Entity Framework Core Code-First yaklaşımı ile aşağıdaki modeller ve ilişkiler yapılandırılmıştır:

1. **`User` & `Role` (Çoka-Bir / N:1):**
   - Kullanıcı adı, şifre hash'i (BCrypt) ve rol bilgisi.
   - Rol bazlı yetkilendirme (`Admin`, `User`).
2. **`BusLine` & `BusSchedule` (Bire-Çok / 1:N):**
   - `BusLine`: Hat kodu (`LineNumber`), güzergâh adı (`Route`), açıklama (`Description`).
   - `BusSchedule`: Hatta bağlı kalkış saati (`DepartureTime`), kalkış noktası (`DeparturePoint`), varış noktası (`ArrivalPoint`), gün tipi (`DayType`: Hafta İçi / Hafta Sonu).
   - Silme kuralı: `Cascade` (Hat silindiğinde tüm saatleri de silinir).
3. **`User` & `News` (Bire-Çok / 1:N):**
   - Başlık (`Title`), içerik (`Content`), kaynak (`Source`), yayın tarihi (`PublishedAt`), yayın durumu (`IsPublished`).
   - Haberi ekleyen yönetici (`UserId` -> `User`).
4. **`User` & `EmergencyContact` (Bire-Çok / 1:N):**
   - Kurum adı (`Name`), telefon numarası (`PhoneNumber`), kategori (`Category`), açıklama (`Description`).
   - Numarayı ekleyen yönetici (`UserId` -> `User`).
5. **`User` & `Earthquake` (Bire-Çok / 1:N):**
   - Büyüklük (`Magnitude`), konum (`Location`), derinlik (`Depth`), zaman (`OccurrenceTime`), kaynak (`Source`: Kandilli / AFAD).
   - Manuel kayıt durumunda ekleyen yönetici (`UserId` -> `User`).
6. **`Pharmacy`:**
   - Eczane adı, adres, nöbet durumu (`IsOnCall`), telefon.

---

## 🌱 Örnek Tohum Verileri (Seed Data)

Uygulama ilk ayağa kalktığında (`SeedData.EnsureCreatedAsync`) veritabanı otomatik kontrol edilir ve aşağıdaki gerçekçi veriler tohumlanır:

- **Kullanıcılar:** `admin` (Admin rolü), `user` (User rolü).
- **Haberler:**
  - *Bandırma Sahil Kordonu Yenileme Projesi Başladı* (Kaynak: Bandırma Belediyesi)
  - *Bandırma Onyedi Eylül Üniversitesi Bahar Şenlikleri Programı Açıklandı* (Kaynak: BANÜ Rektörlüğü)
  - *Bandırma Kuşcenneti Milli Parkı Ziyaretçi Rekoruna Koşuyor* (Kaynak: Doğa Koruma ve Milli Parklar)
  - *Bandırmaspor TFF 1. Lig Maçı Biletleri Satışa Sunuldu* (Kaynak: Bandırmaspor Kulübü)
- **Otobüs Hatları ve Hareket Saatleri:**
  - `H1`: Meydan - Liman - Paşabayır - 600 Evler (12 sefer saati)
  - `H2`: Meydan - Devlet Hastanesi - TOKİ Konutları (11 sefer saati)
  - `H3`: İDO İskelesi - Otogar - BANÜ Merkez Kampüs (11 sefer saati)
- **Acil Durum & Önemli Numaralar:**
  - 112 Acil Çağrı Merkezi
  - (0266) 738 00 22 Bandırma Eğitim ve Araştırma Hastanesi
  - (0266) 714 11 00 Bandırma İlçe Emniyet Müdürlüğü
  - (0266) 711 11 11 Bandırma Belediyesi Çağrı Merkezi
  - (0266) 718 11 10 Bandırma İtfaiye Grup Amirliği
  - 185 BASKİ Su Arıza, 186 UEDAŞ Elektrik Arıza, 187 Aksa Doğalgaz Acil
- **Deprem Bilgileri:**
  - Marmara Denizi (Bandırma Açıkları) - 3.4
  - Erdek Körfezi - 2.9
  - Manyas Gölü Civarı - 3.8
  - Bandırma Edincik Açıkları - 2.2
  - Kapıdağ Yarımadası - 2.6
- **Nöbetçi Eczaneler:** Liman Eczanesi, Merkez Kent Eczanesi, Şifa Eczanesi.

---

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
