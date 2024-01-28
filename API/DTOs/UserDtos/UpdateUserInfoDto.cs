namespace AsparagusN.DTOs;

public class UpdateUserInfoDto
{
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? Birthday { get; set; }
    public AddressDto? HomeAddress { get; set; }
    public AddressDto? WorkAddress { get; set; }
}