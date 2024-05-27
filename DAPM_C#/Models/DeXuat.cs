using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace DAPM_C_.Models;

public partial class DeXuat
{
    [Required(ErrorMessage = "Mã đề xuất rỗng!")]
    public int MaDeXuat { get; set; }

    [Required(ErrorMessage = "Tiêu đề là bắt buộc!")]
    public string? Tieude { get; set; }

    public DateTime? NgayDeXuat { get; set; }

    public string? TrangThai { get; set; }

    public int? MaCuaHang { get; set; }

    public virtual ICollection<ChiTietDeXuat> ChiTietDeXuats { get; set; } = new List<ChiTietDeXuat>();

    public virtual CuaHang? MaCuaHangNavigation { get; set; }
}
