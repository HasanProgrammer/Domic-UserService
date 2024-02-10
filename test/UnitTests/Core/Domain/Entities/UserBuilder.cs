using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Domain.User.Entities;

namespace Core.Domain.Entities;

public class UserBuilder
{
    private readonly IDateTime _dateTime;
    
    private string _Id          = Guid.NewGuid().ToString();
    private string _FirstName   = "Hasan";
    private string _LastName    = "Domic";
    private string _Description = "4gXDlJRPguRND4qZ0dhk0LvZ1TqgYCY0fqvVtZJiCwjLCW3fOEm1HfSYZjzdkaRDhklxbRCz3uwuLKlJmGG89oDl61f0DBhEMsi3r";
    private string _Username    = "HasanProgrammer";
    private string _Password    = "Hasan@123@313@@";
    private string _PhoneNumber = "09026676147";
    private string _Email       = "hasanDomic2020313@gmail.com";

    public UserBuilder(IDateTime dateTime) => _dateTime = dateTime;

    public UserBuilder WithId(string id)
    {
        _Id = id;
        return this;
    }
    
    public UserBuilder WithFirstName(string firstName)
    {
        _FirstName = firstName;
        return this;
    }
    
    public UserBuilder WithLastName(string lastName)
    {
        _LastName = lastName;
        return this;
    }
    
    public UserBuilder WithDescription(string description)
    {
        _Description = description;
        return this;
    }
    
    public UserBuilder WithUsername(string username)
    {
        _Username = username;
        return this;
    }
    
    public UserBuilder WithPassword(string password)
    {
        _Password = password;
        return this;
    }
    
    public UserBuilder WithPhoneNumber(string phoneNumber)
    {
        _PhoneNumber = phoneNumber;
        return this;
    }
    
    public UserBuilder WithEmail(string email)
    {
        _Email = email;
        return this;
    }

    public User Build()
    {
        return new User(
            _dateTime ,
            Guid.NewGuid().ToString() ,
            _FirstName   ,
            _LastName    ,
            _Description ,
            _Username    ,
            _Password    ,
            _PhoneNumber ,
            _Email       ,
            null         ,
            null
        );
    }
}