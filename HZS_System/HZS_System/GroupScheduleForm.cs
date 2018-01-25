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
    public partial class GroupScheduleForm : Form
    {
        private NewDBEntities db = new NewDBEntities();
        Group_schedule selected;
        public GroupScheduleForm()
        {
            InitializeComponent();
            this.btn_save.Click += new EventHandler(this.Insert);
            this.btn_delet.Click += new EventHandler(this.Delete);
            this.btn_update.Click += new EventHandler(this.Update);
            this.dgw_data.RowHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.Info);
            this.Fill_data();
        }

        public void Info(object sender, DataGridViewCellMouseEventArgs e)
        {
            int id = (int)this.dgw_data.Rows[e.RowIndex].Cells[0].Value;
            this.selected = db.Group_schedule.Find(id);
            this.txt_shedule_name.Text = this.selected.group_schedule_name;
            this.dtp_begin_time.Value = selected.group_begin_time;
            this.dtp_end_time.Value = selected.group_end_time;
            this.btn_delet.Visible = true;
            this.btn_update.Visible = true;
            this.btn_save.Visible = false;
        }

        public void Insert(object sender, EventArgs e)
        {
            db.Group_schedule.Add(new Group_schedule()
            {
                group_schedule_name = this.txt_shedule_name.Text,
                group_begin_time = this.dtp_begin_time.Value,
                group_end_time = this.dtp_end_time.Value
            });
            db.SaveChanges();
            this.Cleaner();
        }

        public void Delete(object sender, EventArgs e)
        {
            db.Group_schedule.Remove(this.selected);
            db.SaveChanges();
            this.Cleaner();
        }

        public void Update(object sender, EventArgs e)
        {
            this.selected.group_schedule_name = this.txt_shedule_name.Text;
            this.selected.group_begin_time = this.dtp_begin_time.Value;
            this.selected.group_end_time = this.dtp_end_time.Value;
            db.SaveChanges();
            this.Cleaner();
        }

        private void Fill_data()
        {
            List<Group_schedule> sch = db.Group_schedule.ToList();
            this.dgw_data.Rows.Clear();
            for (int i = 0; i < sch.Count; i++)
            {
                this.dgw_data.Rows.Add();
                this.dgw_data.Rows[i].Cells[0].Value = sch[i].id;
                this.dgw_data.Rows[i].Cells[1].Value = sch[i].group_schedule_name;
                this.dgw_data.Rows[i].Cells[2].Value = sch[i].group_begin_time.ToString("HH:mm-tt");
                this.dgw_data.Rows[i].Cells[3].Value = sch[i].group_end_time.ToString("HH:mm-tt");
            }
        }

        private void Cleaner()
        {
            this.txt_shedule_name.Text = "";
            this.dtp_begin_time.Value = DateTime.Now;
            this.dtp_end_time.Value = DateTime.Now;
            this.btn_delet.Visible = false;
            this.btn_update.Visible = false;
            this.btn_save.Visible = true;
            this.Fill_data();
        }
    }
}
