using System.Text.Json.Serialization;
using Iob.Bank.Domain.Data.Dtos.Base;

namespace Iob.Bank.Domain.Data.Dtos;

public class UserDto : BaseDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("identifier")]
    public string? Identifier { get; set; }
    [JsonPropertyName("birthday")]
    public DateTime Birthday { get; set; }
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [JsonPropertyName("password")]
    public string? Password { get; set; }
    [JsonPropertyName("phone_number")]
    public string? PhoneNumber { get; set; }
    [JsonPropertyName("address")]
    public string? Address { get; set; }
    [JsonPropertyName("user_type_id")]
    public long? UserTypeId { get; set; } = 2; // Customer
    [JsonPropertyName("user_type")]
    public UserTypeDto? UserType { get; set; }
}
