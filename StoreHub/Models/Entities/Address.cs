using System.ComponentModel.DataAnnotations.Schema;

namespace StoreHub.Models.Entities;

public class Address
{
    public int ID { get; set; }
    public string FullName { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string PhoneNumber { get; set; }


    [ForeignKey("ApplicationUser")]
    public string UserId { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }
}
