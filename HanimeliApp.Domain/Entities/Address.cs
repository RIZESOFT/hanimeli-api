using HanimeliApp.Domain.Entities.Abstract;

namespace HanimeliApp.Domain.Entities;

public class Address : BaseEntity<int>
{
    public int UserId { get; set; }
    public string AddressLine { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }

    // Navigation Properties
    public User User { get; set; }
}
