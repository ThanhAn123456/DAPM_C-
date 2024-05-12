using System;
using System.Collections.Generic;

namespace DAPM_C_.Models;

public partial class LoaiSanPham
{
    public int MaLoaiSanPham { get; set; }

    public string? TenLoaiSanPham { get; set; }

    public virtual ICollection<ChiTietSanPham> ChiTietSanPhams { get; set; } = new List<ChiTietSanPham>();
}
