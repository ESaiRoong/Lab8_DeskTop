using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab7_Advanced_Command
{
    public partial class Form1 : Form
    {
        DataTable foodTable;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.LoadCategory();
           //TestServer();
        }
        void TestServer()
        {
            string connectionString = "server = PC740\\MSSQLSERVER01; database = RestaurantManagement; Integrated Security = true; ";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Ket noi thanh cong ");

                    // Thực hiện các thao tác với cơ sở dữ liệu ở đây
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ket noi that bai ");
                }
            }
        }
        private void LoadCategory()
        {
            string connectionString = "server = PC740\\MSSQLSERVER01; database = RestaurantManagement; Integrated Security = true; "; 
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = conn.CreateCommand(); cmd.CommandText = "SELECT ID, Name FROM Category";
            SqlDataAdapter adapter = new SqlDataAdapter(cmd); 
            DataTable dt = new DataTable(); 
             conn.Open();
            // Lấy dữ liệu từ csdi đưa vào DataTable
            adapter.Fill(dt);
            // Đồng kết nối và giải phóng bộ nhớ
            conn.Close();
            conn.Dispose();
            // Đưa dữ liệu vào Combo Box
            cbbCategory.DataSource = dt;
            // Hiển thị tên nhóm sản phẩm
            cbbCategory.DisplayMember = "Name";
            // Nhưng khi lấy giá trị thì lấy ID của nhóm
            cbbCategory.ValueMember = "ID";

        }

        private void cbbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbCategory.SelectedIndex == -1) return;
            string connectionString = "server= PC740\\MSSQLSERVER01 ; database = RestaurantManagement; Integrated Security = true;";
             SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Food WHERE FoodCategoryID = @categoryId";
            // Truyền tham số
            cmd.Parameters.Add("@categoryId", SqlDbType.Int);
            if (cbbCategory.SelectedValue is DataRowView)
            {
                DataRowView rowView = cbbCategory.SelectedValue as DataRowView;
                cmd.Parameters["@categoryID"].Value = rowView["ID"];
            }
           
            else
            {

                cmd.Parameters["@categoryId"].Value = cbbCategory.SelectedValue;
            }
            // Tạo bộ điều phiều dữ liệu
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            foodTable = new DataTable();
            // Mà kết nổi
            conn.Open();
            // Lấy dữ liệu từ csdl đưa vào DataTable
            adapter.Fill(foodTable);
            // Đóng kết nối và giải phóng bộ nhỏ
            conn.Close();
            conn.Dispose();
            // Đưa dữ liệu vào data gridview
            dgvFoodList.DataSource = foodTable;
            // Tính số lượng mẫu tin
            lbQuantity.Text = foodTable.Rows.Count.ToString();
            lbCatName.Text = cbbCategory.Text;
        }

        private void dgvFoodList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ngănCáchToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void tsmCalculateQuantity_Click(object sender, EventArgs e)
        {
            string connectionString = "server=PC740\\MSSQLSERVER01 ; database = RestaurantManagement; Integrated Security = true"; 
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT @numSaleFood = sum(Quantity) FROM BillDetails WHERE FoodID = @foodId";
            // Lây thông tin sản phẩm được chọn
            if (dgvFoodList.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow  = dgvFoodList.SelectedRows[0];
                DataRowView rowView  = selectedRow.DataBoundItem as DataRowView;
                // Truyền tham số
                cmd.Parameters.Add("@foodId", SqlDbType.Int);
                cmd.Parameters["@foodId"].Value = rowView["ID"];
                cmd.Parameters.Add("@numSaleFood", SqlDbType.Int);
                cmd.Parameters["@numSaleFood"].Direction = ParameterDirection.Output;

                conn.Open();

                cmd.ExecuteNonQuery();

                string result = cmd.Parameters["@numSaleFood"].Value.ToString();
                MessageBox.Show("Tổng số lượng món" + rowView["Name"] + "đã bán là: " +  result
                + " " + rowView["Unit"]);
                conn.Close();
            } 
            cmd.Dispose();
            conn.Dispose();
        }

    }
}

