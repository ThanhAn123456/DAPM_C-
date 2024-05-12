using System;
using System.Collections.Generic;

namespace DAPM_C_.Models;

public partial class TaiKhoan
{
    public int MaTk { get; set; }

    public int? MaQuyen { get; set; }

    public int? MaCuaHang { get; set; }

    public string? HoTen { get; set; }

    public string? Cccd { get; set; }

    public string? HinhAnh { get; set; }

    public string? Sdt { get; set; }

    public DateTime? NgaySinh { get; set; }

    public string? Gioitinh { get; set; }

    public string? DiaChi { get; set; }

    public string? Email { get; set; }

    public string? Matkhau { get; set; }

    public virtual CuaHang? MaCuaHangNavigation { get; set; }

    public virtual PhanQuyen? MaQuyenNavigation { get; set; }
}
