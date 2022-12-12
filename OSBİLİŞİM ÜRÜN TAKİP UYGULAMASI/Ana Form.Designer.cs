namespace OSBilişim
{
    partial class Anaform
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Anaform));
            this.siparis_kontrol_btn = new System.Windows.Forms.Button();
            this.ürün_ekle_btn = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.aktifkullanicilar_listbox = new System.Windows.Forms.ListBox();
            this.btn_cikis = new System.Windows.Forms.Button();
            this.tarih_label = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.kayit_tarihi_label = new System.Windows.Forms.Label();
            this.statü_label = new System.Windows.Forms.Label();
            this.soyisim_label = new System.Windows.Forms.Label();
            this.isim_label = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.loginpanel_gelistiren_label = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label = new System.Windows.Forms.Label();
            this.label_6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.logout_label = new System.Windows.Forms.Label();
            this.siparis_olustur_btn = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.diğer_malzeme_grubları_sipariş_oluşturma = new System.Windows.Forms.Button();
            this.diğer_malzeme_grubları_ürün_ekle = new System.Windows.Forms.Button();
            this.yenikullanici_btn = new System.Windows.Forms.Button();
            this.ürün_düzenleme_btn = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // siparis_kontrol_btn
            // 
            this.siparis_kontrol_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.siparis_kontrol_btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.siparis_kontrol_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.siparis_kontrol_btn.Font = new System.Drawing.Font("Century Gothic", 8F);
            this.siparis_kontrol_btn.ForeColor = System.Drawing.Color.White;
            this.siparis_kontrol_btn.Location = new System.Drawing.Point(591, 216);
            this.siparis_kontrol_btn.Name = "siparis_kontrol_btn";
            this.siparis_kontrol_btn.Size = new System.Drawing.Size(111, 45);
            this.siparis_kontrol_btn.TabIndex = 2;
            this.siparis_kontrol_btn.Text = "Sipariş Kontrol";
            this.toolTip1.SetToolTip(this.siparis_kontrol_btn, "Siparişleri kontrol edebileceğiniz sayfaya yönlendirir.");
            this.siparis_kontrol_btn.UseVisualStyleBackColor = false;
            this.siparis_kontrol_btn.Click += new System.EventHandler(this.Siparis_kontrol_btn_Click);
            this.siparis_kontrol_btn.MouseLeave += new System.EventHandler(this.Siparis_kontrol_btn_MouseLeave);
            this.siparis_kontrol_btn.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Siparis_kontrol_btn_MouseMove);
            // 
            // ürün_ekle_btn
            // 
            this.ürün_ekle_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.ürün_ekle_btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ürün_ekle_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ürün_ekle_btn.Font = new System.Drawing.Font("Century Gothic", 8F);
            this.ürün_ekle_btn.ForeColor = System.Drawing.Color.White;
            this.ürün_ekle_btn.Location = new System.Drawing.Point(534, 267);
            this.ürün_ekle_btn.Name = "ürün_ekle_btn";
            this.ürün_ekle_btn.Size = new System.Drawing.Size(111, 45);
            this.ürün_ekle_btn.TabIndex = 3;
            this.ürün_ekle_btn.Text = "Ürün ekle";
            this.toolTip1.SetToolTip(this.ürün_ekle_btn, "Ürünleri düzenleyebileceğiniz sayfaya yönlendirir.\r\n");
            this.ürün_ekle_btn.UseVisualStyleBackColor = false;
            this.ürün_ekle_btn.Click += new System.EventHandler(this.ürün_ekle_btn_Click);
            this.ürün_ekle_btn.MouseLeave += new System.EventHandler(this.Ürün_ekle_ve_düzenle_btn_MouseLeave);
            this.ürün_ekle_btn.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Ürün_ekle_ve_düzenle_btn_MouseMove);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Window;
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.aktifkullanicilar_listbox);
            this.panel2.Controls.Add(this.btn_cikis);
            this.panel2.Controls.Add(this.tarih_label);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.kayit_tarihi_label);
            this.panel2.Controls.Add(this.statü_label);
            this.panel2.Controls.Add(this.soyisim_label);
            this.panel2.Controls.Add(this.isim_label);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.loginpanel_gelistiren_label);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label);
            this.panel2.Controls.Add(this.label_6);
            this.panel2.ForeColor = System.Drawing.Color.Black;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(376, 564);
            this.panel2.TabIndex = 14;
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Panel2_MouseDown);
            this.panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Panel2_MouseMove);
            this.panel2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Panel2_MouseUp);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Century Gothic", 16F);
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(12, 356);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(138, 25);
            this.label7.TabIndex = 21;
            this.label7.Text = "Kullanıcılar;";
            // 
            // aktifkullanicilar_listbox
            // 
            this.aktifkullanicilar_listbox.BackColor = System.Drawing.SystemColors.Window;
            this.aktifkullanicilar_listbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.aktifkullanicilar_listbox.Font = new System.Drawing.Font("Century Gothic", 11F);
            this.aktifkullanicilar_listbox.ForeColor = System.Drawing.Color.Black;
            this.aktifkullanicilar_listbox.IntegralHeight = false;
            this.aktifkullanicilar_listbox.ItemHeight = 20;
            this.aktifkullanicilar_listbox.Location = new System.Drawing.Point(17, 384);
            this.aktifkullanicilar_listbox.Name = "aktifkullanicilar_listbox";
            this.aktifkullanicilar_listbox.Size = new System.Drawing.Size(339, 125);
            this.aktifkullanicilar_listbox.TabIndex = 6;
            this.aktifkullanicilar_listbox.SelectedIndexChanged += new System.EventHandler(this.aktifkullanicilar_listbox_SelectedIndexChanged);
            // 
            // btn_cikis
            // 
            this.btn_cikis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.btn_cikis.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_cikis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_cikis.Font = new System.Drawing.Font("Century Gothic", 8F);
            this.btn_cikis.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_cikis.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_cikis.Location = new System.Drawing.Point(17, 515);
            this.btn_cikis.Name = "btn_cikis";
            this.btn_cikis.Size = new System.Drawing.Size(112, 33);
            this.btn_cikis.TabIndex = 8;
            this.btn_cikis.Text = "Çıkış Yap";
            this.toolTip1.SetToolTip(this.btn_cikis, "Farklı bir hesap ile giriş yapmanız için mevcut hesabınızdan çıkış yapar.");
            this.btn_cikis.UseVisualStyleBackColor = false;
            this.btn_cikis.Click += new System.EventHandler(this.Btn_cikis_Click);
            this.btn_cikis.MouseLeave += new System.EventHandler(this.Btn_cikis_MouseLeave);
            this.btn_cikis.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Btn_cikis_MouseMove);
            // 
            // tarih_label
            // 
            this.tarih_label.AutoSize = true;
            this.tarih_label.Font = new System.Drawing.Font("Century Gothic", 16F);
            this.tarih_label.ForeColor = System.Drawing.Color.Black;
            this.tarih_label.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tarih_label.Location = new System.Drawing.Point(73, 316);
            this.tarih_label.Name = "tarih_label";
            this.tarih_label.Size = new System.Drawing.Size(24, 25);
            this.tarih_label.TabIndex = 18;
            this.tarih_label.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Century Gothic", 16F);
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(12, 316);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 25);
            this.label10.TabIndex = 17;
            this.label10.Text = "Tarih: ";
            // 
            // kayit_tarihi_label
            // 
            this.kayit_tarihi_label.AutoSize = true;
            this.kayit_tarihi_label.Font = new System.Drawing.Font("Century Gothic", 16F);
            this.kayit_tarihi_label.ForeColor = System.Drawing.Color.Black;
            this.kayit_tarihi_label.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.kayit_tarihi_label.Location = new System.Drawing.Point(135, 284);
            this.kayit_tarihi_label.Name = "kayit_tarihi_label";
            this.kayit_tarihi_label.Size = new System.Drawing.Size(24, 25);
            this.kayit_tarihi_label.TabIndex = 16;
            this.kayit_tarihi_label.Text = "0";
            // 
            // statü_label
            // 
            this.statü_label.AutoSize = true;
            this.statü_label.Font = new System.Drawing.Font("Century Gothic", 16F);
            this.statü_label.ForeColor = System.Drawing.Color.Black;
            this.statü_label.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.statü_label.Location = new System.Drawing.Point(78, 252);
            this.statü_label.Name = "statü_label";
            this.statü_label.Size = new System.Drawing.Size(24, 25);
            this.statü_label.TabIndex = 15;
            this.statü_label.Text = "0";
            // 
            // soyisim_label
            // 
            this.soyisim_label.AutoSize = true;
            this.soyisim_label.Font = new System.Drawing.Font("Century Gothic", 16F);
            this.soyisim_label.ForeColor = System.Drawing.Color.Black;
            this.soyisim_label.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.soyisim_label.Location = new System.Drawing.Point(97, 220);
            this.soyisim_label.Name = "soyisim_label";
            this.soyisim_label.Size = new System.Drawing.Size(24, 25);
            this.soyisim_label.TabIndex = 14;
            this.soyisim_label.Text = "0";
            // 
            // isim_label
            // 
            this.isim_label.AutoSize = true;
            this.isim_label.Font = new System.Drawing.Font("Century Gothic", 16F);
            this.isim_label.ForeColor = System.Drawing.Color.Black;
            this.isim_label.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.isim_label.Location = new System.Drawing.Point(60, 188);
            this.isim_label.Name = "isim_label";
            this.isim_label.Size = new System.Drawing.Size(24, 25);
            this.isim_label.TabIndex = 13;
            this.isim_label.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Century Gothic", 16F);
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(12, 284);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(136, 25);
            this.label5.TabIndex = 12;
            this.label5.Text = "Kayıt tarihi: ";
            // 
            // loginpanel_gelistiren_label
            // 
            this.loginpanel_gelistiren_label.AutoSize = true;
            this.loginpanel_gelistiren_label.Font = new System.Drawing.Font("Century Gothic", 7F);
            this.loginpanel_gelistiren_label.ForeColor = System.Drawing.Color.Black;
            this.loginpanel_gelistiren_label.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.loginpanel_gelistiren_label.Location = new System.Drawing.Point(263, 519);
            this.loginpanel_gelistiren_label.Name = "loginpanel_gelistiren_label";
            this.loginpanel_gelistiren_label.Size = new System.Drawing.Size(107, 30);
            this.loginpanel_gelistiren_label.TabIndex = 7;
            this.loginpanel_gelistiren_label.Text = "        Selçuk Şahin \r\nTarafından geliştirildi\r\n";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Century Gothic", 16F);
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(12, 252);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 25);
            this.label4.TabIndex = 11;
            this.label4.Text = "Statü: ";
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Font = new System.Drawing.Font("Century Gothic", 16F);
            this.label.ForeColor = System.Drawing.Color.Black;
            this.label.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label.Location = new System.Drawing.Point(12, 188);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(62, 25);
            this.label.TabIndex = 8;
            this.label.Text = "Adı: ";
            // 
            // label_6
            // 
            this.label_6.AutoSize = true;
            this.label_6.Font = new System.Drawing.Font("Century Gothic", 16F);
            this.label_6.ForeColor = System.Drawing.Color.Black;
            this.label_6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label_6.Location = new System.Drawing.Point(12, 220);
            this.label_6.Name = "label_6";
            this.label_6.Size = new System.Drawing.Size(98, 25);
            this.label_6.TabIndex = 10;
            this.label_6.Text = "Soyadı: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 16F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(26)))), ((int)(((byte)(27)))));
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(517, 176);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 25);
            this.label1.TabIndex = 16;
            this.label1.Text = "ANA SAYFA";
            // 
            // linkLabel1
            // 
            this.linkLabel1.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(31)))), ((int)(((byte)(33)))));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkLabel1.Font = new System.Drawing.Font("Century Gothic", 8F);
            this.linkLabel1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.linkLabel1.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.linkLabel1.Location = new System.Drawing.Point(566, 531);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(151, 16);
            this.linkLabel1.TabIndex = 7;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "selcuksahin158@gmail.com";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel_LinkClicked);
            this.linkLabel1.MouseLeave += new System.EventHandler(this.linkLabel1_MouseLeave);
            this.linkLabel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.linkLabel1_MouseMove);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 8F);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(382, 500);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(212, 48);
            this.label2.TabIndex = 17;
            this.label2.Text = "Bu uygulamaya ilgili \r\nherhangi bir sorun, şikayet ve öneri için\r\nbir -eposta mes" +
    "aji gönderebilirsiniz";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label3.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(745, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(22, 25);
            this.label3.TabIndex = 8;
            this.label3.Text = "-";
            this.toolTip1.SetToolTip(this.label3, "Uygulamayı küçültmenizi sağlar.");
            this.label3.Click += new System.EventHandler(this.Label3_Click);
            this.label3.MouseLeave += new System.EventHandler(this.Label3_MouseLeave);
            this.label3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Label3_MouseMove);
            // 
            // logout_label
            // 
            this.logout_label.AutoSize = true;
            this.logout_label.Cursor = System.Windows.Forms.Cursors.Hand;
            this.logout_label.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold);
            this.logout_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.logout_label.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.logout_label.Location = new System.Drawing.Point(765, 8);
            this.logout_label.Name = "logout_label";
            this.logout_label.Size = new System.Drawing.Size(28, 25);
            this.logout_label.TabIndex = 9;
            this.logout_label.Text = "X";
            this.toolTip1.SetToolTip(this.logout_label, "Uygulamayı kapatmanızı sağlar.");
            this.logout_label.Click += new System.EventHandler(this.Logout_label_Click);
            this.logout_label.MouseLeave += new System.EventHandler(this.Logout_label_MouseLeave);
            this.logout_label.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Logout_label_MouseMove);
            // 
            // siparis_olustur_btn
            // 
            this.siparis_olustur_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.siparis_olustur_btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.siparis_olustur_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.siparis_olustur_btn.Font = new System.Drawing.Font("Century Gothic", 8F);
            this.siparis_olustur_btn.ForeColor = System.Drawing.Color.White;
            this.siparis_olustur_btn.Location = new System.Drawing.Point(474, 216);
            this.siparis_olustur_btn.Name = "siparis_olustur_btn";
            this.siparis_olustur_btn.Size = new System.Drawing.Size(111, 45);
            this.siparis_olustur_btn.TabIndex = 1;
            this.siparis_olustur_btn.Text = "Sipariş Oluştur";
            this.toolTip1.SetToolTip(this.siparis_olustur_btn, "Sipariş oluşturabileceğiniz sayfaya yönlendirir.");
            this.siparis_olustur_btn.UseVisualStyleBackColor = false;
            this.siparis_olustur_btn.Click += new System.EventHandler(this.Siparis_olustur_btn_Click);
            this.siparis_olustur_btn.MouseLeave += new System.EventHandler(this.Siparis_olustur_btn_MouseLeave);
            this.siparis_olustur_btn.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Siparis_olustur_btn_MouseMove);
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            // 
            // diğer_malzeme_grubları_sipariş_oluşturma
            // 
            this.diğer_malzeme_grubları_sipariş_oluşturma.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.diğer_malzeme_grubları_sipariş_oluşturma.Cursor = System.Windows.Forms.Cursors.Hand;
            this.diğer_malzeme_grubları_sipariş_oluşturma.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.diğer_malzeme_grubları_sipariş_oluşturma.Font = new System.Drawing.Font("Century Gothic", 8F);
            this.diğer_malzeme_grubları_sipariş_oluşturma.ForeColor = System.Drawing.Color.White;
            this.diğer_malzeme_grubları_sipariş_oluşturma.Location = new System.Drawing.Point(591, 322);
            this.diğer_malzeme_grubları_sipariş_oluşturma.Name = "diğer_malzeme_grubları_sipariş_oluşturma";
            this.diğer_malzeme_grubları_sipariş_oluşturma.Size = new System.Drawing.Size(111, 59);
            this.diğer_malzeme_grubları_sipariş_oluşturma.TabIndex = 4;
            this.diğer_malzeme_grubları_sipariş_oluşturma.Text = "Diğer Malzeme Grubları Sipariş Oluşturma";
            this.toolTip1.SetToolTip(this.diğer_malzeme_grubları_sipariş_oluşturma, "Diğer malzeme,ürünlerin siparişini oluşturmanız için sayfaya yönlendirir.\r\n");
            this.diğer_malzeme_grubları_sipariş_oluşturma.UseVisualStyleBackColor = false;
            this.diğer_malzeme_grubları_sipariş_oluşturma.Click += new System.EventHandler(this.diğer_malzeme_grubları_sipariş_oluşturma_Click);
            this.diğer_malzeme_grubları_sipariş_oluşturma.MouseLeave += new System.EventHandler(this.Diğer_malzeme_grubları_MouseLeave);
            this.diğer_malzeme_grubları_sipariş_oluşturma.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Diğer_malzeme_grubları_MouseMove);
            // 
            // diğer_malzeme_grubları_ürün_ekle
            // 
            this.diğer_malzeme_grubları_ürün_ekle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.diğer_malzeme_grubları_ürün_ekle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.diğer_malzeme_grubları_ürün_ekle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.diğer_malzeme_grubları_ürün_ekle.Font = new System.Drawing.Font("Century Gothic", 8F);
            this.diğer_malzeme_grubları_ürün_ekle.ForeColor = System.Drawing.Color.White;
            this.diğer_malzeme_grubları_ürün_ekle.Location = new System.Drawing.Point(474, 318);
            this.diğer_malzeme_grubları_ürün_ekle.Name = "diğer_malzeme_grubları_ürün_ekle";
            this.diğer_malzeme_grubları_ürün_ekle.Size = new System.Drawing.Size(111, 63);
            this.diğer_malzeme_grubları_ürün_ekle.TabIndex = 5;
            this.diğer_malzeme_grubları_ürün_ekle.Text = "Diğer Malzeme Grubları Ürün Ekle";
            this.toolTip1.SetToolTip(this.diğer_malzeme_grubları_ürün_ekle, "Diğer ürünleri düzenleyebileceğiniz sayfaya yönlendirir.");
            this.diğer_malzeme_grubları_ürün_ekle.UseVisualStyleBackColor = false;
            this.diğer_malzeme_grubları_ürün_ekle.Click += new System.EventHandler(this.diğer_malzeme_grubları_ürün_ekle_Click);
            this.diğer_malzeme_grubları_ürün_ekle.MouseLeave += new System.EventHandler(this.Diğer_malzeme_grubları_ekle_ve_düzenleme_btn_MouseLeave);
            this.diğer_malzeme_grubları_ürün_ekle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Diğer_malzeme_grubları_ekle_ve_düzenleme_btn_MouseMove);
            // 
            // yenikullanici_btn
            // 
            this.yenikullanici_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.yenikullanici_btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.yenikullanici_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.yenikullanici_btn.Font = new System.Drawing.Font("Century Gothic", 8F);
            this.yenikullanici_btn.ForeColor = System.Drawing.Color.White;
            this.yenikullanici_btn.Location = new System.Drawing.Point(534, 435);
            this.yenikullanici_btn.Name = "yenikullanici_btn";
            this.yenikullanici_btn.Size = new System.Drawing.Size(111, 45);
            this.yenikullanici_btn.TabIndex = 18;
            this.yenikullanici_btn.Text = "Yeni kullanıcı";
            this.toolTip1.SetToolTip(this.yenikullanici_btn, "Yeni bir kullanıcı oluşturabileceğiniz sayfaya yönlendirir.");
            this.yenikullanici_btn.UseVisualStyleBackColor = false;
            this.yenikullanici_btn.Click += new System.EventHandler(this.yenikullanici_btn_Click);
            this.yenikullanici_btn.MouseLeave += new System.EventHandler(this.yenikullanici_btn_MouseLeave);
            this.yenikullanici_btn.MouseMove += new System.Windows.Forms.MouseEventHandler(this.yenikullanici_btn_MouseMove);
            // 
            // ürün_düzenleme_btn
            // 
            this.ürün_düzenleme_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.ürün_düzenleme_btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ürün_düzenleme_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ürün_düzenleme_btn.Font = new System.Drawing.Font("Century Gothic", 8F);
            this.ürün_düzenleme_btn.ForeColor = System.Drawing.Color.White;
            this.ürün_düzenleme_btn.Location = new System.Drawing.Point(534, 384);
            this.ürün_düzenleme_btn.Name = "ürün_düzenleme_btn";
            this.ürün_düzenleme_btn.Size = new System.Drawing.Size(111, 45);
            this.ürün_düzenleme_btn.TabIndex = 19;
            this.ürün_düzenleme_btn.Text = "Ürün Düzenleme";
            this.toolTip1.SetToolTip(this.ürün_düzenleme_btn, "Ürün isimlerini düzenleyebileceğiniz sayfaya yönlendirir.");
            this.ürün_düzenleme_btn.UseVisualStyleBackColor = false;
            this.ürün_düzenleme_btn.Click += new System.EventHandler(this.ürün_düzenleme_btn_Click);
            this.ürün_düzenleme_btn.MouseLeave += new System.EventHandler(this.ürün_düzenleme_btn_MouseLeave);
            this.ürün_düzenleme_btn.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ürün_düzenleme_btn_MouseMove);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::OSBilişim.Properties.Resources.teknikservis_man;
            this.pictureBox1.Location = new System.Drawing.Point(116, 33);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(128, 128);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // Anaform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(802, 559);
            this.Controls.Add(this.ürün_düzenleme_btn);
            this.Controls.Add(this.yenikullanici_btn);
            this.Controls.Add(this.diğer_malzeme_grubları_ürün_ekle);
            this.Controls.Add(this.diğer_malzeme_grubları_sipariş_oluşturma);
            this.Controls.Add(this.siparis_olustur_btn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.logout_label);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.ürün_ekle_btn);
            this.Controls.Add(this.siparis_kontrol_btn);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Anaform";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ana Forum";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Anaform_FormClosed);
            this.Load += new System.EventHandler(this.Anaform_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Anaform_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Anaform_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Anaform_MouseUp);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button siparis_kontrol_btn;
        private System.Windows.Forms.Button ürün_ekle_btn;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Label loginpanel_gelistiren_label;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label logout_label;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_6;
        private System.Windows.Forms.Label isim_label;
        private System.Windows.Forms.Label tarih_label;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label kayit_tarihi_label;
        private System.Windows.Forms.Label statü_label;
        private System.Windows.Forms.Label soyisim_label;
        private System.Windows.Forms.Button siparis_olustur_btn;
        private System.Windows.Forms.Button btn_cikis;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ListBox aktifkullanicilar_listbox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button diğer_malzeme_grubları_sipariş_oluşturma;
        private System.Windows.Forms.Button diğer_malzeme_grubları_ürün_ekle;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button yenikullanici_btn;
        private System.Windows.Forms.Button ürün_düzenleme_btn;
    }
}