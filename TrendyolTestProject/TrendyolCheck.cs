using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System;

namespace TrendyolTestProject
{
    [TestClass]
    public class TrendyolCheck
    {
        ChromeDriver driver = new ChromeDriver(@"C:\chromedriver_win32");

        [TestMethod]
        public void TestMethod1()
        {
            //Note: Uyumsuzluktan dolayı Thread.Sleep ile bekleme işlemleri kullanarak sayfaların tam yüklenmesi beklenmiştir.
            //TODO: Thread Sleep'siz yapma kısmına bakılacak.

            driver.Navigate().GoToUrl("https://www.trendyol.com/");

            Thread.Sleep(2000);
            //Reklamlar kapatıldı.
            IWebElement advertising = driver.FindElementByClassName("fancybox-close");
            advertising.Click();

            Thread.Sleep(2000);
            //Kullanıcı girişi butonuna tıklandı.
            IWebElement login = driver.FindElementById("accountBtn");
            login.Click();

            Thread.Sleep(2000);
            //Mail adres bilgisi yazıldı.
            IWebElement eposta = driver.FindElementById("email");
            eposta.SendKeys("testinium_atakan@gmail.com");

            //Şifre bilgisi yazıldı.
            IWebElement parola = driver.FindElementById("password");
            parola.SendKeys("atakan12345678");

            Thread.Sleep(2000);
            //Giriş butonuna tıklandı.
            IWebElement giris = driver.FindElementById("loginSubmit");
            giris.Click();

            Thread.Sleep(2000);
            //Arama kutucuğuna bilgisayar yazdırıldı.
            IWebElement arama = driver.FindElementByClassName("search-box");
            arama.SendKeys("Bilgisayar");

            Thread.Sleep(2000);
            //Arama listesi sonrasında açılan menüdeki ilk seçenek seçildi.
            ICollection<IWebElement> aramasonuc = driver.FindElementsByClassName("suggestion");
            aramasonuc.FirstOrDefault().Click();

            Thread.Sleep(2000);
            //Açılan ürün listesinden ilk ürün seçildi. TODO: Random olarak seçilmesi gerekiyor.
            ICollection<IWebElement> urunsonuc = driver.FindElementsByClassName("add-to-bs-card");
            urunsonuc.FirstOrDefault().Click();

            //Seçilen ürün sepete eklendi.
            IWebElement sepeteEkle = driver.FindElementByClassName("add-to-bs");
            sepeteEkle.Click();


            Thread.Sleep(2000);
            //Ürünün fiyatı alınarak bir değişkene atıldı.
            IWebElement ufiyat = driver.FindElementByClassName("prc-dsc");
            string urunfiyat = ufiyat.Text;

            try
            {
                //Sepete gidildi.
                Thread.Sleep(2000);
                IWebElement basketPage = driver.FindElementByClassName("basket-button-container");
                basketPage.Click();
            }
            catch 
            {
            }

            Thread.Sleep(2000);
            IWebElement sptFiyatı = driver.FindElementByClassName("pb-basket-item-price");

            string sepetFiyatıStr = sptFiyatı.Text.Split('\n').LastOrDefault();

            //Ürün fiyatı ile sepet fiyatı karşılaştırması yapıldı.
            if (urunfiyat != sepetFiyatıStr)
                Assert.Fail("Ürün fiyatı ile sepet fiyatı aynı değil!");
            else
                Assert.IsTrue(true, "Ürün fiyatı ile sepeti fiyatı aynı!");


            //Ürün adeti arttırma butonuna tıklanır.
            ICollection<IWebElement> productCountAdd = driver.FindElementsByClassName("ty-numeric-counter-button");
            productCountAdd.LastOrDefault().Click();

            //Ürün fiyatı arttırılan değer kadar tekrardan hesaplanır ve kontrol ettirilir.

            string priceStr = sepetFiyatıStr.Replace("TL", "").Replace(" ", "");
            decimal priceControl = decimal.Parse(priceStr);
            priceControl = priceControl * 2;

            priceStr = priceControl + " TL";

            sptFiyatı = driver.FindElementByClassName("pb-basket-item-price");
            sepetFiyatıStr = sptFiyatı.Text.Split('\n').LastOrDefault();

            if (priceStr == sepetFiyatıStr)
            {
                Assert.IsTrue(true, "Adet artması ile hesaplama doğru yapılmıştır.");
            }
            else
            {
                Assert.Fail("Ürün adeti artması sonrasında hesaplama başarısız!");
            }

            //Sepette i-trash iconuna sahip tüm ürünler için kontrol edilir ve hepsi silinir.
            ICollection<IWebElement> sepetUrunListesi = driver.FindElementsByClassName("i-trash");
            foreach (IWebElement silinecekUrun in sepetUrunListesi)
            {
                silinecekUrun.Click();
                Thread.Sleep(1000);
            }

            //i-trash classına göre silme işleminden sonra liste tekrardan çekilir.
            sepetUrunListesi = driver.FindElementsByClassName("i-trash");
            
            Thread.Sleep(1000);

            //Sepetteki ürün sayısı kontrolü yapılır.
            if (sepetUrunListesi.Count == 0)
                Assert.IsTrue(true, "Tüm ürünler başarılı bir şekilde silindi");
            else 
                Assert.Fail("Sepette ürün var. Silinme işlemi başarısız!");

        }
    }
}
