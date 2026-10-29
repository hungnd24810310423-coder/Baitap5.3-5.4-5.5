
using System;
using System.Drawing;
using System.Windows.Forms;
using BUS;
using DTO;

namespace GUI
{
    public class FrmVatTu : Form
    {
        private readonly VatTuBUS bus = new VatTuBUS();

        private TextBox txtMa = new TextBox();
        private TextBox txtTen = new TextBox();
        private TextBox txtDVT = new TextBox();
        private TextBox txtTimKiem = new TextBox();

        private NumericUpDown nudTon = new NumericUpDown();
        private NumericUpDown nudNguong = new NumericUpDown();

        private DataGridView dgvVatTu = new DataGridView();

        private Button btnThem = new Button();
        private Button btnSua = new Button();
        private Button btnXoa = new Button();
        private Button btnLamMoi = new Button();
        private Button btnTimKiem = new Button();
        private Button btnTatCa = new Button();
        private Button btnTonThap = new Button();
        private Button btnTinhGiaTri = new Button();

        private Label lblGiaTri = new Label();

        public FrmVatTu()
        {
            TaoGiaoDien();
            TaiDanhSach();
        }

        private void TaoGiaoDien()
        {
            Text = "QUẢN LÝ VẬT TƯ";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1050, 650);
            MinimumSize = new Size(900, 550);
            Font = new Font("Segoe UI", 10);

            Label lblTieuDe = new Label
            {
                Text = "QUẢN LÝ VẬT TƯ",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 55
            };

            Controls.Add(lblTieuDe);

            // THÔNG TIN VẬT TƯ
            GroupBox grpThongTin = new GroupBox
            {
                Text = "Thông tin vật tư",
                Location = new Point(15, 65),
                Size = new Size(490, 230),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };

            Controls.Add(grpThongTin);

            TaoLabel(grpThongTin, "Mã vật tư:", 20, 35);
            TaoLabel(grpThongTin, "Tên vật tư:", 20, 75);
            TaoLabel(grpThongTin, "Đơn vị tính:", 20, 115);
            TaoLabel(grpThongTin, "Số lượng tồn:", 20, 155);

            // Cho phép nhập mã vật tư
            txtMa.Location = new Point(145, 32);
            txtMa.Size = new Size(300, 28);
            txtMa.ReadOnly = false;
            txtMa.Enabled = true;
            txtMa.BackColor = Color.White;
            txtMa.TabStop = true;
            grpThongTin.Controls.Add(txtMa);

            txtTen.Location = new Point(145, 72);
            txtTen.Size = new Size(300, 28);
            grpThongTin.Controls.Add(txtTen);

            txtDVT.Location = new Point(145, 112);
            txtDVT.Size = new Size(300, 28);
            grpThongTin.Controls.Add(txtDVT);

            nudTon.Location = new Point(145, 152);
            nudTon.Size = new Size(150, 28);
            nudTon.DecimalPlaces = 2;
            nudTon.Minimum = 0;
            nudTon.Maximum = 1000000000;
            grpThongTin.Controls.Add(nudTon);

            btnThem = TaoButton("Thêm", 20, 190, 90);
            btnSua = TaoButton("Sửa", 120, 190, 90);
            btnXoa = TaoButton("Xóa", 220, 190, 90);
            btnLamMoi = TaoButton("Làm mới", 320, 190, 110);

            grpThongTin.Controls.AddRange(new Control[]
            {
                btnThem, btnSua, btnXoa, btnLamMoi
            });

            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += BtnLamMoi_Click;

            // TÌM KIẾM VÀ THỐNG KÊ
            GroupBox grpTimKiem = new GroupBox
            {
                Text = "Tìm kiếm và thống kê",
                Location = new Point(520, 65),
                Size = new Size(495, 230),
                Anchor = AnchorStyles.Top | AnchorStyles.Left |
                         AnchorStyles.Right
            };

            Controls.Add(grpTimKiem);

            TaoLabel(grpTimKiem, "Từ khóa:", 20, 35);

            txtTimKiem.Location = new Point(110, 32);
            txtTimKiem.Size = new Size(350, 28);
            grpTimKiem.Controls.Add(txtTimKiem);

