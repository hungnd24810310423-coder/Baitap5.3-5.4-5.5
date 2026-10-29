
using System;
using Microsoft.Data.SqlClient;
using DTO;

namespace DAL
{
    public class PhieuKhoDAL
    {
        public int Save(PhieuKhoDTO phieu)
        {
            if (phieu.ChiTiet == null || phieu.ChiTiet.Count == 0)
                throw new Exception("Phiếu kho phải có ít nhất một vật tư.");

            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        decimal tongTien = 0;

                        foreach (ChiTietPhieuKhoDTO ct in phieu.ChiTiet)
                        {
                            if (ct.SoLuong <= 0)
                                throw new Exception("Số lượng phải lớn hơn 0.");

                            if (ct.DonGia < 0)
                                throw new Exception("Đơn giá không được âm.");

                            tongTien += ct.SoLuong * ct.DonGia;
                        }

                        string sqlPhieu = @"
                            INSERT INTO dbo.PhieuKho
                                (LoaiPhieu, NgayLap, MaNCC, TongTien)
                            OUTPUT INSERTED.MaPhieu
                            VALUES
                                (@LoaiPhieu, @NgayLap, @MaNCC, @TongTien)";

                        int maPhieu;

                        using (SqlCommand cmd = new SqlCommand(
                            sqlPhieu, conn, trans))
                        {
                            cmd.Parameters.AddWithValue(
                                "@LoaiPhieu", phieu.LoaiPhieu);

                            cmd.Parameters.AddWithValue(
                                "@NgayLap", phieu.NgayLap);

                            cmd.Parameters.Add("@MaNCC",
                                System.Data.SqlDbType.Int).Value =
                                phieu.MaNCC.HasValue
                                    ? (object)phieu.MaNCC.Value
                                    : DBNull.Value;

                            cmd.Parameters.Add("@TongTien",
                                System.Data.SqlDbType.Decimal).Value =
                                tongTien;

                            maPhieu = Convert.ToInt32(
                                cmd.ExecuteScalar());
                        }

                        foreach (ChiTietPhieuKhoDTO ct in phieu.ChiTiet)
                        {
                            string sqlChiTiet = @"
                                INSERT INTO dbo.ChiTietPhieuKho
                                    (MaPhieu, MaVatTu, SoLuong, DonGia)
                                VALUES
                                    (@MaPhieu, @MaVatTu, @SoLuong, @DonGia)";

                            using (SqlCommand cmd = new SqlCommand(
                                sqlChiTiet, conn, trans))
                            {
                                cmd.Parameters.AddWithValue(
                                    "@MaPhieu", maPhieu);

                                cmd.Parameters.AddWithValue(
                                    "@MaVatTu", ct.MaVatTu);

                                cmd.Parameters.Add("@SoLuong",
                                    System.Data.SqlDbType.Decimal).Value =
                                    ct.SoLuong;

                                cmd.Parameters.Add("@DonGia",
                                    System.Data.SqlDbType.Decimal).Value =
                                    ct.DonGia;

                                cmd.ExecuteNonQuery();
                            }

                            string sqlCapNhat;

                            if (phieu.LoaiPhieu == "NHAP")
                            {
                                sqlCapNhat = @"
                                    UPDATE dbo.VatTu
                                    SET SoLuongTon = SoLuongTon + @SoLuong
                                    WHERE MaVatTu = @MaVatTu";
                            }
                            else if (phieu.LoaiPhieu == "XUAT")
                            {
                                sqlCapNhat = @"
                                    UPDATE dbo.VatTu
                                    SET SoLuongTon = SoLuongTon - @SoLuong
                                    WHERE MaVatTu = @MaVatTu
                                      AND SoLuongTon >= @SoLuong";
                            }
                            else
                            {
                                throw new Exception("Loại phiếu không hợp lệ.");
                            }

                            using (SqlCommand cmd = new SqlCommand(
                                sqlCapNhat, conn, trans))
                            {
                                cmd.Parameters.Add("@SoLuong",
                                    System.Data.SqlDbType.Decimal).Value =
                                    ct.SoLuong;

                                cmd.Parameters.AddWithValue(
                                    "@MaVatTu", ct.MaVatTu);

                                int rows = cmd.ExecuteNonQuery();

                                if (rows == 0)
                                {
                                    if (phieu.LoaiPhieu == "XUAT")
                                    {
                                        throw new Exception(
                                            "Không đủ hàng trong kho hoặc mã vật tư không tồn tại.");
                                    }

                                    throw new Exception(
                                        "Mã vật tư không tồn tại.");
                                }
                            }
                        }

                        trans.Commit();
                        return maPhieu;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
