using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Net;

namespace OSBilişim
{
    public partial class Ürün_ekle : Form
    {
        public Ürün_ekle()
        {
            InitializeComponent();
        }
        readonly SqlConnection connection = new SqlConnection("Data Source=192.168.1.123,1433;Network Library=DBMSSOCN; Initial Catalog=OSBİLİSİM;User Id=Admin; Password=1; MultipleActiveResultSets=True;");
        public void Listeyenile()
        {
            ürün_adi_textbox.Text = "";
            ürün_seri_no_textbox.Text = "";
            ürün_stok_kodu_textbox.Text = "";
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                    int tekrarsec = notebook_ürünler_listbox.SelectedIndex;
                    notebook_ürünler_listbox.Items.Clear();
                    SqlCommand komut = new SqlCommand("SELECT * FROM notebook_urunler ", connection);
                    SqlDataReader veriokuyucu;
                    veriokuyucu = komut.ExecuteReader();
                    while (veriokuyucu.Read())
                    {
                        notebook_ürünler_listbox.Items.Add(veriokuyucu["urun_adi"]);
                    }
                    veriokuyucu.Close();
                    connection.Close();

                    if (notebook_ürünler_listbox.SelectedIndex > tekrarsec)
                    {
                        notebook_ürünler_listbox.SelectedIndex = -1;
                    }
                    else { notebook_ürünler_listbox.SelectedIndex = tekrarsec; }

                    notebook_ürün_kodlari_listbox.Items.Clear();
                    connection.Open();
                    SqlCommand komut3 = new SqlCommand("SELECT * FROM notebook_urun_kodları where urun_adi = '" + notebook_ürünler_listbox.SelectedItem + "'", connection);
                    SqlDataReader veriokuyucu3;
                    veriokuyucu3 = komut3.ExecuteReader();
                    while (veriokuyucu3.Read())
                    {
                        notebook_ürün_kodlari_listbox.Items.Add(veriokuyucu3["urun_kodu"]);
                    }
                    veriokuyucu3.Close();
                    connection.Close();

                    notebook_ürün_seri_no_listbox.Items.Clear();
                    connection.Open();
                    SqlCommand üründurum = new SqlCommand("SELECT * FROM notebook_urun_seri_no_stok where urun_adi = '" + notebook_ürünler_listbox.SelectedItem + "' and urun_depo_cikisi = 'OS Depo Merkez'", connection);
                    SqlDataReader üründurumusorgulama;
                    üründurumusorgulama = üründurum.ExecuteReader();
                    while (üründurumusorgulama.Read())
                    {
                        notebook_ürün_seri_no_listbox.Items.Add(üründurumusorgulama["urun_seri_no"]);
                    }
                    üründurumusorgulama.Close();
                    connection.Close();

                    index_seri_no_listbox.Items.Clear();
                    connection.Open();
                    SqlCommand indexdurumsorgulama = new SqlCommand("SELECT * FROM notebook_urun_seri_no_stok where urun_adi = '" + notebook_ürünler_listbox.SelectedItem + "' and urun_depo_cikisi = 'İndex Depo'", connection);
                    SqlDataReader indexdurumsorgulamaokuyucu;
                    indexdurumsorgulamaokuyucu = indexdurumsorgulama.ExecuteReader();
                    while (indexdurumsorgulamaokuyucu.Read())
                    {
                        index_seri_no_listbox.Items.Add(indexdurumsorgulamaokuyucu["urun_seri_no"]);
                    }
                    indexdurumsorgulamaokuyucu.Close();
                    connection.Close();

                    arena_seri_no_listbox.Items.Clear();
                    connection.Open();
                    SqlCommand arenaürün = new SqlCommand("SELECT * FROM notebook_urun_seri_no_stok where urun_adi = '" + notebook_ürünler_listbox.SelectedItem + "' and urun_depo_cikisi = 'Arena Depo'", connection);
                    SqlDataReader arenaürünokuyucu;
                    arenaürünokuyucu = arenaürün.ExecuteReader();
                    while (arenaürünokuyucu.Read())
                    {
                        arena_seri_no_listbox.Items.Add(arenaürünokuyucu["urun_seri_no"]);
                    }
                    arenaürünokuyucu.Close();
                    connection.Close();

                    penta_seri_no_listbox.Items.Clear();
                    connection.Open();
                    SqlCommand pentaürün = new SqlCommand("SELECT * FROM notebook_urun_seri_no_stok where urun_adi = '" + notebook_ürünler_listbox.SelectedItem + "' and urun_depo_cikisi = 'Penta Depo'", connection);
                    SqlDataReader pentaürünokuyucu;
                    pentaürünokuyucu = pentaürün.ExecuteReader();
                    while (pentaürünokuyucu.Read())
                    {
                        penta_seri_no_listbox.Items.Add(pentaürünokuyucu["urun_seri_no"]);
                    }
                    pentaürünokuyucu.Close();
                    connection.Close();
            }
            catch (Exception hata)
            {
                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.Log("Ürün bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + hata.Message, w);

                }
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.DumpLog(r);
                }
                MessageBox.Show("Ürün bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
        private void Ürün_Ekle_ve_Düzenleme_Load(object sender, EventArgs e)
        {
            Kullanıcı_girisi Kullanıcı_girisi = new Kullanıcı_girisi();
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
                connection.Close();
            }
            catch (Exception hata)
            {
                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.Log("Program başlatılmadı.\nİnternet bağlantınızı ya da server bağlantınızı kontrol edin.\nHata kodu: " + hata.Message, w);

                }
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.DumpLog(r);
                }
                MessageBox.Show("Program başlatılmadı.\nİnternet bağlantınızı ya da server bağlantınızı kontrol edin.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            if (notebook_ürünler_listbox.SelectedIndex == -1)
            {
                notebook_ürün_kodlari_listbox.Items.Add("Lütfen bir ürün seçin.");
                notebook_ürün_seri_no_listbox.Items.Add("Lütfen bir ürün seçin.");
            }
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                SqlCommand komut = new SqlCommand("SELECT * FROM notebook_urunler ", connection);
                SqlDataReader veriokuyucu1;
                veriokuyucu1 = komut.ExecuteReader();
                while (veriokuyucu1.Read())
                {
                    notebook_ürünler_listbox.Items.Add(veriokuyucu1["urun_adi"]);
                }
                veriokuyucu1.Close();
                connection.Close();
            }
            catch (Exception hata)
            {
                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.Log("Ürün kodları çekilirken bir hata oluştu.\nİnternet bağlantınızı ya da server bağlantınızı kontrol edin.\nHata kodu: " + hata.Message, w);

                }
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.DumpLog(r);
                }
                MessageBox.Show("Ürün kodları çekilirken bir hata oluştu.\nİnternet bağlantınızı ya da server bağlantınızı kontrol edin.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void Notebook_ürünler_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (notebook_ürünler_listbox.SelectedIndex == -1)
            {
                MessageBox.Show("Geçerli bir ürün seçiniz.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                ürün_adi_textbox.Text = notebook_ürünler_listbox.SelectedItem.ToString();
                try
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    using (var cmd = new SqlCommand(@"select COUNT(urun_seri_no) from notebook_urun_seri_no_stok where urun_durumu = 'Kullanılmadı' and urun_depo_cikisi = 'OS Depo Merkez' and urun_adi = '" + notebook_ürünler_listbox.SelectedItem.ToString() + "'", connection))
                    {   var kalan = Convert.ToInt32(cmd.ExecuteScalar());
                        ürünkalankullanımlabel.Text = "Os depo merkez kullanılmamış ürün sayısı: " + kalan.ToString();}
                    using (var indexurunkontrol = new SqlCommand(@"select COUNT(urun_seri_no) from notebook_urun_seri_no_stok where urun_durumu = 'Kullanılmadı' and urun_depo_cikisi = 'İndex Depo' and urun_adi = '" + notebook_ürünler_listbox.SelectedItem.ToString() + "'", connection))
                    {   var kalan = Convert.ToInt32(indexurunkontrol.ExecuteScalar());
                        indexürünkullanım_label.Text = "İndex depo kullanılmamış ürün sayısı: " + kalan.ToString();}
                    using (var arenaurunkontrol = new SqlCommand(@"select COUNT(urun_seri_no) from notebook_urun_seri_no_stok where urun_durumu = 'Kullanılmadı' and urun_depo_cikisi = 'Arena Depo' and urun_adi = '" + notebook_ürünler_listbox.SelectedItem.ToString() + "'", connection))
                    {   var kalan = Convert.ToInt32(arenaurunkontrol.ExecuteScalar());
                        arenaürünkullanım_label.Text = "Arena depo kullanılmamış ürün sayısı: " + kalan.ToString();}
                    using (var pentaurunkontrol = new SqlCommand(@"select COUNT(urun_seri_no) from notebook_urun_seri_no_stok where urun_durumu = 'Kullanılmadı' and urun_depo_cikisi = 'Penta Depo' and urun_adi = '" + notebook_ürünler_listbox.SelectedItem.ToString() + "'", connection))
                    {   var kalan = Convert.ToInt32(pentaurunkontrol.ExecuteScalar());
                        pentaürünkullanım_label.Text = "Penta depo kullanılmamış ürün sayısı: " + kalan.ToString();}
                    connection.Close();
                }
                catch (Exception hata)
                {
                    using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                    {
                        Kullanıcı_girisi.Log("Ürün bilgileri çekilirken bir hata oluştu.\nİnternet bağlantınızı ya da server bağlantınızı kontrol edin.\nHata kodu: " + hata.Message, w);

                    }
                    using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                    {
                        Kullanıcı_girisi.DumpLog(r);
                    }
                    MessageBox.Show("Ürün bilgileri çekilirken bir hata oluştu.\nİnternet bağlantınızı ya da server bağlantınızı kontrol edin.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            ürün_stok_kodu_textbox.Text = "";
            ürün_seri_no_textbox.Text = "";
            try
            {
                if (notebook_ürünler_listbox.SelectedIndex == -1)
                {
                    notebook_ürün_kodlari_listbox.Items.Clear();
                    notebook_ürün_seri_no_listbox.Items.Clear();
                }
                else
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                        notebook_ürün_kodlari_listbox.Items.Clear();
                        SqlCommand komut3 = new SqlCommand("SELECT * FROM notebook_urun_kodları where urun_adi = '" + notebook_ürünler_listbox.SelectedItem + "'", connection);
                        SqlDataReader veriokuyucu3;
                        veriokuyucu3 = komut3.ExecuteReader();
                        while (veriokuyucu3.Read())
                        {
                            notebook_ürün_kodlari_listbox.Items.Add(veriokuyucu3["urun_kodu"]);
                        }
                        veriokuyucu3.Close();

                        notebook_ürün_seri_no_listbox.Items.Clear();
                        SqlCommand üründurum = new SqlCommand("SELECT urun_seri_no FROM notebook_urun_seri_no_stok where urun_depo_cikisi = 'OS Depo Merkez' and urun_adi = '" + notebook_ürünler_listbox.SelectedItem.ToString() + "'", connection);
                        SqlDataReader üründurumusorgulama;
                        üründurumusorgulama = üründurum.ExecuteReader();
                        while (üründurumusorgulama.Read())
                        {
                            notebook_ürün_seri_no_listbox.Items.Add(üründurumusorgulama["urun_seri_no"]);

                        }
                        üründurumusorgulama.Close();

                        index_seri_no_listbox.Items.Clear();
                        SqlCommand indexurundurum = new SqlCommand("SELECT urun_seri_no FROM notebook_urun_seri_no_stok where urun_depo_cikisi = 'İndex Depo' and urun_adi = '" + notebook_ürünler_listbox.SelectedItem.ToString() + "'", connection);
                        SqlDataReader indexüründurmsorgulama;
                        indexüründurmsorgulama = indexurundurum.ExecuteReader();
                        while (indexüründurmsorgulama.Read())
                        {
                            index_seri_no_listbox.Items.Add(indexüründurmsorgulama["urun_seri_no"]);

                        }
                        indexüründurmsorgulama.Close();

                        penta_seri_no_listbox.Items.Clear();
                        SqlCommand pentadurum = new SqlCommand("SELECT urun_seri_no FROM notebook_urun_seri_no_stok where urun_depo_cikisi = 'Penta Depo' and urun_adi = '" + notebook_ürünler_listbox.SelectedItem.ToString() + "'", connection);
                        SqlDataReader pentadurumsorgulama;
                        pentadurumsorgulama = pentadurum.ExecuteReader();
                        while (pentadurumsorgulama.Read())
                        {
                            penta_seri_no_listbox.Items.Add(pentadurumsorgulama["urun_seri_no"]);

                        }
                        pentadurumsorgulama.Close();

                        arena_seri_no_listbox.Items.Clear();
                        SqlCommand arenadurum = new SqlCommand("SELECT urun_seri_no FROM notebook_urun_seri_no_stok where urun_depo_cikisi = 'Arena Depo' and urun_adi = '" + notebook_ürünler_listbox.SelectedItem.ToString() + "'", connection);
                        SqlDataReader arenadurumsorgulama;
                        arenadurumsorgulama = arenadurum.ExecuteReader();
                        while (arenadurumsorgulama.Read())
                        {
                            arena_seri_no_listbox.Items.Add(arenadurumsorgulama["urun_seri_no"]);

                        }
                        arenadurumsorgulama.Close();
                        connection.Close();
                    }
                }
            }
            catch (Exception hata)
            {
                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.Log("Ürün kodu, seri numarası çekilirken bir hata oluştu.\nİnternet bağlantınızı ya da server bağlantınızı kontrol edin.\nHata kodu: " + hata.Message, w);

                }
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.DumpLog(r);
                }
                MessageBox.Show("Ürün kodu, seri numarası çekilirken bir hata oluştu.\nİnternet bağlantınızı ya da server bağlantınızı kontrol edin.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void Ürün_Ekle_ve_Düzenleme_FormClosed(object sender, FormClosedEventArgs e)
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
                    Kullanıcı_girisi.Log("Kullanıcı bilgileri alınırken bir hata oluştu.\nİnternet bağlantınızı ya da server bağlantınızı kontrol edin.\nHata kodu: " + kullaniciaktifligi.Message, w);

                }
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.DumpLog(r);
                }
                MessageBox.Show("Kullanıcı bilgileri alınırken bir hata oluştu.\nİnternet bağlantınızı ya da server bağlantınızı kontrol edin.\nHata kodu: " + kullaniciaktifligi.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            foreach (var process in Process.GetProcessesByName("OSBilişim"))
            {
                process.Kill();
            }
        }
        ErrorProvider provider = new ErrorProvider();  
        private void Yeni_ürün_ekle_btn_Click(object sender, EventArgs e)
        {
            provider.Clear();
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                if (notebook_ürünler_listbox.Items.Contains(ürün_adi_textbox.Text) && notebook_ürün_kodlari_listbox.Items.Contains(ürün_stok_kodu_textbox.Text))
                {
                    if (notebook_ürün_seri_no_listbox.Items.Contains(ürün_seri_no_textbox.Text))
                    {provider.SetError(ürün_seri_no_textbox, "Bu seri numarası mevcuttur, başka seri numarası kullanınız.");}
                    else if (ürün_adi_textbox.Text == "")
                    {provider.SetError(ürün_adi_textbox, "Ürün adını boş bırakmayınız.");}
                    else if (ürün_seri_no_textbox.Text == "")
                    {provider.SetError(ürün_seri_no_textbox, "Ürün seri numarasını boş bırakmayınız.");}
                    else if (ürün_stok_kodu_textbox.Text == "")
                    {provider.SetError(ürün_stok_kodu_textbox, "Ürün stok kodunu boş bırakmayınız.");}
                    else if (depo_combobox.SelectedIndex == -1)
                    { provider.SetError(depo_combobox, "Ürünün ekleneceği depoyu seçiniz."); }
                    else
                    {
                        string yeniürünserinokayit = "insert into notebook_urun_seri_no_stok(urun_adi,urun_seri_no,urun_adeti,urun_durumu,urun_depo_cikisi) values " + "" + "(@urun_adi,@urun_seri_no,@urun_adeti,@urun_durumu,@urun_depo_cikisi)";
                        SqlCommand yeniürünserinokayitkomut = new SqlCommand(yeniürünserinokayit, connection);
                        yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_adi", ürün_adi_textbox.Text);
                        yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_seri_no", ürün_seri_no_textbox.Text);
                        yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_adeti", "1");
                        yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_durumu", "Kullanılmadı");
                        if (depo_combobox.SelectedItem.ToString() == "OS Depo Merkez")
                        { yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_depo_cikisi", "OS Depo Merkez"); }
                        else if (depo_combobox.SelectedItem.ToString() == "İndex Depo")
                        { yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_depo_cikisi", "İndex Depo"); }
                        else if (depo_combobox.SelectedItem.ToString() == "Arena Depo")
                        { yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_depo_cikisi", "Arena Depo"); }
                        else if (depo_combobox.SelectedItem.ToString() == "Penta Depo")
                        { yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_depo_cikisi", "Penta Depo"); }
                        else { yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_depo_cikisi", "Os Depo Merkez"); }
                        yeniürünserinokayitkomut.ExecuteNonQuery();

                        using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                        {
                            Kullanıcı_girisi.Log(Kullanıcı_girisi.username + " adlı kullanıcı " + ürün_adi_textbox.Text + " ürününün " + ürün_seri_no_textbox.Text + " yeni seri numarasını ekledi.", w);
                        }
                        using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                        {
                            Kullanıcı_girisi.DumpLog(r);
                        }
                        int tekrarsec = notebook_ürünler_listbox.SelectedIndex;
                        MessageBox.Show("Yeni seri numarası başarılı bir şekilde eklendi.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        notebook_ürünler_listbox.SelectedIndex = tekrarsec;
                        ürün_seri_no_textbox.Focus();
                    }
                }
                else if (notebook_ürünler_listbox.Items.Contains(ürün_adi_textbox.Text))
                {
                    if (notebook_ürün_kodlari_listbox.Items.Contains(ürün_stok_kodu_textbox.Text))
                    {provider.SetError(ürün_stok_kodu_textbox, "Bu ürün kodu mevcuttur, başka ürün kodu kullanınız.");}
                    else if (notebook_ürün_seri_no_listbox.Items.Contains(ürün_seri_no_textbox.Text))
                    {provider.SetError(ürün_seri_no_textbox, "Bu seri numarası mevcuttur, başka seri numarası kullanınız.");}
                    else if (ürün_adi_textbox.Text == "")
                    { provider.SetError(ürün_adi_textbox, "Ürün adını boş bırakmayınız.");}
                    else if (ürün_stok_kodu_textbox.Text == "" && ürün_seri_no_textbox.Text.Length <= 0)
                    {provider.SetError(ürün_stok_kodu_textbox, "Ürün stok kodunu boş bırakmayınız.");}
                    else if (ürün_seri_no_textbox.Text == "" && ürün_stok_kodu_textbox.Text.Length <= 0)
                    {provider.SetError(ürün_seri_no_textbox, "Ürün seri numarasını boş bırakmayınız.");}
                    else if (ürün_seri_no_textbox.Text == "" && ürün_stok_kodu_textbox.Text.Length > 0)
                    {
                        string yeniürünkodukayit = "insert into notebook_urun_kodları(urun_adi,urun_kodu) values " + "" + "(@urun_adi,@urun_kodu)";
                        SqlCommand kayitkomut = new SqlCommand(yeniürünkodukayit, connection);
                        kayitkomut.Parameters.AddWithValue("@urun_adi", ürün_adi_textbox.Text);
                        kayitkomut.Parameters.AddWithValue("@urun_kodu", ürün_stok_kodu_textbox.Text);
                        kayitkomut.ExecuteNonQuery();

                        using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                        {
                            Kullanıcı_girisi.Log(Kullanıcı_girisi.username + " adlı kullanıcı " + ürün_adi_textbox.Text + " ürününün " + ürün_stok_kodu_textbox.Text + " yeni ürün kodunu ekledi.", w);
                        }
                        using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                        {
                            Kullanıcı_girisi.DumpLog(r);
                        }
                        MessageBox.Show("Yeni ürün stok kodu başarılı bir şekilde eklendi.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        int tekrarsec = notebook_ürünler_listbox.SelectedIndex;
                        notebook_ürünler_listbox.SelectedIndex = tekrarsec;
                        ürün_stok_kodu_textbox.Focus();
                    }
                    else if (ürün_stok_kodu_textbox.Text == "" && ürün_seri_no_textbox.Text.Length > 0)
                    {
                        if (depo_combobox.SelectedIndex == -1)
                        { provider.SetError(depo_combobox, "Ürünün ekleneceği depoyu seçiniz."); }
                        else
                        {
                            string yeniürünserinokayit = "insert into notebook_urun_seri_no_stok(urun_adi,urun_seri_no,urun_adeti,urun_durumu,urun_depo_cikisi) values " + "" + "(@urun_adi,@urun_seri_no,@urun_adeti,@urun_durumu,@urun_depo_cikisi)";
                            SqlCommand yeniürünserinokayitkomut = new SqlCommand(yeniürünserinokayit, connection);
                            yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_adi", ürün_adi_textbox.Text);
                            yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_seri_no", ürün_seri_no_textbox.Text);
                            yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_adeti", "1");
                            yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_durumu", "Kullanılmadı");
                            if (depo_combobox.SelectedItem.ToString() == "OS Depo Merkez")
                            { yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_depo_cikisi", "OS Depo Merkez"); }
                            else if (depo_combobox.SelectedItem.ToString() == "İndex Depo")
                            { yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_depo_cikisi", "İndex Depo"); }
                            else if (depo_combobox.SelectedItem.ToString() == "Arena Depo")
                            { yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_depo_cikisi", "Arena Depo"); }
                            else if (depo_combobox.SelectedItem.ToString() == "Penta Depo")
                            { yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_depo_cikisi", "Penta Depo"); }
                            else { yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_depo_cikisi", "Os Depo Merkez"); }
                            yeniürünserinokayitkomut.ExecuteNonQuery();

                            using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                            { Kullanıcı_girisi.Log(Kullanıcı_girisi.username + " adlı kullanıcı " + ürün_adi_textbox.Text + " ürününün " + ürün_seri_no_textbox.Text + " yeni seri numarasını ekledi.", w); }
                            using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                            { Kullanıcı_girisi.DumpLog(r); }

                            int tekrarsec = notebook_ürünler_listbox.SelectedIndex;
                            MessageBox.Show("Yeni seri numarası başarılı bir şekilde eklendi.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            notebook_ürünler_listbox.SelectedIndex = tekrarsec;
                            ürün_seri_no_textbox.Focus();
                        }
                    }
                    else
                    {
                        if (ürün_seri_no_textbox.Text.Length > 0 && ürün_stok_kodu_textbox.Text.Length > 0)
                        {
                            string yeniürünkodukayit = "insert into notebook_urun_kodları(urun_adi,urun_kodu) values " + "" + "(@urun_adi,@urun_kodu)";
                            SqlCommand kayitkomut = new SqlCommand(yeniürünkodukayit, connection);
                            kayitkomut.Parameters.AddWithValue("@urun_adi", ürün_adi_textbox.Text);
                            kayitkomut.Parameters.AddWithValue("@urun_kodu", ürün_stok_kodu_textbox.Text);
                            kayitkomut.ExecuteNonQuery();

                            string yeniürünserinokayit = "insert into notebook_urun_seri_no_stok(urun_adi,urun_seri_no,urun_adeti,urun_durumu,urun_depo_cikisi) values " + "" + "(@urun_adi,@urun_seri_no,@urun_adeti,@urun_durumu,@urun_depo_cikisi)";
                            SqlCommand yeniürünserinokayitkomut = new SqlCommand(yeniürünserinokayit, connection);
                            yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_adi", ürün_adi_textbox.Text);
                            yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_seri_no", ürün_seri_no_textbox.Text);
                            yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_adeti", "1");
                            yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_durumu", "Kullanılmadı");
                            if (depo_combobox.SelectedItem.ToString() == "OS Depo Merkez")
                            { yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_depo_cikisi", "OS Depo Merkez"); }
                            else if (depo_combobox.SelectedItem.ToString() == "İndex Depo")
                            { yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_depo_cikisi", "İndex Depo"); }
                            else if (depo_combobox.SelectedItem.ToString() == "Arena Depo")
                            { yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_depo_cikisi", "Arena Depo"); }
                            else if (depo_combobox.SelectedItem.ToString() == "Penta Depo")
                            { yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_depo_cikisi", "Penta Depo"); }
                            else { yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_depo_cikisi", "Os Depo Merkez"); }
                            yeniürünserinokayitkomut.ExecuteNonQuery();

                            using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                            {Kullanıcı_girisi.Log(Kullanıcı_girisi.username + " adlı kullanıcı " + ürün_adi_textbox.Text + " ürününün " + ürün_stok_kodu_textbox.Text + " ürün kodunu ve " + ürün_seri_no_textbox.Text + " yeni seri numarasını ekledi.", w);}
                            using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                            {Kullanıcı_girisi.DumpLog(r);}

                            MessageBox.Show("Yeni ürün stok kodu ve ürün seri numarası başarılı bir şekilde eklendi.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            int tekrarsec = notebook_ürünler_listbox.SelectedIndex;
                            notebook_ürünler_listbox.SelectedIndex = tekrarsec;
                            ürün_adi_textbox.Focus();
                        }
                    }
                }
                else
                {
                    if (notebook_ürünler_listbox.Items.Contains(ürün_adi_textbox.Text))
                    { provider.SetError(ürün_adi_textbox, "Bu ürün adı mevcuttur, başka ürün adı kullanınız.");}
                    else if (notebook_ürün_kodlari_listbox.Items.Contains(ürün_stok_kodu_textbox.Text))
                    {provider.SetError(ürün_stok_kodu_textbox, "Bu ürün kodu mevcuttur, başka ürün kodu kullanınız.");}
                    else if (notebook_ürün_seri_no_listbox.Items.Contains(ürün_seri_no_textbox.Text))
                    {provider.SetError(ürün_seri_no_textbox, "Bu seri numarası mevcuttur, başka seri numarası kullanınız.");}
                    else if (ürün_adi_textbox.Text == "")
                    {provider.SetError(ürün_adi_textbox, "Ürün adını boş bırakmayınız.");}
                    else
                    {
                        string yeniürünnotebookkayit = "insert into notebook_urunler(urun_adi) values " + "" + "(@urun_adi)";
                        SqlCommand yeniürünnotebookkayitkomut = new SqlCommand(yeniürünnotebookkayit, connection);
                        yeniürünnotebookkayitkomut.Parameters.AddWithValue("@urun_adi", ürün_adi_textbox.Text);
                        yeniürünnotebookkayitkomut.ExecuteNonQuery();
                        ürün_adi_textbox.Focus();

                        if (ürün_stok_kodu_textbox.Text.Length > 0)
                        {
                            string yeniürünkayit = "insert into notebook_urun_kodları(urun_adi,urun_kodu) values " + "" + "(@urun_adi,@urun_kodu)";
                            SqlCommand yeniürünkayitkomut = new SqlCommand(yeniürünkayit, connection);
                            yeniürünkayitkomut.Parameters.AddWithValue("@urun_adi", ürün_adi_textbox.Text);
                            yeniürünkayitkomut.Parameters.AddWithValue("@urun_kodu", ürün_stok_kodu_textbox.Text);
                            yeniürünkayitkomut.ExecuteNonQuery();
                        }
                        if (ürün_seri_no_textbox.Text.Length > 0)
                        {
                            if (depo_combobox.SelectedIndex == -1)
                            { provider.SetError(depo_combobox, "Ürünün ekleneceği depoyu seçiniz."); }
                            else
                            {
                                string yeniürünserinokayit = "insert into notebook_urun_seri_no_stok(urun_adi,urun_seri_no,urun_adeti,urun_durumu,urun_depo_cikisi) values " + "" + "(@urun_adi,@urun_seri_no,@urun_adeti,@urun_durumu,@urun_depo_cikisi)";
                                SqlCommand yeniürünserinokayitkomut = new SqlCommand(yeniürünserinokayit, connection);
                                yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_adi", ürün_adi_textbox.Text);
                                yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_seri_no", ürün_seri_no_textbox.Text);
                                yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_adeti", "1");
                                yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_durumu", "Kullanılmadı");
                                if (depo_combobox.SelectedItem.ToString() == "OS Depo Merkez")
                                { yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_depo_cikisi", "OS Depo Merkez"); }
                                else if (depo_combobox.SelectedItem.ToString() == "İndex Depo")
                                { yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_depo_cikisi", "İndex Depo"); }
                                else if (depo_combobox.SelectedItem.ToString() == "Arena Depo")
                                { yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_depo_cikisi", "Arena Depo"); }
                                else if (depo_combobox.SelectedItem.ToString() == "Penta Depo")
                                { yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_depo_cikisi", "Penta Depo"); }
                                else { yeniürünserinokayitkomut.Parameters.AddWithValue("@urun_depo_cikisi", "Os Depo Merkez"); }
                                yeniürünserinokayitkomut.ExecuteNonQuery();
                            }
                        }
                        
                        using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                        {Kullanıcı_girisi.Log(Kullanıcı_girisi.username + " adlı kullanıcı " + ürün_adi_textbox.Text + " yeni ürünü ekledi.", w);}
                        using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                        {Kullanıcı_girisi.DumpLog(r);}

                        MessageBox.Show("Yeni ürün oluşturuldu.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ürün_adi_textbox.Focus();
                    }
                }
                connection.Close();
                Listeyenile();
            }
            catch (Exception hata)
            {
                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.Log("Ürün oluşturulmadı, bağlantı kesildi.\nHata kodu: " + hata.Message, w);

                }
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.DumpLog(r);
                }
                MessageBox.Show("Ürün oluşturulmadı, bağlantı kesildi.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
        private void Notebook_ürün_kodlari_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (notebook_ürün_kodlari_listbox.SelectedIndex == -1)
            {
                MessageBox.Show("Geçerli ürün kodu seçiniz.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                ürün_stok_kodu_textbox.Text = notebook_ürün_kodlari_listbox.SelectedItem.ToString();
            }
        }

        private void Ana_menü_btn_Click(object sender, EventArgs e)
        {
            Anaform Anaform = new Anaform();
            Anaform.Show();
            Hide();
        }
        private void Notebook_ürün_seri_no_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (notebook_ürün_seri_no_listbox.SelectedIndex == -1)
            {
                MessageBox.Show("Geçerli seri numarası seçiniz.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                ürün_seri_no_textbox.Text = notebook_ürün_seri_no_listbox.SelectedItem.ToString();
            }
        }
        private void Secili_ürünü_sil_btn_Click(object sender, EventArgs e)
        {
            if (notebook_ürünler_listbox.SelectedIndex == -1 && notebook_ürün_kodlari_listbox.SelectedIndex == -1 && notebook_ürün_seri_no_listbox.SelectedIndex == -1)
            {
                MessageBox.Show("Silmek istediğiniz bir ürünü seçmeniz gerekmektedir.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (notebook_ürünler_listbox.SelectedIndex >= 0)
                {
                    DialogResult dialog = MessageBox.Show("Seçili ürünü kaldırırsanız ürüne ait ürün kodları, seri numaraları kaldırılacaktır. Kaldırılsın mı?", "OS BİLİŞİM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialog == DialogResult.Yes)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();
                                SqlCommand ürünadikontrol = new SqlCommand("Select * From notebook_urunler where urun_adi='" + ürün_adi_textbox.Text + "'", connection);
                                SqlDataReader ürünadikontrolokuyucu;
                                ürünadikontrolokuyucu = ürünadikontrol.ExecuteReader();
                                if (ürünadikontrolokuyucu.Read())
                                {
                                    SqlCommand ürünstokverisil = new SqlCommand("delete from notebook_urun_seri_no_stok where urun_adi = '" + ürün_adi_textbox.Text + "'", connection);
                                    ürünstokverisil.ExecuteNonQuery();
                                    SqlCommand notebookürünverisil = new SqlCommand("delete from notebook_urunler where urun_adi = '" + ürün_adi_textbox.Text + "'", connection);
                                    notebookürünverisil.ExecuteNonQuery();
                                    SqlCommand notebookürünkodverisil = new SqlCommand("delete from notebook_urun_kodları where urun_adi = '" + ürün_adi_textbox.Text + "'", connection);
                                    notebookürünkodverisil.ExecuteNonQuery();
                                    connection.Close();
                                    using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                                    {
                                        Kullanıcı_girisi.Log(Kullanıcı_girisi.username + " adlı kullanıcı " + notebook_ürünler_listbox.SelectedItem.ToString() + " ürününü sildi.", w);
                                    }
                                    using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                                    {
                                        Kullanıcı_girisi.DumpLog(r);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Seçmiş olduğunuz ürün sisteme kayıtlı değildir. Ürün isminin doğru olup olmadığını kontrol ederek tekrar deneyiniz.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Seçtiğiniz ürün silinmedi.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (Exception hata)
                        {
                            using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                            {
                                Kullanıcı_girisi.Log("Ürün silinmedi, bağlantı kesildi.\nHata kodu: " + hata.Message, w);

                            }
                            using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                            {
                                Kullanıcı_girisi.DumpLog(r);
                            }
                            MessageBox.Show("Ürün silinmedi, bağlantı kesildi.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }
                    }
                }
                Listeyenile();
            }
        }

        private void Secili_ürünün_kod_seri_no_sil_btn_Click(object sender, EventArgs e)
        {
            provider.Clear();
            if (notebook_ürün_kodlari_listbox.SelectedIndex == -1 && notebook_ürün_seri_no_listbox.SelectedIndex == -1 && index_seri_no_listbox.SelectedIndex == -1 && penta_seri_no_listbox.SelectedIndex == -1 && arena_seri_no_listbox.SelectedIndex == -1)
            {
                MessageBox.Show("Silmek istediğiniz ürün kodunu, seri numarasını seçmeniz gerekmektedir.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (notebook_ürün_kodlari_listbox.SelectedIndex >= 0)
                {
                    DialogResult dialog = MessageBox.Show("Seçili olan ürün kodu kaldırılacaktır. Kaldırılsın mı?", "OS BİLİŞİM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialog == DialogResult.Yes)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();
                                SqlCommand ürünstokverisil = new SqlCommand("delete from notebook_urun_kodları where urun_kodu = '" + ürün_stok_kodu_textbox.Text + "'", connection);
                                ürünstokverisil.ExecuteNonQuery();
                                MessageBox.Show("Seçmiş olduğunuz ürün kodu sistemden kaldırıldı.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                connection.Close();

                                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                                {
                                    Kullanıcı_girisi.Log(Kullanıcı_girisi.username + " adlı kullanıcı " + notebook_ürünler_listbox.SelectedItem.ToString() + " ürününün " + notebook_ürün_kodlari_listbox.SelectedItem.ToString() + " ürün kodunu sildi.", w);
                                }
                                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                                {
                                    Kullanıcı_girisi.DumpLog(r);
                                }
                            }
                        }
                        catch (Exception hata)
                        {
                            using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                            {
                                Kullanıcı_girisi.Log("Ürün kodu kaldırılmadı, bağlantı kesild.\n Hata kodu: " + hata.Message, w);

                            }
                            using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                            {
                                Kullanıcı_girisi.DumpLog(r);
                            }
                            MessageBox.Show("Ürün kodu kaldırılmadı, bağlantı kesild.\n Hata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }
                    }
                    else { MessageBox.Show("Seçili olan ürün kodu kaldırılmadı.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }
                if (notebook_ürün_seri_no_listbox.SelectedIndex >= 0)
                {
                    DialogResult dialog = MessageBox.Show("Seçili olan ürünün OS Depo Merkez'de bulunan seri numarası kaldırılacaktır. Kaldırılsın mı?", "OS BİLİŞİM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialog == DialogResult.Yes)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();
                                SqlCommand ürünstokverisil = new SqlCommand("delete from notebook_urun_seri_no_stok where urun_depo_cikisi = 'OS Depo Merkez' and urun_seri_no = '" + ürün_seri_no_textbox.Text + "'", connection);
                                ürünstokverisil.ExecuteNonQuery();
                                MessageBox.Show("Seçili ürün seri numarası kaldırıldı.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                connection.Close();
                                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                                {
                                    Kullanıcı_girisi.Log(Kullanıcı_girisi.username + " adlı kullanıcı " + notebook_ürünler_listbox.SelectedItem.ToString() + " ürününün OS Merkez Depo'da bulunan " + notebook_ürün_seri_no_listbox.SelectedItem.ToString() + " seri numarasını sildi.", w);
                                }
                                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                                {
                                    Kullanıcı_girisi.DumpLog(r);
                                }
                            }
                        }
                        catch (Exception hata)
                        {
                            using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                            {
                                Kullanıcı_girisi.Log("Ürün seri numarası kaldırılmadı, bağlantı kesild.\n Hata kodu: " + hata.Message, w);

                            }
                            using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                            {
                                Kullanıcı_girisi.DumpLog(r);
                            }
                            MessageBox.Show("Ürün seri numarası kaldırılmadı, bağlantı kesild.\n Hata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }
                    }
                    else { MessageBox.Show("Seçili olan ürün seri numarası kaldırılmadı.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }
                if (index_seri_no_listbox.SelectedIndex >= 0)
                {
                    DialogResult dialog = MessageBox.Show("Seçili olan ürünün İndex Depo'da bulunan seri numarası kaldırılacaktır. Kaldırılsın mı?", "OS BİLİŞİM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialog == DialogResult.Yes)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();
                                SqlCommand indexserinosil = new SqlCommand("delete from notebook_urun_seri_no_stok where urun_depo_cikisi = 'İndex Depo' and urun_seri_no = '" + ürün_seri_no_textbox.Text + "'", connection);
                                indexserinosil.ExecuteNonQuery();
                                MessageBox.Show("Seçili ürünün İndex Depo'da bulunan seri numarası kaldırıldı.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                connection.Close();
                                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                                {Kullanıcı_girisi.Log(Kullanıcı_girisi.username + " adlı kullanıcı " + notebook_ürünler_listbox.SelectedItem.ToString() + " ürününün İndex Depo'da bulunan " + index_seri_no_listbox.SelectedItem.ToString() + " seri numarasını sildi.", w);}
                                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                                {Kullanıcı_girisi.DumpLog(r);}
                            }
                        }
                        catch (Exception hata)
                        {
                            using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                            {
                                Kullanıcı_girisi.Log("Ürün seri numarası kaldırılmadı, bağlantı kesild.\n Hata kodu: " + hata.Message, w);

                            }
                            using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                            {
                                Kullanıcı_girisi.DumpLog(r);
                            }
                            MessageBox.Show("Ürün seri numarası kaldırılmadı, bağlantı kesild.\n Hata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }
                    }
                    else { MessageBox.Show("Seçili olan ürün seri numarası kaldırılmadı.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }
                if (penta_seri_no_listbox.SelectedIndex >= 0)
                {
                    DialogResult dialog = MessageBox.Show("Seçili olan ürünün Penta Depo'da bulunan seri numarası kaldırılacaktır. Kaldırılsın mı?", "OS BİLİŞİM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialog == DialogResult.Yes)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();
                                SqlCommand pentaserinosilme = new SqlCommand("delete from notebook_urun_seri_no_stok where urun_depo_cikisi = 'Penta Depo' and urun_seri_no = '" + ürün_seri_no_textbox.Text + "'", connection);
                                pentaserinosilme.ExecuteNonQuery();
                                MessageBox.Show("Seçili ürünün Penta Depo'da bulunan seri numarası kaldırıldı.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                connection.Close();
                                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                                {
                                    Kullanıcı_girisi.Log(Kullanıcı_girisi.username + " adlı kullanıcı " + notebook_ürünler_listbox.SelectedItem.ToString() + " ürününün Penta Depo'da bulunan " + penta_seri_no_listbox.SelectedItem.ToString() + " seri numarasını sildi.", w);
                                }
                                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                                {
                                    Kullanıcı_girisi.DumpLog(r);
                                }
                            }
                        }
                        catch (Exception hata)
                        {
                            using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                            {
                                Kullanıcı_girisi.Log("Ürün seri numarası kaldırılmadı, bağlantı kesild.\n Hata kodu: " + hata.Message, w);

                            }
                            using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                            {
                                Kullanıcı_girisi.DumpLog(r);
                            }
                            MessageBox.Show("Ürün seri numarası kaldırılmadı, bağlantı kesild.\n Hata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }
                    }
                    else { MessageBox.Show("Seçili olan ürün seri numarası kaldırılmadı.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }
                if (arena_seri_no_listbox.SelectedIndex >= 0)
                {
                    DialogResult dialog = MessageBox.Show("Seçili olan ürünün Arena Depo'da bulunan seri numarası kaldırılacaktır. Kaldırılsın mı?", "OS BİLİŞİM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialog == DialogResult.Yes)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();
                                SqlCommand arenaserinosilme = new SqlCommand("delete from notebook_urun_seri_no_stok where urun_depo_cikisi = 'Arena Depo' and urun_seri_no = '" + ürün_seri_no_textbox.Text + "'", connection);
                                arenaserinosilme.ExecuteNonQuery();
                                MessageBox.Show("Seçili ürünün Arena Depo'da bulunan seri numarası kaldırıldı.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                connection.Close();
                                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                                {
                                    Kullanıcı_girisi.Log(Kullanıcı_girisi.username + " adlı kullanıcı " + notebook_ürünler_listbox.SelectedItem.ToString() + " ürününün Arena Depo'da bulunan " + arena_seri_no_listbox.SelectedItem.ToString() + " seri numarasını sildi.", w);
                                }
                                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                                {
                                    Kullanıcı_girisi.DumpLog(r);
                                }
                            }
                        }
                        catch (Exception hata)
                        {
                            using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                            {
                                Kullanıcı_girisi.Log("Ürün seri numarası kaldırılmadı, bağlantı kesild.\n Hata kodu: " + hata.Message, w);

                            }
                            using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                            {
                                Kullanıcı_girisi.DumpLog(r);
                            }
                            MessageBox.Show("Ürün seri numarası kaldırılmadı, bağlantı kesild.\n Hata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }
                    }
                    else { MessageBox.Show("Seçili olan ürün seri numarası kaldırılmadı.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }
                connection.Close();
                Listeyenile();
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

        private void Windows_kücültme_label_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void Ürün_seri_no_textbox_TextChanged(object sender, EventArgs e)
        {
            string serino = "S";
            string serino2 = "s";
            if (ürün_seri_no_textbox.Text.StartsWith(serino) || ürün_seri_no_textbox.Text.StartsWith(serino2))
            {
                string degistir = ürün_seri_no_textbox.Text.Substring(1, ürün_seri_no_textbox.Text.Length - 1);
                ürün_seri_no_textbox.Text = degistir;
            }
        }

        private void Ürünkalankullanımlabel_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                 connection.Open();
                if (notebook_ürünler_listbox.SelectedIndex == -1)
                {
                    MessageBox.Show("Geçerli bir ürün seçiniz.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    SqlCommand komut3 = new SqlCommand("SELECT urun_seri_no FROM notebook_urun_seri_no_stok where urun_durumu = 'Kullanılmadı' and urun_depo_cikisi = 'OS Depo Merkez' and urun_adi = '" + notebook_ürünler_listbox.SelectedItem.ToString() + "'", connection);
                    SqlDataReader veriokuyucu3;
                    veriokuyucu3 = komut3.ExecuteReader();
                    notebook_ürün_seri_no_listbox.Items.Clear();
                    while (veriokuyucu3.Read())
                    {
                        notebook_ürün_seri_no_listbox.Items.Add(veriokuyucu3["urun_seri_no"]);
                    }
                    if (notebook_ürün_seri_no_listbox.Items.Count < 1)
                    {
                        notebook_ürün_seri_no_listbox.Items.Add("Kullanılacak ürün yok.");
                    }
                    veriokuyucu3.Close();
                    connection.Close();
                }
            }
            catch (Exception hata)
            {
                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.Log("Ürün bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + hata.Message, w);

                }
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.DumpLog(r);
                }
                MessageBox.Show("Ürün bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        new
        #region forumharaketettirme
        int Move;
        int Mouse_X;
        int Mouse_Y;
        private void Ürün_Ekle_ve_Düzenleme_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
            this.Cursor = Cursors.SizeAll;

        }

        private void Ürün_Ekle_ve_Düzenleme_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
            this.Cursor = Cursors.Default;
        }

        private void Ürün_Ekle_ve_Düzenleme_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }
        #endregion
        #region forumharaketettirme2 
        private void Panel4_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
            this.Cursor = Cursors.Default;
        }

        private void Panel4_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }

        }

        private void Panel4_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
            this.Cursor = Cursors.SizeAll;
        }

        #endregion
        #region renkayarı
        private void linkLabel1_MouseLeave(object sender, EventArgs e)
        {
            linkLabel1.LinkColor = Color.FromArgb(22, 53, 56);
        }

        private void linkLabel1_MouseMove(object sender, MouseEventArgs e)
        {
            linkLabel1.LinkColor = Color.FromArgb(13, 31, 33);
        }
        private void Ana_menü_btn_MouseMove(object sender, MouseEventArgs e)
        {
            ana_menü_btn.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void Secili_ürünün_kod_seri_no_sil_btn_MouseMove(object sender, MouseEventArgs e)
        {
            secili_ürünün_kod_seri_no_sil_btn.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void Secili_ürünü_sil_btn_MouseMove(object sender, MouseEventArgs e)
        {
            secili_ürünü_sil_btn.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void Yeni_ürün_ekle_btn_MouseMove(object sender, MouseEventArgs e)
        {
            yeni_ürün_ekle_btn.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void Yeni_ürün_ekle_btn_MouseLeave(object sender, EventArgs e)
        {
           yeni_ürün_ekle_btn.BackColor = Color.FromArgb(22, 53, 56);
        }

        private void Secili_ürünü_sil_btn_MouseLeave(object sender, EventArgs e)
        {
          secili_ürünü_sil_btn.BackColor = Color.FromArgb(22, 53, 56);
        }

        private void Secili_ürünün_kod_seri_no_sil_btn_MouseLeave(object sender, EventArgs e)
        {
           secili_ürünün_kod_seri_no_sil_btn.BackColor = Color.FromArgb(22, 53, 56);
        }

        private void Ana_menü_btn_MouseLeave(object sender, EventArgs e)
        {
            ana_menü_btn.BackColor = Color.FromArgb(22, 53, 56);
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://mail.google.com/");
        }

        private void Logout_label_MouseMove(object sender, MouseEventArgs e)
        {
            logout_label.ForeColor = Color.FromArgb(13, 31, 33);
        }

        private void Windows_kücültme_label_MouseMove(object sender, MouseEventArgs e)
        {
            windows_kücültme_label.ForeColor = Color.FromArgb(13, 31, 33);
        }

        private void Logout_label_MouseLeave(object sender, EventArgs e)
        {
            logout_label.ForeColor = Color.FromArgb(22, 53, 56);
        }

        private void Windows_kücültme_label_MouseLeave(object sender, EventArgs e)
        {
            windows_kücültme_label.ForeColor = Color.FromArgb(22, 53, 56);
        }

        #endregion

        private void indexürünkullanım_label_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                if (notebook_ürünler_listbox.SelectedIndex == -1)
                {
                    MessageBox.Show("Geçerli bir ürün seçiniz.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    SqlCommand indexurunkullanımsorgulama = new SqlCommand("SELECT urun_seri_no FROM notebook_urun_seri_no_stok where urun_durumu = 'Kullanılmadı' and urun_depo_cikisi = 'İndex Depo' and urun_adi = '" + notebook_ürünler_listbox.SelectedItem.ToString() + "'", connection);
                    SqlDataReader indexurunkullanımsorgulamaokuyucu;
                    indexurunkullanımsorgulamaokuyucu = indexurunkullanımsorgulama.ExecuteReader();
                    index_seri_no_listbox.Items.Clear();
                    while (indexurunkullanımsorgulamaokuyucu.Read())
                    {
                        index_seri_no_listbox.Items.Add(indexurunkullanımsorgulamaokuyucu["urun_seri_no"]);
                    }
                    if (index_seri_no_listbox.Items.Count < 1)
                    {
                        index_seri_no_listbox.Items.Add("Kullanılacak ürün yok.");
                    }
                    indexurunkullanımsorgulamaokuyucu.Close();
                    connection.Close();
                }
            }
            catch (Exception hata)
            {
                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.Log("Ürün bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + hata.Message, w);

                }
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.DumpLog(r);
                }
                MessageBox.Show("Ürün bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void pentaürünkullanım_label_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                if (notebook_ürünler_listbox.SelectedIndex == -1)
                {
                    MessageBox.Show("Geçerli bir ürün seçiniz.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    SqlCommand pentaurunkullanımsorgulama = new SqlCommand("SELECT urun_seri_no FROM notebook_urun_seri_no_stok where urun_durumu = 'Kullanılmadı' and urun_depo_cikisi = 'Penta Depo' and urun_adi = '" + notebook_ürünler_listbox.SelectedItem.ToString() + "'", connection);
                    SqlDataReader pentaurunkullanımsorgulamaokuyucu;
                    pentaurunkullanımsorgulamaokuyucu = pentaurunkullanımsorgulama.ExecuteReader();
                    penta_seri_no_listbox.Items.Clear();
                    while (pentaurunkullanımsorgulamaokuyucu.Read())
                    {
                        penta_seri_no_listbox.Items.Add(pentaurunkullanımsorgulamaokuyucu["urun_seri_no"]);
                    }
                    if (penta_seri_no_listbox.Items.Count < 1)
                    {
                        penta_seri_no_listbox.Items.Add("Kullanılacak ürün yok.");
                    }
                    pentaurunkullanımsorgulamaokuyucu.Close();
                    connection.Close();
                }
            }
            catch (Exception hata)
            {
                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.Log("Ürün bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + hata.Message, w);

                }
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.DumpLog(r);
                }
                MessageBox.Show("Ürün bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void arenaürünkullanım_label_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                if (notebook_ürünler_listbox.SelectedIndex == -1)
                {
                    MessageBox.Show("Geçerli bir ürün seçiniz.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    SqlCommand arenaurunkullanımsorgulama = new SqlCommand("SELECT urun_seri_no FROM notebook_urun_seri_no_stok where urun_durumu = 'Kullanılmadı' and urun_depo_cikisi = 'Arena Depo' and urun_adi = '" + notebook_ürünler_listbox.SelectedItem.ToString() + "'", connection);
                    SqlDataReader arenaurunkullanımsorgulamaokuyucu;
                    arenaurunkullanımsorgulamaokuyucu = arenaurunkullanımsorgulama.ExecuteReader();
                    arena_seri_no_listbox.Items.Clear();
                    while (arenaurunkullanımsorgulamaokuyucu.Read())
                    {
                        arena_seri_no_listbox.Items.Add(arenaurunkullanımsorgulamaokuyucu["urun_seri_no"]);
                    }
                    if (arena_seri_no_listbox.Items.Count < 1)
                    {
                        arena_seri_no_listbox.Items.Add("Kullanılacak ürün yok.");
                    }
                    arenaurunkullanımsorgulamaokuyucu.Close();
                    connection.Close();
                }
            }
            catch (Exception hata)
            {
                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.Log("Ürün bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + hata.Message, w);

                }
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.DumpLog(r);
                }
                MessageBox.Show("Ürün bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void index_seri_no_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (index_seri_no_listbox.SelectedIndex == -1)
            {
                MessageBox.Show("Geçerli seri numarası seçiniz.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                ürün_seri_no_textbox.Text = index_seri_no_listbox.SelectedItem.ToString();
            }
        }

        private void penta_seri_no_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (penta_seri_no_listbox.SelectedIndex == -1)
            {
                MessageBox.Show("Geçerli seri numarası seçiniz.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                ürün_seri_no_textbox.Text = penta_seri_no_listbox.SelectedItem.ToString();
            }
        }

        private void arena_seri_no_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (arena_seri_no_listbox.SelectedIndex == -1)
            {
                MessageBox.Show("Geçerli seri numarası seçiniz.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                ürün_seri_no_textbox.Text = arena_seri_no_listbox.SelectedItem.ToString();
            }
        }
    }
}