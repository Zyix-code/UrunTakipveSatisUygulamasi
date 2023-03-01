using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using System.Net;
using com.itextpdf.text.pdf;
using System.Globalization;

namespace OSBilişim
{
    public partial class Anaform : Form
    {
        public static string isim, statü;
        ErrorProvider provider = new ErrorProvider();
        public Anaform()
        {
            InitializeComponent();
        }

        readonly SqlConnection connection = new SqlConnection("Data Source=192.168.1.110,1433;Network Library=DBMSSOCN; Initial Catalog=OSBİLİSİM;User Id=Admin; Password=1; MultipleActiveResultSets=True;");
        private void Anaform_Load(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            isim = isim_label.Text;
            statü = statü_label.Text;
            Kullanıcı_girisi Kullanıcı_girisi = new Kullanıcı_girisi();
            timer1.Start();
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
                    
                    Kullanıcı_girisi.güncelversiyon = (string)üründurumusorgulama["version"];
                    programdurumu = (string)üründurumusorgulama["program_durumu"];

                    if (programdurumu == "Arızalı")
                    {
                        MessageBox.Show(((string)üründurumusorgulama["program_arizali"]), "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Application.Exit();
                    }
                    else if (programdurumu == "Kapalı")
                    {
                        MessageBox.Show((string)üründurumusorgulama["program_kapali"], "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                    else if (programdurumu == "Zamanlı Bakım")
                    {
                        DateTime baslangic = DateTime.Now;
                        DateTime bitis = (DateTime)üründurumusorgulama["program_zamanli_bakim_bitis"];
                        TimeSpan kalanzaman = bitis - baslangic;
                        MessageBox.Show(((string)üründurumusorgulama["program_zamanli_bakim"]) + "\n Kalan zaman: " + kalanzaman.Days + " gün " + kalanzaman.Hours + " saat " + kalanzaman.Seconds + " saniye kalmıştır.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Application.Exit();
                    }
                    else if (programdurumu == "Bakım")
                    {
                        MessageBox.Show(((string)üründurumusorgulama["program_bakim"]), "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Application.Exit();
                    }
                    else
                    {
                        if (Convert.ToInt32(Kullanıcı_girisi.güncelversiyon) >= Convert.ToInt32(programversion.FileVersion))
                        {
                            DialogResult dialog = MessageBox.Show("Uygulama'nın yeni sürümünü indirmek ister misiniz?", "OS BİLİŞİM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialog == DialogResult.Yes)
                            {
                                string dosya_dizini = AppDomain.CurrentDomain.BaseDirectory.ToString() + "OSUpdate.exe";
                                File.WriteAllBytes(@"OSUpdate.exe", new WebClient().DownloadData("http://192.168.1.110/Update/OSUpdate.exe"));
                                Process.Start("OSUpdate.exe");
                                System.Threading.Thread.Sleep(1000);
                                Application.Exit();
                            }
                            else
                            {
                                MessageBox.Show("Uygulama eski sürümü ile çalışmayacaktır, yeni sürümünü indiriniz.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Application.Exit();
                            }
                        }
                    }
                }
                connection.Close();
            }
            catch (Exception hata)
            {
                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.Log("Bağlantı kesildi.\nHata kodu: " + hata.Message, w);
                }
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.DumpLog(r);
                }
                MessageBox.Show("Bağlantı kesildi.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                string kullanıcıadı = Kullanıcı_girisi.username.ToLower();
                kullanıcıadı = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kullanıcıadı);
                SqlCommand kullanicikomut = new SqlCommand("Select durum, kullanici_isim, kullanici_statü FROM kullanicilar", connection);
                SqlDataReader aktifkullanici;
                SqlCommand kullanicikontrolsql = new SqlCommand("SELECT kullanici_isim FROM kullanicilar where k_adi ='" + kullanıcıadı + "'", connection);
                SqlDataReader kullanicikontrolveri;
                aktifkullanici = kullanicikomut.ExecuteReader();
                aktifkullanicilar_listbox.Items.Clear();
                while (aktifkullanici.Read())
                {
                    if ((string)aktifkullanici["durum"] == "1")
                        aktifkullanicilar_listbox.Items.Add(aktifkullanici["kullanici_isim"] + " (" + (string)aktifkullanici["kullanici_statü"] + ")" + " Çevrimiçi".ToString());
                    else if ((string)aktifkullanici["durum"] == "0")
                        aktifkullanicilar_listbox.Items.Add(aktifkullanici["kullanici_isim"] + " (" + (string)aktifkullanici["kullanici_statü"] + ")" + " Çevrimdışı".ToString());
                    else
                        aktifkullanicilar_listbox.Items.Add(aktifkullanici["kullanici_isim"] + " (" + (string)aktifkullanici["kullanici_statü"] + ")" + " Çevrimdışı".ToString());
                    
                    kullanicikontrolveri = kullanicikontrolsql.ExecuteReader();
                    while (kullanicikontrolveri.Read())
                    {
                        string kendikullaniciisminikaldirma = kullanicikontrolveri["kullanici_isim"] + " (" + (string)aktifkullanici["kullanici_statü"] + ")" + " Çevrimiçi";
                        if (aktifkullanicilar_listbox.Items.Contains(kendikullaniciisminikaldirma))
                            aktifkullanicilar_listbox.Items.Remove(kendikullaniciisminikaldirma);
                    }
                    kullanicikontrolveri.Close();

                }
                if (aktifkullanicilar_listbox.Items.Count < 1)
                    aktifkullanicilar_listbox.Items.Add("Aktif kullanıcı yoktur.");
               
                aktifkullanici.Close();
                SqlCommand kullanicilar = new SqlCommand("select * from kullanicilar where k_adi = '" + Kullanıcı_girisi.username + "'", connection);
                SqlDataReader kullaniciaciklamasi;
                kullaniciaciklamasi = kullanicilar.ExecuteReader();
                while (kullaniciaciklamasi.Read())
                {
                    isim_label.Text = (string)kullaniciaciklamasi["kullanici_isim"];
                    soyisim_label.Text = (string)kullaniciaciklamasi["kullanici_soyisim"];
                    statü_label.Text = (string)kullaniciaciklamasi["kullanici_statü"];
                    kayit_tarihi_label.Text = (string)kullaniciaciklamasi["kullanici_kayit_tarihi"];
                    if ((string)kullaniciaciklamasi["cinsiyet"] == "E")
                    pictureBox1.Image = Properties.Resources.teknikservis_man;
                    else if ((string)kullaniciaciklamasi["cinsiyet"] == "K")
                        pictureBox1.Image = Properties.Resources.teknikservis_gril;
                    else pictureBox1.Image = Properties.Resources.person_icon_green;

                    string kullanici;
                    kullanici = Kullanıcı_girisi.username.ToLower();
                    if (kullanici == "admin")
                    {
                        isim_label.Location = new Point(60, 220);
                        label.Location = new Point(12, 220);
                        soyisim_label.Visible = false;
                        label_6.Visible = false;
                        yenikullanici_btn.Visible = true;
                        ürün_düzenleme_btn.Visible = true;
                        yenikullanici_btn.Location = new Point(474, 384);
                        ürün_düzenleme_btn.Location = new Point(591, 384);
                    }
                    else if (statü_label.Text == "Teknik Görevli" || statü_label.Text == "Satış Temsilcisi" || statü_label.Text == "Müdür")
                    {
                        yenikullanici_btn.Visible = false;
                        ürün_düzenleme_btn.Visible = true;
                        yenikullanici_btn.Location = new Point(534, 435);
                        ürün_düzenleme_btn.Location = new Point(534, 384);
                    }
                    else
                    {
                        yenikullanici_btn.Visible = false;
                        ürün_düzenleme_btn.Visible = false;
                        yenikullanici_btn.Location = new Point(474, 384);
                        ürün_düzenleme_btn.Location = new Point(591, 384);
                    }

                }
                connection.Close();
            }
            catch (Exception hata)
            {
                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.Log("Kullanıcı bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + hata.Message, w);
                }
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.DumpLog(r);
                }
                MessageBox.Show("Kullanıcı bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            tarih_label.Text = DateTime.Now.ToLongDateString();
            splashScreenManager1.CloseWaitForm();
        }
        private void Anaform_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    Kullanıcı_girisi Kullanıcı_girisi = new Kullanıcı_girisi();
                    SqlCommand kullanicidurumgüncelle = new SqlCommand("Update kullanicilar set durum='" + 0 + "' where k_adi = '" + Kullanıcı_girisi.username + "'", connection);
                    kullanicidurumgüncelle.ExecuteNonQuery();
                }
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
            foreach (var process in Process.GetProcessesByName("OSBilişim"))
            {
                process.Kill();
            }
        }
        private void Logout_label_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                Kullanıcı_girisi Kullanıcı_girisi = new Kullanıcı_girisi();
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
        private void Label3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
          Process.Start("https://mail.google.com/");
        }
        private void Btn_cikis_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    Kullanıcı_girisi Kullanıcı_girisi = new Kullanıcı_girisi();
                    SqlCommand kullanicidurumgüncelle = new SqlCommand("Update kullanicilar set durum='" + 0 + "' where k_adi = '" + Kullanıcı_girisi.username + "'", connection);
                    kullanicidurumgüncelle.ExecuteNonQuery();
                    Kullanıcı_girisi.Show();
                    Hide();
                }
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
        }

