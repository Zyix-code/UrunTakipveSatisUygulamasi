using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OSBilişim
{
    public partial class Yeni_kullanici : Form
    {
        Kullanıcı_girisi Kullanıcı_girisi = new Kullanıcı_girisi();
        public Yeni_kullanici()
        {
            InitializeComponent();
        }
        ErrorProvider provider = new ErrorProvider();

        readonly SqlConnection connection = new SqlConnection("Data Source=192.168.1.123,1433;Network Library=DBMSSOCN; Initial Catalog=OSBİLİSİM;User Id=Admin; Password=1; MultipleActiveResultSets=True;");
        private void Yeni_Kullanici_Load(object sender, EventArgs e)
        {
            kayit_tarihi_textbox.Text = DateTime.Now.ToString();
            sifre_textbox.UseSystemPasswordChar = true;
       
        }

        private void logout_label_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                SqlCommand kullanicidurumgüncelle = new SqlCommand("Update kullanicilar set durum='" + 0 + "' where k_adi = '" + Kullanıcı_girisi.username + "'", connection);
                kullanicidurumgüncelle.ExecuteNonQuery();
            }
            catch (Exception kullaniciaktifligi)
            {
                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.Log("Kullanıcı bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + kullaniciaktifligi.Message, w);

                }
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.DumpLog(r);
                }
                MessageBox.Show("Kullanıcı bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + kullaniciaktifligi.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            Application.Exit();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        new
        #region forumharaket
        int Move;
        int Mouse_X;
        int Mouse_Y;
        private void Yeni_Kullanici_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
            this.Cursor = Cursors.Default;
        }
        private void Yeni_Kullanici_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
            this.Cursor = Cursors.SizeAll;
        }

        private void Yeni_Kullanici_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }
        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
            this.Cursor = Cursors.Default;

        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
            this.Cursor = Cursors.SizeAll;
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }
        #endregion

        private void isim_textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            provider.Clear();
            if (e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
            {
                provider.SetError(isim_textbox, "Kullanıcı ismine sadece harf yazabilirsiniz.");
            }
            else { }
        }

        private void soyisim_textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            provider.Clear();
            if (e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
            {
                provider.SetError(soyisim_textbox, "Kullanıcı soyismine sadece harf yazabilirsiniz.");
            }
            else { }
        }

        private void statü_textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            provider.Clear();
            if (e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
            {
                provider.SetError(statü_textbox, "Kullanıcı statü sadece harf yazabilirsiniz.");
            }
            else { }
        }

        private void kadi_textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            provider.Clear();
            if (e.KeyChar == '£' || e.KeyChar == '½' ||
              e.KeyChar == '€' || e.KeyChar == '₺' ||
              e.KeyChar == '¨' || e.KeyChar == 'æ' ||
              e.KeyChar == 'ß' || e.KeyChar == '´')
            {
                e.Handled = true;
                provider.SetError(kadi_textbox, "Kullanıcı adın'a özel karakter giriş yapmazsınız.");
            }
            if ((int)e.KeyChar >= 33 && (int)e.KeyChar <= 47)
            {
                e.Handled = true;
                provider.SetError(kadi_textbox, "Kullanıcı adın'a özel karakter giriş yapmazsınız.");
            }
            if ((int)e.KeyChar >= 58 && (int)e.KeyChar <= 64)
            {
                e.Handled = true;
                provider.SetError(kadi_textbox, "Kullanıcı adın'a özel karakter giriş yapmazsınız.");
            }
            if ((int)e.KeyChar >= 91 && (int)e.KeyChar <= 96)
            {
                e.Handled = true;
                provider.SetError(kadi_textbox, "Kullanıcı adın'a özel karakter giriş yapmazsınız.");
            }
            if ((int)e.KeyChar >= 123 && (int)e.KeyChar <= 127)
            {
                e.Handled = true;
                provider.SetError(kadi_textbox, "Kullanıcı adın'a özel karakter giriş yapmazsınız.");
            }
        }
        private const string MatchEmailPattern =
                      @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
               + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
                                                [0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
               + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
                                                [0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
               + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
        private void olustur_btn_Click(object sender, EventArgs e)
        {
            provider.Clear();
            if (isim_textbox.Text == "")
                provider.SetError(isim_textbox, "İsim boş bırakılmaz");
            else if (soyisim_textbox.Text == "")
                provider.SetError(soyisim_textbox, "Soyisim boş bırakılmaz");
            else if (statü_textbox.Text == "")
                provider.SetError(statü_textbox, "Statü boş bırakılmaz");
            else if (cinsiyet_combobox.SelectedIndex == -1)
                provider.SetError(cinsiyet_combobox, "Geçerli bir cinsiyet seçiniz.");
            else if (kadi_textbox.Text == "")
                provider.SetError(kadi_textbox, "K.adı boş bırakılmaz");
            else if (sifre_textbox.Text == "")
                provider.SetError(sifre_textbox, "Şifre boş bırakılmaz");
            else if (sifre_textbox.Text.Length < 5)
                provider.SetError(sifre_textbox, "Daha güvenli bir şifre oluşturunuz.");
            else if (guvenlıksorusu_textbox.Text == "")
                provider.SetError(guvenlıksorusu_textbox, "Güvenlik sorusu boş bırakılmaz");
            else if (eposta_textbox.Text == "")
                provider.SetError(eposta_textbox, "E-posta adresi boş bırakılmaz");
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                SqlCommand kullanicilar = new SqlCommand("select k_adi from kullanicilar where k_adi = '" + kadi_textbox.Text + "'", connection);
                SqlDataReader kullaniciaciklamasi;
                kullaniciaciklamasi = kullanicilar.ExecuteReader();
                SqlCommand epostakontrol = new SqlCommand("select kullanici_eposta from kullanicilar where kullanici_eposta = '" + eposta_textbox.Text + "'", connection);
                SqlDataReader epostakontrolokuyucu;
                epostakontrolokuyucu = epostakontrol.ExecuteReader();

                if (kullaniciaciklamasi.Read())
                    provider.SetError(kadi_textbox, "Bu kullanıcı adı zaten kullanımda, farklı bir kullanıcı adı giriniz.");
                else if (epostakontrolokuyucu.Read())
                    provider.SetError(eposta_textbox, "Bu e-posta adresi zaten kullanımda, farklı bir e-posta adresi giriniz.");
                else
                {
                    bool retVal = false;
                    retVal = Regex.IsMatch(eposta_textbox.Text, MatchEmailPattern);
                    if (retVal)
                    {
                        string yenikullanicikayit = "insert into kullanicilar(kullanici_isim,kullanici_soyisim,kullanici_statü,kullanici_eposta,kullanici_güvenlik_sorusu_cevabı,kullanici_kayit_tarihi,k_adi,sifre,durum,banned,cinsiyet) values " + "" + "(@kullanici_isim,@kullanici_soyisim,@kullanici_statü,@kullanici_eposta,@kullanici_güvenlik_sorusu_cevabı,@kullanici_kayit_tarihi,@k_adi,@sifre,@durum,@banned,@cinsiyet)";
                        SqlCommand yenikullanicikayitkomut = new SqlCommand(yenikullanicikayit, connection);
                        yenikullanicikayitkomut.Parameters.AddWithValue("@kullanici_isim", isim_textbox.Text.Trim());
                        yenikullanicikayitkomut.Parameters.AddWithValue("@kullanici_soyisim", soyisim_textbox.Text.Trim());
                        yenikullanicikayitkomut.Parameters.AddWithValue("@kullanici_statü", statü_textbox.Text.Trim());
                        yenikullanicikayitkomut.Parameters.AddWithValue("@kullanici_eposta", eposta_textbox.Text.Trim());
                        yenikullanicikayitkomut.Parameters.AddWithValue("@kullanici_güvenlik_sorusu_cevabı", guvenlıksorusu_textbox.Text.Trim());
                        yenikullanicikayitkomut.Parameters.AddWithValue("@kullanici_kayit_tarihi", DateTime.Now.ToString());
                        yenikullanicikayitkomut.Parameters.AddWithValue("@k_adi", kadi_textbox.Text.Trim());
                        yenikullanicikayitkomut.Parameters.AddWithValue("@sifre", sifre_textbox.Text.Trim());
                        yenikullanicikayitkomut.Parameters.AddWithValue("@durum", 0);
                        yenikullanicikayitkomut.Parameters.AddWithValue("@banned", 0);
                        if (cinsiyet_combobox.SelectedItem == "Erkek")
                            yenikullanicikayitkomut.Parameters.AddWithValue("@cinsiyet", "E");
                        else if (cinsiyet_combobox.SelectedItem == "Kadın")
                            yenikullanicikayitkomut.Parameters.AddWithValue("@cinsiyet", "K");
                        else
                            yenikullanicikayitkomut.Parameters.AddWithValue("@cinsiyet", "Belirtilmemiş");

                        yenikullanicikayitkomut.ExecuteNonQuery();
                        MessageBox.Show("Yeni kullanıcı başarılı bir şekilde oluşturuldu.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                            Kullanıcı_girisi.Log(isim_textbox.Text + " " + soyisim_textbox.Text + " isimli " + kadi_textbox.Text + " kullanıcı adı ile " + kayit_tarihi_textbox.Text + " tarihinde yeni kullanıcı olarak oluşturuldu.", w);
                        using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                            Kullanıcı_girisi.DumpLog(r);

                        isim_textbox.Clear();
                        soyisim_textbox.Clear();
                        statü_textbox.Clear();
                        cinsiyet_combobox.SelectedIndex = -1;
                        kadi_textbox.Clear();
                        sifre_textbox.Clear();
                        guvenlıksorusu_textbox.Clear();
                        eposta_textbox.Clear();
                    }
                    else
                        provider.SetError(eposta_textbox, "Geçerli bir e-posta adresi giriniz.");
                }
                connection.Close();
            }
            catch (Exception hata)
            {
                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                    Kullanıcı_girisi.Log("Kayıt yapılmadı, bağlantı kesildi.\nHata kodu: " + hata.Message, w);
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                    Kullanıcı_girisi.DumpLog(r);

                MessageBox.Show("Kayıt yapılmadı, bağlantı kesildi.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void olustur_btn_MouseMove(object sender, MouseEventArgs e)
        {
            olustur_btn.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void olustur_btn_MouseLeave(object sender, EventArgs e)
        {
            olustur_btn.BackColor = Color.FromArgb(22, 53, 56);
        }

        private void logout_label_MouseMove(object sender, MouseEventArgs e)
        {
            logout_label.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void logout_label_MouseLeave(object sender, EventArgs e)
        {
            logout_label.BackColor = Color.FromArgb(22, 53, 56);
        }

        private void label3_MouseMove(object sender, MouseEventArgs e)
        {
            label3.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void label3_MouseLeave(object sender, EventArgs e)
        {
            label3.BackColor = Color.FromArgb(22, 53, 56);
        }
    }
}
