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
    public partial class GenderPanel : Form
    {
        NewDBEntities db = new NewDBEntities();
        private Gender Selected;
        public GenderPanel()       
        {
            InitializeComponent();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            Gender gender = new Gender();
            gender.gender_name = txt_gender.Text;

            db.Genders.Add(gender);
            db.SaveChanges();
            fillData();

        }
        private void fillData()
        {
            dgv_gender.Rows.Clear();
            int a = 0;
            List<Gender> gender_list = db.Genders.ToList();
            foreach (Gender item in gender_list)
            {
                dgv_gender.Rows.Add();
                dgv_gender.Rows[a].Cells[0].Value = item.id;
                dgv_gender.Rows[a].Cells[1].Value = item.gender_name;
                a++;
            }
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            Selected.gender_name = txt_gender.Text;
            db.SaveChanges();
            fillData();
        }

        private void SelectedGender(object sender, DataGridViewCellMouseEventArgs e)
        {
            int id = Convert.ToInt32(dgv_gender.Rows[e.RowIndex].Cells[0].Value);
            Selected = db.Genders.Find(id);
            txt_gender.Text = Selected.gender_name;
        }
    }
}