            btnTimKiem = TaoButton("Tìm kiếm", 20, 75, 110);
            btnTatCa = TaoButton("Tất cả", 140, 75, 100);
            btnTonThap = TaoButton("Tồn kho thấp", 250, 75, 150);

            grpTimKiem.Controls.AddRange(new Control[]
            {
                btnTimKiem, btnTatCa, btnTonThap
            });

            btnTimKiem.Click += BtnTimKiem_Click;
            btnTatCa.Click += (s, e) => TaiDanhSach();
            btnTonThap.Click += BtnTonThap_Click;

            TaoLabel(grpTimKiem, "Ngưỡng tồn:", 20, 125);

            nudNguong.Location = new Point(130, 122);
            nudNguong.Size = new Size(120, 28);
            nudNguong.DecimalPlaces = 2;
            nudNguong.Minimum = 0;
            nudNguong.Maximum = 1000000000;
            nudNguong.Value = 10;
            grpTimKiem.Controls.Add(nudNguong);

            btnTinhGiaTri = TaoButton(
                "Tính giá trị tồn kho", 20, 165, 200);

            grpTimKiem.Controls.Add(btnTinhGiaTri);
            btnTinhGiaTri.Click += BtnTinhGiaTri_Click;

            lblGiaTri.Location = new Point(230, 165);
            lblGiaTri.Size = new Size(245, 35);
            lblGiaTri.Text = "Giá trị tồn: chưa tính";
            lblGiaTri.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            grpTimKiem.Controls.Add(lblGiaTri);

            // DANH SÁCH VẬT TƯ
            dgvVatTu.Location = new Point(15, 310);
            dgvVatTu.Size = new Size(1000, 290);

            dgvVatTu.Anchor = AnchorStyles.Top | AnchorStyles.Bottom |
                              AnchorStyles.Left | AnchorStyles.Right;

            dgvVatTu.ReadOnly = true;
            dgvVatTu.AllowUserToAddRows = false;
            dgvVatTu.AllowUserToDeleteRows = false;
            dgvVatTu.MultiSelect = false;
            dgvVatTu.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvVatTu.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvVatTu.RowHeadersVisible = false;
            dgvVatTu.BackgroundColor = Color.White;

            dgvVatTu.SelectionChanged += DgvVatTu_SelectionChanged;

            Controls.Add(dgvVatTu);
        }

        private void TaoLabel(
            Control parent, string noiDung, int x, int y)
        {
            Label lbl = new Label
            {
                Text = noiDung,
                Location = new Point(x, y),
                Size = new Size(120, 28),
                TextAlign = ContentAlignment.MiddleLeft
            };

            parent.Controls.Add(lbl);
        }

        private Button TaoButton(
            string noiDung, int x, int y, int width)
        {
            return new Button
            {
                Text = noiDung,
                Location = new Point(x, y),
                Size = new Size(width, 30),
                Cursor = Cursors.Hand
            };
        }

