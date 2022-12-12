using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSBilişim
{
    public partial class Ürün_İsimleri_Düzenleme : Form
    {
        public Ürün_İsimleri_Düzenleme()
        {
            InitializeComponent();
        }
        readonly SqlConnection connection = new SqlConnection("Data Source=192.168.1.123,1433;Network Library=DBMSSOCN; Initial Catalog=OSBİLİSİM;User Id=Admin; Password=1; MultipleActiveResultSets=True;");
        ErrorProvider provider = new ErrorProvider();
        Kullanıcı_girisi Kullanıcı_girisi = new Kullanıcı_girisi();
        private void Ürün_İsimleri_Düzenleme_Load(object sender, EventArgs e)
        {
            notebook_ürünler_listbox.Enabled = false;
            diger_ürünler_listbox.Enabled = false;
            çıkartılacak_ürünler_listbox.Enabled = false;
            kullanilacak_ürünler_listbox.Enabled = false;
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
                            DialogResult dialog = MessageBox.Show("Uygulamanızın yeni sürümünü indirmek ister misiniz?", "OS BİLİŞİM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
                                MessageBox.Show("Uygulamanızı güncellemediğiniz için, program çalışmayacaktır.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Application.Exit();
                            }
                        }
                    }
                }
            }
            catch (Exception hata)
            {   using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                {Kullanıcı_girisi.Log("Program başlatılmadı.\nİnternet bağlantınızı ya da server bağlantınızı kontrol edin.\nHata kodu: " + hata.Message, w);}
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {Kullanıcı_girisi.DumpLog(r);}
                MessageBox.Show("Program başlatılmadı.\nİnternet bağlantınızı ya da server bağlantınızı kontrol edin.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();}
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                SqlCommand komut = new SqlCommand("SELECT urun_adi from notebook_urunler ", connection);
                SqlDataReader veriokuyucu1;
                veriokuyucu1 = komut.ExecuteReader();
                while (veriokuyucu1.Read())
                {
                    notebook_ürünler_listbox.Items.Add(veriokuyucu1["urun_adi"]);
                }
                veriokuyucu1.Close();

                SqlCommand digerürün = new SqlCommand("SELECT diger_urun_adi from diger_ürünler ", connection);
                SqlDataReader digerürünokuyucu;
                digerürünokuyucu = digerürün.ExecuteReader();
                while (digerürünokuyucu.Read())
                {
                    diger_ürünler_listbox.Items.Add(digerürünokuyucu["diger_urun_adi"]);
                }
                digerürünokuyucu.Close();

                SqlCommand kullanilacakurunler = new SqlCommand("SELECT malzeme_adi from notebook_kullanilacak_malzemeler ", connection);
                SqlDataReader kullanilacakurunlerokuyucu;
                kullanilacakurunlerokuyucu = kullanilacakurunler.ExecuteReader();
                while (kullanilacakurunlerokuyucu.Read())
                {
                    kullanilacak_ürünler_listbox.Items.Add(kullanilacakurunlerokuyucu["malzeme_adi"]);
                }
                kullanilacakurunlerokuyucu.Close();

                SqlCommand cikartilacakurunler = new SqlCommand("SELECT malzeme_adi from notebook_cikartilacak_malzemeler ", connection);
                SqlDataReader cikartilacakurunlerokuyucu;
                cikartilacakurunlerokuyucu = cikartilacakurunler.ExecuteReader();
                while (cikartilacakurunlerokuyucu.Read())
                {
                    çıkartılacak_ürünler_listbox.Items.Add(cikartilacakurunlerokuyucu["malzeme_adi"]);
                }
                cikartilacakurunlerokuyucu.Close();
            }
            catch (Exception hata)
            {   using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                {Kullanıcı_girisi.Log("Ürün verileri çekilirken bir hata oluştu.\nİnternet bağlantınızı ya da server bağlantınızı kontrol edin.\nHata kodu: " + hata.Message, w);}
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {Kullanıcı_girisi.DumpLog(r);}

                MessageBox.Show("Ürün verileri çekilirken bir hata oluştu.\nİnternet bağlantınızı ya da server bağlantınızı kontrol edin.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            connection.Close();
        }
        private void diger_ürünler_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            provider.Clear();
            if (diger_ürünler_listbox.SelectedIndex == -1)
                provider.SetError(diger_ürünler_listbox, "Geçerli bir ürün seçiniz.");
            else ürün_adi_textbox.Text = diger_ürünler_listbox.SelectedItem.ToString();

        }
        private void notebook_ürünler_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            provider.Clear();
            if (notebook_ürünler_listbox.SelectedIndex == -1)
                provider.SetError(notebook_ürünler_listbox, "Geçerli bir ürün seçiniz.");
            else ürün_adi_textbox.Text = notebook_ürünler_listbox.SelectedItem.ToString();

        }

        private void notebook_ürünler_radio_CheckedChanged(object sender, EventArgs e)
        {
            ürün_adi_textbox.Text = "";
            ürün_adi_label.Text = "Ürün adı: ";
            ürün_adi_label.Location = new Point(10, 388);
            ürün_adi_textbox.Location = new Point(91, 385);
            ürün_adi_panel.Location = new Point(91, 404);
            Değiştir_btn.Location = new Point(169, 411);
            notebook_ürünler_listbox.Enabled = true;
            diger_ürünler_listbox.Enabled = false;
            çıkartılacak_ürünler_listbox.Enabled = false;
            kullanilacak_ürünler_listbox.Enabled = false;
            diger_ürünler_listbox.SelectedIndex = -1;
            kullanilacak_ürünler_listbox.SelectedIndex = -1;
            çıkartılacak_ürünler_listbox.SelectedIndex = -1;
            provider.Clear();
        }

        private void diğer_ürünler_radio_CheckedChanged(object sender, EventArgs e)
        {
            ürün_adi_textbox.Text = "";
            ürün_adi_label.Text = "Diğer ürün adı: ";
            ürün_adi_label.Location = new Point(10, 388);
            ürün_adi_textbox.Location = new Point(130, 385);
            ürün_adi_panel.Location = new Point(130, 404);
            Değiştir_btn.Location = new Point(208, 411);
            notebook_ürünler_listbox.Enabled = false;
            diger_ürünler_listbox.Enabled = true;
            çıkartılacak_ürünler_listbox.Enabled = false;
            kullanilacak_ürünler_listbox.Enabled = false;
            notebook_ürünler_listbox.SelectedIndex = -1;
            kullanilacak_ürünler_listbox.SelectedIndex = -1;
            çıkartılacak_ürünler_listbox.SelectedIndex = -1;
            provider.Clear();
        }
        private void kullanılacak_ürünler_radio_CheckedChanged(object sender, EventArgs e)
        {
            ürün_adi_textbox.Text = "";
            ürün_adi_label.Text = "Ürün adı: ";
            ürün_adi_label.Location = new Point(10, 388);
            ürün_adi_textbox.Location = new Point(91, 385);
            ürün_adi_panel.Location = new Point(91, 404);
            Değiştir_btn.Location = new Point(169, 411);
            notebook_ürünler_listbox.Enabled = false;
            diger_ürünler_listbox.Enabled = false;
            çıkartılacak_ürünler_listbox.Enabled = false;
            kullanilacak_ürünler_listbox.Enabled = true;
            notebook_ürünler_listbox.SelectedIndex = -1;
            diger_ürünler_listbox.SelectedIndex = -1;
            çıkartılacak_ürünler_listbox.SelectedIndex = -1;
            provider.Clear();
        }

        private void çıkartılan_ürünler_radio_CheckedChanged(object sender, EventArgs e)
        {
            ürün_adi_textbox.Text = "";
            ürün_adi_label.Text = "Ürün adı: ";
            ürün_adi_label.Location = new Point(10, 388);
            ürün_adi_textbox.Location = new Point(91, 385);
            ürün_adi_panel.Location = new Point(91, 404);
            Değiştir_btn.Location = new Point(169, 411);
            notebook_ürünler_listbox.Enabled = false;
            diger_ürünler_listbox.Enabled = false;
            çıkartılacak_ürünler_listbox.Enabled = true;
            kullanilacak_ürünler_listbox.Enabled = false;
            notebook_ürünler_listbox.SelectedIndex = -1;
            diger_ürünler_listbox.SelectedIndex = -1;
            kullanilacak_ürünler_listbox.SelectedIndex = -1;
            provider.Clear();
        }
        #region renkayarları
        private void Değiştir_btn_MouseMove(object sender, MouseEventArgs e)
        {
            Değiştir_btn.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void Değiştir_btn_MouseLeave(object sender, EventArgs e)
        {
            Değiştir_btn.BackColor = Color.FromArgb(22, 53, 56);
        }

        private void ana_menü_btn_MouseMove(object sender, MouseEventArgs e)
        {
            ana_menü_btn.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void ana_menü_btn_MouseLeave(object sender, EventArgs e)
        {
            ana_menü_btn.BackColor = Color.FromArgb(22, 53, 56);
        }

        private void logout_label_MouseMove(object sender, MouseEventArgs e)
        {
            logout_label.ForeColor = Color.FromArgb(13, 31, 33);
        }

        private void logout_label_MouseLeave(object sender, EventArgs e)
        {
            logout_label.ForeColor = Color.FromArgb(22, 53, 56);
        }

        private void minimize_label_MouseMove(object sender, MouseEventArgs e)
        {
            minimize_label.ForeColor = Color.FromArgb(13, 31, 33);
        }

        private void minimize_label_MouseLeave(object sender, EventArgs e)
        {
            minimize_label.ForeColor = Color.FromArgb(22, 53, 56);
        }
        #endregion

        private void logout_label_Click(object sender, EventArgs e)
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
                {Kullanıcı_girisi.Log("Kullanıcı bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + kullaniciaktifligi.Message, w);}
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {Kullanıcı_girisi.DumpLog(r);}
                MessageBox.Show("Kullanıcı bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + kullaniciaktifligi.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            Application.Exit();
        }

        private void minimize_label_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        new
        #region forumharaketettirme
        int Move;
        int Mouse_X;
        int Mouse_Y;
        private void Ürün_İsimleri_Düzenleme_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
            this.Cursor = Cursors.Default;
        }

        private void Ürün_İsimleri_Düzenleme_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }

        private void Ürün_İsimleri_Düzenleme_MouseDown(object sender, MouseEventArgs e)
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
        #endregion

        private void kullanilacak_ürünler_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            provider.Clear();
            ürün_adi_label.Text = "Ürün adı: ";
            ürün_adi_label.Location = new Point(10, 388);
            ürün_adi_textbox.Location = new Point(91, 385);
            ürün_adi_panel.Location = new Point(91, 404);
            Değiştir_btn.Location = new Point(169, 411);
            if (kullanilacak_ürünler_listbox.SelectedIndex == -1)
                provider.SetError(kullanilacak_ürünler_listbox, "Geçerli bir ürün seçiniz.");
            else ürün_adi_textbox.Text = kullanilacak_ürünler_listbox.SelectedItem.ToString();
        }

        private void çıkartılacak_ürünler_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            provider.Clear();
            ürün_adi_label.Text = "Ürün adı: ";
            ürün_adi_label.Location = new Point(10, 388);
            ürün_adi_textbox.Location = new Point(91, 385);
            ürün_adi_panel.Location = new Point(91, 404);
            Değiştir_btn.Location = new Point(169, 411);
            if (çıkartılacak_ürünler_listbox.SelectedIndex == -1)
                provider.SetError(çıkartılacak_ürünler_listbox, "Geçerli bir ürün seçiniz.");
            else ürün_adi_textbox.Text = çıkartılacak_ürünler_listbox.SelectedItem.ToString();
        }

        private void ana_menü_btn_Click(object sender, EventArgs e)
        {
            Anaform anaform = new Anaform();
            anaform.Show();
            Hide();
        }
        public void ListeYenile()
        {
            notebook_ürünler_listbox.Items.Clear();
            diger_ürünler_listbox.Items.Clear();
            kullanilacak_ürünler_listbox.Items.Clear();
            çıkartılacak_ürünler_listbox.Items.Clear();
            try
            {if (connection.State == ConnectionState.Closed)
                connection.Open();

                SqlCommand komut = new SqlCommand("SELECT urun_adi from notebook_urunler ", connection);
                SqlDataReader veriokuyucu1;
                veriokuyucu1 = komut.ExecuteReader();
                while (veriokuyucu1.Read())
                {notebook_ürünler_listbox.Items.Add(veriokuyucu1["urun_adi"]);}
                veriokuyucu1.Close();

                SqlCommand digerürün = new SqlCommand("SELECT diger_urun_adi from diger_ürünler ", connection);
                SqlDataReader digerürünokuyucu;
                digerürünokuyucu = digerürün.ExecuteReader();
                while (digerürünokuyucu.Read())
                {diger_ürünler_listbox.Items.Add(digerürünokuyucu["diger_urun_adi"]);}
                digerürünokuyucu.Close();

                SqlCommand kullanilacakurunler = new SqlCommand("SELECT malzeme_adi from notebook_kullanilacak_malzemeler ", connection);
                SqlDataReader kullanilacakurunlerokuyucu;
                kullanilacakurunlerokuyucu = kullanilacakurunler.ExecuteReader();
                while (kullanilacakurunlerokuyucu.Read())
                {kullanilacak_ürünler_listbox.Items.Add(kullanilacakurunlerokuyucu["malzeme_adi"]);}
                kullanilacakurunlerokuyucu.Close();

                SqlCommand cikartilacakurunler = new SqlCommand("SELECT malzeme_adi from notebook_cikartilacak_malzemeler ", connection);
                SqlDataReader cikartilacakurunlerokuyucu;
                cikartilacakurunlerokuyucu = cikartilacakurunler.ExecuteReader();
                while (cikartilacakurunlerokuyucu.Read())
                {çıkartılacak_ürünler_listbox.Items.Add(cikartilacakurunlerokuyucu["malzeme_adi"]);}
                cikartilacakurunlerokuyucu.Close();
            }
            catch (Exception hata)
            {   using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                {Kullanıcı_girisi.Log("Ürün verileri çekilirken bir hata oluştu.\nİnternet bağlantınızı ya da server bağlantınızı kontrol edin.\nHata kodu: " + hata.Message, w);}
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {Kullanıcı_girisi.DumpLog(r);}

                MessageBox.Show("Ürün verileri çekilirken bir hata oluştu.\nİnternet bağlantınızı ya da server bağlantınızı kontrol edin.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            connection.Close();
        }
        private void Değiştir_btn_Click(object sender, EventArgs e)
        {
            try
            {
                provider.Clear();
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                if (ürün_adi_textbox.Text == "")
                    provider.SetError(ürün_adi_textbox, "Ürün adı boş bırakılmaz.");

                else if (notebook_ürünler_radio.Checked == true)
                {if (notebook_ürünler_listbox.Items.Contains(ürün_adi_textbox.Text))
                        provider.SetError(ürün_adi_textbox, "Notebook ürünlerinde böyle bir ürün mevcut, tekrar deneyiniz.");
                    else
                    {SqlCommand notebookurungüncelle = new SqlCommand("Update notebook_urunler set urun_adi='" + ürün_adi_textbox.Text + "' where urun_adi = '" + notebook_ürünler_listbox.SelectedItem.ToString() + "'", connection);
                        notebookurungüncelle.ExecuteNonQuery();
                        SqlCommand notebookürünkodlarıürünisimgüncelle = new SqlCommand("Update notebook_urun_kodları set urun_adi='" + ürün_adi_textbox.Text + "' where urun_adi = '" + notebook_ürünler_listbox.SelectedItem.ToString() + "'", connection);
                        notebookürünkodlarıürünisimgüncelle.ExecuteNonQuery();
                        SqlCommand notebookürünserinoisimgüncelle = new SqlCommand("Update notebook_urun_seri_no_stok set urun_adi='" + ürün_adi_textbox.Text + "' where urun_adi = '" + notebook_ürünler_listbox.SelectedItem.ToString() + "'", connection);
                        notebookürünserinoisimgüncelle.ExecuteNonQuery();
                        MessageBox.Show("Yeni ürün adı başarılı şekilde güncellenmiştir.", "OS Bilişim", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                        { Kullanıcı_girisi.Log(Kullanıcı_girisi.username + " adlı kullanıcı " + notebook_ürünler_listbox.SelectedItem.ToString().ToUpper() + " adlı ürünü'nün adını " + ürün_adi_textbox.Text.ToUpper() + " olarak değiştirdi.", w);}
                        using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                        { Kullanıcı_girisi.DumpLog(r); ListeYenile();}}} 

                else if (diğer_ürünler_radio.Checked == true)
                {if (diger_ürünler_listbox.Items.Contains(ürün_adi_textbox.Text))
                        provider.SetError(ürün_adi_textbox, "Diğer ürünlerinde böyle bir ürün mevcut, tekrar deneyiniz.");
                    else
                    {SqlCommand digerurunkodugüncelle = new SqlCommand("Update diger_ürünler set diger_urun_adi='" + ürün_adi_textbox.Text + "' where diger_urun_adi = '" + diger_ürünler_listbox.SelectedItem.ToString() + "'", connection);
                        digerurunkodugüncelle.ExecuteNonQuery();
                        SqlCommand digerurunkoduisimgüncelle = new SqlCommand("Update diger_ürü_kodları set diger_urun_adi='" + ürün_adi_textbox.Text + "' where diger_urun_adi = '" + diger_ürünler_listbox.SelectedItem.ToString() + "'", connection);
                        digerurunkoduisimgüncelle.ExecuteNonQuery();
                        SqlCommand digerurunserinoisimgüncelle = new SqlCommand("Update diger_ürün_Stok set diger_urun_adi='" + ürün_adi_textbox.Text + "' where diger_urun_adi = '" + diger_ürünler_listbox.SelectedItem.ToString() + "'", connection);
                        digerurunserinoisimgüncelle.ExecuteNonQuery();
                        MessageBox.Show("Yeni ürün adı başarılı şekilde güncellenmiştir.", "OS Bilişim", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                        { Kullanıcı_girisi.Log(Kullanıcı_girisi.username + " adlı kullanıcı " + diger_ürünler_listbox.SelectedItem.ToString().ToUpper() + " adlı ürünü'nün adını " + ürün_adi_textbox.Text.ToUpper() + " olarak değiştirdi.", w); }
                        using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                        { Kullanıcı_girisi.DumpLog(r);ListeYenile();}}} 

                else if (kullanılacak_ürünler_radio.Checked == true)
                {if (kullanilacak_ürünler_listbox.Items.Contains(ürün_adi_textbox.Text))
                        provider.SetError(ürün_adi_textbox, "Kullanılacak ürünlerinde böyle bir ürün mevcut, tekrar deneyiniz.");
                    else
                    {   SqlCommand kullanilacakmalzemegüncelle = new SqlCommand("Update notebook_kullanilacak_malzemeler set malzeme_adi='" + ürün_adi_textbox.Text + "' where malzeme_adi = '" + kullanilacak_ürünler_listbox.SelectedItem.ToString() + "'", connection);
                        kullanilacakmalzemegüncelle.ExecuteNonQuery();
                        MessageBox.Show("Yeni ürün adı başarılı şekilde güncellenmiştir.", "OS Bilişim", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                        { Kullanıcı_girisi.Log(Kullanıcı_girisi.username + " adlı kullanıcı " + kullanilacak_ürünler_listbox.SelectedItem.ToString().ToUpper() + " adlı ürünü'nün adını " + ürün_adi_textbox.Text.ToUpper() + " olarak değiştirdi.", w); }
                        using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                        { Kullanıcı_girisi.DumpLog(r);ListeYenile();}}} 

                else if (çıkartılan_ürünler_radio.Checked == true)
                {if (çıkartılacak_ürünler_listbox.Items.Contains(ürün_adi_textbox.Text))
                        provider.SetError(ürün_adi_textbox, "Çıkartılacak ürünlerinde böyle bir ürün mevcut, tekrar deneyiniz.");
                    else
                    {SqlCommand cikartilacakmalzemegüncelle = new SqlCommand("Update notebook_cikartilacak_malzemeler set malzeme_adi='" + ürün_adi_textbox.Text + "' where malzeme_adi = '" + çıkartılacak_ürünler_listbox.SelectedItem.ToString() + "'", connection);
                        cikartilacakmalzemegüncelle.ExecuteNonQuery();
                        MessageBox.Show("Yeni ürün adı başarılı şekilde güncellenmiştir.", "OS Bilişim", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                        { Kullanıcı_girisi.Log(Kullanıcı_girisi.username + " adlı kullanıcı " + çıkartılacak_ürünler_listbox.SelectedItem.ToString().ToUpper() + " adlı ürünü'nün adını " + ürün_adi_textbox.Text.ToUpper() + " olarak değiştirdi.", w); }
                        using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                        { Kullanıcı_girisi.DumpLog(r); ListeYenile();}}}}

            catch (Exception hata)
            {   using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                {Kullanıcı_girisi.Log("Ürün verileri çekilirken bir hata oluştu.\nİnternet bağlantınızı ya da server bağlantınızı kontrol edin.\nHata kodu: " + hata.Message, w);}
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                { Kullanıcı_girisi.DumpLog(r);}
                MessageBox.Show("Ürün verileri çekilirken bir hata oluştu.\nİnternet bağlantınızı ya da server bağlantınızı kontrol edin.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();}

            connection.Close();
        }
    }
}
