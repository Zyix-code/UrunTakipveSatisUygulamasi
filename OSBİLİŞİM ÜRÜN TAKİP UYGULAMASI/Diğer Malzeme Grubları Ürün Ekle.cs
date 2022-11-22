using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace OSBilişim
{
    public partial class Diger_malzeme_grubları_urun_ekle : Form
    {
        public Diger_malzeme_grubları_urun_ekle()
        {
            InitializeComponent();
        }
        readonly SqlConnection connection = new SqlConnection("Data Source=192.168.1.123,1433;Network Library=DBMSSOCN; Initial Catalog=OSBİLİSİM;User Id=Admin; Password=1; MultipleActiveResultSets=True;");

        public void Listeyenile()
        {
            diger_ürün_adi_textbox.Text = "";
            diger_ürün_serino_textbox.Text = "";
            diger_ürün_kodu_textbox.Text = "";
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    int tekrarsec = diger_ürün_adi_listbox.SelectedIndex;
                    diger_ürün_adi_listbox.Items.Clear();
                    SqlCommand komut = new SqlCommand("SELECT diger_urun_adi from diger_ürünler ", connection);
                    SqlDataReader veriokuyucu;
                    veriokuyucu = komut.ExecuteReader();
                    while (veriokuyucu.Read())
                    {
                        diger_ürün_adi_listbox.Items.Add(veriokuyucu["diger_urun_adi"]);
                    }
                    veriokuyucu.Close();
                    connection.Close();
                    if (diger_ürün_adi_listbox.SelectedIndex > tekrarsec)
                    { diger_ürün_adi_listbox.SelectedIndex = -1; }
                    else { diger_ürün_adi_listbox.SelectedIndex = tekrarsec; }

                    diger_ürün_kodu_listbox.Items.Clear();
                    connection.Open();
                    SqlCommand urunstokkodutakip = new SqlCommand("SELECT diger_urun_stok_kodu from diger_ürün_kodları where diger_urun_adi = '" + diger_ürün_adi_listbox.SelectedItem + "'", connection);
                    SqlDataReader urunstokkoduokuyucu;
                    urunstokkoduokuyucu = urunstokkodutakip.ExecuteReader();
                    while (urunstokkoduokuyucu.Read())
                    {
                        diger_ürün_kodu_listbox.Items.Add(urunstokkoduokuyucu["diger_urun_stok_kodu"]);
                    }

                    urunstokkoduokuyucu.Close();
                    connection.Close();
                    diger_ürün_serino_listbox.Items.Clear();
                    connection.Open();
                    SqlCommand urunserinotakip = new SqlCommand("SELECT diger_urun_serino from diger_ürün_stok where diger_urun_adi = '" + diger_ürün_adi_listbox.SelectedItem + "'", connection);
                    SqlDataReader urunserinookuyucu;
                    urunserinookuyucu = urunserinotakip.ExecuteReader();
                    while (urunserinookuyucu.Read())
                    {
                        diger_ürün_serino_listbox.Items.Add(urunserinookuyucu["diger_urun_serino"]);
                    }
                    urunserinookuyucu.Close();

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
        ErrorProvider provider = new ErrorProvider();
        private void Diğer_malzeme_Grubları_Ürün_ekle_ve_Düzenleme_Load(object sender, EventArgs e)
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
                    Kullanıcı_girisi.Log("Bağlantı kesildi.\nHata kodu: " + hata.Message, w);
                }
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.DumpLog(r);
                }
                MessageBox.Show("Bağlantı kesildi.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            if (diger_ürün_adi_listbox.SelectedIndex == -1)
            {
                diger_ürün_kodu_listbox.Items.Add("Lütfen bir ürün seçin.");
                diger_ürün_serino_listbox.Items.Add("Lütfen bir ürün seçin.");
            }
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                SqlCommand komut = new SqlCommand("SELECT * FROM diger_ürünler ", connection);
                SqlDataReader veriokuyucu1;
                veriokuyucu1 = komut.ExecuteReader();
                while (veriokuyucu1.Read())
                {
                    diger_ürün_adi_listbox.Items.Add(veriokuyucu1["diger_urun_adi"]);
                }
                veriokuyucu1.Close();
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

        private void Diger_ürünler_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (diger_ürün_adi_listbox.SelectedIndex == -1)
            {
                MessageBox.Show("Geçerli ürün seçiniz.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                diger_ürün_adi_textbox.Text = diger_ürün_adi_listbox.SelectedItem.ToString();
                try
                {
                    using (var cn = new SqlConnection("server=192.168.1.123,1433;database=OSBİLİSİM;UId=Admin;Pwd=1;MultipleActiveResultSets=True;"))
                    using (var cmd = new SqlCommand(@"select COUNT(diger_urun_serino) from diger_ürün_stok where diger_urun_durumu = 'Kullanılmadı' and diger_urun_adi = '" + diger_ürün_adi_listbox.SelectedItem.ToString() + "'", cn))
                    {
                        cn.Open();
                        var kalan = Convert.ToInt32(cmd.ExecuteScalar());
                        ürünkalankullanımlabel.Text = "Kullanılmamış ürün sayısı: " + kalan.ToString();
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
                    MessageBox.Show("Ürün bilgileri alınamadı, bağlantı kesildi.\nHata kodu:" + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            diger_ürün_kodu_textbox.Text = "";
            try
            {
                if (diger_ürün_adi_listbox.SelectedIndex == -1)
                {
                    diger_ürün_kodu_listbox.Items.Clear();
                    diger_ürün_serino_listbox.Items.Clear();
                }
                else
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                        diger_ürün_kodu_listbox.Items.Clear();
                        SqlCommand komut3 = new SqlCommand("SELECT * FROM diger_ürün_kodları where diger_urun_adi = '" + diger_ürün_adi_listbox.SelectedItem + "'", connection);
                        SqlDataReader veriokuyucu3;
                        veriokuyucu3 = komut3.ExecuteReader();
                        while (veriokuyucu3.Read())
                        {
                            diger_ürün_kodu_listbox.Items.Add(veriokuyucu3["diger_urun_stok_kodu"]);
                        }
                        veriokuyucu3.Close();

                        diger_ürün_serino_listbox.Items.Clear();
                        SqlCommand üründurum = new SqlCommand("SELECT * FROM diger_ürün_stok where diger_urun_adi = '" + diger_ürün_adi_listbox.SelectedItem + "'", connection);
                        SqlDataReader üründurumusorgulama;
                        üründurumusorgulama = üründurum.ExecuteReader();
                        while (üründurumusorgulama.Read())
                        {
                            diger_ürün_serino_listbox.Items.Add(üründurumusorgulama["diger_urun_serino"]);

                        }
                        üründurumusorgulama.Close();

                    }
                    connection.Close();
                }
            }
            catch (Exception hata)
            {
                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.Log("Ürün bilgileri alınamadı, bağlantı kesildi.\nHata kodu:" + hata.Message, w);

                }
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.DumpLog(r);
                }
                MessageBox.Show("Ürün bilgileri alınamadı, bağlantı kesildi.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void Diger_ürün_kodları_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (diger_ürün_kodu_listbox.SelectedIndex == -1)
            {
                MessageBox.Show("Geçerli ürün kodu seçiniz.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                diger_ürün_kodu_textbox.Text = diger_ürün_kodu_listbox.SelectedItem.ToString();
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

        private void Secili_ürünün_kod_seri_no_sil_btn_Click(object sender, EventArgs e)
        {
            if (diger_ürün_kodu_listbox.SelectedIndex == -1 && diger_ürün_serino_listbox.SelectedIndex == -1)
            {
                MessageBox.Show("Silinmesi gereken geçerli ürün kodunu, seri numarasını seçmeniz gerekmektedir.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (diger_ürün_kodu_listbox.SelectedIndex >= 0)
                {
                    DialogResult dialog = MessageBox.Show("Seçili olan ürün kodu kaldırılacaktır. Kaldırılsın mı?", "OS BİLİŞİM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialog == DialogResult.Yes)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();
                                SqlCommand ürünstokverisil = new SqlCommand("delete from diger_ürün_kodları where diger_urun_stok_kodu = '" + diger_ürün_kodu_textbox.Text + "'", connection);
                                ürünstokverisil.ExecuteNonQuery();
                                MessageBox.Show("Seçili ürün kodu kaldırıldı.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                connection.Close();

                                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                                {
                                    Kullanıcı_girisi.Log(Kullanıcı_girisi.username + " adlı kullanıcı " + diger_ürün_adi_listbox.SelectedItem.ToString() + " ürününün " + diger_ürün_kodu_listbox.SelectedItem.ToString() + " ürün kodunu sildi.", w);
                                }
                                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                                {
                                    Kullanıcı_girisi.DumpLog(r);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Seçili olan ürün kodu kaldırılmadı.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (Exception hata)
                        {
                            using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                            {
                                Kullanıcı_girisi.Log("Ürün kodu kaldırılmadı, bağlantı kesildi.\nHata kodu: " + hata.Message, w);

                            }
                            using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                            {
                                Kullanıcı_girisi.DumpLog(r);
                            }
                            MessageBox.Show("Ürün kodu kaldırılmadı, bağlantı kesildi.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }
                    }
                    else { MessageBox.Show("Seçili olan ürün kodu kaldırılmadı.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }

                if (diger_ürün_serino_listbox.SelectedIndex >= 0)
                {
                    DialogResult dialog = MessageBox.Show("Seçili olan ürünün seri numarası kaldırılacaktır. Kaldırılsın mı?", "OS BİLİŞİM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialog == DialogResult.Yes)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();
                                SqlCommand ürünstokverisil = new SqlCommand("delete from diger_ürün_stok where diger_urun_serino = '" + diger_ürün_serino_textbox.Text + "'", connection);
                                ürünstokverisil.ExecuteNonQuery();
                                MessageBox.Show("Seçili ürün seri numarası kaldırıldı.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                connection.Close();
                               
                                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                                {
                                    Kullanıcı_girisi.Log(Kullanıcı_girisi.username + " adlı kullanıcı " + diger_ürün_adi_listbox.SelectedItem.ToString() + " ürününün " + diger_ürün_serino_listbox.SelectedItem.ToString() + " seri numarasını sildi.", w);
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
                                Kullanıcı_girisi.Log("Ürün seri numarası kaldırılmadı, bağlantı kesildi.\nHata kodu: " + hata.Message, w);

                            }
                            using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                            {
                                Kullanıcı_girisi.DumpLog(r);
                            }

                            MessageBox.Show("Ürün seri numarası kaldırılmadı, bağlantı kesildi.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }
                    }
                    else { MessageBox.Show("Seçili olan ürün seri numarası kaldırılmadı.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }
                Listeyenile();
                connection.Close();
            }
        }

        private void Ana_menü_btn_Click(object sender, EventArgs e)
        {
            Anaform anaform = new Anaform();
            anaform.Show();
            Hide();
        }

        private void Windows_kücültme_label_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Secili_ürünü_sil_btn_Click(object sender, EventArgs e)
        {
            if (diger_ürün_adi_listbox.SelectedIndex == -1 && diger_ürün_kodu_listbox.SelectedIndex == -1)
            {
                MessageBox.Show("Silinmesi gereken geçerli ürünleri seçmeniz gerekmektedir.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (diger_ürün_adi_listbox.SelectedIndex >= 0)
                {
                    DialogResult dialog = MessageBox.Show("Seçili ürünü kaldırırsanız ürüne ait ürün kodları, seri numaraları kaldırılacaktır. Kaldırılsın mı?", "OS BİLİŞİM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialog == DialogResult.Yes)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();
                                SqlCommand ürünadikontrol = new SqlCommand("Select * From diger_ürünler where diger_urun_adi = '" + diger_ürün_adi_textbox.Text + "'", connection);
                                SqlDataReader ürünadikontrolokuyucu;
                                ürünadikontrolokuyucu = ürünadikontrol.ExecuteReader();
                                if (ürünadikontrolokuyucu.Read())
                                {
                                    SqlCommand notebookürünverisil = new SqlCommand("delete from diger_ürünler where diger_urun_adi = '" + diger_ürün_adi_textbox.Text + "'", connection);
                                    notebookürünverisil.ExecuteNonQuery();
                                    SqlCommand notebookürünkodverisil = new SqlCommand("delete from diger_ürün_kodları where diger_urun_adi = '" + diger_ürün_adi_textbox.Text + "'", connection);
                                    notebookürünkodverisil.ExecuteNonQuery();
                                    SqlCommand notebookürünserinoverisil = new SqlCommand("delete from diger_ürün_stok where diger_urun_adi = '" + diger_ürün_adi_textbox.Text + "'", connection);
                                    notebookürünserinoverisil.ExecuteNonQuery();
                                    MessageBox.Show("Seçili ürün başarılı şekilde kaldırıldı.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                                    {Kullanıcı_girisi.Log(Kullanıcı_girisi.username + " adlı kullanıcı " + diger_ürün_adi_listbox.SelectedItem.ToString() + " ürününü sildi.", w);}
                                    using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                                    { Kullanıcı_girisi.DumpLog(r);}
                                    
                                }
                                else
                                {
                                    MessageBox.Show("Yazılan ürün sisteme kayıtlı değildir. Geçerli ürünü silmek için ürün adını düzeltiniz.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                connection.Close();
                            }
                            else
                            {
                                MessageBox.Show("Seçili ürün silinmedi.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (Exception hata)
                        {
                            using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                            {
                                Kullanıcı_girisi.Log("Seçili ürün silinmedi, bağlantı kesildi.\nHata kodu: " + hata.Message, w);

                            }
                            using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                            {
                                Kullanıcı_girisi.DumpLog(r);
                            }

                            MessageBox.Show("Seçili ürün silinmedi, bağlantı kesildi.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }
                    }
                }
                Listeyenile();
                connection.Close();
            }
        }

        private void Yeni_ürün_ekle_btn_Click(object sender, EventArgs e)
        {
            provider.Clear();
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    if (diger_ürün_adi_listbox.Items.Contains(diger_ürün_adi_textbox.Text) && diger_ürün_kodu_listbox.Items.Contains(diger_ürün_kodu_textbox.Text))
                    {
                        if (diger_ürün_serino_listbox.Items.Contains(diger_ürün_serino_textbox.Text))
                        {
                            provider.SetError(diger_ürün_serino_textbox, "Bu seri numarası mevcuttur. Başka seri numarası kullanınız.");
                        }
                        else if (diger_ürün_adi_textbox.Text == "")
                        {
                            provider.SetError(diger_ürün_adi_textbox, "Ürün adını boş bırakmayınız.");
                        }
                        else if (diger_ürün_serino_textbox.Text == "")
                        {
                            provider.SetError(diger_ürün_serino_textbox, "Ürün seri numarasını boş bırakmayınız.");
                        }
                        else if (diger_ürün_kodu_textbox.Text == "")
                        {
                            provider.SetError(diger_ürün_kodu_textbox, "Ürün stok kodunu boş bırakmayınız.");
                        }
                        else
                        {
                            string yeniürünserinokayit = "insert into diger_ürün_stok(diger_urun_adi,diger_urun_serino,diger_urun_adeti,diger_urun_durumu) values " + "" + "(@diger_urun_adi,@diger_urun_serino,@diger_urun_adeti,@diger_urun_durumu)";
                            SqlCommand yeniürünserinokayitkomut = new SqlCommand(yeniürünserinokayit, connection);
                            yeniürünserinokayitkomut.Parameters.AddWithValue("@diger_urun_adi", diger_ürün_adi_textbox.Text);
                            yeniürünserinokayitkomut.Parameters.AddWithValue("@diger_urun_serino", diger_ürün_serino_textbox.Text);
                            yeniürünserinokayitkomut.Parameters.AddWithValue("@diger_urun_adeti", "1");
                            yeniürünserinokayitkomut.Parameters.AddWithValue("@diger_urun_durumu", "Kullanılmadı");
                            yeniürünserinokayitkomut.ExecuteNonQuery();
                            using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                            {
                                Kullanıcı_girisi.Log(Kullanıcı_girisi.username + " adlı kullanıcı " + diger_ürün_adi_textbox.Text + " ürününün " + diger_ürün_serino_textbox.Text + " yeni seri numarasını ekledi.", w);
                            }
                            using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                            {
                                Kullanıcı_girisi.DumpLog(r);
                            }
                            int tekrarsec = diger_ürün_adi_listbox.SelectedIndex;
                            MessageBox.Show("Yeni seri numarası başarılı bir şekilde eklendi.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            diger_ürün_adi_listbox.SelectedIndex = tekrarsec;
                            diger_ürün_serino_textbox.Focus();
                        }
                    }
                    else if (diger_ürün_adi_listbox.Items.Contains(diger_ürün_adi_textbox.Text))
                    {
                        if (diger_ürün_kodu_listbox.Items.Contains(diger_ürün_kodu_textbox.Text))
                        {
                            provider.SetError(diger_ürün_kodu_textbox, "Bu ürün kodu mevcuttur. Başka ürün kodu kullanınız.");
                        }
                        else if (diger_ürün_serino_listbox.Items.Contains(diger_ürün_serino_textbox.Text))
                        {
                            provider.SetError(diger_ürün_serino_textbox, "Bu seri numarası mevcuttur. Başka seri numarası kullanınız.");
                        }
                        else if (diger_ürün_adi_textbox.Text == "")
                        {
                            provider.SetError(diger_ürün_adi_textbox, "Ürün adını boş bırakmayınız.");
                        }
                        else if (diger_ürün_kodu_textbox.Text == "" && diger_ürün_serino_textbox.Text.Length <= 0)
                        {
                            provider.SetError(diger_ürün_kodu_textbox, "Ürün stok kodunu boş bırakmayınız.");
                        }
                        else if (diger_ürün_serino_textbox.Text == "" && diger_ürün_kodu_textbox.Text.Length <= 0)
                        {
                            provider.SetError(diger_ürün_serino_textbox, "Ürün seri numarasını boş bırakmayınız.");
                        }
                        else if (diger_ürün_serino_textbox.Text == "" && diger_ürün_kodu_textbox.Text.Length > 0)
                        {
                            string yeniürünkodukayit = "insert into diger_ürün_kodları(diger_urun_adi,diger_urun_stok_kodu) values " + "" + "(@diger_urun_adi,@diger_urun_stok_kodu)";
                            SqlCommand kayitkomut = new SqlCommand(yeniürünkodukayit, connection);
                            kayitkomut.Parameters.AddWithValue("@diger_urun_adi", diger_ürün_adi_textbox.Text);
                            kayitkomut.Parameters.AddWithValue("@diger_urun_stok_kodu", diger_ürün_kodu_textbox.Text);
                            kayitkomut.ExecuteNonQuery();

                            using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                            {
                                Kullanıcı_girisi.Log(Kullanıcı_girisi.username + " adlı kullanıcı " + diger_ürün_adi_textbox.Text + " ürününün " + diger_ürün_kodu_textbox.Text + " yeni ürün kodunu ekledi.", w);
                            }
                            using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                            {
                                Kullanıcı_girisi.DumpLog(r);
                            }
                            MessageBox.Show("Yeni ürün stok kodu başarılı bir şekilde eklendi.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            int tekrarsec = diger_ürün_adi_listbox.SelectedIndex;
                            diger_ürün_adi_listbox.SelectedIndex = tekrarsec;
                            diger_ürün_kodu_textbox.Focus();
                        }
                        else if (diger_ürün_kodu_textbox.Text == "" && diger_ürün_serino_textbox.Text.Length >= 0)
                        {
                            string yeniürünserinokayit = "insert into diger_ürün_stok(diger_urun_adi,diger_urun_serino,diger_urun_adeti,diger_urun_durumu) values " + "" + "(@diger_urun_adi,@diger_urun_serino,@diger_urun_adeti,@diger_urun_durumu)";
                            SqlCommand yeniürünserinokayitkomut = new SqlCommand(yeniürünserinokayit, connection);
                            yeniürünserinokayitkomut.Parameters.AddWithValue("@diger_urun_adi", diger_ürün_adi_textbox.Text);
                            yeniürünserinokayitkomut.Parameters.AddWithValue("@diger_urun_serino", diger_ürün_serino_textbox.Text);
                            yeniürünserinokayitkomut.Parameters.AddWithValue("@diger_urun_adeti", "1");
                            yeniürünserinokayitkomut.Parameters.AddWithValue("@diger_urun_durumu", "Kullanılmadı");
                            yeniürünserinokayitkomut.ExecuteNonQuery();

                            using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                            {
                                Kullanıcı_girisi.Log(Kullanıcı_girisi.username + " adlı kullanıcı " + diger_ürün_adi_textbox.Text + " ürününün " + diger_ürün_serino_textbox.Text + " yeni seri numarasını ekledi.", w);
                            }
                            using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                            {
                                Kullanıcı_girisi.DumpLog(r);
                            }
                            MessageBox.Show("Yeni seri numarası başarılı bir şekilde eklendi.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            int tekrarsec = diger_ürün_adi_listbox.SelectedIndex;
                            diger_ürün_adi_listbox.SelectedIndex = tekrarsec;
                            diger_ürün_serino_textbox.Focus();
                        }
                        else
                        {
                            if (diger_ürün_serino_textbox.Text.Length > 0 && diger_ürün_kodu_textbox.Text.Length > 0)
                            {
                                string yeniürünkodukayit = "insert into diger_ürün_kodları(diger_urun_adi,diger_urun_stok_kodu) values " + "" + "(@diger_urun_adi,@diger_urun_stok_kodu)";
                                SqlCommand kayitkomut = new SqlCommand(yeniürünkodukayit, connection);
                                kayitkomut.Parameters.AddWithValue("@diger_urun_adi", diger_ürün_adi_textbox.Text);
                                kayitkomut.Parameters.AddWithValue("@diger_urun_stok_kodu", diger_ürün_kodu_textbox.Text);
                                kayitkomut.ExecuteNonQuery();

                                string yeniürünserinokayit = "insert into diger_ürün_stok(diger_urun_adi,diger_urun_serino,diger_urun_adeti,diger_urun_durumu) values " + "" + "(@diger_urun_adi,@diger_urun_serino,@diger_urun_adeti,@diger_urun_durumu)";
                                SqlCommand yeniürünserinokayitkomut = new SqlCommand(yeniürünserinokayit, connection);
                                yeniürünserinokayitkomut.Parameters.AddWithValue("@diger_urun_adi", diger_ürün_adi_textbox.Text);
                                yeniürünserinokayitkomut.Parameters.AddWithValue("@diger_urun_serino", diger_ürün_serino_textbox.Text);
                                yeniürünserinokayitkomut.Parameters.AddWithValue("@diger_urun_adeti", "1");
                                yeniürünserinokayitkomut.Parameters.AddWithValue("@diger_urun_durumu", "Kullanılmadı");
                                yeniürünserinokayitkomut.ExecuteNonQuery();

                                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                                {
                                    Kullanıcı_girisi.Log(Kullanıcı_girisi.username + " adlı kullanıcı " + diger_ürün_adi_textbox.Text + " ürününün " + diger_ürün_serino_textbox.Text + " ürün kodunu ve " + diger_ürün_serino_textbox.Text + " yeni seri numarasını ekledi.", w);
                                }
                                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                                {
                                    Kullanıcı_girisi.DumpLog(r);
                                }
                                MessageBox.Show("Yeni ürün stok kodu ve ürün seri numarası başarılı bir şekilde eklendi.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                int tekrarsec = diger_ürün_adi_listbox.SelectedIndex;
                                diger_ürün_adi_listbox.SelectedIndex = tekrarsec;
                                diger_ürün_adi_textbox.Focus();
                            }
                        }
                    }
                    else
                    {
                        if (diger_ürün_adi_listbox.Items.Contains(diger_ürün_adi_textbox.Text))
                        {
                            provider.SetError(diger_ürün_adi_textbox, "Bu ürün adı mevcuttur. Başka ürün adı kullanınız.");
                        }
                        else if (diger_ürün_kodu_listbox.Items.Contains(diger_ürün_kodu_textbox.Text))
                        {
                            provider.SetError(diger_ürün_kodu_textbox, "Bu ürün kodu mevcuttur. Başka ürün kodu kullanınız.");
                        }
                        else if (diger_ürün_serino_listbox.Items.Contains(diger_ürün_serino_textbox.Text))
                        {
                            provider.SetError(diger_ürün_serino_textbox, "Bu seri numarası mevcuttur. Başka seri numarası kullanınız.");
                        }
                        else if (diger_ürün_adi_textbox.Text == "")
                        {
                            provider.SetError(diger_ürün_adi_textbox, "Ürün adını boş bırakmayınız.");
                        }
                        else
                        {
                            string yeniürünnotebookkayit = "insert into diger_ürünler(diger_urun_adi) values " + "" + "(@diger_urun_adi)";
                            SqlCommand yeniürünnotebookkayitkomut = new SqlCommand(yeniürünnotebookkayit, connection);
                            yeniürünnotebookkayitkomut.Parameters.AddWithValue("@diger_urun_adi", diger_ürün_adi_textbox.Text);
                            yeniürünnotebookkayitkomut.ExecuteNonQuery();
                            diger_ürün_adi_textbox.Focus();

                            if (diger_ürün_kodu_textbox.Text.Length > 0)
                            {
                                string yeniürünkayit = "insert into diger_ürün_kodları(diger_urun_adi,diger_urun_stok_kodu) values " + "" + "(@diger_urun_adi,@diger_urun_stok_kodu)";
                                SqlCommand yeniürünkayitkomut = new SqlCommand(yeniürünkayit, connection);
                                yeniürünkayitkomut.Parameters.AddWithValue("@diger_urun_adi", diger_ürün_adi_textbox.Text);
                                yeniürünkayitkomut.Parameters.AddWithValue("@diger_urun_stok_kodu", diger_ürün_kodu_textbox.Text);
                                yeniürünkayitkomut.ExecuteNonQuery();
                            }
                            if (diger_ürün_serino_textbox.Text.Length > 0)
                            {
                                string yeniürünserinokayit = "insert into diger_ürün_stok(diger_urun_adi,diger_urun_serino,diger_urun_adeti,diger_urun_durumu) values " + "" + "(@diger_urun_adi,@diger_urun_serino,@diger_urun_adeti,@diger_urun_durumu)";
                                SqlCommand yeniürünserinokayitkomut = new SqlCommand(yeniürünserinokayit, connection);
                                yeniürünserinokayitkomut.Parameters.AddWithValue("@diger_urun_adi", diger_ürün_adi_textbox.Text);
                                yeniürünserinokayitkomut.Parameters.AddWithValue("@diger_urun_serino", diger_ürün_serino_textbox.Text);
                                yeniürünserinokayitkomut.Parameters.AddWithValue("@diger_urun_adeti", "1");
                                yeniürünserinokayitkomut.Parameters.AddWithValue("@diger_urun_durumu", "Kullanılmadı");
                                yeniürünserinokayitkomut.ExecuteNonQuery();
                            }
                            using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                            {
                                Kullanıcı_girisi.Log(Kullanıcı_girisi.username + " adlı kullanıcı " + diger_ürün_adi_textbox.Text + " yeni ürünü ekledi.", w);
                            }
                            using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                            {
                                Kullanıcı_girisi.DumpLog(r);
                            }

                            MessageBox.Show("Yeni ürün oluşturuldu.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            diger_ürün_adi_textbox.Focus();
                        }
                    }
                }
                connection.Close();
                Listeyenile();
            }
            catch (Exception hata)
            {
                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.Log("Yeni ürün eklenmedi, bağlantı kesildi.\nHata kodu: " + hata.Message, w);

                }
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.DumpLog(r);
                }
                MessageBox.Show("Yeni ürün eklenmedi, bağlantı kesildi.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
        private void Diger_ürün_serino_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (diger_ürün_serino_listbox.SelectedIndex == -1)
            {
                MessageBox.Show("Geçerli seri numarası seçiniz.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                diger_ürün_serino_textbox.Text = diger_ürün_serino_listbox.SelectedItem.ToString();
            }
        }

        private void Diğer_malzeme_Grubları_Ürün_ekle_ve_Düzenleme_FormClosed(object sender, FormClosedEventArgs e)
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

        private void Ürünkalankullanımlabel_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                if (diger_ürün_adi_listbox.SelectedIndex == -1)
                {
                    MessageBox.Show("Geçerli bir ürün seçiniz.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    SqlCommand komut3 = new SqlCommand("SELECT diger_urun_serino FROM diger_ürün_stok where diger_urun_durumu = 'Kullanılmadı' and diger_urun_adi = '" + diger_ürün_adi_listbox.SelectedItem.ToString() + "'", connection);
                    SqlDataReader veriokuyucu3;
                    veriokuyucu3 = komut3.ExecuteReader();
                    diger_ürün_serino_listbox.Items.Clear();
                    while (veriokuyucu3.Read())
                    {
                        diger_ürün_serino_listbox.Items.Add(veriokuyucu3["diger_urun_serino"]);
                    }
                    if (diger_ürün_serino_listbox.Items.Count < 1)
                    {
                        diger_ürün_serino_listbox.Items.Add("Kullanılacak ürün yok.");
                    }
                    veriokuyucu3.Close();
                    connection.Close();
                }
            }
            catch (Exception hata)
            {
                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.Log("Ürün stok takibi yapılmadı, bağlantı kesildi.\nHata kodu: " + hata.Message, w);

                }
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.DumpLog(r);
                }
                MessageBox.Show("Ürün stok takibi yapılmadı, bağlantı kesildi.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://mail.google.com/");
        }

        #region forumharaketettirme
        new
        int Move;
        int Mouse_X;
        int Mouse_Y;
        private void Diğer_malzeme_Grubları_Ürün_ekle_ve_Düzenleme_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
            this.Cursor = Cursors.Default;
        }

        private void Diğer_malzeme_Grubları_Ürün_ekle_ve_Düzenleme_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }

        private void Diğer_malzeme_Grubları_Ürün_ekle_ve_Düzenleme_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
            this.Cursor = Cursors.SizeAll;
        }
        private void Panel4_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }

        }

        private void Panel4_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
            this.Cursor = Cursors.Default;
        }

        private void Panel4_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
            this.Cursor = Cursors.SizeAll;
        }

        #endregion
        #region renkayarları
        private void linkLabel1_MouseLeave(object sender, EventArgs e)
        {
            linkLabel1.LinkColor = Color.FromArgb(22, 53, 56);
        }

        private void linkLabel1_MouseMove(object sender, MouseEventArgs e)
        {
            linkLabel1.LinkColor = Color.FromArgb(13, 31, 33);
        }
        private void Secili_ürünün_kod_seri_no_sil_btn_MouseMove(object sender, MouseEventArgs e)
        {
            secili_ürünün_kod_seri_no_sil_btn.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void Yeni_ürün_ekle_btn_MouseMove(object sender, MouseEventArgs e)
        {
           yeni_ürün_ekle_btn.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void Secili_ürünü_sil_btn_MouseMove(object sender, MouseEventArgs e)
        {
            secili_ürünü_sil_btn.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void Ana_menü_btn_MouseMove(object sender, MouseEventArgs e)
        {
            ana_menü_btn.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void Yeni_ürün_ekle_btn_MouseLeave(object sender, EventArgs e)
        {
            yeni_ürün_ekle_btn.BackColor = Color.FromArgb(22, 53, 56);
        }

        private void Secili_ürünün_kod_seri_no_sil_btn_MouseLeave(object sender, EventArgs e)
        {
             secili_ürünün_kod_seri_no_sil_btn.BackColor = Color.FromArgb(22, 53, 56);
        }

        private void Secili_ürünü_sil_btn_MouseLeave(object sender, EventArgs e)
        {
             secili_ürünü_sil_btn.BackColor = Color.FromArgb(22, 53, 56);
        }

        private void Ana_menü_btn_MouseLeave(object sender, EventArgs e)
        {
             ana_menü_btn.BackColor = Color.FromArgb(22, 53, 56);
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
            logout_label.ForeColor =Color.FromArgb(22, 53, 56);
        }

        private void Windows_kücültme_label_MouseLeave(object sender, EventArgs e)
        {
            windows_kücültme_label.ForeColor =Color.FromArgb(22, 53, 56);
        }
        #endregion
    }
}