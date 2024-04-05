using System.Linq.Expressions;
using AsparagusN.Data.Entities;

namespace AsparagusN.Specifications;

public class SplashScreenAndSliderSpecification : BaseSpecification<MediaUrl>
{
    public SplashScreenAndSliderSpecification(bool isSplash = true)
        : base(x=>(x.IsSplashScreenUrl == isSplash))
    {
    }
    public SplashScreenAndSliderSpecification(int sliderPhotoId)
        : base(x=>(x.IsSplashScreenUrl == false) && x.Id == sliderPhotoId)
    {
    }
}