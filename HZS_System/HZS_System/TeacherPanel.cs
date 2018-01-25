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
    public partial class TeacherPanel : Form
    {
        NewDBEntities db = new NewDBEntities();
        OpenFileDialog file = new OpenFileDialog();
        private Teacher SelectdTeacher;
        public TeacherPanel()
        {
            InitializeComponent();
            fillTeacherData();
            this.dgv_teacher.RowHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.Teacher_info);
            this.lbl_upload.Click += new EventHandler(this.File_upload);
            this.btn_update.Click += new EventHandler(this.Update);
        }

        private void Teacher_info(object sender, DataGridViewCellMouseEventArgs e)
        {
            int id = (int)this.dgv_teacher.Rows[e.RowIndex].Cells[0].Value;
            this.SelectdTeacher = db.Teachers.Find(id);
            
            txt_name.Text = SelectdTeacher.teacher_name;
            txt_surname.Text = SelectdTeacher.teacher_surname;
            txt_phone.Text = SelectdTeacher.teacher_phone;
            txt_info.Text = SelectdTeacher.teacher_info;
            txt_gender.Text = SelectdTeacher.Gender.gender_name;
            txt_password.ReadOnly = true;
            textBox3.ReadOnly = true;
            btn_add.Visible = false;
            btn_delete.Visible = true;
            btn_update.Visible = true;
        }

        private void cleareliyan()
        {
            txt_name.Text = "";
            txt_surname.Text = "";
            textBox3.Text = "";
            txt_phone.Text = "";
            txt_info.Text = "";
            txt_gender.Text = "";
            txt_password.Text = "";
            txt_password.ReadOnly = false;
            textBox3.ReadOnly = false;
            btn_add.Visible = true;
            btn_delete.Visible = false;
            btn_update.Visible = false;

        }


        private void btn_add_Click(object sender, EventArgs e)
        {
            string photo = DateTime.Now.ToString("ddMMyyyyHHssmm") + file.SafeFileName;
            Teacher teacher = new Teacher();

            teacher.teacher_name = txt_name.Text;
            teacher.teacher_surname = txt_surname.Text;
            teacher.teacher_phone = txt_phone.Text;
            teacher.teacher_email = textBox3.Text;
            teacher.teacher_info = txt_info.Text;
            int gender_id = db.Genders.Where(t => t.gender_name == this.txt_gender.Text).First().id;
            teacher.teacher_gender_id = gender_id;
            teacher.teacher_password = Extentions.getHashCode(txt_password.Text);
            teacher.teacher_photo = photo;

            db.Teachers.Add(teacher);
            db.SaveChanges();
            fillTeacherData();
            cleareliyan();


            WebClient webclient = new WebClient();
            webclient.DownloadFile(file.FileName, Extentions.uploads_path + photo);
        }

        private void fillTeacherData()
        {
            dgv_teacher.Rows.Clear();
            int a = 0;
            List<Teacher> teacher_list = db.Teachers.ToList();
            foreach (Teacher item in teacher_list)
            {
                dgv_teacher.Rows.Add();
                dgv_teacher.Rows[a].Cells[0].Value = item.id;
                dgv_teacher.Rows[a].Cells[1].Value = item.teacher_name;
                dgv_teacher.Rows[a].Cells[2].Value = item.teacher_surname;
                dgv_teacher.Rows[a].Cells[3].Value = item.teacher_email;
                dgv_teacher.Rows[a].Cells[4].Value = item.Gender.gender_name;
                dgv_teacher.Rows[a].Cells[5].Value = item.teacher_phone;
                dgv_teacher.Rows[a].Cells[6].Value = item.teacher_info;
                a++;
            }
        }

        private void TeacherPanel_Load(object sender, EventArgs e)
        {
            foreach (var item in db.Genders.ToList())
            {
                txt_gender.Items.Add(item.gender_name);
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            db.Teachers.Remove(this.SelectdTeacher);
            db.SaveChanges();
            fillTeacherData();
            cleareliyan();
        }


        private void File_upload(object sender, EventArgs e)
        {
            file.ShowDialog();
            this.pct_teacher.Image = Image.FromFile(file.FileName);
        }


        private void Update(object sender, EventArgs e)
        {

            this.SelectdTeacher.teacher_name = txt_name.Text;
            this.SelectdTeacher.teacher_surname = txt_surname.Text;
            this.SelectdTeacher.teacher_phone = txt_phone.Text;
            this.SelectdTeacher.teacher_info = txt_info.Text;
            int gender_id = db.Genders.Where(t => t.gender_name == this.txt_gender.Text).First().id;
            this.SelectdTeacher.teacher_gender_id = gender_id;

            db.SaveChanges();
            fillTeacherData();
            cleareliyan();
        }
    }
}

