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
    public partial class GroupTypeForm : Form
    {
        public GroupTypeForm()
        {
            InitializeComponent();
            this.btn_save.Click += new EventHandler(this.Insert);
            this.btn_delet.Click += new EventHandler(this.Delete);
            this.btn_update.Click += new EventHandler(this.Update);
            this.dgw_data.RowHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.Info);
            this.Fill_data();
            this.Fill_cmb();
        }

        private NewDBEntities db = new NewDBEntities();
        Group_types selected;
    

        public void Info(object sender, DataGridViewCellMouseEventArgs e)
        {
            int id = (int)this.dgw_data.Rows[e.RowIndex].Cells[0].Value;
            this.selected = db.Group_types.Find(id);
            this.txt_type_name.Text = this.selected.group_type_name;
            this.cmb_schedule.Text =    this.selected.Group_schedule.group_schedule_name;
            this.btn_delet.Visible = true;
            this.btn_update.Visible = true;
            this.btn_save.Visible = false;
        }

        public void Insert(object sender, EventArgs e)
        {
            int schedule_id = (int)db.Group_schedule.First(s => s.group_schedule_name == this.cmb_schedule.Text).id;
            db.Group_types.Add(new Group_types()
            {
                group_type_name = this.txt_type_name.Text,
                group_schedule_id = schedule_id
            });
            db.SaveChanges();
            this.Cleaner();
        }

        public void Delete(object sender, EventArgs e)
        {
            db.Group_types.Remove(this.selected);
            db.SaveChanges();
            this.Cleaner();
        }

        public void Update(object sender, EventArgs e)
        {
            int schedule_id = (int)db.Group_schedule.First(s => s.group_schedule_name == this.cmb_schedule.Text).id;

            this.selected.group_type_name = this.txt_type_name.Text;
            this.selected.group_schedule_id = schedule_id;
            
            db.SaveChanges();
            this.Cleaner();
        }

        private void Fill_data()
        {
            List<Group_types> types = db.Group_types.ToList();
            this.dgw_data.Rows.Clear();
            for (int i = 0; i < types.Count; i++)
            {
                this.dgw_data.Rows.Add();
                this.dgw_data.Rows[i].Cells[0].Value = types[i].id;
                this.dgw_data.Rows[i].Cells[1].Value = types[i].group_type_name;
                this.dgw_data.Rows[i].Cells[2].Value = types[i].Group_schedule.group_schedule_name;
            }
        }

        private void Cleaner()
        {
            this.txt_type_name.Text = "";
            this.cmb_schedule.SelectedIndex = -1;
            this.btn_delet.Visible = false;
            this.btn_update.Visible = false;
            this.btn_save.Visible = true;
            this.Fill_data();
        }

        private void Fill_cmb()
        {
            foreach(Group_schedule item in db.Group_schedule.ToList())
            {
                this.cmb_schedule.Items.Add(item.group_schedule_name);
            }
        }
    }
}
