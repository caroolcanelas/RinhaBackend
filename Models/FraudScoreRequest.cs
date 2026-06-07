
using System.Text.Json.Serialization;

public class FraudScoreRequest
{
    [JsonPropertyName("id")] 
    public string Id {get; set;} = "";
    
    [JsonPropertyName("transaction")]
    public required TransactionDTO  transactionDTO {get; set;} 

    [JsonPropertyName("customer")]
    public required CustomerDTO customerDTO {get; set;} 

    [JsonPropertyName("merchant")]
    public required MerchantDTO merchantDTO {get; set;}

    [JsonPropertyName("terminal")]
    public required TerminalDTO terminalDTO {get; set;}

    [JsonPropertyName("last_transaction")]
    public required LastTransactionDTO lastTransactionDTO {get; set;}
}

public class TransactionDTO
{
    [JsonPropertyName("amount")]
    public double Amount {get; set;}

    [JsonPropertyName("installments")]
    public int Installments {get; set;}

    [JsonPropertyName("requested_at")]
    public DateTimeOffset RequestAt {get; set;}

}

public class CustomerDTO
{
    [JsonPropertyName("avg_amount")]
    public double AvgAmount {get; set;}

    [JsonPropertyName("tx_count_24h")]
    public int TxCount24h {get; set;}

    [JsonPropertyName("known_merchants")]
    public string[] KnownMerchants {get; set;}= [];
}

public class MerchantDTO
{
    [JsonPropertyName("id")]
    public string Id {get; set;}= "";

    [JsonPropertyName("mcc")]
    public int Mcc {get; set;}

    [JsonPropertyName("avg_amount")]
    public double AvgAmount{get; set;}
}

public class TerminalDTO
{
    [JsonPropertyName("is_online")]
    public bool Isonline {get; set;}

    [JsonPropertyName("card_present")]
    public bool CardPresent {get; set;}

    [JsonPropertyName("km_from_home")]
    public double KmFromHome {get; set;}
}

public class LastTransactionDTO
{
    [JsonPropertyName("timestamp")]
    public DateTimeOffset Timestamp {get; set;}

    [JsonPropertyName("km_from_current")]
    public double KmFromCurrent {get; set;}
}