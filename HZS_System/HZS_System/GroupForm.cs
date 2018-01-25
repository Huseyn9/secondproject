using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace HZS_System
{
    public partial class GroupForm : Form
    {
        NewDBEntities db = new NewDBEntities();
        Group selected;
        public GroupForm()
        {
            InitializeComponent();
            this.Fill_cmb();
            this.Fill_data();
            this.btn_save.Click += new EventHandler(this.Insert);
            this.btn_delet.Click += new EventHandler(this.Delete);
            this.btn_update.Click += new EventHandler(this.Update);
            this.dgw_group_data.RowHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.Info);
        }

        private void Info(object sender, DataGridViewCellMouseEventArgs e)
        {
            int id = (int)this.dgw_group_data.Rows[e.RowIndex].Cells[0].Value;
            this.selected = db.Groups.Find(id);

            this.txt_group_name.Text = this.selected.group_name;
            this.cmb_group_type.Text = this.selected.Group_types.group_type_name;
            this.cmb_mentor.Text = this.selected.Mentor.mentor_name;
            this.cmb_teacher.Text = this.selected.Teacher.teacher_name;
            this.dtp_begin_date.Value = this.selected.group_start_date;

            this.btn_save.Visible = false;
            this.btn_delet.Visible = true;
            this.btn_update.Visible = true;
        }

        private void Insert(object sender, EventArgs e)
        {
            int type_id = db.Group_types.First(g => g.group_type_name == this.cmb_group_type.Text).id;
            int teacher_id = db.Teachers.First(g => g.teacher_name == this.cmb_teacher.Text).id;
            int mentor_id = db.Mentors.First(g => g.mentor_name == this.cmb_mentor.Text).id;
            db.Groups.Add(new Group()
            {
                group_name = this.txt_group_name.Text,
                group_type_id = type_id,
                group_techer_id =teacher_id,
                group_mentor_id = mentor_id,
                group_start_date = this.dtp_begin_date.Value
            });
            db.SaveChanges();
            this.Cleaner();
        }

        private void Delete(object sender, EventArgs e)
        {
            db.Groups.Remove(this.selected);
            db.SaveChanges();
            this.Cleaner();
        }

        private void Update(object sender, EventArgs e)
        {
            int type_id = db.Group_types.First(g => g.group_type_name == this.cmb_group_type.Text).id;
            int teacher_id = db.Teachers.First(g => g.teacher_name == this.cmb_teacher.Text).id;
            int mentor_id = db.Mentors.First(g => g.mentor_name == this.cmb_mentor.Text).id;

            this.selected.group_name = this.txt_group_name.Text;
            this.selected.group_type_id = type_id;
            this.selected.group_techer_id = teacher_id;
            this.selected.group_mentor_id = mentor_id;
            this.selected.group_start_date = this.dtp_begin_date.Value;          
            db.SaveChanges();
            this.Cleaner();
        }

        private void Fill_cmb()
        {
            foreach (Group_types item in db.Group_types.ToList())
            {
                this.cmb_group_type.Items.Add(item.group_type_name);
            }
            foreach (Teacher item in db.Teachers.ToList())
            {
                this.cmb_teacher.Items.Add(item.teacher_name);
            }
            foreach (Mentor item in db.Mentors.ToList())
            {
                this.cmb_mentor.Items.Add(item.mentor_name);
            }
        }

        private void Cleaner()
        {
            this.txt_group_name.Text = "";
            this.cmb_group_type.SelectedIndex = -1;
            this.cmb_mentor.SelectedIndex = -1;
            this.cmb_teacher.SelectedIndex = -1;
            this.dtp_begin_date.Value = DateTime.Now;
            this.Fill_data();
            this.btn_save.Visible = true;
            this.btn_delet.Visible = false;
            this.btn_update.Visible = false;
        }

        private void Fill_data()
        {
            this.dgw_group_data.Rows.Clear();
            List<Group> groups = db.Groups.ToList();
            for(int i = 0; i < groups.Count; i++)
            {
                this.dgw_group_data.Rows.Add();
                this.dgw_group_data.Rows[i].Cells[0].Value = groups[i].id;
                this.dgw_group_data.Rows[i].Cells[1].Value = groups[i].group_name;
                this.dgw_group_data.Rows[i].Cells[2].Value = groups[i].Group_types.group_type_name;
                this.dgw_group_data.Rows[i].Cells[3].Value = groups[i].Teacher.teacher_name;
                this.dgw_group_data.Rows[i].Cells[4].Value = groups[i].Mentor.mentor_name;
                this.dgw_group_data.Rows[i].Cells[5].Value = groups[i].group_start_date.ToString("dd-MMMM-yyyy");
            }
        }

        
    }
}
