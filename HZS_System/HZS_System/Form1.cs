using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HZS_System
{
    public partial class Form1 : Form
    {
        NewDBEntities db = new NewDBEntities();
        public Form1()
        {
            InitializeComponent();
            this.btn_login.Click += new EventHandler(this.Login);
        }

        private void Login(object sender, EventArgs e)
        {
            string password =Extentions.getHashCode(this.txt_password.Text);
            string email = this.txt_email.Text;

            Admin admin = db.Admins.FirstOrDefault(a => a.email==email && a.password == password);
            Student student = db.Students.FirstOrDefault(s => s.student_email == email && s.student_password == password);
            Mentor mentor = db.Mentors.FirstOrDefault(m => m.mentor_email == email && m.mentor_password == password);
            Teacher teacher = db.Teachers.FirstOrDefault(t => t.teacher_email == email && t.teacher_password == password);

            List<Group> groups = db.Groups.ToList();
            string group_name = "";
            if (admin != null) 
            {
                AdminPanelForm form = new AdminPanelForm();
                form.ShowDialog();
            }
            else if (student != null)
            {
                MessageBox.Show("Student");
            }
            else if (mentor != null)
            {
                foreach (Group item in groups)
                {
                    group_name =  item.group_mentor_id == mentor.id ? item.group_name : "No Group";
                }
                ProfileForm f = new ProfileForm(mentor);
                
                f.id = mentor.id;
                f.lbl_name.Text = mentor.mentor_name;
                f.lbl_surname.Text = mentor.mentor_surname;
                f.lbl_email.Text = mentor.mentor_email;
                f.lbl_gender.Text = mentor.Gender.gender_name;
                f.lbl_phone.Text = mentor.mentor_phone;
                f.txt_info.Text = mentor.mentor_info;
                f.lbl_group.Text = group_name;
                f.pct_mentor.Image = Image.FromFile(Extentions.uploads_path + mentor.mentor_photo);
                f.ShowDialog();
            }
            else if (teacher != null)
            {
                
                foreach (Group item in groups)
                {
                    group_name = item.group_techer_id == teacher.id ? item.group_name : "No Group";
                }
                ProfileForm f = new ProfileForm(teacher);

                f.id = teacher.id;
                f.lbl_name.Text = teacher.teacher_name;
                f.lbl_surname.Text = teacher.teacher_surname;
                f.lbl_email.Text = teacher.teacher_email;
                f.lbl_gender.Text = teacher.Gender.gender_name;
                f.lbl_phone.Text = teacher.teacher_phone;
                f.txt_info.Text = teacher.teacher_info;
                f.lbl_group.Text = group_name;
                f.pct_mentor.Image = Image.FromFile(Extentions.uploads_path + teacher.teacher_photo);
                f.btn_students.Visible = true;
                f.btn_task.Visible = true;
                f.ShowDialog();
            }
            else
            {
                this.lblError.Text = "Email or Password incorrect";
            }
            
        }

       
    }
}
