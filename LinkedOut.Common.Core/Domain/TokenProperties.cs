namespace LinkedOut.Common.Domain;

public record TokenProperties
{
    public bool? Enable { get; set; }
    
    public string SecretKey { get; set; } = null!;

    public string Issuer { get; set; } = null!;

    public string Audience { get; set; } = null!;
    
    public long ExpiresTime { get; set; }
}