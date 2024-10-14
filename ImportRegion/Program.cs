using FindNest.Data;
using FindNest.Data.Models;
using NuGet.Common;
using OfficeOpenXml;

namespace ImportRegion;

class Program
{
    static void Main(string[] args)
    {
        // Đường dẫn tới file Excel
        string filePath = @"DanhSachTinhThanh.xlsx";
        
        List<Region> regions = new List<Region>();
        
        if (File.Exists(filePath))
        {
            FileInfo fileInfo = new FileInfo(filePath);

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                        // Đọc dữ liệu từ ô
                        var id = worksheet.Cells[row, 6].Text;
                        var name = worksheet.Cells[row, 5].Text;
                        var parentId = worksheet.Cells[row, 4].Text;
                        Console.WriteLine($"{id} {name} {parentId}");
                        if (string.IsNullOrWhiteSpace(id))
                        {
                            continue;
                        }
                        Region region = new Region
                        {
                            Id = int.Parse(id) + 10000,
                            ParentId = int.Parse(parentId) + 100,
                            Name = name,
                            Level = 3
                        };
                        regions.Add(region);
                }
            }
        }
        else
        {
            Console.WriteLine("File không tồn tại.");
        }
        FindNestDbContext context = new FindNestDbContext();
        regions = regions.DistinctBy(x => x.Id).ToList();
        // context.AddRange(regions);
        // context.SaveChanges();
        foreach (var region in regions)
        { 
            Console.WriteLine(region);
        }
    }
}