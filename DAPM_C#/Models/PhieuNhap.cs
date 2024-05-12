using System;
using System.Collections.Generic;

namespace DAPM_C_.Models;

public partial class PhieuNhap
{
    public int MaPhieuNhap { get; set; }

    public int? MaNcc { get; set; }

    public DateTime? NgayNhapHang { get; set; }

    public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();

    public virtual NhaCungCap? MaNccNavigation { get; set; }
}
