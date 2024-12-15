using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;

public class ImageHelper
{
    private string connectionString;

    public ImageHelper(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public byte[] ImageToByteArray(Image image)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg); // Lưu ảnh dưới định dạng JPEG
            return ms.ToArray(); // Trả về mảng byte
        }
    }

    public void SaveImageToDatabase(string name, decimal price, Image image)
    {
        byte[] imageBytes = ImageToByteArray(image); // Chuyển hình ảnh thành mảng byte

        string query = "INSERT INTO Products (Name, Price, Image) VALUES (@Name, @Price, @Image)"; // Câu lệnh SQL

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Image", imageBytes); // Thêm mảng byte vào cơ sở dữ liệu

                connection.Open();
                cmd.ExecuteNonQuery(); // Thực thi câu lệnh
            }
        }
    }

    public Image LoadImageFromDatabase(int productId)
    {
        string query = "SELECT Image FROM Products WHERE ProductID = @ProductID"; // Câu lệnh SQL để tải ảnh
        byte[] imageBytes = null;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@ProductID", productId);

                connection.Open();
                imageBytes = cmd.ExecuteScalar() as byte[]; // Trả về mảng byte của hình ảnh
            }
        }

        if (imageBytes != null)
        {
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                return Image.FromStream(ms); // Chuyển mảng byte thành hình ảnh và trả về
            }
        }
        return null;
    }
}
