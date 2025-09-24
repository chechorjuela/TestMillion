using TestMillion.Domain.Common.Entities;

namespace TestMillion.Domain.Entities;

public class Owner : Entity
{
    public string Name { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public DateOnly Birthday { get; private set; }
    public string Photo { get; private set; } = string.Empty;
    
    public Owner(string name, string address, DateOnly birthday)
    {
        Name = name;
        Address = address;
        Birthday = birthday;
    }

    // For MongoDB serialization
    private Owner() { }
}