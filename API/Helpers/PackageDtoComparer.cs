using AsparagusN.DTOs.PackageDtos;

namespace AsparagusN.Helpers;

public class PackageDtoComparer : Comparer<PackageDto>
{
    public override int Compare(PackageDto? x, PackageDto? y)
    {
        var xAnd = x.IsCustomerInfoPrinted && x.IsMealsInfoPrinted;
        var yAnd = y.IsCustomerInfoPrinted && y.IsMealsInfoPrinted;

        if (x != y) return (xAnd ? 1 : -1);
        if (xAnd) return 1;
        if (yAnd) return -1;
        return (x.IsCustomerInfoPrinted || x.IsMealsInfoPrinted) ? 1 : -1;
    }
}