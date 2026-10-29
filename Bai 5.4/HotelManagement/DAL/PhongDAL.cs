
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using DTO;

namespace DAL
{
    public class PhongDAL
    {
        public List<PhongDTO> GetAll()
        {
            List<PhongDTO> list = new List<PhongDTO>();

            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                string sql = @"
                    SELECT MaPhong, SoPhong, LoaiPhong,
                           DonGiaTheoGio, TrangThai
                    FROM dbo.Phong
                    ORDER BY SoPhong";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new PhongDTO
                        {
                            MaPhong = Convert.ToInt32(reader["MaPhong"]),
                            SoPhong = reader["SoPhong"].ToString() ?? "",
                            LoaiPhong = reader["LoaiPhong"].ToString() ?? "",
                            DonGiaTheoGio = Convert.ToDecimal(
                                reader["DonGiaTheoGio"]),
                            TrangThai = reader["TrangThai"].ToString() ?? ""
                        });
                    }
                }
            }

            return list;
        }

        public int Insert(PhongDTO p)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                string sql = @"
                    INSERT INTO dbo.Phong
                        (SoPhong, LoaiPhong, DonGiaTheoGio, TrangThai)
                    VALUES
                        (@SoPhong, @LoaiPhong, @DonGiaTheoGio, @TrangThai)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    AddParameters(cmd, p);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int Update(PhongDTO p)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                string sql = @"
                    UPDATE dbo.Phong
                    SET SoPhong = @SoPhong,
                        LoaiPhong = @LoaiPhong,
                        DonGiaTheoGio = @DonGiaTheoGio,
                        TrangThai = @TrangThai
                    WHERE MaPhong = @MaPhong";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    AddParameters(cmd, p);
                    cmd.Parameters.AddWithValue("@MaPhong", p.MaPhong);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int Delete(int maPhong)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                string sql = @"
                    DELETE FROM dbo.Phong
                    WHERE MaPhong = @MaPhong";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaPhong", maPhong);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        private void AddParameters(SqlCommand cmd, PhongDTO p)
        {
            cmd.Parameters.AddWithValue("@SoPhong", p.SoPhong);
            cmd.Parameters.AddWithValue("@LoaiPhong", p.LoaiPhong);
            cmd.Parameters.AddWithValue(
                "@DonGiaTheoGio", p.DonGiaTheoGio);
            cmd.Parameters.AddWithValue("@TrangThai", p.TrangThai);
        }
    }
}
