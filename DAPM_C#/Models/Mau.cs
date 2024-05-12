using System;
using System.Collections.Generic;

namespace DAPM_C_.Models;

public partial class Mau
{
    public int MaMau { get; set; }

    public string? TenMau { get; set; }

    public virtual ICollection<ChiTietSanPham> ChiTietSanPhams { get; set; } = new List<ChiTietSanPham>();
}
