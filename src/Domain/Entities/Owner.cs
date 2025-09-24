using TestMillion.Domain.Common.Entities;

namespace TestMillion.Domain.Entities;

public class Owner : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateOnly Birthdate { get; set; }
    public string Photo { get; set; } = string.Empty;
    
    public Owner(string name, string address, DateOnly birthdate)
    {
        Name = name;
        Address = address;
        Birthdate = birthdate;
    }

    // For MongoDB serialization
    private Owner() { }
}