        private void Siparis_olustur_btn_Click(object sender, EventArgs e)
        {
            Siparis_olusturma Siparis_olusturma = new Siparis_olusturma();
            Siparis_olusturma.Show();
            Hide();
        }

        private void Siparis_kontrol_btn_Click(object sender, EventArgs e)
        {
            Siparis_kontrol Siparis_kontrol = new Siparis_kontrol();
            Siparis_kontrol.Show();
            Hide();
        }

        private void ürün_ekle_btn_Click(object sender, EventArgs e)
        {
            Ürün_ekle Ürün_ekle = new Ürün_ekle();
            Ürün_ekle.Show();
            Hide();
        }
        private void diğer_malzeme_grubları_sipariş_oluşturma_Click(object sender, EventArgs e)
        {
            Diger_malzeme_grubları_siparis_olusturma Diger_malzeme_grubları_siparis_olusturma = new Diger_malzeme_grubları_siparis_olusturma();
            Diger_malzeme_grubları_siparis_olusturma.Show();
            Hide();
        }
        private void diğer_malzeme_grubları_ürün_ekle_Click(object sender, EventArgs e)
        {
            Diger_malzeme_grubları_urun_ekle Diger_malzeme_grubları_urun_ekle = new Diger_malzeme_grubları_urun_ekle();
            Diger_malzeme_grubları_urun_ekle.Show();
            Hide();
        }

