using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace HZS_System
{
    public partial class MentorForm : Form
    {
        NewDBEntities db = new NewDBEntities();
        OpenFileDialog file_dialog = new OpenFileDialog();
        Mentor selectedMentor;
        string photo_name;

        public MentorForm()
        {
            InitializeComponent();
            this.Fill_cmb();
            this.Cleaner();
            this.Add_events();
        }

        private void Add_events()
        {
            this.btn_save.Click += new EventHandler(this.Add_mentor);
            this.lbl_upload.Click += new EventHandler(this.File_upload);
            this.dgwMentor.RowHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.Mentor_info);
            this.btn_delet.Click += new EventHandler(this.Delete);
            this.btn_update.Click += new EventHandler(this.Update);
        }


        private void Fill_mentor_data(List<Mentor> mentors)
        {
            this.dgwMentor.Rows.Clear();
            for (int i = 0; i < mentors.Count; i++)
            {
                this.dgwMentor.Rows.Add();
                this.dgwMentor.Rows[i].Cells[0].Value = mentors[i].id;
                this.dgwMentor.Rows[i].Cells[1].Value = mentors[i].mentor_name;
                this.dgwMentor.Rows[i].Cells[2].Value = mentors[i].mentor_surname;
                this.dgwMentor.Rows[i].Cells[3].Value = mentors[i].mentor_email;
                this.dgwMentor.Rows[i].Cells[4].Value = mentors[i].mentor_phone;
                this.dgwMentor.Rows[i].Cells[5].Value = mentors[i].mentor_info;
                this.dgwMentor.Rows[i].Cells[6].Value = mentors[i].Gender.gender_name;
            }
        }

        private void Fill_cmb()
        {
            foreach (Gender item in db.Genders.ToList())
            {
                this.cmb_gender.Items.Add(item.gender_name);
            }
        }

        private void Cleaner()
        {
            this.pct_mentor_photo.Image = Image.FromFile(Extentions.uploads_path + "mentor.jpg");
            this.Fill_mentor_data(db.Mentors.ToList());
            this.txt_email.Text = "";
            this.txt_info.Text = "";
            this.txt_name.Text = "";
            this.txt_password.Text = "";
            this.txt_phone.Text = "";
            this.txt_surname.Text = "";
            this.cmb_gender.SelectedIndex = -1;
            this.lbl_error.Text = "";
            this.btn_delet.Visible = false;
            this.btn_update.Visible = false;
            this.btn_save.Visible = true;
            this.txt_email.ReadOnly = false;
            this.txt_password.ReadOnly = false;
        }

        private void File_upload(object sender, EventArgs e)
        {
            file_dialog.ShowDialog();
            if (file_dialog.FileName != "")
            {
                this.pct_mentor_photo.Image = Image.FromFile(file_dialog.FileName);
            }
        }

        private void Add_mentor(object sender, EventArgs e)
        {
            if (this.Checking())
            {
                photo_name = DateTime.Now.ToString("ddMMyyyyssmmHH") + file_dialog.SafeFileName;
                int gen_id = db.Genders.FirstOrDefault(g => g.gender_name == this.cmb_gender.Text).id;
                Mentor mentor = new Mentor();
                mentor.mentor_name = txt_name.Text;
                mentor.mentor_surname = txt_surname.Text;
                mentor.mentor_email = txt_email.Text;
                mentor.mentor_info = txt_info.Text;
                mentor.mentor_phone = txt_phone.Text;
                mentor.mentor_photo = photo_name;
                mentor.mentor_gender_id = gen_id;
                mentor.mentor_password = Extentions.getHashCode(this.txt_password.Text);
                db.Mentors.Add(mentor);
                db.SaveChanges();
                this.Cleaner();

                WebClient webcilent = new WebClient();
                webcilent.DownloadFile(file_dialog.FileName, Extentions.uploads_path + photo_name);
                this.Cleaner();
            }
            else
            {
                this.lbl_error.Text = "Do not empty !!!";
            }
        }

        private bool Checking()
        {
            if(this.txt_email.Text==""||this.txt_info.Text==""||this.txt_name.Text==""||
               this.txt_password.Text==""||this.txt_phone.Text==""||this.txt_surname.Text==""||
               cmb_gender.Text==""|| file_dialog.FileName==""
                )
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void Mentor_info(object sender, DataGridViewCellMouseEventArgs e)
        {
            int id = (int)this.dgwMentor.Rows[e.RowIndex].Cells[0].Value;
            this.selectedMentor = db.Mentors.Find(id);

            this.txt_name.Text = selectedMentor.mentor_name;
            this.txt_surname.Text = selectedMentor.mentor_surname;
            this.txt_info.Text = selectedMentor.mentor_info;
            this.txt_phone.Text = selectedMentor.mentor_phone;
            this.cmb_gender.Text = selectedMentor.Gender.gender_name;

            this.txt_email.ReadOnly = true;
            this.txt_password.ReadOnly = true;
            this.lbl_upload.Click -= File_upload;
            this.btn_delet.Visible = true;
            this.btn_update.Visible = true;
            this.btn_save.Visible = false;
        }

        private void Update(object sender, EventArgs e)
        {
                int gen_id = db.Genders.FirstOrDefault(g => g.gender_name == this.cmb_gender.Text).id;
                this.selectedMentor.mentor_name = txt_name.Text;
                this.selectedMentor.mentor_surname = txt_surname.Text;
                this.selectedMentor.mentor_info = txt_info.Text;
                this.selectedMentor.mentor_phone = txt_phone.Text;
                this.selectedMentor.mentor_gender_id = gen_id;
                db.SaveChanges();
                this.Cleaner();
                this.lbl_upload.Click += File_upload;
        }

        private void Delete(object sender, EventArgs e)
        {
            db.Mentors.Remove(this.selectedMentor);
            db.SaveChanges();
            this.Cleaner();
            this.lbl_upload.Click += File_upload;
        }
    }
}
