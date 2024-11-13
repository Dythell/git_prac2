using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.Net.NetworkInformation;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        policy => policy.AllowAnyOrigin()
                         .AllowAnyMethod()
                         .AllowAnyHeader());
});

var app = builder.Build();

app.UseCors("AllowAnyOrigin");


List<order> bd =
[   
    new order(1, DateTime.Now, "�������", "�������� � �������", "����� �������", "���� ������", "� ��������"),
    new order(2, new DateTime(2024, 10, 5), "��������", "�������� � �������", "����� �������", "������ ������", "� ��������")
];

bool isUpdatedStatus = false;
string message = "";

app.MapGet("/", () =>
{
    if (isUpdatedStatus)
    {
        string buffer = message;
        isUpdatedStatus = false;
        message = "";
        return Results.Json(new OrderUpdateStatus(bd, buffer));
    }
    else
        return Results.Json(bd);
});

app.MapPost("/", ( order order) =>
{
    bd.Add(order);
    return Results.Created($"/{order.Number}", order);
});

app.MapPost("/{orderNumber}", (int orderNumber, OrderUpdate orderUpdate) =>
{
    var orderToUpdate = bd.FirstOrDefault(o => o.Number == orderNumber);

    if (orderToUpdate == null)
    {
        return Results.NotFound(new { Message = $"������ � ������� {orderNumber} �� �������" });
    }

    if (!string.IsNullOrEmpty(orderUpdate.Status))
    {  
        if (orderToUpdate.Status != orderUpdate.Status)
    {
        orderToUpdate.Status = orderUpdate.Status;
        isUpdatedStatus = true;
        message += "������ ������ ����� " + orderNumber + " �������";
    }}

    if (!string.IsNullOrEmpty(orderUpdate.ProblemDesc))
    {
        orderToUpdate.ProblemDesc = orderUpdate.ProblemDesc;
    }

    if (!string.IsNullOrEmpty(orderUpdate.Master))
    {
        orderToUpdate.Master = orderUpdate.Master;
    }

    if (!string.IsNullOrEmpty(orderUpdate.Comment))
    {
        orderToUpdate.Comment.Add (orderUpdate.Comment);
    }

    return Results.Ok(orderToUpdate);
});

app.MapGet("/{orderNumber}", (int orderNumber) => bd.Find(o => o.Number == orderNumber));

app.MapGet("/filter/{param}", (string param) => bd.FindAll(o =>
    o.Device == param ||
    o.ProblemType == param ||
    o.ProblemDesc == param ||
    o.Client == param ||
    o.Status == param ||
    o.Master == param ));

app.Run();

record class OrderUpdate(string Status, string ProblemDesc, string Master, string Comment);

record class OrderUpdateStatus(List<order> bd, string message);

class order
{
    public int Number { get; set; }
    public DateTime DateAdd { get; set; }
    public string Device { get; set; }
    public string ProblemType { get; set; }
    public string ProblemDesc { get; set; }
    public string Client { get; set; }
    public string Status { get; set; }
    public string Master { get; set; } = "�� ��������";
    public List<string> Comment { get; set; } = [];

    public order(int number, DateTime dateAdd, string device, string problemType, string problemDesc, string client, string status)
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