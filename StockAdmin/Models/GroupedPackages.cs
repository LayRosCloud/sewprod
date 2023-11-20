using System.Collections.Generic;

namespace StockAdmin.Models;

public class GroupedPackages
{
    public string FullName { get; set; } = "";
    public List<PackageEntity> Packages { get; set; } = new();

}