namespace API.Features._Shared.Endpoints;

public class VisaCardPaymentRequest
{
    public string Owner { get; set; } 
    public string CardNumber { get; set; } 
    public int ExpirationMonth { get; set; }
    public int ExpirationYear { get; set; }
    public int Cvv { get; set; }
}