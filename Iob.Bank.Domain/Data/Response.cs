using System;
using System.Text.Json.Serialization;

namespace Iob.Bank.Domain.Data;

public class Response<T>
{
    [JsonPropertyName("success")]
    public bool Success { get; private set; }
    [JsonPropertyName("data")]
    public T? Data { get; private set; }
    [JsonPropertyName("error_message")]
    public string? ErrorMessage { get; private set; }

    public static Response<T> CreateSuccess(T data)
    {
        return new Response<T>() { Success = true, Data = data };
    }

    public static Response<T> CreateError(string errorMessage)
    {
        return new Response<T>() { Success = false, ErrorMessage = errorMessage };
    }
}
