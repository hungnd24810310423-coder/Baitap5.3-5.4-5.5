
using System;
using DTO;
using DAL;

namespace BUS
{
    public class PhieuKhoBUS
    {
        private readonly PhieuKhoDAL dal = new PhieuKhoDAL();

        public int Save(PhieuKhoDTO p)
        {
            if (p == null)
                throw new Exception("Phiếu kho không hợp lệ.");

            if (p.LoaiPhieu != "NHAP" && p.LoaiPhieu != "XUAT")
                throw new Exception("Loại phiếu phải là NHAP hoặc XUAT.");

            if (p.LoaiPhieu == "NHAP" && !p.MaNCC.HasValue)
                throw new Exception("Phiếu nhập phải chọn nhà cung cấp.");

            if (p.LoaiPhieu == "XUAT")
                p.MaNCC = null;

            if (p.ChiTiet == null || p.ChiTiet.Count == 0)
                throw new Exception("Vui lòng thêm vật tư vào phiếu.");

            foreach (ChiTietPhieuKhoDTO ct in p.ChiTiet)
            {
                if (string.IsNullOrWhiteSpace(ct.MaVatTu))
                    throw new Exception("Có chi tiết chưa chọn vật tư.");

                if (ct.SoLuong <= 0)
                    throw new Exception("Số lượng phải lớn hơn 0.");

                if (ct.DonGia < 0)
                    throw new Exception("Đơn giá không được âm.");
            }

            return dal.Save(p);
        }
    }
}
