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

namespace HZS_System
{
    public partial class StudentPanel : Form
    {
        NewDBEntities db = new NewDBEntities();
        OpenFileDialog file = new OpenFileDialog();
        Student selectedStu;
        public StudentPanel()
        {
            InitializeComponent();
            this.Fill_cmb();
            this.Add_event();
            this.CLeaner();
        }

        private void Add_event()
        {
            this.btn_save.Click += new EventHandler(this.Add_student);
            this.lbl_upload.Click += new EventHandler(this.Upload_file);
            this.dgw_student.RowHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.Info_data);
            this.btn_delet.Click += new EventHandler(this.Delete);
            this.btn_update.Click += new EventHandler(this.Update);
        }

        private void Upload_file(object sender, EventArgs e)
        {
            file.ShowDialog();
            this.pct_student_photo.Image = Image.FromFile(file.FileName);
        }

        private void Add_student(object sender, EventArgs e)
        {
            string photo = DateTime.Now.ToString("ddMMyyyyHHssmm") + file.SafeFileName;
            int gen_id = db.Genders.First(g => g.gender_name == this.cmb_gender.Text).id;
            int group_id = db.Groups.First(g => g.group_name == this.cmb_group.Text).id;

            db.Students.Add(new Student()
            {
                student_name = this.txt_name.Text,
                student_surname = this.txt_surname.Text,
                student_email = this.txt_email.Text,
                student_phone = this.txt_phone.Text,
                student_github_account = this.txt_account.Text,
                student_info = this.txt_info.Text,
                student_photo = photo,
                student_cap_point = this.txt_cap_point.Text == "" ? 0 : Convert.ToDouble(this.txt_cap_point.Text),
                student_group_id = group_id,
                student_gender_id = gen_id,
                student_password = Extentions.getHashCode(this.txt_password.Text)
            });
            db.SaveChanges();
            this.CLeaner();

            WebClient webclient = new WebClient();
            webclient.DownloadFile(file.FileName, Extentions.uploads_path + photo);
        }

        private void Update(object sender, EventArgs e)
        {
            int gen_id = db.Genders.First(g => g.gender_name == this.cmb_gender.Text).id;
            int group_id = db.Groups.First(g => g.group_name == this.cmb_group.Text).id;


            this.selectedStu.student_name = this.txt_name.Text;
            this.selectedStu.student_surname = this.txt_surname.Text;
            this.selectedStu.student_phone = this.txt_phone.Text;
            this.selectedStu.student_github_account = this.txt_account.Text;
            this.selectedStu.student_info = this.txt_info.Text;
            this.selectedStu.student_cap_point = this.txt_cap_point.Text == "" ? 0 : Convert.ToDouble(this.txt_cap_point.Text);
            this.selectedStu.student_group_id = group_id;
            this.selectedStu.student_gender_id = gen_id;
            
            db.SaveChanges();
            this.CLeaner();
        }
        private void Delete(object sender, EventArgs e)
        {
            db.Students.Remove(this.selectedStu);
            db.SaveChanges();
            this.CLeaner();
        }


        private void Info_data(object sender, DataGridViewCellMouseEventArgs e)
        {
            int id = (int)this.dgw_student.Rows[e.RowIndex].Cells[0].Value;
            this.selectedStu = db.Students.Find(id);


            this.txt_account.Text = this.selectedStu.student_github_account;
            this.txt_cap_point.Text = this.selectedStu.student_cap_point+"";
            this.txt_info.Text = this.selectedStu.student_info;
            this.txt_name.Text = this.selectedStu.student_name;
            this.txt_password.ReadOnly = true;
            this.txt_email.ReadOnly = true;
            this.txt_phone.Text = this.selectedStu.student_phone;
            this.txt_surname.Text = this.selectedStu.student_surname;
            this.cmb_gender.Text = this.selectedStu.Gender.gender_name;
            this.cmb_group.Text = this.selectedStu.Group.group_name;
            this.btn_save.Visible = false;
            this.btn_delet.Visible = true;
            this.btn_update.Visible = true;
        }

        private void Fill_cmb()
        {
            foreach (Gender item in db.Genders.ToList())
            {
                this.cmb_gender.Items.Add(item.gender_name);
            }
            foreach (Group item in db.Groups.ToList())
            {
                this.cmb_group.Items.Add(item.group_name);
            }
        }

        private void CLeaner()
        {
            this.pct_student_photo.Image = Image.FromFile(Extentions.uploads_path + "student.png");
            this.txt_account.Text = "";
            this.txt_cap_point.Text = "";
            this.txt_email.Text = "";
            this.txt_info.Text = "";
            this.txt_name.Text = "";
            this.txt_password.Text = "";
            this.txt_phone.Text = "";
            this.txt_surname.Text = "";
            this.cmb_gender.SelectedIndex = -1;
            this.cmb_group.SelectedIndex = -1;
            this.btn_save.Visible = true;
            this.btn_delet.Visible = false;
            this.btn_update.Visible = false;
            this.txt_password.ReadOnly = false;
            this.txt_email.ReadOnly = false;
            this.Fill_data();
        }

        private void Fill_data()
        {
            this.dgw_student.Rows.Clear();
            List<Student> stu = db.Students.ToList();
            for(int a = 0; a < stu.Count; a++)
            {
                this.dgw_student.Rows.Add();
                this.dgw_student.Rows[a].Cells[0].Value = stu[a].id;
                this.dgw_student.Rows[a].Cells[1].Value = stu[a].student_name;
                this.dgw_student.Rows[a].Cells[2].Value = stu[a].student_surname;
                this.dgw_student.Rows[a].Cells[3].Value = stu[a].student_email;
                this.dgw_student.Rows[a].Cells[4].Value = stu[a].student_cap_point;
                this.dgw_student.Rows[a].Cells[5].Value = stu[a].student_phone;
                this.dgw_student.Rows[a].Cells[6].Value = stu[a].Gender.gender_name;
                this.dgw_student.Rows[a].Cells[7].Value = stu[a].Group.group_name;
                this.dgw_student.Rows[a].Cells[8].Value = stu[a].student_github_account;
                this.dgw_student.Rows[a].Cells[9].Value = stu[a].student_info;
            }
        }
    }
}
