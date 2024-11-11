var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


List<order> bd = new List<order>()
{
    new order(1, DateTime.Now, "�������", "�������� � �������", "����� �������", "���� ������", OrderStatus.�_��������)
};

app.MapGet("/", () => bd);

app.MapPost("/", (order newOrder) =>
{
    newOrder.Number = bd.Any() ? bd.Max(o => o.Number) + 1 : 1;
    newOrder.DateAdd = DateTime.Now;
    newOrder.Status = OrderStatus.�_��������;

    bd.Add(newOrder);

    return Results.Created($"/{newOrder.Number}", newOrder);
});


app.Run();
enum OrderStatus
{
    �_�������� = 1,
    �_������ = 2,
    ��������� = 3,
    ��_��������� = 4,
}

class order
{
    public int Number { get; set; }
    public DateTime DateAdd { get; set; }
    public string Device { get; set; }
    public string ProblemType { get; set; }
    public string ProblemDesc { get; set; }
    public string Client { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.�_��������;
    public string? Master { get; set; } = "�� ��������";

    public order(int number, DateTime dateAdd, string device, string problemType, string problemDesc, string client, OrderStatus status)
    {
        Number = number;
        DateAdd = dateAdd;
        Device = device;
        ProblemType = problemType;
        ProblemDesc = problemDesc;
        Client = client;
        Status = status;
    }
}