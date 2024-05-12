using System;
using System.Collections.Generic;

namespace DAPM_C_.Models;

public partial class DeXuat
{
    public int MaDeXuat { get; set; }

    public string? Tieude { get; set; }

    public DateTime? NgayDeXuat { get; set; }

    public virtual ICollection<ChiTietDeXuat> ChiTietDeXuats { get; set; } = new List<ChiTietDeXuat>();
}
