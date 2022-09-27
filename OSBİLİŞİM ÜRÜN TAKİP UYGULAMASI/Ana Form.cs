using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using System.Net;

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

        readonly SqlConnection connection = new SqlConnection("Data Source=192.168.1.123,1433;Network Library=DBMSSOCN; Initial Catalog=OSBİLİSİM;User Id=Admin; Password=1; MultipleActiveResultSets=True;");
        private void Anaform_Load(object sender, EventArgs e)
        {
            isim = isim_label.Text;
            statü = statü_label.Text;
            Kullanicigirisiform Kullanicigirisiform = new Kullanicigirisiform();
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
                    
                    Kullanicigirisiform.güncelversiyon = (string)üründurumusorgulama["version"];
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
                        if (Convert.ToInt32(Kullanicigirisiform.güncelversiyon) >= Convert.ToInt32(programversion.FileVersion))
                        {
                            DialogResult dialog = MessageBox.Show("Uygulama'nın yeni sürümünü indirmek ister misiniz?", "OS BİLİŞİM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialog == DialogResult.Yes)
                            {
                                string dosya_dizini = AppDomain.CurrentDomain.BaseDirectory.ToString() + "OSUpdate.exe";
                                File.WriteAllBytes(@"OSUpdate.exe", new WebClient().DownloadData("http://192.168.1.123/Update/OSUpdate.exe"));
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
                    Kullanicigirisiform.Log("Bağlantı kesildi.\nHata kodu: " + hata.Message, w);
                }
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {
                    Kullanicigirisiform.DumpLog(r);
                }
                MessageBox.Show("Bağlantı kesildi.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                SqlCommand kullanicikomut = new SqlCommand("SELECT kullanici_isim, kullanici_statü FROM kullanicilar where durum = '1'", connection);
                SqlDataReader aktifkullanici;
                aktifkullanici = kullanicikomut.ExecuteReader();
                aktifkullanicilar_listbox.Items.Clear();
                while (aktifkullanici.Read())
                {
                    aktifkullanicilar_listbox.Items.Add(aktifkullanici["kullanici_isim"] + " (" + (string)aktifkullanici["kullanici_statü"] + ")".ToString());
                    string kendikullaniciisminikaldirma = Kullanicigirisiform.username + " (" + (string)aktifkullanici["kullanici_statü"] + ")";
                    if (aktifkullanicilar_listbox.Items.Contains(kendikullaniciisminikaldirma))
                    {
                        aktifkullanicilar_listbox.Items.Remove(kendikullaniciisminikaldirma);
                    }
                }
                aktifkullanici.Close();
                SqlCommand kullanicilar = new SqlCommand("select * from kullanicilar where k_adi = '" + Kullanicigirisiform.username + "'", connection);
                SqlDataReader kullaniciaciklamasi;
                kullaniciaciklamasi = kullanicilar.ExecuteReader();
                while (kullaniciaciklamasi.Read())
                {
                    isim_label.Text = (string)kullaniciaciklamasi["kullanici_isim"];
                    soyisim_label.Text = (string)kullaniciaciklamasi["kullanici_soyisim"];
                    statü_label.Text = (string)kullaniciaciklamasi["kullanici_statü"];
                    kayit_tarihi_label.Text = (string)kullaniciaciklamasi["kullanici_kayit_tarihi"];
                    if ((string)kullaniciaciklamasi["cinsiyet"] == "E")
                    pictureBox1.Image = Properties.Resources.person_icon_man;
                    else if ((string)kullaniciaciklamasi["cinsiyet"] == "K")
                        pictureBox1.Image = Properties.Resources.person_icon_gril;
                    else pictureBox1.Image = Properties.Resources.person_icon_green;

                }
                connection.Close();
            }
            catch (Exception hata)
            {
                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                {
                    Kullanicigirisiform.Log("Kullanıcı bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + hata.Message, w);
                }
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {
                    Kullanicigirisiform.DumpLog(r);
                }
                MessageBox.Show("Kullanıcı bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            if (statü_label.Text == "Teknik Görevli" || statü_label.Text == "Teknisyen" || statü_label.Text == "Teknik")
            {
                diğer_malzeme_grubları.Visible = true;
                siparis_olustur_btn.Visible = true;
                ürün_ekle_ve_düzenle_btn.Visible = true;
                Diğer_malzeme_grubları_ekle_ve_düzenleme_btn.Visible = true;
                siparis_kontrol_btn.Location = new System.Drawing.Point(591, 216);
                siparis_olustur_btn.Location = new System.Drawing.Point(474, 216);
                ürün_ekle_ve_düzenle_btn.Location = new System.Drawing.Point(474, 267);
            }
            if (statü_label.Text == "Ana Bilgisayar" || isim_label.Text == "ANA PC")
            {
                isim_label.Location = new System.Drawing.Point(60, 220);
                label.Location = new System.Drawing.Point(12, 220);
                soyisim_label.Visible = false;
                label_6.Visible = false;
            }
            else
            {
                Diğer_malzeme_grubları_ekle_ve_düzenleme_btn.Visible = true;
                diğer_malzeme_grubları.Visible = true;
                siparis_kontrol_btn.Location = new System.Drawing.Point(591, 216);
                siparis_olustur_btn.Location = new System.Drawing.Point(474, 216);
                ürün_ekle_ve_düzenle_btn.Location = new System.Drawing.Point(474, 267);

            }
            tarih_label.Text = DateTime.Now.ToLongDateString();
        }
        private void Anaform_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    Kullanicigirisiform kullanicigirisiform = new Kullanicigirisiform();
                    SqlCommand kullanicidurumgüncelle = new SqlCommand("Update kullanicilar set durum='" + 0 + "' where k_adi = '" + Kullanicigirisiform.username + "'", connection);
                    kullanicidurumgüncelle.ExecuteNonQuery();
                    kullanicigirisiform.Show();
                    Hide();
                }
            }
            catch (Exception kullaniciaktifligi)
            {
                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                {
                    Kullanicigirisiform.Log("Kullanıcı bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + kullaniciaktifligi.Message, w);

                }
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {
                    Kullanicigirisiform.DumpLog(r);
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
                Kullanicigirisiform kullanicigirisiform = new Kullanicigirisiform();
                SqlCommand kullanicidurumgüncelle = new SqlCommand("Update kullanicilar set durum='" + 0 + "' where k_adi = '" + Kullanicigirisiform.username + "'", connection);
                kullanicidurumgüncelle.ExecuteNonQuery();
            }
            catch (Exception kullaniciaktifligi)
            {
                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                {
                    Kullanicigirisiform.Log("Kullanıcı bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + kullaniciaktifligi.Message, w);

                }
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {
                    Kullanicigirisiform.DumpLog(r);
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
                    Kullanicigirisiform Kullanicigirisiform = new Kullanicigirisiform();
                    SqlCommand kullanicidurumgüncelle = new SqlCommand("Update kullanicilar set durum='" + 0 + "' where k_adi = '" + Kullanicigirisiform.username + "'", connection);
                    kullanicidurumgüncelle.ExecuteNonQuery();
                    Kullanicigirisiform.Show();
                    Hide();
                }
            }
            catch (Exception kullaniciaktifligi)
            {
                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                {
                    Kullanicigirisiform.Log("Kullanıcı bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + kullaniciaktifligi.Message, w);

                }
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {
                    Kullanicigirisiform.DumpLog(r);
                }
                MessageBox.Show("Kullanıcı bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + kullaniciaktifligi.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Siparis_olustur_btn_Click(object sender, EventArgs e)
        {
            Siparisolusturmaform siparisolusturmaform = new Siparisolusturmaform();
            siparisolusturmaform.Show();
            Hide();
        }

        private void Siparis_kontrol_btn_Click(object sender, EventArgs e)
        {
            Sipariskontrolform sipariskontrolforum = new Sipariskontrolform();
            sipariskontrolforum.Show();
            Hide();
        }

        private void Ürün_ekle_ve_düzenle_btn_Click(object sender, EventArgs e)
        {
            Ürüneklemedüzenlemeform Ürüneklemedüzenlemeform = new Ürüneklemedüzenlemeform();
            Ürüneklemedüzenlemeform.Show();
            Hide();
        }
        private void Diğer_malzeme_grubları_Click(object sender, EventArgs e)
        {
            Diğer_Malzeme_Grubları diğer_Malzeme_Grubları = new Diğer_Malzeme_Grubları();
            diğer_Malzeme_Grubları.Show();
            Hide();
        }
        private void Diğer_malzeme_grubları_ekle_ve_düzenleme_btn_Click(object sender, EventArgs e)
        {
            Diğer_malzeme_Grubları_Ürün_ekle_ve_Düzenleme diğer_Malzeme_Grubları = new Diğer_malzeme_Grubları_Ürün_ekle_ve_Düzenleme();
            diğer_Malzeme_Grubları.Show();
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
            ürün_ekle_ve_düzenle_btn.BackColor = Color.FromArgb(22, 53, 56);
        }

        private void Ürün_ekle_ve_düzenle_btn_MouseMove(object sender, MouseEventArgs e)
        {
            ürün_ekle_ve_düzenle_btn.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void Diğer_malzeme_grubları_MouseMove(object sender, MouseEventArgs e)
        {
            diğer_malzeme_grubları.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void Diğer_malzeme_grubları_MouseLeave(object sender, EventArgs e)
        {
            diğer_malzeme_grubları.BackColor = Color.FromArgb(22, 53, 56);
            diğer_malzeme_grubları.ForeColor = Color.White;
        }

        private void Diğer_malzeme_grubları_ekle_ve_düzenleme_btn_MouseMove(object sender, MouseEventArgs e)
        {
            Diğer_malzeme_grubları_ekle_ve_düzenleme_btn.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void Diğer_malzeme_grubları_ekle_ve_düzenleme_btn_MouseLeave(object sender, EventArgs e)
        {
            Diğer_malzeme_grubları_ekle_ve_düzenleme_btn.BackColor = Color.FromArgb(22, 53, 56);
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
        private void aktifkullanicilar_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            provider.Clear();
            ToolTip Aciklama = new ToolTip();
            string cinsiyetsorgulama = " ";
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
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
            catch (Exception hata)
            {
                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                {
                    Kullanicigirisiform.Log("Kullanıcı bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + hata.Message, w);

                }
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {
                    Kullanicigirisiform.DumpLog(r);
                }
                MessageBox.Show("Kullanıcı bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            connection.Close();
        }

       
       
    }
}