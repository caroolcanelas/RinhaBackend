using System.Runtime.InteropServices;
using System.Security.Cryptography.Xml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/ready");

app.MapPost("/fraud-score", (FraudScoreRequest request) =>
{
    var response = new FraudScoreResponse
    {
        Approved = true,
        FraudScore = 0.0,
    };

    double[] rawVector = BuildVector(request);
    double[] normalizeVector = NormalizeVector(rawVector);
    var references = FakeReferences.Create();
    references.ForEach(n =>
    {
        double result = distance(normalizeVector, n.Vector);

        Console.WriteLine($"result = {result}");
        Console.WriteLine($"isFraud = {n.IsFraud}");

    });



    return response;
});

double[] BuildVector(FraudScoreRequest request)
{
    double[] vector = new double[14];
    Console.WriteLine("Entrei na build vector");

    //0 - amount
    vector[0] = request.transactionDTO.Amount;

    //1 - installments
    vector[1] = request.transactionDTO.Installments;

    //2 - amount_vs_avg
    vector[2] = request.transactionDTO.Amount / request.customerDTO.AvgAmount;

    //3 - hour_of_day
    double hora = request.transactionDTO.RequestAt.Hour;
    vector[3] = hora;

    //4 - day_of_week
    double dayOfWeek = (int)request.transactionDTO.RequestAt.DayOfWeek;
    if (dayOfWeek == 0)
    {
        vector[4] = 6;
    }
    else
    {
        vector[4] = dayOfWeek - 1;
    }

    if (request.lastTransactionDTO != null)
    {
        //5 - minutes_since_last_tx
        TimeSpan minutes = request.transactionDTO.RequestAt - request.lastTransactionDTO.Timestamp;
        double finalMinutes = minutes.TotalMinutes;
        vector[5] = finalMinutes;

        //6 - km_from_last_tx
        vector[6] = request.lastTransactionDTO.KmFromCurrent;

    }
    else
    {
        vector[5] = -1;
        vector[6] = -1;
    }

    //7 - km_from_home
    vector[7] = request.terminalDTO.KmFromHome;

    //8 - tx_count_24h
    vector[8] = request.customerDTO.TxCount24h;


    //9 - is_online
    if (request.terminalDTO.Isonline == true)
    {
        vector[9] = 1;
    }

    //10 - card_present
    if (request.terminalDTO.CardPresent == true)
    {
        vector[10] = 1;
    }

    //11 - unknown_merchant
    int unknownMerchant = 1;

    for (int i = 0; i <= request.customerDTO.KnownMerchants.Length - 1; i++)
    {


        if (request.customerDTO.KnownMerchants[i] == request.merchantDTO.Id)
        {
            unknownMerchant = 0;
            break;
        }
    }

    vector[11] = unknownMerchant;

    //12 - mcc_risk (TODO: substituir por risco real)
    vector[12] = 0.5;

    //13 - merchant_avg_amount
    vector[13] = request.merchantDTO.AvgAmount;

    for (int i = 0; i <= vector.Length - 1; i++)
    {
        Console.WriteLine($"Vector[{i}] = {vector[i]}");
    }

    return vector;
}


double[] NormalizeVector(double[] rawVector)
{

    int max_amount = 10000;
    int max_installments = 12;
    int amount_vs_avg_ratio = 10;
    int max_minutes = 1440;
    int max_km = 1000;
    int max_tx_count_24h = 20;
    int max_merchant_avg_amount = 10000;
    double[] normalizedVector = new double[14];

    //0 - amount
    normalizedVector[0] = limit(rawVector[0] / max_amount);

    //1 - installments
    normalizedVector[1] = limit(rawVector[1] / max_installments);

    //2 - amount_vs_avg
    normalizedVector[2] = limit(rawVector[2] / amount_vs_avg_ratio);

    //3 - hour_of_day
    normalizedVector[3] = limit(rawVector[3] / 23);

    //4 - day_of_week - 
    normalizedVector[4] = limit(rawVector[4] / 6.0);

    if (rawVector[5] != -1)
    {
        //5 - minutes_since_last_tx
        normalizedVector[5] = limit(rawVector[5] / max_minutes);
    }
    else
    {
        normalizedVector[5] = rawVector[5];
    }

    if (rawVector[6] != -1)
    {
        //6 - km_from_last_tx
        normalizedVector[6] = limit(rawVector[6] / max_km);
    }
    else
    {
        normalizedVector[6] = rawVector[6];
    }

    //7 - km_from_home
    normalizedVector[7] = limit(rawVector[7] / max_km);

    //8 - tx_count_24h
    normalizedVector[8] = limit(rawVector[8] / max_tx_count_24h);


    //9 - is_online - já normalizado
    normalizedVector[9] = limit(rawVector[9]);

    //10 - card_present - já normalizado
    normalizedVector[10] = limit(rawVector[10]);

    //11 - unknown_merchant - TODO
    normalizedVector[11] = limit(rawVector[11]);

    //12 - mcc_risk (TODO: substituir por risco real)
    normalizedVector[12] = limit(rawVector[12]);

    //13 - merchant_avg_amount
    normalizedVector[13] = limit(rawVector[13] / max_merchant_avg_amount);

    for (int i = 0; i <= normalizedVector.Length - 1; i++)
    {
        Console.WriteLine($"normalizedVector[{i}] = {normalizedVector[i]}");
    }

    return normalizedVector;
}

double limit(double value)
{
    if (value > 1)
    {
        Console.WriteLine($" entrei em 0 valor = {value}");
        value = 1;
    }
    if (value < 0)
    {
        Console.WriteLine($"entrei em 0 valor = {value}");
        value = 0;
    }
    return value;
}

double distance(double[] normalizedVectorValues, double[] referenceVectorValues)
{
    double sum = 0;

    if (normalizedVectorValues.Length != referenceVectorValues.Length)
    {
        throw new ArgumentOutOfRangeException("Vectors should have the same Length");
    }

    for (int i = 0; i <= normalizedVectorValues.Length - 1; i++)
    {
        Console.WriteLine($"normalizedVectorValues[{i}] = {normalizedVectorValues[i]}");
        Console.WriteLine($"referenceVectorValues[{i}] = {referenceVectorValues[i]}");

        double difference = normalizedVectorValues[i] - referenceVectorValues[i];

        Console.WriteLine($"difference = {difference}");

        double square = difference * difference;

        Console.WriteLine($"square = {square}");

        sum += square;

    }
    double result = Math.Sqrt(sum);

    Console.WriteLine($"result = {result}");

    return result;
}
app.Run();

