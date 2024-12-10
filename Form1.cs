using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace C_CuoiKi
{
    public partial class Form1 : Form
    {
        string connectstring = "Data Source=DESKTOP-OVN06OJ\\MSSQLSERVER02;Database=ShoeStoreDB;User ID=sa;Password = 123456789; TrustServerCertificate=True";
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adt;
        DataTable dtb = new DataTable();

        private int targetHeight = 200;
        private int currentHeight = 0;

        public Form1()
        {
            InitializeComponent();
            con = new SqlConnection(connectstring);
            loadData();

            groupBox2.Visible = false;
            buttonAdd.Visible = false;
            buttonEdit.Visible = false;
            buttonDelete.Visible = false;
            buttonRefreshInfo.Visible = false;
            groupBox2.Height = currentHeight;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            con = new SqlConnection(connectstring);

        }

        private void loadData()
        {
            try
            {
                con.Open();
                cmd = new SqlCommand("select * from Products", con);
                adt = new SqlDataAdapter(cmd);

                dtb.Clear();
                adt.Fill(dtb);
                dataGridView1.DataSource = dtb;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dataGridView1.CurrentRow.Index;
            textBox1.Text = dataGridView1.Rows[i].Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.Rows[i].Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.Rows[i].Cells[3].Value.ToString();
            textBox4.Text = dataGridView1.Rows[i].Cells[4].Value.ToString();
            textBox5.Text = dataGridView1.Rows[i].Cells[5].Value.ToString();
            textBox6.Text = dataGridView1.Rows[i].Cells[6].Value.ToString();
            textBox7.Text = dataGridView1.Rows[i].Cells[7].Value.ToString();

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd = con.CreateCommand();
                cmd.CommandText = "INSERT INTO Products VALUES('" + textBox1.Text + "', '" + textBox2.Text + "', '" + textBox3.Text + "', '" + textBox4.Text + "', '" + textBox5.Text + "', '" + textBox6.Text + "', '" + textBox7.Text + "')";
                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm dữ liệu thành công!");
                loadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                int productId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

                cmd = con.CreateCommand();
                cmd.CommandText = "UPDATE Products SET Name = '" + textBox1.Text + "', " +
                                  "Description = '" + textBox2.Text + "', " +
                                  "Size = '" + textBox3.Text + "', " +
                                  "Color = '" + textBox4.Text + "', " +
                                  "Price = '" + textBox5.Text + "', " +
                                  "StockQuantity = '" + textBox6.Text + "', " +
                                  "ImagePath = '" + textBox7.Text + "' " +
                                  "WHERE ProductID = " + productId;

                cmd.ExecuteNonQuery();
                MessageBox.Show("Cập nhật dữ liệu thành công!");
                loadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null)
                {
                    MessageBox.Show("Vui lòng chọn một dòng để xóa!");
                    return;
                }

                int productId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa dòng này?", "Xác nhận xóa", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    using (SqlConnection con = new SqlConnection(connectstring))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("DELETE FROM Products WHERE ProductID = @productId", con);
                        cmd.Parameters.AddWithValue("@productId", productId);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Xóa thành công!");
                        loadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void buttonRefreshInfo_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
            groupBox2.Visible = true;
            buttonDelete.Visible = true;
            buttonRefreshInfo.Visible = true;
            buttonAdd.Visible = true;
            buttonEdit.Visible = true;

            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (currentHeight < targetHeight)
            {
                currentHeight += 10;
                groupBox2.Height = currentHeight;
            }
            else
            {
                timer1.Stop();
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
