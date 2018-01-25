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
            string password = this.txt_password.Text;
            string email = this.txt_email.Text;
            Admin admin = db.Admins.FirstOrDefault(t => t.email==email && t.password == password);
            Student student = db.Students.FirstOrDefault(s => s.student_email == email && s.student_password == password);
            Mentor mentor = db.Mentors.FirstOrDefault(m => m.mentor_email == email && m.mentor_password == password);
            Teacher teacher = db.Teachers.FirstOrDefault(t => t.teacher_email == email && t.teacher_password == password);
            if (admin != null) 
            {
                AdminPanelForm form1 = new AdminPanelForm();
                form1.ShowDialog();
            }
            else if (student != null)
            {
                MessageBox.Show("Strudent");
            }
            else if (mentor != null)
            {
                MentorProfileForm form1 = new MentorProfileForm();
                form1.ShowDialog();
            }
            else if (teacher != null)
            {
                MessageBox.Show("Teacher");
            }
            else
            {
                this.lblError.Text = "Email or Password incorrect";
            }
        }
    }
}
