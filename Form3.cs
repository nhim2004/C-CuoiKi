using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace C_CuoiKi
{
    public partial class FormQLKho : Form
    {
        string connectstring = "Data Source=DESKTOP-OVN06OJ\\MSSQLSERVER02;Database=ShoeStoreDB;User ID=sa;Password = 123456789; TrustServerCertificate=True";
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adt;
        DataTable dtbInventory = new DataTable();
        DataTable dtbProducts = new DataTable();




        public FormQLKho()
        {
            InitializeComponent();
            con = new SqlConnection(connectstring);
            loadInventoryData();
            loadProductData();
            dataGridView1.ReadOnly = true;
            dataGridView2.ReadOnly = true;


            panel1.Visible = false;
            dataGridView2.CellMouseEnter += new DataGridViewCellEventHandler(dataGridView2_CellMouseEnter);
            dataGridView2.CellMouseLeave += new DataGridViewCellEventHandler(dataGridView2_CellMouseLeave);

        }



        private void loadInventoryData()
        {
            try
            {
                con.Open();
                cmd = new SqlCommand("SELECT * FROM Inventory", con);
                adt = new SqlDataAdapter(cmd);

                dtbInventory.Clear();
                adt.Fill(dtbInventory);
                dataGridView1.DataSource = dtbInventory;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu Inventory: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void loadProductData()
        {
            try
            {
                con.Open();
                cmd = new SqlCommand("SELECT * FROM Products", con);
                adt = new SqlDataAdapter(cmd);

                dtbProducts.Clear();
                adt.Fill(dtbProducts);
                dataGridView2.DataSource = dtbProducts;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu Products: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            loadInventoryData();
            loadProductData();
        }




        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int inventoryID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["InventoryID"].Value);
                int productID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ProductID"].Value);
                int quantity = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Quantity"].Value);
                DateTime lastUpdated = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells["LastUpdated"].Value);

                MessageBox.Show($"InventoryID: {inventoryID}\nProductID: {productID}\nQuantity: {quantity}\nLast Updated: {lastUpdated}");
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    int i = e.RowIndex;

                    textBox1.Text = dataGridView1.Rows[i].Cells[1].Value?.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xử lý dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView2_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView2.Columns["ImagePath"].Index)  
                {
                    string imagePath = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();

                    if (!string.IsNullOrEmpty(imagePath) && System.IO.File.Exists(imagePath))
                    {
                        pictureBox1.Image = Image.FromFile(imagePath);
                        pictureBox1.Visible = true;

                        Point mousePosition = dataGridView2.PointToClient(Cursor.Position);
                        pictureBox1.Location = new Point(mousePosition.X + 10, mousePosition.Y + 10); 
                    }
                    else
                    {
                        pictureBox1.Visible = false; 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị hình ảnh: " + ex.Message);
            }
        }


        private void dataGridView2_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView2.Columns["ImagePath"].Index)  
            {
                string imagePath = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();

                if (!string.IsNullOrEmpty(imagePath) && System.IO.File.Exists(imagePath))
                {
                    pictureBox1.Image = Image.FromFile(imagePath);
                    pictureBox1.Visible = true;
                    Point mousePosition = dataGridView2.PointToClient(Cursor.Position);
                    pictureBox1.Location = new Point(mousePosition.X + 10, mousePosition.Y + 10); 
                }
                else
                {
                    pictureBox1.Visible = false;  
                }
            }
        }



        private void dataGridView2_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            pictureBox1.Visible = false;
        }


        private void button1_Click(object sender, EventArgs e)
        {


            button1.Visible = false;
            panel1.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin!");
                    return;
                }

                int productID = Convert.ToInt32(textBox1.Text);
                if (!int.TryParse(textBox2.Text, out int quantity))
                {
                    MessageBox.Show("Số lượng phải là số nguyên!");
                    return;
                }

                con.Open();
                cmd = new SqlCommand("INSERT INTO Inventory(ProductID, Quantity, LastUpdated) VALUES(@ProductID, @Quantity, @LastUpdated)", con);
                cmd.Parameters.AddWithValue("@ProductID", productID);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@LastUpdated", DateTime.Now);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("UPDATE Products SET StockQuantity = StockQuantity + @Quantity WHERE ProductID = @ProductID", con);
                cmd.Parameters.AddWithValue("@ProductID", productID);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Thêm thành công!");
                loadInventoryData();
                loadProductData();


            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm dữ liệu: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                // Tải lại dữ liệu cho Inventory và Products
                loadInventoryData();
                loadProductData();

                // Hiển thị thông báo thành công
                MessageBox.Show("Dữ liệu đã được cập nhật!");
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi nếu có vấn đề trong quá trình tải lại dữ liệu
                MessageBox.Show("Lỗi khi cập nhật dữ liệu: " + ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }

}
