CREATE DATABASE QuanLyNhaHang;
GO

USE QuanLyNhaHang;
GO
CREATE TABLE NguoiDung (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    TenDangNhap NVARCHAR(50) NOT NULL UNIQUE,
    MatKhau NVARCHAR(255) NOT NULL,
    HoTen NVARCHAR(100),
    VaiTro NVARCHAR(20) CHECK (VaiTro IN (N'Admin',N'Client')) NOT NULL
);
CREATE TABLE BanAn (
    BanID INT IDENTITY(1,1) PRIMARY KEY,
    TenBan NVARCHAR(50) NOT NULL,
    TrangThai NVARCHAR(20) CHECK (TrangThai IN (N'Trống', N'Đang dùng', N'Đặt trước')) NOT NULL DEFAULT N'Trống'
);
CREATE TABLE ThucDon (
    MonID INT IDENTITY(1,1) PRIMARY KEY,
    TenMon NVARCHAR(100) NOT NULL,
    DonGia DECIMAL(18,2) NOT NULL,
    DonViTinh NVARCHAR(20) DEFAULT N'Phần',
    TrangThai BIT DEFAULT 1  -- 1: còn bán, 0: ngừng bán
);
CREATE TABLE DatBan (
    DatBanID INT IDENTITY(1,1) PRIMARY KEY,
    BanID INT FOREIGN KEY REFERENCES BanAn(BanID),
    UserID INT FOREIGN KEY REFERENCES NguoiDung(UserID),
    NgayDat DATETIME DEFAULT GETDATE(),
    TrangThai NVARCHAR(20) CHECK (TrangThai IN (N'Chờ duyệt', N'Đã duyệt',N'Đã hủy')) NOT NULL DEFAULT N'Chờ duyệt'
);
CREATE TABLE HoaDon (
    HoaDonID INT IDENTITY(1,1) PRIMARY KEY,
    BanID INT FOREIGN KEY REFERENCES BanAn(BanID),
    UserID INT FOREIGN KEY REFERENCES NguoiDung(UserID),
    NgayLap DATETIME DEFAULT GETDATE(),
    TongTien DECIMAL(18,2) DEFAULT 0,
    TrangThai NVARCHAR(20) CHECK (TrangThai IN (N'Chưa thanh toán', N'Đã thanh toán')) NOT NULL DEFAULT N'Chưa thanh toán'
);
CREATE TABLE ChiTietHoaDon (
    CTHD_ID INT IDENTITY(1,1) PRIMARY KEY,
    HoaDonID INT FOREIGN KEY REFERENCES HoaDon(HoaDonID),
    MonID INT FOREIGN KEY REFERENCES ThucDon(MonID),
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    DonGia DECIMAL(18,2) NOT NULL
);
-- Người dùng
INSERT INTO NguoiDung (TenDangNhap, MatKhau, HoTen, VaiTro)
VALUES (N'admin', N'123', N'Quản trị viên', N'Admin'),
       (N'dung', N'123', N'Nguyễn Văn A', N'Client');

-- Bàn ăn
INSERT INTO BanAn (TenBan, TrangThai)
VALUES (N'Bàn 1', N'Trống'),
       (N'Bàn 2', N'Trống'),
       (N'Bàn 3', N'Đặt trước');

-- Thực đơn
INSERT INTO ThucDon (TenMon, DonGia, DonViTinh)
VALUES 
(N'Cơm gà', 45000, N'Phần'),
(N'Phở bò', 50000, N'Tô'),
(N'Nước ngọt', 15000, N'Chai'),
(N'Bánh mì ốp la', 30000, N'Ổ'),
(N'Lẩu thái', 200000, N'Nồi');
 Select* From  ThucDon;
  Select* From  NguoiDung;
    Select* From  BanAn;
DELETE FROM ThucDon;
DBCC CHECKIDENT ('ThucDon', RESEED, 0);
DELETE FROM NguoiDung WHERE UserID = 5;

ALTER TABLE NguoiDung
DROP CONSTRAINT CK_NguoiDung_VaiTro_44BA81F0;
