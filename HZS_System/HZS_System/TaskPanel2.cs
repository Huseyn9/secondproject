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
    public partial class TaskPanel2 : Form
    {
        NewDBEntities db = new NewDBEntities();
        private Task Selected;
        public TaskPanel2()
        {
            InitializeComponent();
            fillData();
            this.dgv_task.RowHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.TaskPanel2_Load);
        }

        private void TaskPanel2_Load(object sender, EventArgs e)
        {
            foreach (var item in db.Task_types)
            {
                cmb_type.Items.Add(item.task_type_name);
            }
            foreach (var item in db.Groups)
            {
                cmb_group.Items.Add(item.group_name);
            }
            foreach (var item in db.Students)
            {
                cmb_student.Items.Add(item.student_name);
            }
        }

        private void btm_save_Click(object sender, EventArgs e)
        {
            Task task = new Task();
            int Task_type_id = db.Task_types.FirstOrDefault(t => t.task_type_name == cmb_type.Text).id;
            task.task_type_id = Task_type_id;
            task.task_start_date = start_date.Value;
            task.task_end_date = end_date.Value;
            task.task_point = Convert.ToDouble(txt_point.Text);
            task.task_note = txt_point.Text;
            task.task_source = txt_source.Text;
            int Task_group_id = db.Groups.FirstOrDefault(t => t.group_name == cmb_group.Text).id;
            task.task_group_id = Task_group_id;
            int Task_student_id = db.Students.FirstOrDefault(t => t.student_name == cmb_student.Text).id;
            task.task_student_id = Task_student_id;

            db.Tasks.Add(task);
            db.SaveChanges();
            fillData();
            cleareliyan();
        }

        private void fillData()
        {
            dgv_task.Rows.Clear();
            int i = 0;
            List<Task> PatientList = db.Tasks.ToList();
            foreach (Task item in PatientList)
            {
                dgv_task.Rows.Add();
                dgv_task.Rows[i].Cells[0].Value = item.id;
                dgv_task.Rows[i].Cells[1].Value = item.Task_types.task_type_name;
                dgv_task.Rows[i].Cells[2].Value = item.task_start_date;
                dgv_task.Rows[i].Cells[3].Value = item.task_end_date;
                dgv_task.Rows[i].Cells[4].Value = item.task_point;
                dgv_task.Rows[i].Cells[5].Value = item.task_source;
                dgv_task.Rows[i].Cells[6].Value = item.task_note;
                dgv_task.Rows[i].Cells[7].Value = item.Group.group_name;
                dgv_task.Rows[i].Cells[8].Value = item.Student.student_name;
                i++;
            }
        }
        private void cleareliyan()
        {
            cmb_type.Text = "";
            txt_point.Text = "";
            txt_note.Text = "";
            txt_source.Text = "";
            cmb_group.Text = "";
            cmb_student.Text = "";

        }

        private void btn_delete_Click(object sender, DataGridViewCellMouseEventArgs e)
        {
            int id = (int)this.dgv_task.Rows[e.RowIndex].Cells[0].Value;
            this.Selected = db.Tasks.Find(id);
            int no = Convert.ToInt32(dgv_task.Rows[e.RowIndex].Cells[0].Value);

            cmb_type.Text = this.Selected.Task_types.task_type_name;
            txt_point.Text = this.Selected.task_point.ToString();
            txt_note.Text = this.Selected.task_note.ToString();
            txt_source.Text = this.Selected.task_source.ToString();
            cmb_group.Text = this.Selected.Group.group_name;
            cmb_student.Text = this.Selected.Student.student_name;
            this.btm_save.Visible = false;
            this.btn_delete.Visible = true;
            this.btn_update.Visible = true;
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            db.Tasks.Remove(Selected);
            db.SaveChanges();
            cleareliyan();
            fillData();
        }

        private void cmb_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_type.Text = "";
        }

        private void btn_update_Click(object sender, EventArgs e)
        {

            int Task_type_id = db.Task_types.FirstOrDefault(t => t.task_type_name == cmb_type.Text).id;
            Selected.task_type_id = Task_type_id;
            Selected.task_start_date = start_date.Value;
            Selected.task_end_date = end_date.Value;
            Selected.task_point = Convert.ToDouble(txt_point.Text);
            Selected.task_note = txt_point.Text;
            Selected.task_source = txt_source.Text;
            int Task_group_id = db.Groups.FirstOrDefault(t => t.group_name == cmb_group.Text).id;
            Selected.task_group_id = Task_group_id;
            int Task_student_id = db.Students.FirstOrDefault(t => t.student_name == cmb_student.Text).id;
            Selected.task_student_id = Task_student_id;


            db.SaveChanges();
            cleareliyan();
            fillData();
        }
    }
}
