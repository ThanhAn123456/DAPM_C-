using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAPM_C_.Models;

public partial class ChiTietDeXuat
{
    public int MaDeXuat { get; set; }

    public int MaChiTietSanPham { get; set; }

    public string? LyDoDeXuat { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập số lượng đề xuất!")]
    public int? SoLuongDeXuat { get; set; }

    public string? TrangThaiDeXuat { get; set; }

    public int? SoLuongDuyet { get; set; }

    public virtual ChiTietSanPham MaChiTietSanPhamNavigation { get; set; } = null!;

    public virtual DeXuat MaDeXuatNavigation { get; set; } = null!;
}
