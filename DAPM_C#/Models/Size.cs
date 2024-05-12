using System;
using System.Collections.Generic;

namespace DAPM_C_.Models;

public partial class Size
{
    public int MaSize { get; set; }

    public string? TenSize { get; set; }

    public virtual ICollection<ChiTietSanPham> ChiTietSanPhams { get; set; } = new List<ChiTietSanPham>();
}
