using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace HZS_System
{
    public partial class ProfileForm : Form
    {
        private NewDBEntities db = new NewDBEntities();
        public int id;
        Mentor mentor;
        Teacher teacher;
        public ProfileForm(Mentor m)
        {
            this.Init();
            mentor = m;
        }
        public ProfileForm(Teacher t)
        {
            this.Init();
            teacher = t;
        }
       
        
        private void Init()
        {
            InitializeComponent();
            this.lbl_settings.Click += new EventHandler(this.Settings);
            this.btn_save.Click += new EventHandler(this.Update);
        }

        private void Settings(object sender, EventArgs e)
        {
            string password = Interaction.InputBox("Enter your password", "Confirem password");
            if (mentor != null)
            {
                if (mentor.mentor_password == Extentions.getHashCode(password))
                {
                    this.group.Visible = true;
                    this.txt_mail.Text = mentor.mentor_email;
                    this.txt_password.Text = password;
                    this.lbl_error.Text = "";
                }
                else
                {
                    this.lbl_error.Text = "Password incorrect";
                }
            }
            else if (teacher != null)
            {
                if (teacher.teacher_password == Extentions.getHashCode(password))
                {
                    this.group.Visible = true;
                    this.txt_mail.Text = teacher.teacher_email;
                    this.txt_password.Text = password;
                    this.lbl_error.Text = "";
                }
                else
                {
                    this.lbl_error.Text = "Password incorrect";
                }
            }
        }

        private void Update(object sender, EventArgs e)
        {
            if (mentor != null)
            {
                mentor.mentor_email = this.txt_mail.Text;
                mentor.mentor_password = Extentions.getHashCode(this.txt_password.Text);
                db.SaveChanges();
                this.group.Visible = false;
                mentor = db.Mentors.Find(Convert.ToInt32(this.id));
                this.lbl_email.Text = mentor.mentor_email;
            }
            else if (teacher != null)
            {
                teacher.teacher_email = this.txt_mail.Text;
                teacher.teacher_password = Extentions.getHashCode(this.txt_password.Text);
                db.SaveChanges();
                this.group.Visible = false;
                teacher = db.Teachers.Find(Convert.ToInt32(this.id));
                this.lbl_email.Text = teacher.teacher_email;
            }
        }
    }
}
