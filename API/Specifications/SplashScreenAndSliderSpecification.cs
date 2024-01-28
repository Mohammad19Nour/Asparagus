using System.Linq.Expressions;
using AsparagusN.Entities;

namespace AsparagusN.Specifications;

public class SplashScreenAndSliderSpecification : BaseSpecification<MediaUrl>
{
    public SplashScreenAndSliderSpecification(bool isSplash = true)
        : base(x=>(x.IsSplashScreenUrl == isSplash))
    {
    }
}