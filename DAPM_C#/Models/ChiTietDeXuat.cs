using System;
using System.Collections.Generic;

namespace DAPM_C_.Models;

public partial class ChiTietDeXuat
{
    public int MaDeXuat { get; set; }

    public int? MaCuaHang { get; set; }

    public int MaSanPham { get; set; }

    public int MaChiTietSanPham { get; set; }

    public string? LyDoDeXuat { get; set; }

    public int? SoLuongDeXuat { get; set; }

    public string? TrangThaiDeXuat { get; set; }

    public int? SoLuongDuyet { get; set; }

    public string? TrangThaiVanChuyen { get; set; }

    public string? XacNhanNhanHang { get; set; }

    public virtual ChiTietSanPham ChiTietSanPham { get; set; } = null!;

    public virtual CuaHang? MaCuaHangNavigation { get; set; }

    public virtual DeXuat MaDeXuatNavigation { get; set; } = null!;
}
