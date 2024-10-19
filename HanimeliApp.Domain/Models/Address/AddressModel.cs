using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.User;

namespace HanimeliApp.Domain.Models.Address;

public class AddressModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string AddressLine { get; set; } = null!;
    public int StateId { get; set; }
    public string State { get; set; } = null!;
    public int CityId { get; set; }
    public string City { get; set; } = null!;
    public int CountryId { get; set; }
    public string Country { get; set; } = null!;
    public string? PostalCode { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

    public UserModel User { get; set; } = null!;
}