using System;
using System.Collections.Generic;

namespace DAPM_C_.Models;

public partial class ChiTietSanPham
{
    public int MaChiTietSanPham { get; set; }

    public int? SoLuong { get; set; }

    public string? HinhAnh { get; set; }

    public string? Nsx { get; set; }

    public string? ChatLieu { get; set; }

    public int? GiaTien { get; set; }

    public int? MaSanPham { get; set; }

    public int? MaLoaiSanPham { get; set; }

    public int? MaMau { get; set; }

    public int? MaSize { get; set; }

    public int? MaDoiTuong { get; set; }

    public virtual ICollection<ChiTietDeXuat> ChiTietDeXuats { get; set; } = new List<ChiTietDeXuat>();

    public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();

    public virtual DoiTuong? MaDoiTuongNavigation { get; set; }

    public virtual LoaiSanPham? MaLoaiSanPhamNavigation { get; set; }

    public virtual Mau? MaMauNavigation { get; set; }

    public virtual Sanpham? MaSanPhamNavigation { get; set; }

    public virtual Size? MaSizeNavigation { get; set; }
}
