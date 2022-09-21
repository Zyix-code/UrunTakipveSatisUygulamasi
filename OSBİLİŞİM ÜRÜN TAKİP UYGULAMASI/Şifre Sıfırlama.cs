using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Diagnostics;
using System.IO;
using System.Net.Mime;
using System.Drawing;

namespace OSBilişim
{
    public partial class Sifresıfırlamaforum : Form
    {
        public Sifresıfırlamaforum()
        {
            InitializeComponent();
        }
        ErrorProvider provider = new ErrorProvider();
        private void Logout_label_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Label3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://mail.google.com/");
        }
        public string güvenliksorusucevabı;
        string onaykodu, eposta = "deneme@gmail.com";
        string kullanicieskisifre;
        private void Btn_giris_Click(object sender, EventArgs e)
        {
            provider.Clear();
            if (yenisifretextbox.Text == yenisifretekrartextbox.Text)
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                SqlCommand komut3 = new SqlCommand("SELECT * FROM kullanicilar where k_adi ='" + kullanıcıadıtextbox.Text + "'", connection);
                SqlDataReader veriokuyucu3;
                veriokuyucu3 = komut3.ExecuteReader();
                while (veriokuyucu3.Read())
                {
                    güvenliksorusucevabı = veriokuyucu3["kullanici_güvenlik_sorusu_cevabı"].ToString();
                    kullanicieskisifre = veriokuyucu3["sifre"].ToString();
                }
                veriokuyucu3.Close();
                if (güvenliksorusutextbox.Text == güvenliksorusucevabı)
                {
                    if (onaykodu == güvenlikonaykodutextbox.Text)
                    {
                        if (kullanicieskisifre == yenisifretextbox.Text)
                        { provider.SetError(yenisifretextbox, "Girdiğiniz şifre eski şifreniz ile uyuşmaktadır. Lütfen bu şifre ile giriş sağlayınız."); }
                        else if (kullanicieskisifre == yenisifretekrartextbox.Text)
                        { provider.SetError(yenisifretekrartextbox, "Girdiğiniz şifre eski şifreniz ile uyuşmaktadır. Lütfen bu şifre ile giriş sağlayınız.");  }
                        else
                        {
                            SqlCommand ürümdurumunugüncelle = new SqlCommand("update kullanicilar set sifre= '" + yenisifretextbox.Text + "' where k_adi = '" + kullanıcıadıtextbox.Text + "'", connection);
                            ürümdurumunugüncelle.ExecuteNonQuery();
                            MessageBox.Show("Şifreniz başarılı şekilde sıfırlanmıştır, lütfen tekrar giriş yapınız.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Onaykoduolustur();
                        }
                    }
                    else { provider.SetError(güvenlikonaykodutextbox, "Girdiğiniz onay kodu yanlıştır.");}
                }
                else { provider.SetError(güvenliksorusutextbox, "Girdiğiniz güvenlik sorusu cevabı yanlıştır."); }
            }
            else { provider.SetError(yenisifretextbox,"Girdiğiniz şifreler birbiri ile uyuşmamaktadır kontrol ediniz."); provider.SetError(yenisifretekrartextbox, "Girdiğiniz şifreler birbiri ile uyuşmamaktadır kontrol ediniz."); }
        }
        readonly SqlConnection connection = new SqlConnection("Data Source=192.168.1.123,1433;Network Library=DBMSSOCN; Initial Catalog=OSBİLİSİM;User Id=Admin; Password=1; MultipleActiveResultSets=True;");
        private void Sifresıfırlamaforum_Load(object sender, EventArgs e)
        {
            yenisifretekrartextbox.UseSystemPasswordChar = true;
            yenisifretextbox.UseSystemPasswordChar = true;
            Kullanicigirisiform Kullanicigirisiform = new Kullanicigirisiform();

            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                string programdurumu = "";
                FileVersionInfo programversion = FileVersionInfo.GetVersionInfo(@"OSBilişim.exe");
                SqlCommand üründurum = new SqlCommand("select version,versiyon_aciklama,yeni_program_indirme_linki,program_durumu,program_arizali,program_kapali,program_bakim,program_zamanli_bakim,program_zamanli_bakim_bitis from version", connection);
                SqlDataReader üründurumusorgulama;
                üründurumusorgulama = üründurum.ExecuteReader();
                while (üründurumusorgulama.Read())
                {

                    Kullanicigirisiform.güncelversiyon = (string)üründurumusorgulama["version"];
                    programdurumu = (string)üründurumusorgulama["program_durumu"];

                    if (programdurumu == "Arızalı")
                    {
                        MessageBox.Show(((string)üründurumusorgulama["program_arizali"]), "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Environment.Exit(0);
                    }
                    else if (programdurumu == "Kapalı")
                    {
                        MessageBox.Show((string)üründurumusorgulama["program_kapali"], "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Environment.Exit(0);
                    }
                    else if (programdurumu == "Zamanlı Bakım")
                    {
                        DateTime baslangic = DateTime.Now;
                        DateTime bitis = (DateTime)üründurumusorgulama["program_zamanli_bakim_bitis"];
                        TimeSpan kalanzaman = bitis - baslangic;
                        MessageBox.Show(((string)üründurumusorgulama["program_zamanli_bakim"]) + "\n Kalan zaman: " + kalanzaman.Days + " gün " + kalanzaman.Hours + " saat " + kalanzaman.Seconds + " saniye kalmıştır.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Environment.Exit(0);
                    }
                    else if (programdurumu == "Bakım")
                    {
                        MessageBox.Show(((string)üründurumusorgulama["program_bakim"]), "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Environment.Exit(0);
                    }
                    else
                    {
                        if (Convert.ToInt32(Kullanicigirisiform.güncelversiyon) >= Convert.ToInt32(programversion.FileVersion))
                        {
                            DialogResult dialog = MessageBox.Show("Uygulamanızın yeni sürümünü indirmek ister misiniz?", "OS BİLİŞİM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialog == DialogResult.Yes)
                            {
                                string dosya_dizini = AppDomain.CurrentDomain.BaseDirectory.ToString() + "OSUpdate.exe";
                                File.WriteAllBytes(@"OSUpdate.exe", new WebClient().DownloadData("http://192.168.1.123/Update/OSUpdate.exe"));
                                Process.Start("OSUpdate.exe");
                                System.Threading.Thread.Sleep(1000);
                                Environment.Exit(0);
                            }
                            else
                            {
                                MessageBox.Show("Uygulamanızı güncellemediğiniz için, program çalışmayacaktır.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Application.Exit();
                            }
                        }
                    }
                }
                connection.Close();
            }
            catch (Exception hata)
            {
                MessageBox.Show("Program başlatılmadı.\nİnternet bağlantınızı ya da server bağlantınızı kontrol edin.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
            private void Sifremiunuttumlinklabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            provider.Clear();
            try
            {
                SmtpClient sc = new SmtpClient
                {
                    Port = 587,
                    Host = "delta.veriyum.net",
                    EnableSsl = true
                };
                if (kullanıcıadıtextbox.Text == "")
                { provider.SetError(kullanıcıadıtextbox, "Kullanıcı adını boş bırakmayınız."); }
                else
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    SqlDataReader kullanıcısorgula;
                    SqlCommand kullanıcıarama = new SqlCommand
                    {
                        Connection = connection,
                        CommandText = "Select * From kullanicilar where k_adi='" + kullanıcıadıtextbox.Text + "'"
                    };

                    kullanıcısorgula = kullanıcıarama.ExecuteReader();
                    if (kullanıcısorgula.Read())
                    {
                        kullanıcısorgula.Close();

                        SqlCommand komut3 = new SqlCommand("SELECT * FROM kullanicilar where k_adi ='" + kullanıcıadıtextbox.Text + "'", connection);
                        SqlDataReader veriokuyucu3;
                        veriokuyucu3 = komut3.ExecuteReader();
                        while (veriokuyucu3.Read())
                        {
                            eposta = veriokuyucu3["kullanici_eposta"].ToString();
                            güvenliksorusucevabı = veriokuyucu3["kullanici_güvenlik_sorusu_cevabı"].ToString();
                        }
                        veriokuyucu3.Close();
                        string kime = eposta;
                        string konu = "OSBİLİŞİM - Güvenlik Sorusu";
                        sc.Credentials = new NetworkCredential("teknik@trentatek.com.tr", "M9H8zRS3!");
                        MailMessage mail = new MailMessage
                        {
                            From = new MailAddress("teknik@trentatek.com.tr", "OS BİLİŞİM")
                        };
                        mail.To.Add(kime);
                        mail.Subject = konu;
                        mail.IsBodyHtml = true;
                        string htmlString =
                            " <html>" +
                            " <head>" +
                            " <meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                            " <style type='text/css'>" +
                            " .sayfa" +
                            " {" +
                            " background: white;" +
                            " text-align:center;" +
                            " width: 605px;" +
                            " border: solid 2px solid black;" +
                            " border-radius: 5px;" +
                            " font: small/1.5 Arial,Helvetica,sans-serif;" +
                            " font-weight: bold;" +
                            " }" +
                            " .fotograf" +
                            " {" +
                            " text-align:left;" +
                            " margin-bottom: -200px;" +
                            " height: 0px;" +
                            " }" +
                            " .üstalan" +
                            " {" +
                            " background: #3ea2e6;" +
                            " height:2px;" +
                            " color: #3ea2e6;" +
                            " border-radius: 5px;" +
                            " }" +
                            " .teknikservis" +
                            " {" +
                            " border-left: 4px solid #3ea2e6;" +
                            " height: 180px;" +
                            " margin-left: 312px;" +
                            " margin-bottom: 20px;" +
                            " }" +
                            " .altalan" +
                            " {" +
                            " background: #3ea2e6;" +
                            " height:2px;" +
                            " color: #3ea2e6;" +
                            " border-radius: 5px;" +
                            " color: red;" +
                            " }" +
                            " .tarih" +
                            " {" +
                            " text-align:right;" +
                            " margin-bottom: -20px;" +
                            " }" +
                            " </style>" +
                            " </head>" +
                            " <body>" +
                            " <div class='sayfa'>" +
                            " <div class='üstalan'> </div>" +
                            " <div class='tarih'>" +
                            " <p>Tarih: " + DateTime.Now + " </p>" +
                            " </div>" +
                            " <div class= 'fotograf'>" +
                            " <img style = ' margin-left: -430px; ' src=\"https://www.osbilisim.com.tr/wp-content/uploads/2021/05/footer.png\" />" +
                            " </div>" +
                            " <div class='teknikservis'>" +
                            " <p style = 'font-size: 19px; text-align: center;  padding-top: 70px;' > SELÇUK ŞAHİN <br> teknik@trentatek.com.tr</br> </p>" +
                            " </div> " +
                            " <p> " + kullanıcıadıtextbox.Text + " adlı kullanıcı güvenlik sorusu cevabı talebinde bulunmuştur.<br>Güvenlik sorusu cevabınız: " + güvenliksorusucevabı + " </br></p> " +
                            " <div class='altalan'></div>" +
                            " <p>" +
                            " Bu e-posta otomatik oluşturulmuştur. Lütfen cevap vermeyiniz." +
                            " </p>" +
                            " </div>" +
                            " </body>" +
                            " </html>";
                        mail.Body = htmlString;
                        sc.Send(mail);
                        MessageBox.Show("Kullanıcı adınıza ait e-mail adresini kontrol ediniz, güvenlik sorusu cevabı gönderilmiştir.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else { provider.SetError(kullanıcıadıtextbox, "Böyle bir kullanıcı bulunamadı, tekrar deneyiniz."); }
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Güvenlik sorusu cevabı talebiniz oluşturulmadı, bir hata oluştu lütfen daha sonra tekrar deneyiniz.\nİnternet bağlantınızı ya da server bağlantınızı kontrol edin.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            connection.Close();
        }

        private void Sifre_goster_gizle_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (sifre_goster_gizle_checkbox.Checked == true)
            {
                yenisifretextbox.UseSystemPasswordChar = false;
                yenisifretekrartextbox.UseSystemPasswordChar = false;
            }
            else
            {
                yenisifretextbox.UseSystemPasswordChar = true;
                yenisifretekrartextbox.UseSystemPasswordChar = true;
            }
        }
        public void Onaykoduolustur()
        {
            Random random = new Random();
            int s1, s2, s3, s4;
            int h1, h2, h3;
            s1 = random.Next(1, 10);
            s2 = random.Next(10, 20);
            s3 = random.Next(20, 30);
            s4 = random.Next(30, 40);
            h1 = random.Next(65, 91);
            h2 = random.Next(65, 91);
            h3 = random.Next(65, 91);
            char k1, k2, k3;
            k1 = Convert.ToChar(h1);
            k2 = Convert.ToChar(h2);
            k3 = Convert.ToChar(h3);
            onaykodu = s1.ToString() + s2.ToString() + k1 + s3.ToString() + k2 + s4.ToString() + k3;
        }
        private void LinkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://mail.google.com/");
        }
        private void Btn_onaykodugönder_Click(object sender, EventArgs e)
        {
            provider.Clear();
            try
            {
                Onaykoduolustur();
                SmtpClient sc = new SmtpClient
                {
                    Port = 587,
                    Host = "delta.veriyum.net",
                    EnableSsl = true
                };
                if (kullanıcıadıtextbox.Text == "")
                { provider.SetError(kullanıcıadıtextbox, "Kullanıcı adını boş bırakmayınız."); }
                else
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    SqlDataReader kullanıcısorgula;
                    SqlCommand kullanıcıarama = new SqlCommand
                    {
                        Connection = connection,
                        CommandText = "Select * From kullanicilar where k_adi='" + kullanıcıadıtextbox.Text + "'"
                    };

                    kullanıcısorgula = kullanıcıarama.ExecuteReader();
                    if (kullanıcısorgula.Read())
                    {
                        kullanıcısorgula.Close();

                        SqlCommand komut3 = new SqlCommand("SELECT * FROM kullanicilar where k_adi ='" + kullanıcıadıtextbox.Text + "'", connection);
                        SqlDataReader veriokuyucu3;
                        veriokuyucu3 = komut3.ExecuteReader();
                        while (veriokuyucu3.Read())
                        {
                            eposta = veriokuyucu3["kullanici_eposta"].ToString();

                        }
                        veriokuyucu3.Close();
                        string kime = eposta;
                        string konu = "OSBİLİŞİM - Parola Sıfırlama";

                        sc.Credentials = new NetworkCredential("teknik@trentatek.com.tr", "M9H8zRS3!");
                        MailMessage mail = new MailMessage
                        {
                            From = new MailAddress("teknik@trentatek.com.tr", "OS BİLİŞİM")
                        };
                        mail.To.Add(kime);
                        mail.Subject = konu;
                        mail.IsBodyHtml = true;
                        string htmlString = "<html>" +
                            " <head>" +
                            " <meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                            " <style type='text/css'>" +
                            " .sayfa" +
                            " {" +
                            " background: white;" +
                            " text-align:center;" +
                            " width: 605px;" +
                            " border: solid 2px solid black;" +
                            " border-radius: 5px;" +
                            " font: small/1.5 Arial,Helvetica,sans-serif;" +
                            " font-weight: bold;" +
                            " }" +
                            " .fotograf" +
                            " {" +
                            " text-align:left;" +
                            " margin-bottom: -200px;" +
                            " height: 0px;" +
                            " }" +
                            " .üstalan" +
                            " {" +
                            " background: #3ea2e6;" +
                            " height:2px;" +
                            " color: #3ea2e6;" +
                            " border-radius: 5px;" +
                            " }" +
                            " .teknikservis" +
                            " {" +
                            " border-left: 4px solid #3ea2e6;" +
                            " height: 180px;" +
                            " margin-left: 312px;" +
                            " margin-bottom: 20px;" +
                            " }" +
                            " .altalan" +
                            " {" +
                            " background: #3ea2e6;" +
                            " height:2px;" +
                            " color: #3ea2e6;" +
                            " border-radius: 5px;" +
                            " color: red;" +
                            " }" +
                            " .tarih" +
                            " {" +
                            " text-align:right;" +
                            " margin-bottom: -20px;" +
                            " }" +
                            " </style>" +
                            " </head>" +
                            " <body>" +
                            " <div class='sayfa'>" +
                            " <div class='üstalan'> </div>" +
                            " <div class='tarih'>" +
                            " <p>Tarih: " + DateTime.Now + " </p>" +
                            " </div>" +
                            " <div class= 'fotograf'>" +
                            " <img style = ' margin-left: -430px; ' src=\"https://www.osbilisim.com.tr/wp-content/uploads/2021/05/footer.png\" />" +
                            " </div>" +
                            " <div class='teknikservis'>" +
                            " <p style = 'font-size: 19px; text-align: center;  padding-top: 70px;' > SELÇUK ŞAHİN <br> teknik@trentatek.com.tr</br> </p>" +
                            " </div> " +
                            " <p> " + kullanıcıadıtextbox.Text + " adlı kullanıcı şifre sıfırlama talebinde bulunmuştur.<br>Şifre sıfırlama onay kodunuz: " + onaykodu + " </br></p> " +
                            " <div class='altalan'></div>" +
                            " <p>" +
                            " Bu e-posta otomatik oluşturulmuştur. Lütfen cevap vermeyiniz." +
                            " </p>" +
                            " </div>" +
                            " </body>" +
                            " </html>";
                        mail.Body = htmlString;
                        sc.Send(mail);
                        MessageBox.Show("Kullanıcı adınıza ait e-mail adresini kontrol ediniz, onay kodu gönderilmiştir.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else { provider.SetError(kullanıcıadıtextbox, "Böyle bir kullanıcı bulunamadı, tekrar deneyiniz."); }
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Şifre sıfırlama talebiniz oluşturulurken bir hata oluştu lütfen daha sonra tekrar deneyiniz.\nİnternet bağlantınızı ya da server bağlantınızı kontrol edin.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            connection.Close();
        }

        new
        #region forumharaket
        int Move;
        int Mouse_X;
        int Mouse_Y;
        private void Sifresıfırlamaforum_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
            this.Cursor = Cursors.Default;
        }

        private void Sifresıfırlamaforum_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }

        private void Sifresıfırlamaforum_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
            this.Cursor = Cursors.SizeAll;
        }


        private void Panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }

        private void Panel2_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
            this.Cursor = Cursors.Default;
        }

        private void Panel2_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
            this.Cursor = Cursors.SizeAll;
        }
        #endregion
        #region renkayarları
        private void Btn_sıfırla_MouseMove(object sender, MouseEventArgs e)
        {
            btn_sıfırla.BackColor = Color.DarkGreen;
            btn_sıfırla.ForeColor = Color.Black;
        }

        private void Btn_onaykodugönder_MouseMove(object sender, MouseEventArgs e)
        {
            btn_onaykodugönder.BackColor = Color.DarkGreen;
            btn_onaykodugönder.ForeColor = Color.Black;
        }

        private void Btn_onaykodugönder_MouseLeave(object sender, EventArgs e)
        {
            btn_onaykodugönder.BackColor = Color.MediumSeaGreen;
            btn_onaykodugönder.ForeColor = Color.White;
        }

        private void Btn_sıfırla_MouseLeave(object sender, EventArgs e)
        {
            btn_sıfırla.BackColor = Color.MediumSeaGreen;
            btn_sıfırla.ForeColor = Color.White;
        }
        private void Logout_label_MouseMove(object sender, MouseEventArgs e)
        {
            logout_label.ForeColor = Color.Black;
        }

        private void Label3_MouseMove(object sender, MouseEventArgs e)
        {
            label3.ForeColor = Color.Black;
        }
        private void Label3_MouseLeave(object sender, EventArgs e)
        {
            label3.ForeColor = Color.Gray;
        }

        private void linkLabel1_MouseLeave(object sender, EventArgs e)
        {
            linkLabel1.LinkColor = Color.MediumSeaGreen;
        }

        private void linkLabel1_MouseMove(object sender, MouseEventArgs e)
        {
            linkLabel1.LinkColor = Color.DarkGreen;
        }
        private void şifremiunuttumlinklabel_MouseLeave(object sender, EventArgs e)
        {
            şifremiunuttumlinklabel.LinkColor = Color.MediumSeaGreen;
        }

        private void şifremiunuttumlinklabel_MouseMove(object sender, MouseEventArgs e)
        {
            şifremiunuttumlinklabel.LinkColor = Color.DarkGreen;
        }

        private void Logout_label_MouseLeave(object sender, EventArgs e)
        {
            logout_label.ForeColor = Color.Gray;
        }
        #endregion
    }
}