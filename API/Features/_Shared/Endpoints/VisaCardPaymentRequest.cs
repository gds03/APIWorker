namespace API.Features._Shared.Endpoints;

public class VisaCardPaymentRequest
{
    public string CardNumber { get; set; } 
    public int ExpirationMonth { get; set; }
    public int ExpirationYear { get; set; }
    public string Cvv { get; set; }
}