using System;
using System.Collections.Generic;

namespace DAPM_C_.Models;

public partial class DoiTuong
{
    public int MaDoiTuong { get; set; }

    public string? TenDoiTuong { get; set; }

    public virtual ICollection<ChiTietSanPham> ChiTietSanPhams { get; set; } = new List<ChiTietSanPham>();
}
