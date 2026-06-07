
public class FakeReference
{
    public double[] Vector { get; set; } = [];
    public bool IsFraud { get; set; }
}

public static class FakeReferences
{
    public static List<FakeReference> Create()
    {
        var references = new List<FakeReference>();

        references.Add(new FakeReference
        {
            Vector = new double[]
            {
            0.01,
            0.0833,
            0.05,
            0.8261,
            0.1667,
            -1,
            -1,
            0.0432,
            0.25,
            0,
            1,
            0,
            0.2,
            0.0416},
            IsFraud = false
        });

        references.Add(new FakeReference
        {
            Vector = new double[]
            {
            0.0109,
            0.1667,
            0.05,
            0.3913,
            0.6667,
            0.3007,
            0.0139,
            0.0154,
            0.2,
            0,
            1,
            0,
            0.15,
            0.0282
            },
            IsFraud = false
        });

        references.Add(new FakeReference
        {
            Vector = new double[]
                    {
             0.0336,
            0.1667,
            0.05,
            0.4348,
            0.6667,
            0.1278,
            0.0008,
            0.017,
            0.1,
            0,
            1,
            0,
            0.2,
            0.02
                    },
            IsFraud = false
        });

        references.Add(new FakeReference
        {
            Vector = new double[]
                            {
             0.0415,
            0.25,
            0.05,
            0.7391,
            1,
            0.2375,
            0.0121,
            0.0005,
            0.2,
            0,
            1,
            0,
            0.3,
            0.0493
                            },
            IsFraud = false
        });

        references.Add(new FakeReference
        {
            Vector = new double[]
                        {
             0.0291,
            0.0833,
            0.05,
            0.3913,
            0.3333,
            0.3028,
            0.0044,
            0.028,
            0.1,
            0,
            1,
            0,
            0.3,
            0.043
                        },
            IsFraud = false
        });

        references.Add(new FakeReference
        {
            Vector = new double[]
                        {
             0.5796,
            0.9167,
            1,
            0.0435,
            0,
            0.0056,
            0.4394,
            0.4598,
            0.4,
            1,
            0,
            1,
            0.85,
            0.0032
                        },
            IsFraud = true,
        });


        return references;
    }
}
