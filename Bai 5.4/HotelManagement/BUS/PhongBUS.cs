
using System;
using System.Collections.Generic;
using DTO;
using DAL;

namespace BUS
{
    public class PhongBUS
    {
        private readonly PhongDAL dal = new PhongDAL();

        public List<PhongDTO> GetAll()
        {
            return dal.GetAll();
        }

        public void Add(PhongDTO p)
        {
            Validate(p);

            try
            {
                dal.Insert(p);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                    throw new Exception("Số phòng đã tồn tại.");

                throw;
            }
        }

        public void Update(PhongDTO p)
        {
            if (p.MaPhong <= 0)
                throw new Exception("Mã phòng không hợp lệ.");

            Validate(p);

            try
            {
                if (dal.Update(p) == 0)
                    throw new Exception("Không tìm thấy phòng cần sửa.");
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                    throw new Exception("Số phòng đã tồn tại.");

                throw;
            }
        }

        public void Delete(int maPhong)
        {
            if (maPhong <= 0)
                throw new Exception("Vui lòng chọn phòng cần xóa.");

            try
            {
                if (dal.Delete(maPhong) == 0)
                    throw new Exception("Không tìm thấy phòng cần xóa.");
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    throw new Exception(
                        "Không thể xóa phòng vì phòng đã có phiếu đặt phòng.");
                }

                throw;
            }
        }

        private void Validate(PhongDTO p)
        {
            if (string.IsNullOrWhiteSpace(p.SoPhong))
                throw new Exception("Vui lòng nhập số phòng.");

            if (string.IsNullOrWhiteSpace(p.LoaiPhong))
                throw new Exception("Vui lòng chọn loại phòng.");

            if (p.DonGiaTheoGio < 0)
                throw new Exception("Đơn giá không được âm.");

            if (string.IsNullOrWhiteSpace(p.TrangThai))
                throw new Exception("Vui lòng chọn trạng thái phòng.");
        }
    }
}
