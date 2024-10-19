namespace HanimeliApp.Domain.Dtos.Address;

public class CreateAddressRequest
{
    public int UserId { get; set; }
    public string AddressLine { get; set; } = null!;
    public int StateId { get; set; }
    public int CityId { get; set; }
    public int CountryId { get; set; }
    public string? PostalCode { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}