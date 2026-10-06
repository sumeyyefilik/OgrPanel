using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OgrPanel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int secilenid = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            listele();
        }

        void listele()
        {
            try
            {
                OkulDBEntities db = new OkulDBEntities();
                dataGridView1.DataSource = db.Student.ToList();
                basliklariDuzenle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı bağlantı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void basliklariDuzenle()
        {
            if (dataGridView1.Columns.Count >= 5)
            {
                dataGridView1.Columns[0].HeaderText = "Öğrenci ID";
                dataGridView1.Columns[1].HeaderText = "Adı";
                dataGridView1.Columns[2].HeaderText = "Soyadı";
                dataGridView1.Columns[3].HeaderText = "E-Posta";
                dataGridView1.Columns[4].HeaderText = "Öğrenci No";

                dataGridView1.Columns[0].Width = 80;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Lütfen en azından öğrenci adını giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtName.Focus();
                    return;
                }

                Student prs = new Student();
                prs.Name = txtName.Text.Trim();
                prs.Surname = txtSurname.Text.Trim();
                prs.Email = txtEmail.Text.Trim();
                prs.Number = txtNumber.Text.Trim();

                OkulDBEntities db = new OkulDBEntities();
                db.Student.Add(prs);
                db.SaveChanges();

                listele();
                temizle();
                MessageBox.Show("Öğrenci başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ekleme hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (secilenid == 0)
                {
                    MessageBox.Show("Lütfen silinecek öğrenciyi tablodan seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult onay = MessageBox.Show("Seçili öğrenciyi silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (onay != DialogResult.Yes) return;

                OkulDBEntities db = new OkulDBEntities();
                var silinecek = db.Student.Find(secilenid);
                if (silinecek != null)
                {
                    db.Student.Remove(silinecek);
                    db.SaveChanges();
                    listele();
                    temizle();
                    MessageBox.Show("Öğrenci başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Silme hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (secilenid == 0)
                {
                    MessageBox.Show("Lütfen güncellenecek öğrenciyi tablodan seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                OkulDBEntities db = new OkulDBEntities();
                var guncel = db.Student.Find(secilenid);
                if (guncel != null)
                {
                    guncel.Name = txtName.Text.Trim();
                    guncel.Surname = txtSurname.Text.Trim();
                    guncel.Email = txtEmail.Text.Trim();
                    guncel.Number = txtNumber.Text.Trim();
                    db.SaveChanges();
                    listele();
                    MessageBox.Show("Öğrenci bilgileri başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Güncelleme hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string aranan = txtName.Text.Trim();
                OkulDBEntities db = new OkulDBEntities();
                var sonuc = db.Student.Where(x => x.Name.Contains(aranan)).ToList();
                dataGridView1.DataSource = sonuc;
                basliklariDuzenle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Arama hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            listele();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
                {
                    var row = dataGridView1.Rows[e.RowIndex];
                    if (row.Cells[0].Value != null)
                    {
                        secilenid = Convert.ToInt32(row.Cells[0].Value);
                        txtName.Text = row.Cells[1].Value?.ToString();
                        txtSurname.Text = row.Cells[2].Value?.ToString();
                        txtEmail.Text = row.Cells[3].Value?.ToString();
                        if (row.Cells.Count > 4)
                        {
                            txtNumber.Text = row.Cells[4].Value?.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Seçim hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void temizle()
        {
            secilenid = 0;
            txtName.Clear();
            txtSurname.Clear();
            txtEmail.Clear();
            txtNumber.Clear();
        }
    }
}