namespace AsparagusN.Data.Entities;

public class MediaUrl
{
    public MediaUrl()
    {
    }

    public int Id { get; set; }
    public string Url { get; set; }
    public bool IsSplashScreenUrl { get; set; } = false;

}