        new
        #region forumharaketettirme
        int Move;
        int Mouse_X;
        int Mouse_Y;
        private void Panel2_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
            this.Cursor = Cursors.Default;

        }

        private void Panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }

        private void Panel2_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
            this.Cursor = Cursors.SizeAll;
        }
        #endregion
        #region forumharaketettirme2
        private void Anaform_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
            this.Cursor = Cursors.Default;
        }

        private void Anaform_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
            this.Cursor = Cursors.SizeAll;
        }

        private void Anaform_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }

        #endregion
        #region renkayarları
        private void Siparis_olustur_btn_MouseMove(object sender, MouseEventArgs e)
        {
            siparis_olustur_btn.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void Siparis_olustur_btn_MouseLeave(object sender, EventArgs e)
        {
            siparis_olustur_btn.BackColor = Color.FromArgb(22, 53, 56);
        }

        private void Siparis_kontrol_btn_MouseLeave(object sender, EventArgs e)
        {
            siparis_kontrol_btn.BackColor = Color.FromArgb(22, 53, 56);
        }

        private void Siparis_kontrol_btn_MouseMove(object sender, MouseEventArgs e)
        {
            siparis_kontrol_btn.BackColor = Color.FromArgb(13, 31, 33);
           
        }

        private void Ürün_ekle_ve_düzenle_btn_MouseLeave(object sender, EventArgs e)
        {
            ürün_ekle_btn.BackColor = Color.FromArgb(22, 53, 56);
        }

        private void Ürün_ekle_ve_düzenle_btn_MouseMove(object sender, MouseEventArgs e)
        {
            ürün_ekle_btn.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void Diğer_malzeme_grubları_MouseMove(object sender, MouseEventArgs e)
        {
            diğer_malzeme_grubları_sipariş_oluşturma.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void Diğer_malzeme_grubları_MouseLeave(object sender, EventArgs e)
        {
            diğer_malzeme_grubları_sipariş_oluşturma.BackColor = Color.FromArgb(22, 53, 56);
            diğer_malzeme_grubları_sipariş_oluşturma.ForeColor = Color.White;
        }

        private void Diğer_malzeme_grubları_ekle_ve_düzenleme_btn_MouseMove(object sender, MouseEventArgs e)
        {
            diğer_malzeme_grubları_ürün_ekle.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void Diğer_malzeme_grubları_ekle_ve_düzenleme_btn_MouseLeave(object sender, EventArgs e)
        {
            diğer_malzeme_grubları_ürün_ekle.BackColor = Color.FromArgb(22, 53, 56);
        }

        private void Btn_cikis_MouseLeave(object sender, EventArgs e)
        {
            btn_cikis.BackColor = Color.FromArgb(22, 53, 56);
        }
        private void Logout_label_MouseMove(object sender, MouseEventArgs e)
        {
            logout_label.ForeColor = Color.FromArgb(13, 31, 33);
        }

        private void Label3_MouseMove(object sender, MouseEventArgs e)
        {
            label3.ForeColor = Color.FromArgb(13, 31, 33);
        }

        private void Logout_label_MouseLeave(object sender, EventArgs e)
        {
            logout_label.ForeColor = Color.FromArgb(22, 53, 56);
        }

        private void Label3_MouseLeave(object sender, EventArgs e)
        {
            label3.ForeColor = Color.FromArgb(22, 53, 56);
        }

        private void linkLabel1_MouseLeave(object sender, EventArgs e)
        {
            linkLabel1.LinkColor = Color.FromArgb(22, 53, 56);
        }

        private void linkLabel1_MouseMove(object sender, MouseEventArgs e)
        {
            linkLabel1.LinkColor = Color.FromArgb(13, 31, 33);
        }
        private void Btn_cikis_MouseMove(object sender, MouseEventArgs e)
        {
            btn_cikis.BackColor = Color.FromArgb(13, 31, 33);

        }
        #endregion

        private void yenikullanici_btn_Click(object sender, EventArgs e)
        {
            Yeni_kullanici Yeni_kullanici = new Yeni_kullanici();
            Yeni_kullanici.Show();
        }

        private void yenikullanici_btn_MouseLeave(object sender, EventArgs e)
        {
            yenikullanici_btn.BackColor = Color.FromArgb(22, 53, 56);
        }

        private void yenikullanici_btn_MouseMove(object sender, MouseEventArgs e)
        {
            yenikullanici_btn.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void ürün_düzenleme_btn_MouseLeave(object sender, EventArgs e)
        {
            ürün_düzenleme_btn.BackColor = Color.FromArgb(22, 53, 56);
        }

        private void ürün_düzenleme_btn_MouseMove(object sender, MouseEventArgs e)
        {
            ürün_düzenleme_btn.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void ürün_düzenleme_btn_Click(object sender, EventArgs e)
        {
            Ürün_İsimleri_Düzenleme ürün_İsimleri_Düzenleme = new Ürün_İsimleri_Düzenleme();
            ürün_İsimleri_Düzenleme.Show();
            Hide();
        }

        private void aktifkullanicilar_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            provider.Clear();
            ToolTip Aciklama = new ToolTip();
            string cinsiyetsorgulama = " ";
           
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                if (aktifkullanicilar_listbox.SelectedIndex == -1)
                {
                    provider.SetError(aktifkullanicilar_listbox,"Geçerli bir kullanıcı seçiniz.");
                    Aciklama.SetToolTip(aktifkullanicilar_listbox, "Kullanıcı bulunamadı.");
                }
                else
                {
                    string kullanıcı = aktifkullanicilar_listbox.SelectedItem.ToString();
                    string aranan = kullanıcı.Substring(0, kullanıcı.IndexOf(' '));
                    SqlCommand kullanicilar = new SqlCommand("select * from  kullanicilar where kullanici_isim = '" + aranan + "'", connection);
                    SqlDataReader kullaniciaciklamasi;
                    kullaniciaciklamasi = kullanicilar.ExecuteReader();
                    while (kullaniciaciklamasi.Read())
                    {
                        if ((string)kullaniciaciklamasi["cinsiyet"] == "E")
                            cinsiyetsorgulama = "Erkek";
                        else if ((string)kullaniciaciklamasi["cinsiyet"] == "K")
                            cinsiyetsorgulama = "Kadın";
                        else
                            cinsiyetsorgulama = "Belirtilmemiş";
                        Aciklama.SetToolTip(aktifkullanicilar_listbox,
                            "Adı: " + (string)kullaniciaciklamasi["kullanici_isim"] +
                            "\nSoyadı: " + (string)kullaniciaciklamasi["kullanici_soyisim"] +
                            "\nStatü: " + (string)kullaniciaciklamasi["kullanici_statü"] +
                            "\nCinsiyet: " + cinsiyetsorgulama +
                            "\nKayıt tarihi: " + (string)kullaniciaciklamasi["kullanici_kayit_tarihi"]);

                    }
                }
            }
            catch (Exception hata)
            {
                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.Log("Kullanıcı bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + hata.Message, w);

                }
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.DumpLog(r);
                }
                MessageBox.Show("Kullanıcı bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            connection.Close();
        }

       
       
    }
}