        // TẢI DANH SÁCH
        private void TaiDanhSach()
        {
            try
            {
                dgvVatTu.DataSource = null;
                dgvVatTu.DataSource = bus.GetAll();

                DatTieuDeCot("MaVatTu", "Mã vật tư");
                DatTieuDeCot("TenVatTu", "Tên vật tư");
                DatTieuDeCot("DVT", "Đơn vị tính");
                DatTieuDeCot("SoLuongTon", "Số lượng tồn");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh sách vật tư.\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void DatTieuDeCot(string tenCot, string tieuDe)
        {
            if (dgvVatTu.Columns.Contains(tenCot))
            {
                dgvVatTu.Columns[tenCot].HeaderText = tieuDe;
            }
        }

        // LẤY DỮ LIỆU TỪ FORM
        private VatTuDTO LayThongTin()
        {
            return new VatTuDTO
            {
                MaVatTu = txtMa.Text.Trim(),
                TenVatTu = txtTen.Text.Trim(),
                DVT = txtDVT.Text.Trim(),
                SoLuongTon = nudTon.Value
            };
        }

        // THÊM VẬT TƯ
        private void BtnThem_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMa.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã vật tư.");
                    txtMa.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên vật tư.");
                    txtTen.Focus();
                    return;
                }

                if (txtMa.Text.Trim().Length > 20)
                {
                    MessageBox.Show("Mã vật tư không được vượt quá 20 ký tự.");
                    txtMa.Focus();
                    return;
                }

                bus.Add(LayThongTin());

                MessageBox.Show("Thêm vật tư thành công!");

                TaiDanhSach();
                XoaTrang();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Thêm vật tư thất bại.\n" + ex.Message,
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        // CHỌN DÒNG TRONG BẢNG
        private void DgvVatTu_SelectionChanged(
            object? sender, EventArgs e)
        {
            if (dgvVatTu.CurrentRow == null)
                return;

            if (dgvVatTu.CurrentRow.DataBoundItem is VatTuDTO v)
            {
                txtMa.Text = v.MaVatTu;
                txtTen.Text = v.TenVatTu;
                txtDVT.Text = v.DVT;

                nudTon.Value = Math.Max(
                    nudTon.Minimum,
                    Math.Min(nudTon.Maximum, v.SoLuongTon));
            }
        }

        // SỬA VẬT TƯ
        private void BtnSua_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMa.Text))
                {
                    MessageBox.Show("Vui lòng chọn vật tư cần sửa.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên vật tư.");
                    return;
                }

                bus.Update(LayThongTin());

                MessageBox.Show("Cập nhật vật tư thành công!");

                TaiDanhSach();
                XoaTrang();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Cập nhật thất bại.\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // XÓA VẬT TƯ
        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            try
            {
                string ma = txtMa.Text.Trim();

                if (string.IsNullOrWhiteSpace(ma))
                {
                    MessageBox.Show("Vui lòng chọn vật tư cần xóa.");
                    return;
                }

                DialogResult ketQua = MessageBox.Show(
                    "Bạn có chắc muốn xóa vật tư " + ma + "?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (ketQua != DialogResult.Yes)
                    return;

                bus.Delete(ma);

                MessageBox.Show("Xóa vật tư thành công!");

                TaiDanhSach();
                XoaTrang();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể xóa vật tư. Có thể vật tư đã được sử dụng trong phiếu kho.\n"
                    + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        // LÀM MỚI FORM
        private void BtnLamMoi_Click(object? sender, EventArgs e)
        {
            XoaTrang();
        }

        private void XoaTrang()
        {
            dgvVatTu.ClearSelection();

            txtMa.Clear();
            txtTen.Clear();
            txtDVT.Clear();
            nudTon.Value = 0;

            txtMa.ReadOnly = false;
            txtMa.Enabled = true;
            txtMa.Focus();
        }

        // TÌM KIẾM
        private void BtnTimKiem_Click(object? sender, EventArgs e)
        {
            try
            {
                dgvVatTu.DataSource = null;
                dgvVatTu.DataSource = bus.Search(txtTimKiem.Text.Trim());

                DatTieuDeCot("MaVatTu", "Mã vật tư");
                DatTieuDeCot("TenVatTu", "Tên vật tư");
                DatTieuDeCot("DVT", "Đơn vị tính");
                DatTieuDeCot("SoLuongTon", "Số lượng tồn");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tìm kiếm thất bại.\n" + ex.Message);
            }
        }

        // VẬT TƯ TỒN THẤP
        private void BtnTonThap_Click(object? sender, EventArgs e)
        {
            try
            {
                dgvVatTu.DataSource = null;
                dgvVatTu.DataSource = bus.GetLowStock(nudNguong.Value);

                DatTieuDeCot("MaVatTu", "Mã vật tư");
                DatTieuDeCot("TenVatTu", "Tên vật tư");
                DatTieuDeCot("DVT", "Đơn vị tính");
                DatTieuDeCot("SoLuongTon", "Số lượng tồn");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy danh sách tồn thấp.\n" + ex.Message);
            }
        }

        // TÍNH GIÁ TRỊ TỒN KHO
        private void BtnTinhGiaTri_Click(object? sender, EventArgs e)
        {
            try
            {
                decimal tong = bus.GetInventoryValue();
                lblGiaTri.Text = "Giá trị tồn: " + tong.ToString("N0");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tính giá trị tồn kho.\n" + ex.Message);
            }
        }
    }
}
