
using System;
using System.Collections.Generic;
using DTO;
using DAL;

namespace BUS
{
    public class VatTuBUS
    {
        private readonly VatTuDAL dal = new VatTuDAL();

        public List<VatTuDTO> GetAll()
        {
            return dal.GetAll();
        }

        public List<VatTuDTO> Search(string keyword)
        {
            return dal.Search(keyword?.Trim() ?? "");
        }

        public void Add(VatTuDTO v)
        {
            Validate(v);

            if (v.SoLuongTon < 0)
                throw new Exception("Tồn kho không được âm.");

            dal.Insert(v);
        }

        public void Update(VatTuDTO v)
        {
            Validate(v);
            dal.Update(v);
        }

        public void Delete(string maVatTu)
        {
            if (string.IsNullOrWhiteSpace(maVatTu))
                throw new Exception("Vui lòng chọn mã vật tư.");

            dal.Delete(maVatTu.Trim());
        }

        public List<VatTuDTO> GetLowStock(decimal threshold)
        {
            if (threshold < 0)
                throw new Exception("Ngưỡng tồn kho không được âm.");

            return dal.GetLowStock(threshold);
        }

        public decimal GetInventoryValue()
        {
            return dal.GetInventoryValue();
        }

        private void Validate(VatTuDTO v)
        {
            if (string.IsNullOrWhiteSpace(v.MaVatTu))
                throw new Exception("Vui lòng nhập mã vật tư.");

            if (v.MaVatTu.Trim().Length > 20)
                throw new Exception("Mã vật tư tối đa 20 ký tự.");

            if (string.IsNullOrWhiteSpace(v.TenVatTu))
                throw new Exception("Vui lòng nhập tên vật tư.");

            if (v.TenVatTu.Trim().Length > 150)
                throw new Exception("Tên vật tư tối đa 150 ký tự.");
        }
    }
}
