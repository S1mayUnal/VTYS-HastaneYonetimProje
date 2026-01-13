T.C. MARMARA ÜNİVERSİTESİ
TEKNOLOJİ FAKÜLTESİ
BİLGİSAYAR MÜHENDİSLİĞİ BÖLÜMÜ

VERİTABANI YÖNETİM SİSTEMLERİ
PROJE FİNAL RAPORU
2025-2026 GÜZ DÖNEMİ


1. PROJE GRUBU VE KULLANILAN TEKNOLOJİLER

Öğrenci Adı Soyadı: Simay Ünal
Öğrenci Numarası: 170424822

Öğrenci Adı Soyadı: Serenay Çalışır
Öğrenci Numarası: 170219005

Öğrenci Adı Soyadı: Elif Biçan
Öğrenci Numarası: 100921017

Öğrenci Adı Soyadı: Sümeyye Karaköse
Öğrenci Numarası: 100921041

Bu proje kapsamında SQL Server üzerinde çalışan bir Hastane Yönetim Sistemi geliştirilmiştir.
Veritabanı tarafında MSSQL Server, T-SQL, Stored Procedure, Index ve Constraint yapıları
kullanılmıştır. Uygulama arayüzü tarafında veritabanına doğrudan tablolar yerine VIEW’ler
üzerinden erişim sağlanmıştır.

Proje tanıtım videosu: HastaneYonetimSistemiVideo.mp4


2. GITHUB PROJE BAĞLANTISI

https://github.com/S1mayUnal/VTYS-HastaneYonetimProje


3. PROJENİN AMACI

Bu projenin amacı, bir hastanenin temel iş süreçlerini dijital ortamda yönetebilen,
güvenli ve sürdürülebilir bir Hastane Yönetim Sistemi geliştirmektir.
Hasta, doktor, hemşire, randevu, yatış, oda, bölüm, tahlil ve ilaç gibi temel
bileşenler sistem içerisinde yönetilmektedir.

Sistem aşağıdaki işlemleri gerçekleştirmektedir:
- Hasta ve doktor kayıtlarının tutulması
- Randevu oluşturma ve randevu takibi
- Muayene ve tanı kayıtlarının yapılması
- Tahlil sonuçlarının dijital ortamda saklanması
- İlaçların barkod bazlı takibi ve reçetelendirilmesi
- Yatış ve oda bilgilerinin yönetilmesi
- Faturalandırma ve ödeme takibi
- Rol bazlı kullanıcı giriş sistemi


4. VERİTABANI TABLOLARI VE İLİŞKİLER

Projede kullanılan tablolar:
Doktor, Hasta, Bolum, Oda, Yatis, Randevu, Kullanici, Ilac, Tahlil,
RandevuTahlil, ReceteDetay, Hemsire, HastaBakim, Fatura, Nobet

Tüm tablolar 3NF normalizasyon kurallarına uygun olarak tasarlanmıştır.
Veri tekrarını önlemek amacıyla RandevuTahlil ve ReceteDetay ara tabloları
kullanılmıştır.

Temel ilişkiler:
- Hasta - Randevu (1-N)
- Hasta - Fatura (1-N)
- Doktor - Randevu (1-N)
- Bolum - Oda (1-N)
- Oda - Yatis (1-N)
- Hasta - Yatis (1-N)
- Hasta/Doktor - Kullanici (1-1)
- Randevu - Tahlil (M-N)
- Randevu - Ilac (M-N)
- Yatis - Hemsire (M-N)
- Bolum - Nobet (1-N)


5. ER DİYAGRAMI

Veritabanı ER diyagramı bilgisayar ortamında çizilmiş olup GitHub projesinin
docs klasörü içerisinde paylaşılmıştır.


6. DDL ÖRNEKLERİ

Doktor ve Hasta tabloları için CREATE TABLE komutları projede tanımlanmıştır.
Tablolarda PRIMARY KEY, FOREIGN KEY, CHECK ve UNIQUE kısıtları kullanılmıştır.


7. DML ÖRNEKLERİ

Projede INSERT, UPDATE ve DELETE işlemlerini içeren DML sorguları
kullanılmış ve test edilmiştir.


8. SQL SORGULARI

Projede toplam 10 adet SQL sorgusu yazılmıştır.
Sorgular basitten karmaşığa doğru ilerlemekte olup;
JOIN, GROUP BY, HAVING ve aggregate fonksiyonlar kullanılmaktadır.
Sorgular randevu, yatış, oda doluluk oranı, nöbet bilgileri ve tahlil
analizlerini kapsamaktadır.


9. VERİTABANI BAĞLANTISI VE ARAYÜZ

Proje arayüzü Figma ile tasarlanmıştır.
Backend .NET, frontend Vue.js ve Tailwind CSS kullanılarak geliştirilmiştir.
Frontend ile backend arasındaki iletişim Axios üzerinden API endpointleri
aracılığıyla sağlanmıştır.

Hasta ve doktorlar için ayrı kullanıcı arayüzleri oluşturulmuş,
randevu yönetimi, tahlil sonuçları ve fatura görüntüleme
işlevleri uygulanmıştır.


10. TRANSACTION KAVRAMI

Transaction, bir veya birden fazla veritabanı işleminin
bir bütün olarak ele alınmasını sağlar.
İşlemler ya tamamen başarılı olur (COMMIT)
ya da hata durumunda geri alınır (ROLLBACK).

Projede randevu güncelleme işlemleri transaction
kullanılarak gerçekleştirilmiştir.


11. VIEW VE STORED PROCEDURE

View, bir veya birden fazla tablodan elde edilen
verilerin sanal tablo olarak sunulmasını sağlar.
Stored Procedure ise tekrar kullanılabilir
SQL kod bloklarıdır.

Projede aktif randevular için VIEW,
kullanıcı girişi için Stored Procedure
kullanılmıştır.
