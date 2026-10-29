
-- =====================================================
-- BUOC 4: TAO CSDL QUAN LY KHACH SAN
-- =====================================================

IF DB_ID(N'HotelDB') IS NULL
BEGIN
    CREATE DATABASE HotelDB;
END;
GO

USE HotelDB;
GO

-- =====================================================
-- 1. XOA CAC BANG CU NEU DA TON TAI
-- Xoa bang con truoc, bang cha sau
-- =====================================================

IF OBJECT_ID(N'dbo.SuDungDichVu', N'U') IS NOT NULL
    DROP TABLE dbo.SuDungDichVu;
GO

IF OBJECT_ID(N'dbo.PhieuDatPhong', N'U') IS NOT NULL
    DROP TABLE dbo.PhieuDatPhong;
GO

IF OBJECT_ID(N'dbo.DichVu', N'U') IS NOT NULL
    DROP TABLE dbo.DichVu;
GO

IF OBJECT_ID(N'dbo.KhachHang', N'U') IS NOT NULL
    DROP TABLE dbo.KhachHang;
GO

IF OBJECT_ID(N'dbo.Phong', N'U') IS NOT NULL
    DROP TABLE dbo.Phong;
GO

-- =====================================================
-- 2. TAO BANG PHONG
-- =====================================================

CREATE TABLE dbo.Phong
(
    MaPhong INT IDENTITY(1,1) NOT NULL,
    SoPhong NVARCHAR(10) NOT NULL,
    LoaiPhong NVARCHAR(50) NOT NULL,
    DonGiaTheoGio DECIMAL(18,2) NOT NULL,
    TrangThai NVARCHAR(30) NOT NULL,

    CONSTRAINT PK_Phong PRIMARY KEY (MaPhong),
    CONSTRAINT UQ_Phong_SoPhong UNIQUE (SoPhong),
    CONSTRAINT CK_Phong_DonGia CHECK (DonGiaTheoGio >= 0)
);
GO

-- =====================================================
-- 3. TAO BANG KHACH HANG
-- =====================================================

CREATE TABLE dbo.KhachHang
(
    MaKH INT IDENTITY(1,1) NOT NULL,
    HoTen NVARCHAR(100) NOT NULL,
    CCCD NVARCHAR(20) NULL,
    SoDienThoai NVARCHAR(20) NULL,

    CONSTRAINT PK_KhachHang PRIMARY KEY (MaKH)
);
GO

-- =====================================================
-- 4. TAO BANG DICH VU
-- =====================================================

CREATE TABLE dbo.DichVu
(
    MaDV INT IDENTITY(1,1) NOT NULL,
    TenDV NVARCHAR(100) NOT NULL,
    DonGia DECIMAL(18,2) NOT NULL,

    CONSTRAINT PK_DichVu PRIMARY KEY (MaDV),
    CONSTRAINT CK_DichVu_DonGia CHECK (DonGia >= 0)
);
GO

-- =====================================================
-- 5. TAO BANG PHIEU DAT PHONG
-- =====================================================

CREATE TABLE dbo.PhieuDatPhong
(
    MaPhieu INT IDENTITY(1,1) NOT NULL,
    MaKH INT NOT NULL,
    MaPhong INT NOT NULL,
    NgayNhanPhong DATETIME NOT NULL,
    NgayTraPhong DATETIME NULL,
    TongTienThanhToan DECIMAL(18,2) NOT NULL
        CONSTRAINT DF_Phieu_TongTien DEFAULT (0),

    CONSTRAINT PK_PhieuDatPhong PRIMARY KEY (MaPhieu),

    CONSTRAINT FK_Phieu_KhachHang
        FOREIGN KEY (MaKH)
        REFERENCES dbo.KhachHang(MaKH),

    CONSTRAINT FK_Phieu_Phong
        FOREIGN KEY (MaPhong)
        REFERENCES dbo.Phong(MaPhong),

    CONSTRAINT CK_Phieu_TongTien
        CHECK (TongTienThanhToan >= 0),

    CONSTRAINT CK_Phieu_Ngay
        CHECK (
            NgayTraPhong IS NULL
            OR NgayTraPhong >= NgayNhanPhong
        )
);
GO

-- =====================================================
-- 6. TAO BANG SU DUNG DICH VU
-- =====================================================

CREATE TABLE dbo.SuDungDichVu
(
    MaSuDung INT IDENTITY(1,1) NOT NULL,
    MaPhieu INT NOT NULL,
    MaDV INT NOT NULL,
    SoLuong INT NOT NULL,
    DonGia DECIMAL(18,2) NOT NULL,
    ThoiGian DATETIME NOT NULL
        CONSTRAINT DF_SuDungDichVu_ThoiGian
        DEFAULT (GETDATE()),

    CONSTRAINT PK_SuDungDichVu PRIMARY KEY (MaSuDung),

    CONSTRAINT FK_SuDungDichVu_Phieu
        FOREIGN KEY (MaPhieu)
        REFERENCES dbo.PhieuDatPhong(MaPhieu),

    CONSTRAINT FK_SuDungDichVu_DichVu
        FOREIGN KEY (MaDV)
        REFERENCES dbo.DichVu(MaDV),

    CONSTRAINT CK_SuDungDichVu_SoLuong
        CHECK (SoLuong > 0),

    CONSTRAINT CK_SuDungDichVu_DonGia
        CHECK (DonGia >= 0)
);
GO

-- =====================================================
-- 7. THEM DU LIEU MAU CHO PHONG
-- =====================================================

INSERT INTO dbo.Phong
    (SoPhong, LoaiPhong, DonGiaTheoGio, TrangThai)
VALUES
    (N'101', N'Thường', 50000, N'Trống'),
    (N'102', N'Thường', 50000, N'Trống'),
    (N'201', N'VIP', 100000, N'Trống'),
    (N'202', N'VIP', 120000, N'Trống');
GO

-- =====================================================
-- 8. THEM DU LIEU MAU CHO KHACH HANG
-- =====================================================

INSERT INTO dbo.KhachHang
    (HoTen, CCCD, SoDienThoai)
VALUES
    (N'Nguyễn Văn An', N'001000000001', N'0901234567'),
    (N'Trần Thị Bình', N'001000000002', N'0912345678');
GO

-- =====================================================
-- 9. THEM DU LIEU MAU CHO DICH VU
-- =====================================================

INSERT INTO dbo.DichVu (TenDV, DonGia)
VALUES
    (N'Nước suối', 10000),
    (N'Nước ngọt', 15000),
    (N'Mì ly', 20000),
    (N'Giặt ủi', 30000);
GO

-- =====================================================
-- 10. KIEM TRA KET QUA
-- =====================================================

SELECT * FROM dbo.Phong;
SELECT * FROM dbo.KhachHang;
SELECT * FROM dbo.DichVu;
SELECT * FROM dbo.PhieuDatPhong;
SELECT * FROM dbo.SuDungDichVu;
GO
