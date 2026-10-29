
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using DTO;

namespace DAL
{
    public class VatTuDAL
    {
        public List<VatTuDTO> GetAll()
        {
            List<VatTuDTO> list = new List<VatTuDTO>();

            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                string sql = @"
                    SELECT MaVatTu, TenVatTu, DVT, SoLuongTon
                    FROM dbo.VatTu
                    ORDER BY MaVatTu";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new VatTuDTO
                        {
                            MaVatTu = rd["MaVatTu"].ToString() ?? "",
                            TenVatTu = rd["TenVatTu"].ToString() ?? "",
                            DVT = rd["DVT"].ToString() ?? "",
                            SoLuongTon = Convert.ToDecimal(rd["SoLuongTon"])
                        });
                    }
                }
            }

            return list;
        }

        public List<VatTuDTO> Search(string keyword)
        {
            List<VatTuDTO> list = new List<VatTuDTO>();

            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                string sql = @"
                    SELECT MaVatTu, TenVatTu, DVT, SoLuongTon
                    FROM dbo.VatTu
                    WHERE MaVatTu LIKE @Keyword
                       OR TenVatTu LIKE @Keyword
                    ORDER BY MaVatTu";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Keyword",
                        System.Data.SqlDbType.NVarChar, 150)
                        .Value = "%" + keyword + "%";

                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            list.Add(new VatTuDTO
                            {
                                MaVatTu = rd["MaVatTu"].ToString() ?? "",
                                TenVatTu = rd["TenVatTu"].ToString() ?? "",
                                DVT = rd["DVT"].ToString() ?? "",
                                SoLuongTon = Convert.ToDecimal(
                                    rd["SoLuongTon"])
                            });
                        }
                    }
                }
            }

            return list;
        }

        public void Insert(VatTuDTO v)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                string sql = @"
                    INSERT INTO dbo.VatTu
                        (MaVatTu, TenVatTu, DVT, SoLuongTon)
                    VALUES
                        (@Ma, @Ten, @DVT, @Ton)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Ma", v.MaVatTu);
                    cmd.Parameters.AddWithValue("@Ten", v.TenVatTu);
                    cmd.Parameters.AddWithValue("@DVT",
                        string.IsNullOrWhiteSpace(v.DVT)
                            ? (object)DBNull.Value : v.DVT);
                    cmd.Parameters.AddWithValue("@Ton", v.SoLuongTon);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(VatTuDTO v)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                string sql = @"
                    UPDATE dbo.VatTu
                    SET TenVatTu = @Ten,
                        DVT = @DVT
                    WHERE MaVatTu = @Ma";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Ma", v.MaVatTu);
                    cmd.Parameters.AddWithValue("@Ten", v.TenVatTu);
                    cmd.Parameters.AddWithValue("@DVT",
                        string.IsNullOrWhiteSpace(v.DVT)
                            ? (object)DBNull.Value : v.DVT);

                    if (cmd.ExecuteNonQuery() == 0)
                        throw new Exception("Không tìm thấy vật tư.");
                }
            }
        }

        public void Delete(string maVatTu)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                string sql = @"
                    DELETE FROM dbo.VatTu
                    WHERE MaVatTu = @Ma";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Ma", maVatTu);

                    if (cmd.ExecuteNonQuery() == 0)
                        throw new Exception("Không tìm thấy vật tư.");
                }
            }
        }

        public List<VatTuDTO> GetLowStock(decimal threshold)
        {
            List<VatTuDTO> list = new List<VatTuDTO>();

            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                string sql = @"
                    SELECT MaVatTu, TenVatTu, DVT, SoLuongTon
                    FROM dbo.VatTu
                    WHERE SoLuongTon < @Nguong
                    ORDER BY SoLuongTon ASC";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Nguong",
                        System.Data.SqlDbType.Decimal).Value = threshold;

                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            list.Add(new VatTuDTO
                            {
                                MaVatTu = rd["MaVatTu"].ToString() ?? "",
                                TenVatTu = rd["TenVatTu"].ToString() ?? "",
                                DVT = rd["DVT"].ToString() ?? "",
                                SoLuongTon = Convert.ToDecimal(
                                    rd["SoLuongTon"])
                            });
                        }
                    }
                }
            }

            return list;
        }

        public decimal GetInventoryValue()
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                string sql = @"
                    SELECT ISNULL(SUM(SoLuongTon * GiaTri.DonGia), 0)
                    FROM dbo.VatTu v
                    OUTER APPLY
                    (
                        SELECT TOP 1 ct.DonGia
                        FROM dbo.ChiTietPhieuKho ct
                        INNER JOIN dbo.PhieuKho p
                            ON p.MaPhieu = ct.MaPhieu
                        WHERE ct.MaVatTu = v.MaVatTu
                          AND p.LoaiPhieu = N'NHAP'
                        ORDER BY p.NgayLap DESC, p.MaPhieu DESC
                    ) AS GiaTri";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    return Convert.ToDecimal(cmd.ExecuteScalar());
                }
            }
        }
    }
}
