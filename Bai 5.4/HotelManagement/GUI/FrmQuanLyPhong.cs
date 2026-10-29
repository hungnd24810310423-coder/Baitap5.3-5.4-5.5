
using System;
using System.Drawing;
using System.Windows.Forms;
using BUS;
using DTO;

namespace GUI
{
    public class FrmQuanLyPhong : Form
    {
        private readonly PhongBUS bus = new PhongBUS();

        private TextBox txtMaPhong = new TextBox();
        private TextBox txtSoPhong = new TextBox();
        private ComboBox cboLoaiPhong = new ComboBox();
        private NumericUpDown nudDonGia = new NumericUpDown();
        private ComboBox cboTrangThai = new ComboBox();
        private DataGridView dgvPhong = new DataGridView();

        private Button btnThem = new Button();
        private Button btnSua = new Button();
        private Button btnXoa = new Button();
        private Button btnLamMoi = new Button();
        private Button btnTaiLai = new Button();

        public FrmQuanLyPhong()
        {
            InitializeUI();
            LoadRooms();
        }

        private void InitializeUI()
        {
            Text = "QUAN LY KHACH SAN - QUAN LY PHONG";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1000, 650);
            MinimumSize = new Size(850, 550);
            Font = new Font("Segoe UI", 10);
            BackColor = Color.WhiteSmoke;

            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ PHÒNG KHÁCH SẠN",
                Dock = DockStyle.Top,
                Height = 55,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 17, FontStyle.Bold),
                ForeColor = Color.DarkBlue
            };
            Controls.Add(lblTitle);

            Panel pnlInput = new Panel
            {
                Dock = DockStyle.Top,
                Height = 190,
                Padding = new Padding(15),
                BackColor = Color.White
            };
            Controls.Add(pnlInput);

            Label lblMa = CreateLabel("Mã phòng:", 15, 15);
            txtMaPhong.SetBounds(130, 12, 180, 28);
            txtMaPhong.ReadOnly = true;
            txtMaPhong.BackColor = Color.Gainsboro;

            Label lblSo = CreateLabel("Số phòng:", 350, 15);
            txtSoPhong.SetBounds(465, 12, 180, 28);

            Label lblLoai = CreateLabel("Loại phòng:", 15, 60);
            cboLoaiPhong.SetBounds(130, 57, 180, 28);
            cboLoaiPhong.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoaiPhong.Items.AddRange(new object[]
            {
                "Thường", "VIP"
            });

            Label lblGia = CreateLabel("Giá theo giờ:", 350, 60);
            nudDonGia.SetBounds(465, 57, 180, 28);
            nudDonGia.Minimum = 0;
            nudDonGia.Maximum = 100000000;
            nudDonGia.ThousandsSeparator = true;
            nudDonGia.DecimalPlaces = 0;
            nudDonGia.Increment = 5000;

            Label lblTrangThai = CreateLabel("Trạng thái:", 15, 105);
            cboTrangThai.SetBounds(130, 102, 180, 28);
            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTrangThai.Items.AddRange(new object[]
            {
                "Trống", "Đang sử dụng", "Bảo trì"
            });

            pnlInput.Controls.AddRange(new Control[]
            {
                lblMa, txtMaPhong,
                lblSo, txtSoPhong,
                lblLoai, cboLoaiPhong,
                lblGia, nudDonGia,
                lblTrangThai, cboTrangThai
            });

            FlowLayoutPanel pnlButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 55,
                Padding = new Padding(10, 5, 10, 5),
                BackColor = Color.WhiteSmoke
            };

            SetupButton(btnThem, "Thêm phòng", 110);
            SetupButton(btnSua, "Sửa phòng", 110);
            SetupButton(btnXoa, "Xóa phòng", 110);
            SetupButton(btnLamMoi, "Làm mới", 110);
            SetupButton(btnTaiLai, "Tải lại", 110);

            pnlButtons.Controls.AddRange(new Control[]
            {
                btnThem, btnSua, btnXoa, btnLamMoi, btnTaiLai
            });

            Controls.Add(pnlButtons);

            dgvPhong.Dock = DockStyle.Fill;
            dgvPhong.AllowUserToAddRows = false;
            dgvPhong.AllowUserToDeleteRows = false;
            dgvPhong.ReadOnly = true;
            dgvPhong.MultiSelect = false;
            dgvPhong.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvPhong.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvPhong.RowHeadersVisible = false;
            dgvPhong.BackgroundColor = Color.White;
            dgvPhong.SelectionChanged += DgvPhong_SelectionChanged;

            Controls.Add(dgvPhong);

            // Đưa lưới xuống dưới các khu vực phía trên
            dgvPhong.BringToFront();
            pnlButtons.BringToFront();
            pnlInput.BringToFront();
            lblTitle.BringToFront();

            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += (s, e) => ClearInputs();
            btnTaiLai.Click += (s, e) => LoadRooms();
        }

        private Label CreateLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Left = x,
                Top = y,
                Width = 110,
                Height = 28,
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        private void SetupButton(Button btn, string text, int width)
        {
            btn.Text = text;
            btn.Width = width;
            btn.Height = 35;
            btn.Margin = new Padding(5);
            btn.FlatStyle = FlatStyle.Standard;
        }

        private void LoadRooms()
        {
            try
            {
                dgvPhong.DataSource = null;
                dgvPhong.DataSource = bus.GetAll();

                if (dgvPhong.Columns["MaPhong"] != null)
                    dgvPhong.Columns["MaPhong"].HeaderText = "Mã phòng";

                if (dgvPhong.Columns["SoPhong"] != null)
                    dgvPhong.Columns["SoPhong"].HeaderText = "Số phòng";

                if (dgvPhong.Columns["LoaiPhong"] != null)
                    dgvPhong.Columns["LoaiPhong"].HeaderText = "Loại phòng";

                if (dgvPhong.Columns["DonGiaTheoGio"] != null)
                {
                    dgvPhong.Columns["DonGiaTheoGio"].HeaderText =
                        "Giá theo giờ";

                    dgvPhong.Columns["DonGiaTheoGio"].DefaultCellStyle
                        .Format = "N0";
                }

                if (dgvPhong.Columns["TrangThai"] != null)
                    dgvPhong.Columns["TrangThai"].HeaderText = "Trạng thái";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh sách phòng.\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private PhongDTO ReadInput()
        {
            return new PhongDTO
            {
                SoPhong = txtSoPhong.Text.Trim(),
                LoaiPhong = cboLoaiPhong.Text,
                DonGiaTheoGio = nudDonGia.Value,
                TrangThai = cboTrangThai.Text
            };
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            try
            {
                PhongDTO p = ReadInput();
                bus.Add(p);

                MessageBox.Show("Thêm phòng thành công!");
                LoadRooms();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtMaPhong.Text, out int maPhong))
                {
                    MessageBox.Show("Vui lòng chọn phòng cần sửa.");
                    return;
                }

                PhongDTO p = ReadInput();
                p.MaPhong = maPhong;

                bus.Update(p);

                MessageBox.Show("Cập nhật phòng thành công!");
                LoadRooms();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtMaPhong.Text, out int maPhong))
                {
                    MessageBox.Show("Vui lòng chọn phòng cần xóa.");
                    return;
                }

                DialogResult result = MessageBox.Show(
                    "Bạn có chắc muốn xóa phòng này?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                bus.Delete(maPhong);

                MessageBox.Show("Xóa phòng thành công!");
                LoadRooms();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DgvPhong_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvPhong.CurrentRow == null)
                return;

            if (dgvPhong.CurrentRow.DataBoundItem is not PhongDTO p)
                return;

            txtMaPhong.Text = p.MaPhong.ToString();
            txtSoPhong.Text = p.SoPhong;
            cboLoaiPhong.SelectedItem = p.LoaiPhong;
            nudDonGia.Value = Math.Min(
                nudDonGia.Maximum,
                Math.Max(nudDonGia.Minimum, p.DonGiaTheoGio));
            cboTrangThai.SelectedItem = p.TrangThai;
        }

        private void ClearInputs()
        {
            txtMaPhong.Clear();
            txtSoPhong.Clear();
            cboLoaiPhong.SelectedIndex = -1;
            nudDonGia.Value = 0;
            cboTrangThai.SelectedIndex = -1;
            dgvPhong.ClearSelection();
            txtSoPhong.Focus();
        }
    }
}
