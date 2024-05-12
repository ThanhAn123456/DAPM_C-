using System;
using System.Collections.Generic;

namespace DAPM_C_.Models;

public partial class Sanpham
{
    public int MaSanPham { get; set; }

    public string? TenSanPham { get; set; }

    public virtual ICollection<ChiTietSanPham> ChiTietSanPhams { get; set; } = new List<ChiTietSanPham>();
}
