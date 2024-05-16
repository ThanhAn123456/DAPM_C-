using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAPM_C_.Models;

public partial class QuanlyphanphoikhoYodyContext : DbContext
{
    public QuanlyphanphoikhoYodyContext()
    {
    }

    public QuanlyphanphoikhoYodyContext(DbContextOptions<QuanlyphanphoikhoYodyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietDeXuat> ChiTietDeXuats { get; set; }

    public virtual DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }

    public virtual DbSet<ChiTietSanPham> ChiTietSanPhams { get; set; }

    public virtual DbSet<CuaHang> CuaHangs { get; set; }

    public virtual DbSet<DeXuat> DeXuats { get; set; }

    public virtual DbSet<DoiTuong> DoiTuongs { get; set; }

    public virtual DbSet<LoaiSanPham> LoaiSanPhams { get; set; }

    public virtual DbSet<Mau> Maus { get; set; }

    public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }

    public virtual DbSet<PhanQuyen> PhanQuyens { get; set; }

    public virtual DbSet<PhieuNhap> PhieuNhaps { get; set; }

    public virtual DbSet<Sanpham> Sanphams { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=AN;Initial Catalog=quanlyphanphoikhoYody;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietDeXuat>(entity =>
        {
            entity.HasKey(e => new { e.MaDeXuat, e.MaChiTietSanPham }).HasName("PK__ChiTietD__384F455EC7A66737");

            entity.ToTable("ChiTietDeXuat", tb => tb.HasTrigger("updateSoLuong_CTDX"));

            entity.Property(e => e.LyDoDeXuat).HasMaxLength(500);
            entity.Property(e => e.TrangThaiDeXuat).HasMaxLength(2);
            entity.Property(e => e.TrangThaiVanChuyen).HasMaxLength(2);
            entity.Property(e => e.XacNhanNhanHang).HasMaxLength(2);

            entity.HasOne(d => d.MaChiTietSanPhamNavigation).WithMany(p => p.ChiTietDeXuats)
                .HasForeignKey(d => d.MaChiTietSanPham)
                .HasConstraintName("FK__ChiTietDe__MaChi__5629CD9C");

            entity.HasOne(d => d.MaCuaHangNavigation).WithMany(p => p.ChiTietDeXuats)
                .HasForeignKey(d => d.MaCuaHang)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ChiTietDe__MaCua__5535A963");

            entity.HasOne(d => d.MaDeXuatNavigation).WithMany(p => p.ChiTietDeXuats)
                .HasForeignKey(d => d.MaDeXuat)
                .HasConstraintName("FK__ChiTietDe__MaDeX__5441852A");
        });

        modelBuilder.Entity<ChiTietPhieuNhap>(entity =>
        {
            entity.HasKey(e => new { e.MaPhieuNhap, e.MaChiTietSanPham }).HasName("PK__ChiTietP__0E1BED0023300EF7");

            entity.ToTable("ChiTietPhieuNhap", tb => tb.HasTrigger("UpdateSoLuong_CTPN"));

            entity.HasOne(d => d.MaChiTietSanPhamNavigation).WithMany(p => p.ChiTietPhieuNhaps)
                .HasForeignKey(d => d.MaChiTietSanPham)
                .HasConstraintName("FK__ChiTietPh__MaChi__4D94879B");

            entity.HasOne(d => d.MaPhieuNhapNavigation).WithMany(p => p.ChiTietPhieuNhaps)
                .HasForeignKey(d => d.MaPhieuNhap)
                .HasConstraintName("FK__ChiTietPh__MaPhi__4CA06362");
        });

        modelBuilder.Entity<ChiTietSanPham>(entity =>
        {
            entity.HasKey(e => e.MaChiTietSanPham).HasName("PK__ChiTietS__A6B023B0D9318A8E");

            entity.ToTable("ChiTietSanPham");

            entity.Property(e => e.ChatLieu).HasMaxLength(50);
            entity.Property(e => e.HinhAnh)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Nsx)
                .HasMaxLength(50)
                .HasColumnName("NSX");

            entity.HasOne(d => d.MaDoiTuongNavigation).WithMany(p => p.ChiTietSanPhams)
                .HasForeignKey(d => d.MaDoiTuong)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ChiTietSa__MaDoi__44FF419A");

            entity.HasOne(d => d.MaLoaiSanPhamNavigation).WithMany(p => p.ChiTietSanPhams)
                .HasForeignKey(d => d.MaLoaiSanPham)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ChiTietSa__MaLoa__4222D4EF");

            entity.HasOne(d => d.MaMauNavigation).WithMany(p => p.ChiTietSanPhams)
                .HasForeignKey(d => d.MaMau)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ChiTietSa__MaMau__4316F928");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.ChiTietSanPhams)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ChiTietSa__MaSan__412EB0B6");

            entity.HasOne(d => d.MaSizeNavigation).WithMany(p => p.ChiTietSanPhams)
                .HasForeignKey(d => d.MaSize)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ChiTietSa__MaSiz__440B1D61");
        });

        modelBuilder.Entity<CuaHang>(entity =>
        {
            entity.HasKey(e => e.MaCuaHang).HasName("PK__CuaHang__0840BCA65AC92621");

            entity.ToTable("CuaHang");

            entity.Property(e => e.DiaChi).HasMaxLength(100);
            entity.Property(e => e.TenCuahang).HasMaxLength(50);
        });

        modelBuilder.Entity<DeXuat>(entity =>
        {
            entity.HasKey(e => e.MaDeXuat).HasName("PK__DeXuat__222447655547F89C");

            entity.ToTable("DeXuat");

            entity.Property(e => e.NgayDeXuat).HasColumnType("datetime");
            entity.Property(e => e.Tieude).HasMaxLength(100);
        });

        modelBuilder.Entity<DoiTuong>(entity =>
        {
            entity.HasKey(e => e.MaDoiTuong).HasName("PK__DoiTuong__291408A13E6E47EB");

            entity.ToTable("DoiTuong");

            entity.Property(e => e.TenDoiTuong).HasMaxLength(50);
        });

        modelBuilder.Entity<LoaiSanPham>(entity =>
        {
            entity.HasKey(e => e.MaLoaiSanPham).HasName("PK__LoaiSanP__ECCF699F4ED854E1");

            entity.ToTable("LoaiSanPham");

            entity.Property(e => e.TenLoaiSanPham).HasMaxLength(50);
        });

        modelBuilder.Entity<Mau>(entity =>
        {
            entity.HasKey(e => e.MaMau).HasName("PK__Mau__3A5BBB7D7AC2931C");

            entity.ToTable("Mau");

            entity.Property(e => e.TenMau).HasMaxLength(50);
        });

        modelBuilder.Entity<NhaCungCap>(entity =>
        {
            entity.HasKey(e => e.MaNcc).HasName("PK__NhaCungC__3A185DEB550812FA");

            entity.ToTable("NhaCungCap");

            entity.Property(e => e.MaNcc).HasColumnName("MaNCC");
            entity.Property(e => e.DiaChi).HasMaxLength(50);
            entity.Property(e => e.NhanVienLienHe).HasMaxLength(50);
            entity.Property(e => e.Sdt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SDT");
            entity.Property(e => e.TenNcc)
                .HasMaxLength(50)
                .HasColumnName("TenNCC");
        });

        modelBuilder.Entity<PhanQuyen>(entity =>
        {
            entity.HasKey(e => e.MaQuyen).HasName("PK__PhanQuye__1D4B7ED4B688B955");

            entity.ToTable("PhanQuyen");

            entity.Property(e => e.TenQuyen).HasMaxLength(50);
        });

        modelBuilder.Entity<PhieuNhap>(entity =>
        {
            entity.HasKey(e => e.MaPhieuNhap).HasName("PK__PhieuNha__1470EF3BD5D61041");

            entity.ToTable("PhieuNhap");

            entity.Property(e => e.MaNcc).HasColumnName("MaNCC");
            entity.Property(e => e.NgayNhapHang).HasColumnType("datetime");

            entity.HasOne(d => d.MaNccNavigation).WithMany(p => p.PhieuNhaps)
                .HasForeignKey(d => d.MaNcc)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__PhieuNhap__MaNCC__49C3F6B7");
        });

        modelBuilder.Entity<Sanpham>(entity =>
        {
            entity.HasKey(e => e.MaSanPham).HasName("PK__Sanpham__FAC7442DA9868400");

            entity.ToTable("Sanpham");

            entity.Property(e => e.TenSanPham).HasMaxLength(50);
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.MaSize).HasName("PK__Size__A787E7EDD0F70EFE");

            entity.ToTable("Size");

            entity.Property(e => e.TenSize)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.MaTk).HasName("PK__TaiKhoan__27250070D5E73623");

            entity.ToTable("TaiKhoan");

            entity.Property(e => e.MaTk).HasColumnName("MaTK");
            entity.Property(e => e.Cccd)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CCCD");
            entity.Property(e => e.DiaChi).HasMaxLength(100);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Gioitinh).HasMaxLength(10);
            entity.Property(e => e.HinhAnh)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.Matkhau)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("matkhau");
            entity.Property(e => e.NgaySinh).HasColumnType("datetime");
            entity.Property(e => e.Sdt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SDT");

            entity.HasOne(d => d.MaCuaHangNavigation).WithMany(p => p.TaiKhoans)
                .HasForeignKey(d => d.MaCuaHang)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__TaiKhoan__MaCuaH__5BE2A6F2");

            entity.HasOne(d => d.MaQuyenNavigation).WithMany(p => p.TaiKhoans)
                .HasForeignKey(d => d.MaQuyen)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__TaiKhoan__MaQuye__5AEE82B9");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
