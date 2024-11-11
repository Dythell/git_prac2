var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

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
    int number;
    DateTime DateAdd;
    string device;
    string problemType;
    string problemDesc;
    string client;
    OrderStatus status = OrderStatus.�_��������;
    string? master = "�� ��������";

    public order(int number, DateTime dateAdd, string device, string problemType, string problemDesc, string client, OrderStatus status, string? master)
    {
        this.number = number;
        DateAdd = dateAdd;
        this.device = device;
        this.problemType = problemType;
        this.problemDesc = problemDesc;
        this.client = client;
        this.status = status;
        this.master = master;
    }

    public int Number { get => number; set => number = value; }
    public DateTime DateAdd1 { get => DateAdd; set => DateAdd = value; }
    public string Device { get => device; set => device = value; }
    public string ProblemType { get => problemType; set => problemType = value; }
    public string ProblemDesc { get => problemDesc; set => problemDesc = value; }
    public string Client { get => client; set => client = value; }
    public string? Master { get => master; set => master = value; }
    internal OrderStatus Status { get => status; set => status = value; }
}

