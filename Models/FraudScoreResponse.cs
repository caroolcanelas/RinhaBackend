using System.Text.Json.Serialization;

public class FraudScoreResponse
{

    [JsonPropertyName("approved")]
    public bool Approved {get; set;}


    [JsonPropertyName("fraud_score")]
    public double FraudScore {get; set;}
}