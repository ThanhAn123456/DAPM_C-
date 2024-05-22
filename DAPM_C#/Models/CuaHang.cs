using System;
using System.Collections.Generic;

namespace DAPM_C_.Models;

public partial class CuaHang
{
    public int MaCuaHang { get; set; }

    public string? TenCuahang { get; set; }

    public string? DiaChi { get; set; }

    public virtual ICollection<DeXuat> DeXuats { get; set; } = new List<DeXuat>();

    public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
}
