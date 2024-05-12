using System;
using System.Collections.Generic;

namespace DAPM_C_.Models;

public partial class ChiTietPhieuNhap
{
    public int MaPhieuNhap { get; set; }

    public int MaSanPham { get; set; }

    public int MaChiTietSanPham { get; set; }

    public int? SoLuongNhap { get; set; }

    public int? GiaNhap { get; set; }

    public virtual ChiTietSanPham ChiTietSanPham { get; set; } = null!;

    public virtual PhieuNhap MaPhieuNhapNavigation { get; set; } = null!;
}
