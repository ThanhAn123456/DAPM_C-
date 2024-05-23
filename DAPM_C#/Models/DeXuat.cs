using System;
using System.Collections.Generic;

namespace DAPM_C_.Models;

public partial class DeXuat
{
    public int MaDeXuat { get; set; }

    public string? Tieude { get; set; }

    public DateTime? NgayDeXuat { get; set; }

    public string? TrangThai { get; set; }

    public int? MaCuaHang { get; set; }

    public virtual ICollection<ChiTietDeXuat> ChiTietDeXuats { get; set; } = new List<ChiTietDeXuat>();

    public virtual CuaHang? MaCuaHangNavigation { get; set; }
}
