# MonexiaSqlGenerator

MonexiaSqlGenerator, Monexia kişisel finans yönetim uygulaması için gerçekçi ve çeşitli finansal işlem verileri (gelir/gider) üreten, bu verileri SQL `INSERT` komutları olarak dışa aktarabilen bir C# konsol uygulamasıdır. Özellikle test, demo ve geliştirme ortamlarında büyük hacimli kullanıcı işlemi verisi oluşturmak için idealdir.

---

## 🚀 Özellikler

- **Gerçekçi Finansal Kayıtlar:** Farklı kullanıcılar için gelir ve gider işlemleri, çeşitli kategorilerde otomatik olarak üretilir.
- **Kategori Desteği:** Gıda, ulaşım, konut, sağlık, eğitim, eğlence, fatura, giyim, tatil, kredi kartı ödemesi, abonelikler ve daha fazlası.
- **Gelir/Gider Oranı Ayarı:** Her kullanıcı için gelir ve gider oranı özelleştirilebilir.
- **SQL Script Çıktısı:** Üretilen işlemler, doğrudan SQL Server’a aktarılabilecek şekilde `.sql` dosyalarına yazılır.
- **Gerçekçi Tarih ve Açıklamalar:** İşlemler, iki yıllık periyotta rastgele ve mantıklı tarihlerle, açıklamalarla birlikte oluşturulur.
- **Çoklu Kullanıcı Desteği:** Birden fazla kullanıcıya ait işlem verisi aynı anda üretilebilir.

---

## 📁 Proje Yapısı
MonexiaSqlGenerator/     
├── Program.cs # Ana uygulama ve veri üretim mantığı     
├── (Çıktı) transactions_userX.sql # Her kullanıcı için üretilen SQL dosyaları    


## 🛠️ Özelleştirme

- **Kullanıcı ve Kayıt Sayısı:**  
  `Program.cs` dosyasında, her kullanıcı için `userId`, `totalRecords` ve `incomeRatio` parametrelerini değiştirebilirsiniz.
- **Kategori ve Açıklamalar:**  
  Gelir ve gider kategorileri ile açıklamalar, enum ve ilgili fonksiyonlarda düzenlenebilir.
- **Tarih Aralığı:**  
  Varsayılan olarak son iki yıl için veri üretir. `yearCount` değişkeni ile değiştirilebilir.

---

## 📄 Lisans

Bu projenin lisans durumu için [LICENSE](LICENSE) kısmını kontrol edebilirsiniz.

---

## 📞 İletişim

Her türlü soru ve öneriniz için:  
**E-posta:** msemihbulut@gmail.com
