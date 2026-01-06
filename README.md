# 📦 OSBilişim – Ürün Satış, Stok ve Sipariş Takip Sistemi

<p align="center">
  <img src="https://media.giphy.com/media/Y4ak9Ki2GZCbJxAnJD/giphy.gif" width="150px">
</p>

<p align="center">
  <b>C# ve SQL mimarisi üzerine kurulu, çoklu kullanıcı destekli gelişmiş stok ve sipariş yönetim paneli.</b><br>
  Ürün giriş-çıkışlarını, stok durumunu ve sipariş süreçlerini tek bir merkezden yönetmek için tasarlanmıştır.
</p>

---

## 🚀 Özellikler

- ✔ **Stok Yönetimi:** Ürün ekleme, düzenleme ve stok miktarlarını anlık takip etme.
- ✔ **Sipariş Sistemi:** Kullanıcıların oluşturduğu siparişlerin, ağdaki diğer yetkili üyeler tarafından anlık görüntülenebilmesi.
- ✔ **Kullanıcı Paneli:** Güvenli giriş (Login), yeni kullanıcı kaydı ve şifre sıfırlama işlemleri.
- ✔ **Malzeme Grupları:** Ürünleri kategorize etme ve grup bazlı sipariş oluşturma.
- ✔ **Veritabanı Entegrasyonu:** Yerel (Localhost) veya Uzak (Global) sunucu desteği ile ölçeklenebilir yapı.
- ✔ **Sipariş Kontrol:** Bekleyen ve tamamlanan siparişlerin durum takibi.

<p align="center">
  <img src="https://img.shields.io/badge/Language-C%23-239120?logo=c-sharp&logoColor=white&style=flat-square">
  <img src="https://img.shields.io/badge/.NET-Framework-512BD4?logo=dotnet&logoColor=white&style=flat-square">
  <img src="https://img.shields.io/badge/Database-SQL_Server-CC2927?logo=microsoftsqlserver&logoColor=white&style=flat-square">
  <img src="https://img.shields.io/badge/Platform-Windows-0078D6?logo=windows&logoColor=white&style=flat-square">
  <img src="https://img.shields.io/badge/License-GPLv3-blue.svg?style=flat-square">
</p>

---

## 🧠 Sistem Nasıl Çalışır?

Uygulama, **istemci-sunucu (Client-Server)** mimarisi ile çalışır:

### 1️⃣ Kimlik Doğrulama
- Uygulama açılışında `Kullanıcı Girişi` ekranı karşılar. Yetkisi olmayan kullanıcılar sisteme erişemez.

### 2️⃣ Ürün ve Stok İşlemleri
- Yetkili kullanıcılar `Ürün Ekle` modülü ile envantere yeni stok girişi yapar.
- Ürün isimleri ve kategorileri dinamik olarak düzenlenebilir.

### 3️⃣ Sipariş Döngüsü (Network)
- Bir kullanıcı `Sipariş Oluşturma` ekranından yeni bir talep girdiğinde, veriler merkezi SQL veritabanına işlenir.
- Diğer kullanıcılar (örn: Depo Sorumlusu veya Yönetici), kendi ekranlarında bu siparişleri anlık olarak görür ve `Sipariş Kontrol` modülü ile onaylayıp işleme alabilir.

---

## 🛠️ Kurulum ve Yapılandırma

Bu proje veritabanı bağlantısı gerektirir. Çalıştırmadan önce aşağıdaki adımları izleyin:

### 1️⃣ Veritabanı Kurulumu
Proje dosyaları içerisinde bulunan (veya ayrıca sağlanan) SQL Script dosyasını çalıştırarak gerekli tabloları (`Users`, `Products`, `Orders` vb.) oluşturun.

### 2️⃣ Bağlantı Ayarları (App.config)
Proje dizinindeki `App.config` dosyasını açın ve `connectionString` değerini kendi sunucunuza göre düzenleyin:

```xml
<connectionStrings>
    <add name="OSBilisimDB" 
         connectionString="Data Source=LOCALHOST;Initial Catalog=OSBilisim;Integrated Security=True" 
         providerName="System.Data.SqlClient" />
</connectionStrings>
(Eğer uygulamayı global/uzak sunucuda kullanacaksanız, IP adresini ve kullanıcı bilgilerini buraya girmeniz yeterlidir.)
```

### 3️⃣ Çalıştırma
Visual Studio üzerinden projeyi derleyin (Build) ve başlatın.

⚖️ Lisans
Bu proje GNU General Public License v3.0 ile lisanslanmıştır. Projenin tüm kullanıcıları, lisansın koşullarına uymak kaydıyla projeyi özgürce kullanabilir, değiştirebilir ve paylaşabilir.

🤝 İletişim
<p align="left"> <a href="https://discordapp.com/users/481831692399673375"><img src="https://img.shields.io/badge/Discord-Zyix%231002-7289DA?logo=discord&style=flat-square"></a> <a href="https://www.youtube.com/channel/UC7uBi3y2HOCLde5MYWECynQ?view_as=subscriber"><img src="https://img.shields.io/badge/YouTube-Subscribe-red?logo=youtube&style=flat-square"></a> <a href="https://www.reddit.com/user/_Zyix"><img src="https://img.shields.io/badge/Reddit-Profile-orange?logo=reddit&style=flat-square"></a> <a href="https://open.spotify.com/user/07288iyoa19459y599jutdex6"><img src="https://img.shields.io/badge/Spotify-Follow-green?logo=spotify&style=flat-square"></a> </p>

