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
    public partial class TaskTypeForm : Form
    {
        public TaskTypeForm()
        {
            InitializeComponent();
            this.btn_save.Click += new EventHandler(this.Insert);
            this.btn_delet.Click += new EventHandler(this.Delete);
            this.btn_update.Click += new EventHandler(this.Update);
            this.dgw_data.RowHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.Info);
            this.Fill_data();
        }
        

        private NewDBEntities db = new NewDBEntities();
        Task_types selected;


        public void Info(object sender, DataGridViewCellMouseEventArgs e)
        {
            int id = (int)this.dgw_data.Rows[e.RowIndex].Cells[0].Value;
            this.selected = db.Task_types.Find(id);
            this.txt_type_name.Text = this.selected.task_type_name;
            this.txt_rate.Text = this.selected.task_type_rate+"";
            this.btn_delet.Visible = true;
            this.btn_update.Visible = true;
            this.btn_save.Visible = false;
        }

        public void Insert(object sender, EventArgs e)
        {
            db.Task_types.Add(new Task_types()
            {
                task_type_name = this.txt_type_name.Text,
                task_type_rate = Convert.ToDouble(this.txt_rate.Text)
            });
            db.SaveChanges();
            this.Cleaner();
        }

        public void Delete(object sender, EventArgs e)
        {
            db.Task_types.Remove(this.selected);
            db.SaveChanges();
            this.Cleaner();
        }

        public void Update(object sender, EventArgs e)
        {
            this.selected.task_type_name = this.txt_type_name.Text;
            this.selected.task_type_rate = Convert.ToDouble(this.txt_rate.Text);
            db.SaveChanges();
            this.Cleaner();
        }

        private void Fill_data()
        {
            List<Task_types> types = db.Task_types.ToList();
            this.dgw_data.Rows.Clear();
            for (int i = 0; i < types.Count; i++)
            {
                this.dgw_data.Rows.Add();
                this.dgw_data.Rows[i].Cells[0].Value = types[i].id;
                this.dgw_data.Rows[i].Cells[1].Value = types[i].task_type_name;
                this.dgw_data.Rows[i].Cells[2].Value = types[i].task_type_rate;
            }
        }

        private void Cleaner()
        {
            this.txt_type_name.Text = "";
            this.txt_rate.Text = "";
            this.btn_delet.Visible = false;
            this.btn_update.Visible = false;
            this.btn_save.Visible = true;
            this.Fill_data();
        }
        
    }
}
