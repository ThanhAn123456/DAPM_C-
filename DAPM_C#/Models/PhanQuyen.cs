using System;
using System.Collections.Generic;

namespace DAPM_C_.Models;

public partial class PhanQuyen
{
    public int MaQuyen { get; set; }

    public string? TenQuyen { get; set; }

    public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
}
