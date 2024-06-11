# BTCK_LTC#

## Mục lục

- [Giới thiệu](#giới-thiệu)
- [Chức năng chính](#chức-năng-chính)
- [Yêu cầu hệ thống](#yêu-cầu-hệ-thống)
- [Cài đặt](#cài-đặt)
- [Cấu hình cơ sở dữ liệu](#cấu-hình-cơ-sở-dữ-liệu)
- [Tính năng](#tính-năng)
- [Cấu trúc dự án](#cấu-trúc-dự-án)

## Giới thiệu

Người hướng dẫn	: Nguyễn Thị Hà Quyên

Nhóm sinh viên thực hiện 	: Nhóm 11

- Nguyễn Hoàng Ninh
- Nguyễn Thành An
- Lê Ích Lương 
- Nguyễn Nhật Tân 


Đề tài:  XÂY DỰNG WEBSITE QUẢN LÝ PHÂN PHỐI HÀNG HÓA TẠI CÔNG TY TNHH THỜI TRANG YODY

## Chức năng chính

- Quản lý thông tin Sản phẩm, Chi tiết Sản phẩm, Đề xuất, Chi tiết đề xuất,...
- Tìm kiếm và lọc Sản phẩm, Chi tiết Sản phẩm, Đề xuất, Chi tiết đề xuất,... theo nhiều điều kiện khác nhau
- Phân trang, thống kê, xuất file, login và phân quyền theo chức vụ

## Yêu cầu hệ thống

- .NET Core SDK 3.1 trở lên
- SQL Server 2016 trở lên
- Visual Studio 2019 hoặc bất kỳ IDE nào hỗ trợ .NET Core
- Trình duyệt web hiện đại (Chrome, Firefox, Edge)

## Các package cần cài đặt

Dưới đây là danh sách các package NuGet cần cài đặt cho dự án:

- `iTextSharp.LGPLv2.Core`
- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Tools`
- `Microsoft.VisualStudio.Web.CodeGeneration.Design`
- `X.PagedList.Mvc.Core`

Để cài đặt các package này, bạn có thể sử dụng lệnh sau trong Package Manager Console hoặc thông qua giao diện quản lý NuGet trong Visual Studio:

```bash
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Tools
...
```
## Cài đặt

1. **Clone repository:**

    ```bash
    git clone https://github.com/ThanhAn123456/DAPM_C-.git
    cd DAPM_C-
    ```

2. **Khôi phục các gói NuGet:**

    ```bash
    dotnet restore
    ```

3. **Cấu hình chuỗi kết nối đến SQL Server trong file `appsettings.json`:**

    ```json
    "ConnectionStrings": {
      "QuanLyBaiDangCongTyConnection": "Data Source=YourServerName;Initial Catalog=YourDatabaseName;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False"
    }
    ```

4. **Tạo mô hình từ cơ sở dữ liệu hiện có:**

    Trong Visual Studio, mở Package Manager Console và chạy lệnh sau:

    ```bash
    Scaffold-DbContext "Data Source=YourServerName;Initial Catalog=YourDatabaseName;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
    ```
Lệnh này sẽ tạo các lớp mô hình và ngữ cảnh (DbContext) dựa trên cơ sở dữ liệu hiện có.

5. **Chạy ứng dụng:**

    ```bash
    dotnet run
    ```

6. **Mở trình duyệt và truy cập:**

    ```
    http://localhost:your_port
    ```

## Cấu hình cơ sở dữ liệu

Đảm bảo rằng SQL Server của bạn đang chạy và bạn đã tạo một cơ sở dữ liệu với các bảng cần thiết. Cập nhật thông tin kết nối trong `appsettings.json` với thông tin cụ thể của bạn.

## Tính năng
   #### Phân quyền
  -Có Login, logout
  
  -Có phân quyền truy cập các trang theo chức vụ
  #### Quản lý
  -Có đầy đủ các chức năng CRUD

  -Thêm đề xuất, duyệt đề xuất

  -Thống kê, xuất file PDF

  -Tìm kiếm và lọc theo từng chức năng
  
  -Phân trang

## Cấu trúc dự án

- **Controllers:** Chứa các controller điều khiển luồng dữ liệu và xử lý yêu cầu từ người dùng.
- **Models:** Chứa các lớp mô hình đại diện cho dữ liệu của ứng dụng, được tạo từ cơ sở dữ liệu bằng cách sử dụng Database First.
- **Views:** Chứa các file giao diện (Razor Pages) để hiển thị dữ liệu cho người dùng.
- **wwwroot:** Chứa các file tĩnh như CSS, JavaScript và hình ảnh.
