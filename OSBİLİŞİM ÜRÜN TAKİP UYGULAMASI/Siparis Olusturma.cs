using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.ComponentModel;

namespace OSBilişim
{
    public partial class Siparis_olusturma : Form
    {
        public Siparis_olusturma()
        {
            InitializeComponent();

        }
        ErrorProvider provider = new ErrorProvider();
        readonly SqlConnection connection = new SqlConnection("Data Source=192.168.1.123,1433;Network Library=DBMSSOCN; Initial Catalog=OSBİLİSİM;User Id=Admin; Password=1; MultipleActiveResultSets=True;");
        private void Siparisolusturmaform_Load(object sender, EventArgs e)
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
                        MessageBox.Show((string)üründurumusorgulama["program_bakim"], "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                SqlCommand komut = new SqlCommand("SELECT * FROM notebook_urunler ", connection);
                SqlDataReader veriokuyucu;
                veriokuyucu = komut.ExecuteReader();
                while (veriokuyucu.Read())
                {
                    ürünadi.Items.Add(veriokuyucu["urun_adi"]);
                }

                veriokuyucu.Close();
                SqlCommand komut2 = new SqlCommand("SELECT * FROM notebook_kullanilacak_malzemeler ", connection);
                SqlDataReader veriokuyucu2; ;
                veriokuyucu2 = komut2.ExecuteReader();
                while (veriokuyucu2.Read())
                {
                    kullanilacak_malzemeler_listbox.Items.Add(veriokuyucu2["malzeme_adi"]);
                }
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

        private void Silbtn_Click(object sender, EventArgs e)
        {
            provider.Clear();

            if (kullanilacak_malzemeler_listesi.SelectedIndex > -1)
                kullanilacak_malzemeler_listesi.Items.RemoveAt(kullanilacak_malzemeler_listesi.SelectedIndex);
            else if (kullanilacak_malzemeler_listesi.SelectedIndex == -1)
                provider.SetError(kullanilacak_malzemeler_listesi, "Kullanılmayacak malzemeyi seçiniz.");
        }
        private void Liste_temizle_Click(object sender, EventArgs e)
        {
            provider.Clear();
            if (kullanilacak_malzemeler_listesi.Items.Count > 0)
            {
                DialogResult dialog = MessageBox.Show("Kullanılacak malzemeler listesini temizlemek istediğinize emin misiniiz?", "OS BİLİŞİM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    MessageBox.Show("Malzeme listesi temizlendi.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    kullanilacak_malzemeler_listesi.Items.Clear();
                }
                else
                {
                    MessageBox.Show("Malzeme listesi temizlenmedi.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                provider.SetError(kullanilacak_malzemeler_listesi, "Kullanılacak malzeme listesi boş.");
            }
        }
        private void Ürunadi_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ürünadi.SelectedIndex == -1)
                {
                    ürünstokkodu.Items.Clear();
                    ürünserino_checklistbox.Items.Clear();
                }
                else
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                        ürünstokkodu.Items.Clear();
                        ürünserino_checklistbox.Items.Clear();
                        satış_yapılan_firma.Items.Clear();

                        SqlCommand komut3 = new SqlCommand("SELECT urun_kodu FROM notebook_urun_kodları where urun_adi ='" + ürünadi.SelectedItem + "'", connection);
                        SqlDataReader veriokuyucu3;
                        veriokuyucu3 = komut3.ExecuteReader();
                        while (veriokuyucu3.Read())
                        {
                            ürünstokkodu.Items.Add(veriokuyucu3["urun_kodu"]);

                        }
                        veriokuyucu3.Close();

                        SqlCommand satisyapilanfirmalar = new SqlCommand("SELECT satis_yapilan_firmalar FROM firmalarimiz", connection);
                        SqlDataReader satisyapilanfirmalarokuyucu;
                        satisyapilanfirmalarokuyucu = satisyapilanfirmalar.ExecuteReader();
                        while (satisyapilanfirmalarokuyucu.Read())
                        {
                           satış_yapılan_firma.Items.Add(satisyapilanfirmalarokuyucu["satis_yapilan_firmalar"]);

                        }
                        satisyapilanfirmalarokuyucu.Close();
                        connection.Close();
                    }
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

        private void Siparis_gonder_Click(object sender, EventArgs e)
        {
            provider.Clear();
            if (ürünadi.SelectedIndex == -1)
            {
                provider.SetError(ürünadi, "Geçerli bir ürün seçiniz.");
            }
            else if (ürünstokkodu.SelectedIndex == -1)
            {
                provider.SetError(ürünstokkodu, "Geçerli bir ürün stok kodu seçiniz.");
            }
            else if (ürünserino_checklistbox.CheckedItems.Count <= 0)
            {
                provider.SetError(ürünserino_checklistbox, "Mevcut seri numaralarından birini seçiniz.");
            }
            else if ((string)ürünserino_checklistbox.SelectedItem == "Ürün kalmamıştır")
            {
                provider.SetError(ürünserino_checklistbox, "Stokta ürün kalmamıştır, tedarik ediniz.");
            }
            else if (ürünadetitextbox.Text == "")
            {
                provider.SetError(ürünadetitextbox, "Ürün adetini boş bırakmayınız.");
            }
            else if (Convert.ToInt32(ürünadetitextbox.Text) < 1)
            {
                provider.SetError(ürünadetitextbox, "Ürün adeti 1'den küçük olamaz.");
            }
            else if (aliciaditextbox.Text == "")
            {
                provider.SetError(aliciaditextbox, "Ürünün alıcı adını giriniz.");
            }
            else if (alicisoyaditextboxt.Text == "")
            {
                provider.SetError(alicisoyaditextboxt, "Ürünün alıcı soyadını giriniz.");
            }
            else if (satış_yapılan_firma.SelectedIndex == -1)
            {
                provider.SetError(satış_yapılan_firma, "Ürün hangi firma tarafından satılıyor seçiniz.");
            }
            else if (ürünün_satıldığı_firma.SelectedIndex == -1)
            {
                provider.SetError(ürünün_satıldığı_firma, "Ürün hangi firma adı altında satılıyor seçiniz.");
            }
            else if (sipariş_numarası_textbox.Text == "")
            {
                provider.SetError(sipariş_numarası_textbox, "Sipariş numarasını boş bırakmayınız.");
            }
            else if (kullanilacak_malzemeler_listesi.Items.Count < 1)
            {
                provider.SetError(kullanilacak_malzemeler_listesi, "Ürün için takılacak malzeme girilmemiştir malzeme giriniz eğer ürün için malzeme takılmayacaksa (Ürün orjinal hali ile gönderilecektir.) seçeneğini ekleyiniz.");
            }
            else
            {
                DialogResult dialog = new DialogResult();
                dialog = MessageBox.Show("Ürün siparişi oluşturulacaktır siparişin doğruluğunu onaylıyor musunuz?", "OS BİLİŞİM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    try
                    {
                        if (ürünün_satıldığı_firma.SelectedItem.ToString() == "Diğer")
                        {
                            provider.Clear();
                            using (var cn = new SqlConnection("server=192.168.1.123,1433;database=OSBİLİSİM;UId=Admin;Pwd=1;MultipleActiveResultSets=True;"))
                            using (var cmd = new SqlCommand(@"select max(try_cast(sip_no as int)) from siparisler where urunun_satildigi_firma = 'Diğer'", cn))
                            {
                                cn.Open();
                                var id = Convert.ToInt32(cmd.ExecuteScalar());
                                int sipno = id + 1;
                                if (id >= Convert.ToInt64(sipariş_numarası_textbox.Text))
                                {
                                    sipariş_numarası_textbox.Text = $"{id + 1}";
                                    provider.SetError(sipariş_numarası_textbox, $"Girdiğiniz sipariş numarası küçüktür, girebileceğiniz en düşük sipariş numarası: {id + 1}\nSizin için sipariş numarası otamatik olarak girilmiştir.");
                                }
                                else if (Convert.ToInt64(sipariş_numarası_textbox.Text) > sipno)
                                {
                                    sipariş_numarası_textbox.Text = $"{id + 1}";
                                    provider.SetError(sipariş_numarası_textbox, $"Girdiğiniz sipariş numarası büyüktür, girebileceğiniz en yüksek sipariş numarası: {id + 1}\nSizin için sipariş numarası otamatik olarak girilmiştir.");
                                }
                                else
                                {
                                    PrintDialog printDialog1 = new PrintDialog
                                    {
                                        Document = printDocument1
                                    };
                                    DialogResult result = printDialog1.ShowDialog();
                                    if (result == DialogResult.OK)
                                    {
                                        printDocument1.Print();
                                    }
                                    if (connection.State == ConnectionState.Closed)
                                        connection.Open();
                                    string sipariskayit = "insert into siparisler(sip_no,urun_adi,urun_stok_kodu,urun_seri_no,urun_adeti,teslim_alacak_kisi_adi,teslim_alacak_kisi_soyadi,satis_yapilan_firma,urunun_satildigi_firma,kullanilacak_malzemeler,urun_hazirlik_durumu,urun_hakkinda_aciklama,siparis_tarihi) values " + "" + "(@sip_no,@urun_adi,@urun_stok_kodu,@urun_seri_no,@urun_adeti,@teslim_alacak_kisi_adi,@teslim_alacak_kisi_soyadi,@satis_yapilan_firma,@urunun_satildigi_firma,@kullanilacak_malzemeler,@urun_hazirlik_durumu,@urun_hakkinda_aciklama,@siparis_tarihi)";
                                    SqlCommand kayitkomut = new SqlCommand(sipariskayit, connection);
                                    kayitkomut.Parameters.AddWithValue("@sip_no", sipariş_numarası_textbox.Text);
                                    kayitkomut.Parameters.AddWithValue("@urun_adi", ürünadi.SelectedItem);
                                    kayitkomut.Parameters.AddWithValue("@urun_stok_kodu", ürünstokkodu.SelectedItem);
                                    kayitkomut.Parameters.AddWithValue("@urun_adeti", ürünadetitextbox.Text);
                                    kayitkomut.Parameters.AddWithValue("@teslim_alacak_kisi_adi", aliciaditextbox.Text);
                                    kayitkomut.Parameters.AddWithValue("@teslim_alacak_kisi_soyadi", alicisoyaditextboxt.Text);
                                    kayitkomut.Parameters.AddWithValue("@satis_yapilan_firma", satış_yapılan_firma.SelectedItem);
                                    kayitkomut.Parameters.AddWithValue("@urunun_satildigi_firma", ürünün_satıldığı_firma.SelectedItem);
                                    kayitkomut.Parameters.AddWithValue("@kullanilacak_malzemeler", kullanilacak_malzemeler_listesi.Items.Cast<string>().Aggregate((current, next) => $"{current} {"/"} {next}"));
                                    kayitkomut.Parameters.AddWithValue("@siparis_tarihi", DateTime.Now);
                                    string serino = "";
                                    foreach (object item in ürünserino_checklistbox.CheckedItems)
                                    {string checkedItem = item.ToString();
                                        serino = serino + checkedItem + " / ";}
                                    kayitkomut.Parameters.AddWithValue("@urun_seri_no", serino.Trim(new Char[] { ' ', '/', '.' }));
                                    if (satış_yapılan_firma.SelectedItem.ToString() == "OS Depo Merkez")
                                    {
                                        for (int i = 0; i < ürünserino_checklistbox.CheckedItems.Count; i++)
                                        {
                                            SqlCommand ürüngüncelle = new SqlCommand("update notebook_urun_seri_no_stok set urun_durumu = '" + "Ürün Kullanıldı" + "' where urun_seri_no = '" + ürünserino_checklistbox.CheckedItems[i] + "'and urun_depo_cikisi = 'OS Depo Merkez'", connection);
                                            ürüngüncelle.ExecuteNonQuery();
                                        }
                                        if (aciklama_textbox.Text == ""){ kayitkomut.Parameters.AddWithValue("@urun_hakkinda_aciklama", "Ürün hakkında açıklama girilmemiştir."); }
                                        else { kayitkomut.Parameters.AddWithValue("@urun_hakkinda_aciklama", aciklama_textbox.Text); }
                                        kayitkomut.Parameters.AddWithValue("@urun_hazirlik_durumu", ürün_hazirlik_durumu_textbox.Text);
                                        MessageBox.Show("Sipariş oluşturuldu, hazırlanmasını bekleyiniz.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else if (satış_yapılan_firma.SelectedItem.ToString() == "Trenta Depo")
                                    {
                                        for (int i = 0; i < ürünserino_checklistbox.CheckedItems.Count; i++)
                                        {
                                            SqlCommand ürüngüncelle = new SqlCommand("update notebook_urun_seri_no_stok set urun_durumu = '" + "Ürün Kullanıldı" + "' where urun_seri_no = '" + ürünserino_checklistbox.CheckedItems[i] + "'and urun_depo_cikisi = 'OS Depo Merkez'", connection);
                                            ürüngüncelle.ExecuteNonQuery();
                                        }
                                        if (aciklama_textbox.Text == "") { kayitkomut.Parameters.AddWithValue("@urun_hakkinda_aciklama", "Ürün hakkında açıklama girilmemiştir."); }
                                        else { kayitkomut.Parameters.AddWithValue("@urun_hakkinda_aciklama", aciklama_textbox.Text); }
                                        kayitkomut.Parameters.AddWithValue("@urun_hazirlik_durumu", ürün_hazirlik_durumu_textbox.Text);
                                        MessageBox.Show("Sipariş oluşturuldu, hazırlanmasını bekleyiniz.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else if (satış_yapılan_firma.SelectedItem.ToString() == "Arena Depo")
                                    {
                                        for (int i = 0; i < ürünserino_checklistbox.CheckedItems.Count; i++)
                                        {
                                            SqlCommand ürüngüncelle = new SqlCommand("update notebook_urun_seri_no_stok set urun_durumu = '" + "Ürün Kullanıldı" + "' where urun_seri_no = '" + ürünserino_checklistbox.CheckedItems[i] + "'and urun_depo_cikisi = 'Arena Depo'", connection);
                                            ürüngüncelle.ExecuteNonQuery();
                                        }
                                        kayitkomut.Parameters.AddWithValue("@urun_hakkinda_aciklama", "Ürün arena depo üzerinden çıkış yapılmıştır.");
                                        kayitkomut.Parameters.AddWithValue("@urun_hazirlik_durumu","Sipariş onaylandı");
                                        MessageBox.Show("Sipariş oluşturuldu, gönderim için arena depo iletişime geçmeyi unutmayınız.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else if (satış_yapılan_firma.SelectedItem.ToString() == "İndex Depo")
                                    {
                                        for (int i = 0; i < ürünserino_checklistbox.CheckedItems.Count; i++)
                                        {
                                            SqlCommand ürüngüncelle = new SqlCommand("update notebook_urun_seri_no_stok set urun_durumu = '" + "Ürün Kullanıldı" + "' where urun_seri_no = '" + ürünserino_checklistbox.CheckedItems[i] + "'and urun_depo_cikisi = 'İndex Depo'", connection);
                                            ürüngüncelle.ExecuteNonQuery();
                                        } 
                                        kayitkomut.Parameters.AddWithValue("@urun_hakkinda_aciklama", "Ürün index depo üzerinden çıkış yapılmıştır.");
                                        kayitkomut.Parameters.AddWithValue("@urun_hazirlik_durumu", "Sipariş onaylandı");
                                        MessageBox.Show("Sipariş oluşturuldu, gönderim için index depo iletişime geçmeyi unutmayınız.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else if (satış_yapılan_firma.SelectedItem.ToString() == "Penta Depo")
                                    {
                                        for (int i = 0; i < ürünserino_checklistbox.CheckedItems.Count; i++)
                                        {
                                            SqlCommand ürüngüncelle = new SqlCommand("update notebook_urun_seri_no_stok set urun_durumu = '" + "Ürün Kullanıldı" + "' where urun_seri_no = '" + ürünserino_checklistbox.CheckedItems[i] + "'and urun_depo_cikisi = 'Penta Depo'", connection);
                                            ürüngüncelle.ExecuteNonQuery();
                                        }
                                        kayitkomut.Parameters.AddWithValue("@urun_hakkinda_aciklama", "Ürün penta depo üzerinden çıkış yapılmıştır.");
                                        kayitkomut.Parameters.AddWithValue("@urun_hazirlik_durumu", "Sipariş onaylandı");
                                        MessageBox.Show("Sipariş oluşturuldu, gönderim için penta depo iletişime geçmeyi unutmayınız.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        for (int i = 0; i < ürünserino_checklistbox.CheckedItems.Count; i++)
                                        {
                                            SqlCommand ürüngüncelle = new SqlCommand("update notebook_urun_seri_no_stok set urun_durumu = '" + "Ürün Kullanıldı" + "' where urun_seri_no = '" + ürünserino_checklistbox.CheckedItems[i] + "'and urun_depo_cikisi = 'OS Depo Merkez'", connection);
                                            ürüngüncelle.ExecuteNonQuery();
                                        }
                                        if (aciklama_textbox.Text == "") { kayitkomut.Parameters.AddWithValue("@urun_hakkinda_aciklama", "Ürün hakkında açıklama girilmemiştir."); }
                                        else { kayitkomut.Parameters.AddWithValue("@urun_hakkinda_aciklama",aciklama_textbox.Text); }
                                        kayitkomut.Parameters.AddWithValue("@urun_hazirlik_durumu", ürün_hazirlik_durumu_textbox.Text);
                                    }
                                    kayitkomut.ExecuteNonQuery();
                                    connection.Close();
                                    using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                                    { Kullanıcı_girisi.Log(aliciaditextbox.Text + " " + alicisoyaditextboxt.Text + " adlı alıcı " + ürünadi.SelectedItem.ToString() + " ürününün " + serino.Trim(new Char[] { ' ', '/', '.' }) + " seri numarasını seçerek, " + Kullanıcı_girisi.username + " tarafından alıcı için sipariş oluşturuldu.", w);}
                                    using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                                    {Kullanıcı_girisi.DumpLog(r);}

                                    ürünadetitextbox.Text = "0";
                                    ürünadi.SelectedIndex = -1;
                                    ürünstokkodu.SelectedIndex = -1;
                                    kullanilacak_malzemeler_listesi.Items.Clear();
                                    aliciaditextbox.Text = "";
                                    alicisoyaditextboxt.Text = "";
                                    satış_yapılan_firma.SelectedIndex = -1;
                                    ürünün_satıldığı_firma.SelectedIndex = -1;
                                    kullanilacak_malzemeler_listbox.SelectedIndex = -1;
                                    kullanilacak_malzeme_adeti_textbox.Text = "0";
                                    aciklama_textbox.Text = "";
                                    ürünserino_checklistbox.Items.Clear();
                                    sipariş_numarası_textbox.Text = "";
                                }
                            }
                        }
                        else
                        {
                            if (connection.State == ConnectionState.Closed)
                                connection.Open();
                            string sipariskayit = "insert into siparisler(sip_no,urun_adi,urun_stok_kodu,urun_seri_no,urun_adeti,teslim_alacak_kisi_adi,teslim_alacak_kisi_soyadi,satis_yapilan_firma,urunun_satildigi_firma,kullanilacak_malzemeler,urun_hazirlik_durumu,urun_hakkinda_aciklama,siparis_tarihi) values " + "" + "(@sip_no,@urun_adi,@urun_stok_kodu,@urun_seri_no,@urun_adeti,@teslim_alacak_kisi_adi,@teslim_alacak_kisi_soyadi,@satis_yapilan_firma,@urunun_satildigi_firma,@kullanilacak_malzemeler,@urun_hazirlik_durumu,@urun_hakkinda_aciklama,@siparis_tarihi)";
                            SqlCommand kayitkomut = new SqlCommand(sipariskayit, connection);
                            kayitkomut.Parameters.AddWithValue("@sip_no", sipariş_numarası_textbox.Text);
                            kayitkomut.Parameters.AddWithValue("@urun_adi", ürünadi.SelectedItem);
                            kayitkomut.Parameters.AddWithValue("@urun_stok_kodu", ürünstokkodu.SelectedItem);
                            kayitkomut.Parameters.AddWithValue("@urun_adeti", ürünadetitextbox.Text);
                            kayitkomut.Parameters.AddWithValue("@teslim_alacak_kisi_adi", aliciaditextbox.Text);
                            kayitkomut.Parameters.AddWithValue("@teslim_alacak_kisi_soyadi", alicisoyaditextboxt.Text);
                            kayitkomut.Parameters.AddWithValue("@satis_yapilan_firma", satış_yapılan_firma.SelectedItem);
                            kayitkomut.Parameters.AddWithValue("@urunun_satildigi_firma", ürünün_satıldığı_firma.SelectedItem);
                            kayitkomut.Parameters.AddWithValue("@kullanilacak_malzemeler", kullanilacak_malzemeler_listesi.Items.Cast<string>().Aggregate((current, next) => $"{current} {"/"} {next}"));
                            kayitkomut.Parameters.AddWithValue("@siparis_tarihi", DateTime.Now);
                            string serino = "";
                            foreach (object item in ürünserino_checklistbox.CheckedItems)
                            {
                                string checkedItem = item.ToString();
                                serino = serino + checkedItem + " / ";
                            }
                            kayitkomut.Parameters.AddWithValue("@urun_seri_no", serino.Trim(new Char[] { ' ', '/', '.' }));
                            if (satış_yapılan_firma.SelectedItem.ToString() == "OS Depo Merkez")
                            {
                                for (int i = 0; i < ürünserino_checklistbox.CheckedItems.Count; i++)
                                {
                                    SqlCommand ürüngüncelle = new SqlCommand("update notebook_urun_seri_no_stok set urun_durumu = '" + "Ürün Kullanıldı" + "' where urun_seri_no = '" + ürünserino_checklistbox.CheckedItems[i] + "'and urun_depo_cikisi = 'OS Depo Merkez'", connection);
                                    ürüngüncelle.ExecuteNonQuery();
                                }
                                if (aciklama_textbox.Text == "") { kayitkomut.Parameters.AddWithValue("@urun_hakkinda_aciklama", "Ürün hakkında açıklama girilmemiştir."); }
                                else { kayitkomut.Parameters.AddWithValue("@urun_hakkinda_aciklama", aciklama_textbox.Text); }
                                kayitkomut.Parameters.AddWithValue("@urun_hazirlik_durumu", ürün_hazirlik_durumu_textbox.Text);
                                MessageBox.Show("Sipariş oluşturuldu, hazırlanmasını bekleyiniz.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else if (satış_yapılan_firma.SelectedItem.ToString() == "Trenta Depo")
                            {
                                for (int i = 0; i < ürünserino_checklistbox.CheckedItems.Count; i++)
                                {
                                    SqlCommand ürüngüncelle = new SqlCommand("update notebook_urun_seri_no_stok set urun_durumu = '" + "Ürün Kullanıldı" + "' where urun_seri_no = '" + ürünserino_checklistbox.CheckedItems[i] + "'and urun_depo_cikisi = 'OS Depo Merkez'", connection);
                                    ürüngüncelle.ExecuteNonQuery();
                                }
                                if (aciklama_textbox.Text == "") { kayitkomut.Parameters.AddWithValue("@urun_hakkinda_aciklama", "Ürün hakkında açıklama girilmemiştir."); }
                                else { kayitkomut.Parameters.AddWithValue("@urun_hakkinda_aciklama", aciklama_textbox.Text); }
                                kayitkomut.Parameters.AddWithValue("@urun_hazirlik_durumu", ürün_hazirlik_durumu_textbox.Text);
                                MessageBox.Show("Sipariş oluşturuldu, hazırlanmasını bekleyiniz.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else if (satış_yapılan_firma.SelectedItem.ToString() == "Arena Depo")
                            {
                                for (int i = 0; i < ürünserino_checklistbox.CheckedItems.Count; i++)
                                {
                                    SqlCommand ürüngüncelle = new SqlCommand("update notebook_urun_seri_no_stok set urun_durumu = '" + "Ürün Kullanıldı" + "' where urun_seri_no = '" + ürünserino_checklistbox.CheckedItems[i] + "'and urun_depo_cikisi = 'Arena Depo'", connection);
                                    ürüngüncelle.ExecuteNonQuery();
                                }
                                kayitkomut.Parameters.AddWithValue("@urun_hakkinda_aciklama", "Ürün arena depo üzerinden çıkış yapılmıştır.");
                                kayitkomut.Parameters.AddWithValue("@urun_hazirlik_durumu", "Sipariş onaylandı");
                                MessageBox.Show("Sipariş oluşturuldu, gönderim için arena depo iletişime geçmeyi unutmayınız.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else if (satış_yapılan_firma.SelectedItem.ToString() == "İndex Depo")
                            {
                                for (int i = 0; i < ürünserino_checklistbox.CheckedItems.Count; i++)
                                {
                                    SqlCommand ürüngüncelle = new SqlCommand("update notebook_urun_seri_no_stok set urun_durumu = '" + "Ürün Kullanıldı" + "' where urun_seri_no = '" + ürünserino_checklistbox.CheckedItems[i] + "'and urun_depo_cikisi = 'İndex Depo'", connection);
                                    ürüngüncelle.ExecuteNonQuery();
                                }
                                kayitkomut.Parameters.AddWithValue("@urun_hakkinda_aciklama", "Ürün index depo üzerinden çıkış yapılmıştır.");
                                kayitkomut.Parameters.AddWithValue("@urun_hazirlik_durumu", "Sipariş onaylandı");
                                MessageBox.Show("Sipariş oluşturuldu, gönderim için index depo iletişime geçmeyi unutmayınız.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else if (satış_yapılan_firma.SelectedItem.ToString() == "Penta Depo")
                            {
                                for (int i = 0; i < ürünserino_checklistbox.CheckedItems.Count; i++)
                                {
                                    SqlCommand ürüngüncelle = new SqlCommand("update notebook_urun_seri_no_stok set urun_durumu = '" + "Ürün Kullanıldı" + "' where urun_seri_no = '" + ürünserino_checklistbox.CheckedItems[i] + "'and urun_depo_cikisi = 'Penta Depo'", connection);
                                    ürüngüncelle.ExecuteNonQuery();
                                }
                                kayitkomut.Parameters.AddWithValue("@urun_hakkinda_aciklama", "Ürün penta depo üzerinden çıkış yapılmıştır.");
                                kayitkomut.Parameters.AddWithValue("@urun_hazirlik_durumu", "Sipariş onaylandı");
                                MessageBox.Show("Sipariş oluşturuldu, gönderim için penta depo iletişime geçmeyi unutmayınız.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                for (int i = 0; i < ürünserino_checklistbox.CheckedItems.Count; i++)
                                {
                                    SqlCommand ürüngüncelle = new SqlCommand("update notebook_urun_seri_no_stok set urun_durumu = '" + "Ürün Kullanıldı" + "' where urun_seri_no = '" + ürünserino_checklistbox.CheckedItems[i] + "'and urun_depo_cikisi = 'OS Depo Merkez'", connection);
                                    ürüngüncelle.ExecuteNonQuery();
                                }
                                if (aciklama_textbox.Text == "") { kayitkomut.Parameters.AddWithValue("@urun_hakkinda_aciklama", "Ürün hakkında açıklama girilmemiştir."); }
                                else { kayitkomut.Parameters.AddWithValue("@urun_hakkinda_aciklama", aciklama_textbox.Text); }
                                kayitkomut.Parameters.AddWithValue("@urun_hazirlik_durumu", ürün_hazirlik_durumu_textbox.Text);
                            }
                            kayitkomut.ExecuteNonQuery();
                            connection.Close();

                            using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                            {Kullanıcı_girisi.Log(aliciaditextbox.Text + " " + alicisoyaditextboxt.Text + " adlı alıcı " + ürünadi.SelectedItem.ToString() + " ürününün " + serino.Trim(new Char[] { ' ', '/', '.' }) + " seri numarasını seçerek, " + Kullanıcı_girisi.username + " tarafından alıcı için sipariş oluşturuldu.", w);}
                            using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                            { Kullanıcı_girisi.DumpLog(r);}
                            ürünadetitextbox.Text = "0";
                            ürünadi.SelectedIndex = -1;
                            ürünstokkodu.SelectedIndex = -1;
                            kullanilacak_malzemeler_listesi.Items.Clear();
                            aliciaditextbox.Text = "";
                            alicisoyaditextboxt.Text = "";
                            satış_yapılan_firma.SelectedIndex = -1;
                            ürünün_satıldığı_firma.SelectedIndex = -1;
                            kullanilacak_malzemeler_listbox.SelectedIndex = -1;
                            kullanilacak_malzeme_adeti_textbox.Text = "0";
                            aciklama_textbox.Text = "";
                            ürünserino_checklistbox.Items.Clear();
                            sipariş_numarası_textbox.Text = "";
                            MessageBox.Show("Sipariş oluşturuldu, hazırlanmasını bekleyiniz.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception hata)
                    {
                        using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                        {
                            Kullanıcı_girisi.Log("Sipariş oluşturulmadı, bağlantı kesildi.\nHata kodu: " + hata.Message, w);

                        }
                        using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                        {
                            Kullanıcı_girisi.DumpLog(r);
                        }
                        MessageBox.Show("Sipariş oluşturulmadı, bağlantı kesildi.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                }
                else
                {
                    MessageBox.Show("Sipariş oluşturulmadı, gerekli bilgileri tekrar tamamlayınız.", "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            connection.Close();
        }
        private void Ürünadetitextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            provider.Clear();
            if (e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                provider.SetError(ürünadetitextbox, "Ürün adeti sadece rakam & sayı ile giriş yapılabilir.");
            }
            else { }
        }
        private void Malzeme_ekle_btn_Click(object sender, EventArgs e)
        {
            string kullanilackamalzemekontrol;
            provider.Clear();   
            if (kullanilacak_malzeme_adeti_textbox.Text == "")
            {
                if (kullanilacak_malzemeler_listbox.SelectedItem.ToString() == "Ürün orjinal hali ile gönderilecektir")
                {
                    if (kullanilacak_malzemeler_listesi.Items.Contains(kullanilacak_malzemeler_listbox.SelectedItem))
                    {
                        provider.SetError(kullanilacak_malzemeler_listbox, "Malzemeler listesinde aynı ürün mevcut ürünü silip güncelledikten sonra tekrar ekleyiniz.");
                    }
                    else
                    {
                        kullanilacak_malzemeler_listesi.Items.Add(kullanilacak_malzemeler_listbox.SelectedItem);
                    }
                }
                else
                {
                    provider.SetError(kullanilacak_malzeme_adeti_textbox, "Kullanılacak malzeme adetini giriniz.");
                }
            }
            else if (kullanilacak_malzemeler_listbox.SelectedIndex == -1)
            {
                provider.SetError(kullanilacak_malzemeler_listbox, "Lütfen geçerli malzeme seçiniz.");
            }
            else if (kullanilacak_malzemeler_listbox.Text == "")
            {
                provider.SetError(kullanilacak_malzemeler_listbox, "Lütfen geçerli malzeme seçiniz.");
            }
            else if (Convert.ToInt32(kullanilacak_malzeme_adeti_textbox.Text) < 1)
            {
                if (kullanilacak_malzemeler_listbox.SelectedItem.ToString() == "Ürün orjinal hali ile gönderilecektir")
                {
                    if (kullanilacak_malzemeler_listesi.Items.Contains(kullanilacak_malzemeler_listbox.SelectedItem))
                    {
                        provider.SetError(kullanilacak_malzemeler_listbox, "Malzemeler listesinde aynı ürün mevcut ürünü silip güncelledikten sonra tekrar ekleyiniz.");
                    }
                    else
                    {
                        kullanilacak_malzemeler_listesi.Items.Add(kullanilacak_malzemeler_listbox.SelectedItem);
                    }
                }
                else
                {
                    provider.SetError(kullanilacak_malzeme_adeti_textbox, "Kullanılacak malzeme adeti 1'den küçük olamaz.");
                }
            }
            else
            {
                string mouse = "Mouse";
                string canta = "Çanta";
                string lisans = "Windows";

                foreach (string item in kullanilacak_malzemeler_listbox.SelectedItems)
                {
                    if (item.ToLower().Contains(mouse.ToLower()))
                    {
                        kullanilackamalzemekontrol = kullanilacak_malzemeler_listbox.SelectedItem + " (" + kullanilacak_malzeme_adeti_textbox.Text + " Adet)" + " Verildi";
                        if (kullanilacak_malzemeler_listesi.Items.Contains(kullanilackamalzemekontrol))
                        {
                            provider.SetError(kullanilacak_malzemeler_listbox, "Malzemeler listesinde aynı ürün mevcut ürünü silip güncelledikten sonra tekrar ekleyiniz.");
                        }
                        else
                        {
                            kullanilacak_malzemeler_listesi.Items.Add(kullanilacak_malzemeler_listbox.SelectedItem + " (" + kullanilacak_malzeme_adeti_textbox.Text + " Adet)" + " Verildi");
                        }

                    }
                    else if (item.ToLower().Contains(canta.ToLower()))
                    {
                        kullanilackamalzemekontrol = kullanilacak_malzemeler_listbox.SelectedItem + " (" + kullanilacak_malzeme_adeti_textbox.Text + " Adet)" + " Verildi";
                        if (kullanilacak_malzemeler_listesi.Items.Contains(kullanilackamalzemekontrol))
                        {
                            provider.SetError(kullanilacak_malzemeler_listbox, "Malzemeler listesinde aynı ürün mevcut ürünü silip güncelledikten sonra tekrar ekleyiniz.");
                        }
                        else
                        {
                            kullanilacak_malzemeler_listesi.Items.Add(kullanilacak_malzemeler_listbox.SelectedItem + " (" + kullanilacak_malzeme_adeti_textbox.Text + " Adet)" + " Verildi");
                        }
                    }
                    else if (item.ToLower().Contains(lisans.ToLower()))
                    {
                        kullanilackamalzemekontrol = kullanilacak_malzemeler_listbox.SelectedItem + " (" + kullanilacak_malzeme_adeti_textbox.Text + " Adet)" + " Kullanıldı";
                        if (kullanilacak_malzemeler_listesi.Items.Contains(kullanilackamalzemekontrol))
                        {
                            provider.SetError(kullanilacak_malzemeler_listbox, "Malzemeler listesinde aynı ürün mevcut ürünü silip güncelledikten sonra tekrar ekleyiniz.");
                        }
                        else
                        {
                            kullanilacak_malzemeler_listesi.Items.Add(kullanilacak_malzemeler_listbox.SelectedItem + " (" + kullanilacak_malzeme_adeti_textbox.Text + " Adet)" + " Kullanıldı");
                        }
                    }
                    else
                    {
                        kullanilackamalzemekontrol = kullanilacak_malzemeler_listbox.SelectedItem + " (" + kullanilacak_malzeme_adeti_textbox.Text + " Adet)" + " Takılacak";
                        if (kullanilacak_malzemeler_listesi.Items.Contains(kullanilackamalzemekontrol))
                        {
                            provider.SetError(kullanilacak_malzemeler_listbox, "Malzemeler listesinde aynı ürün mevcut ürünü silip güncelledikten sonra tekrar ekleyiniz.");
                        }
                        else
                        {
                            kullanilacak_malzemeler_listesi.Items.Add(kullanilacak_malzemeler_listbox.SelectedItem + " (" + kullanilacak_malzeme_adeti_textbox.Text + " Adet)" + " Takılacak");
                        }
                    }
                }

            }
        }
        private void Aliciaditextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            provider.Clear();
            if (e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
            {
                provider.SetError(aliciaditextbox, "Alıcı adı sadece harf ile giriş yapılabilir.");
            }
            else { }
        }

        private void Alicisoyaditextboxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            provider.Clear();
            if (e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
            {
                provider.SetError(alicisoyaditextboxt, "Alıcı soyadı sadece harf ile giriş yapılabilir.");
            }
            else { }
        }
        private void Ana_menü_btn_Click(object sender, EventArgs e)
        {
            Anaform anaform = new Anaform();
            anaform.Show();
            Hide();
        }
        private void Siparisolusturmaform_FormClosed(object sender, FormClosedEventArgs e)
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
        public string urunkontrol;
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

        private void Kullanilacak_malzeme_adeti_textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            provider.Clear();
            if (e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                provider.SetError(ürünadetitextbox, "Ürün adeti sadece rakam & sayı ile giriş yapılabilir.");
            }
            else { }
        }
        private void PrintDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                Font myFont = new Font("Calibri", 20, FontStyle.Bold);
                Font tarihfont = new Font("Calibri", 8, FontStyle.Bold);
                SolidBrush sbrush = new SolidBrush(Color.Black);
                Pen myPen = new Pen(Color.Black);
                StringFormat ortala = new StringFormat
                {
                    Alignment = StringAlignment.Center
                };

                e.Graphics.DrawRectangle(myPen, 10, 10, 785, 160);
                e.Graphics.DrawImage(Properties.Resources.footer, 15, 15, 220, 150);
                e.Graphics.DrawLine(myPen, new Point(250, 170), new Point(250, 10));
                e.Graphics.DrawString("ÜRÜN/HİZMET\nSİPARİŞ FORMU", myFont, sbrush, 320, 55);
                e.Graphics.DrawLine(myPen, new Point(570, 170), new Point(570, 10));
                e.Graphics.DrawString($"Oruç Reis Mah. Tekstil Kent Cad. Tekstilkent Sit.\nA 13 Blok No:63 Esenler / İSTANBUL\n+90 531 263 89 16\nwww.osbilisim.com.tr\ninfo@osbilisim.com.tr\nTarih: {DateTime.Now:dd.MM.yyyy HH:mm:ss} ", tarihfont, sbrush, 683, 55, ortala);

                myFont = new Font("Calibri", 20, FontStyle.Bold);
                e.Graphics.DrawString("MÜŞTERİ BİLGİLERİ", myFont, sbrush, 300, 200);
                myFont = new Font("Calibri", 12, FontStyle.Bold);
                e.Graphics.DrawRectangle(myPen, 15, 240, 785, 35);
                e.Graphics.DrawString("Müşteri Adı:", myFont, sbrush, 100, 250);
                e.Graphics.DrawRectangle(myPen, 15, 275, 785, 35);
                e.Graphics.DrawString("Sipariş No:", myFont, sbrush, 100, 285);
                e.Graphics.DrawRectangle(myPen, 15, 310, 785, 35);
                e.Graphics.DrawString("Sipariş Tarihi:", myFont, sbrush, 100, 320);


                e.Graphics.DrawString(aliciaditextbox.Text.ToUpper() + " " + alicisoyaditextboxt.Text.ToUpper(), myFont, sbrush, 192, 250);
                e.Graphics.DrawString(sipariş_numarası_textbox.Text.ToUpper(), myFont, sbrush, 179, 285);
                e.Graphics.DrawString($"{ DateTime.Now:dd.MM.yyyy HH:mm}", myFont, sbrush, 198, 320);

                myFont = new Font("Calibri", 20, FontStyle.Bold);
                e.Graphics.DrawString("ÜRÜN BİLGİLERİ", myFont, sbrush, 300, 355);
                myFont = new Font("Calibri", 12, FontStyle.Bold);
                e.Graphics.DrawRectangle(myPen, 15, 395, 785, 35);
                e.Graphics.DrawString("Ürün Adı:", myFont, sbrush, 100, 405);
                e.Graphics.DrawRectangle(myPen, 15, 430, 785, 35);
                e.Graphics.DrawString("Ürün Stok Kodu:", myFont, sbrush, 100, 440);
                e.Graphics.DrawRectangle(myPen, 15, 465, 785, 35);
                e.Graphics.DrawString("Ürün Seri No:", myFont, sbrush, 100, 475);
                e.Graphics.DrawRectangle(myPen, 15, 500, 785, 35);
                e.Graphics.DrawString("Ürün Adeti:", myFont, sbrush, 100, 510);
                e.Graphics.DrawString(ürünadi.SelectedItem.ToString().ToUpper(), myFont, sbrush, 172, 405);
                e.Graphics.DrawString(ürünstokkodu.SelectedItem.ToString().ToUpper(), myFont, sbrush, 221, 440);
                string serino = "";
                foreach (object item in ürünserino_checklistbox.CheckedItems)
                {
                    string checkedItem = item.ToString();
                    serino = serino + checkedItem + "-";
                }
                e.Graphics.DrawString(serino.ToUpper().Trim(new Char[] { ' ', '-', '.' }), myFont, sbrush, 198, 475);
                e.Graphics.DrawString(ürünadetitextbox.Text, myFont, sbrush, 186, 510);


                e.Graphics.DrawRectangle(myPen, 15, 587, 785, 200);
                Font teslimedenfont = new Font("Calibri", 20, FontStyle.Bold);
                e.Graphics.DrawString("Teslim Eden", myFont, sbrush, 123, 600);
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                SqlCommand kullanicilar = new SqlCommand("select * from kullanicilar where k_adi = '" + Kullanıcı_girisi.username + "'", connection);
                SqlDataReader kullaniciaciklamasi;
                kullaniciaciklamasi = kullanicilar.ExecuteReader();
                while (kullaniciaciklamasi.Read())
                {
                    e.Graphics.DrawString((string)kullaniciaciklamasi["kullanici_isim"].ToString().ToUpper() + " " + (string)kullaniciaciklamasi["kullanici_soyisim"].ToString().ToUpper(), teslimedenfont, sbrush,80, 630);
                    e.Graphics.DrawString("("+(string)kullaniciaciklamasi["kullanici_statü"]+")".ToString().ToUpper(), teslimedenfont, sbrush, 80, 660);
                }
                connection.Close();
                e.Graphics.DrawString("Teslim Alan", myFont, sbrush, 570, 600);
            }
            catch (Exception hata)
            {
                using (StreamWriter w = File.AppendText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.Log("Yazıcı çıktısı alınırken bir hata oluştu.\nHata kodu: " + hata.Message,w);

                }
                using (StreamReader r = File.OpenText("OSBilisim-log.log"))
                {
                    Kullanıcı_girisi.DumpLog(r);
                }
                MessageBox.Show("Yazıcı çıktısı alınırken bir hata oluştu.\nHata kodu: " + hata.Message, "OS BİLİŞİM", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://mail.google.com/");
        }
        new
        #region forumharaketettirme
        int Move;
        int Mouse_X;
        int Mouse_Y;
        private void Siparisolusturmaform_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }

        private void Siparisolusturmaform_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
            this.Cursor = Cursors.Default;
        }

        private void Siparisolusturmaform_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
            this.Cursor = Cursors.SizeAll;
        }
        #endregion
        #region forumharaketettirme2 
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
        #region renkayarı
        private void linkLabel1_MouseLeave(object sender, EventArgs e)
        {
            linkLabel1.LinkColor = Color.FromArgb(22, 53, 56);
        }

        private void linkLabel1_MouseMove(object sender, MouseEventArgs e)
        {
            linkLabel1.LinkColor = Color.FromArgb(13, 31, 33);
        }
        private void Silbtn_MouseMove(object sender, MouseEventArgs e)
        {
            silbtn.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void Liste_temizle_MouseMove(object sender, MouseEventArgs e)
        {
            liste_temizle.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void Siparis_gonder_MouseMove(object sender, MouseEventArgs e)
        {
            siparis_gonder.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void Malzeme_ekle_btn_MouseMove(object sender, MouseEventArgs e)
        {
            malzeme_ekle_btn.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void Ana_menü_btn_MouseMove(object sender, MouseEventArgs e)
        {
            ana_menü_btn.BackColor = Color.FromArgb(13, 31, 33);
        }

        private void Liste_temizle_MouseLeave(object sender, EventArgs e)
        {
            liste_temizle.BackColor = Color.FromArgb(22, 53, 56);
        }

        private void Silbtn_MouseLeave(object sender, EventArgs e)
        {
            silbtn.BackColor = Color.FromArgb(22, 53, 56);
        }

        private void Siparis_gonder_MouseLeave(object sender, EventArgs e)
        {
            siparis_gonder.BackColor = Color.FromArgb(22, 53, 56);
        }

        private void Malzeme_ekle_btn_MouseLeave(object sender, EventArgs e)
        {
            malzeme_ekle_btn.BackColor = Color.FromArgb(22, 53, 56);
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
            logout_label.ForeColor = Color.FromArgb(22, 53, 56);
        }

        private void Windows_kücültme_label_MouseLeave(object sender, EventArgs e)
        {
            windows_kücültme_label.ForeColor = Color.FromArgb(22, 53, 56);
        }


        #endregion

        private void satış_yapılan_firma_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ürünadi.SelectedIndex == -1)
                {
                    ürünstokkodu.Items.Clear();
                    ürünserino_checklistbox.Items.Clear();
                }
                else
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    if (satış_yapılan_firma.SelectedItem.ToString() == "OS Depo Merkez")
                    {
                        ürünserino_checklistbox.Items.Clear();
                        SqlCommand üründurum = new SqlCommand("SELECT urun_seri_no FROM notebook_urun_seri_no_stok where urun_durumu = 'Kullanılmadı' and urun_depo_cikisi = 'OS Depo Merkez' and urun_adi = '" + ürünadi.SelectedItem.ToString() + "'", connection);
                        SqlDataReader üründurumusorgulama;
                        üründurumusorgulama = üründurum.ExecuteReader();
                        while (üründurumusorgulama.Read())
                        {
                            ürünserino_checklistbox.Items.Add(üründurumusorgulama["urun_seri_no"]);

                        }
                        üründurumusorgulama.Close();
                    }
                    else if (satış_yapılan_firma.SelectedItem.ToString() == "Trenta Depo")
                    {
                        ürünserino_checklistbox.Items.Clear();
                        SqlCommand üründurum = new SqlCommand("SELECT urun_seri_no FROM notebook_urun_seri_no_stok where urun_durumu = 'Kullanılmadı' and urun_depo_cikisi = 'OS Depo Merkez' and urun_adi = '" + ürünadi.SelectedItem.ToString() + "'", connection);
                        SqlDataReader üründurumusorgulama;
                        üründurumusorgulama = üründurum.ExecuteReader();
                        while (üründurumusorgulama.Read())
                        {
                            ürünserino_checklistbox.Items.Add(üründurumusorgulama["urun_seri_no"]);

                        }
                        üründurumusorgulama.Close();
                    }
                    else if (satış_yapılan_firma.SelectedItem.ToString() == "İndex Depo")
                    {
                        ürünserino_checklistbox.Items.Clear();
                        SqlCommand indexurundurum = new SqlCommand("SELECT urun_seri_no FROM notebook_urun_seri_no_stok where urun_durumu = 'Kullanılmadı' and urun_depo_cikisi = 'İndex Depo' and urun_adi = '" + ürünadi.SelectedItem.ToString() + "'", connection);
                        SqlDataReader indexüründurmsorgulama;
                        indexüründurmsorgulama = indexurundurum.ExecuteReader();
                        while (indexüründurmsorgulama.Read())
                        {
                            ürünserino_checklistbox.Items.Add(indexüründurmsorgulama["urun_seri_no"]);

                        }
                        indexüründurmsorgulama.Close();
                    }
                    else if (satış_yapılan_firma.SelectedItem.ToString() == "Penta Depo")
                    {
                        ürünserino_checklistbox.Items.Clear();
                        SqlCommand pentadurum = new SqlCommand("SELECT urun_seri_no FROM notebook_urun_seri_no_stok where urun_durumu = 'Kullanılmadı' and urun_depo_cikisi = 'Penta Depo' and urun_adi = '" + ürünadi.SelectedItem.ToString() + "'", connection);
                        SqlDataReader pentadurumsorgulama;
                        pentadurumsorgulama = pentadurum.ExecuteReader();
                        while (pentadurumsorgulama.Read())
                        {
                            ürünserino_checklistbox.Items.Add(pentadurumsorgulama["urun_seri_no"]);

                        }
                        pentadurumsorgulama.Close();
                    }
                    else if (satış_yapılan_firma.SelectedItem.ToString() == "Arena Depo")
                    {
                        ürünserino_checklistbox.Items.Clear();
                        SqlCommand arenadurum = new SqlCommand("SELECT urun_seri_no FROM notebook_urun_seri_no_stok where urun_durumu = 'Kullanılmadı' and urun_depo_cikisi = 'Arena Depo' and urun_adi = '" + ürünadi.SelectedItem.ToString() + "'", connection);
                        SqlDataReader arenadurumsorgulama;
                        arenadurumsorgulama = arenadurum.ExecuteReader();
                        while (arenadurumsorgulama.Read())
                        {
                            ürünserino_checklistbox.Items.Add(arenadurumsorgulama["urun_seri_no"]);

                        }
                        arenadurumsorgulama.Close();
                    }
                    
                    connection.Close();
                    if (ürünserino_checklistbox.Items.Count < 1)
                    {
                        ürünserino_checklistbox.Items.Clear();
                        ürünserino_checklistbox.Items.Add("Ürün kalmamıştır");
                    }
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

        private void sipariş_numarası_textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            provider.Clear();
            if (e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                provider.SetError(sipariş_numarası_textbox, "Sipariş numarası sadece rakam & sayı ile giriş yapılabilir.");
            }
            else { }
        }
    }
}