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


List<order> bd = new List<order>()
{
    new order(1, DateTime.Now, "Ноутбук", "Проблема с экраном", "Экран треснул", "Иван Иванов", "в ожидании"),
    new order(2, new DateTime(2024, 10, 5), "Смартфон", "Проблема с экраном", "Экран треснул", "Степан Иванов", "в ожидании")
};

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
        return Results.NotFound(new { Message = $"Заявка с номером {orderNumber} не найдена" });
    }

    if(orderToUpdate.Status != orderUpdate.Status)
    {
        orderToUpdate.Status = orderUpdate.Status;
        isUpdatedStatus = true;
        message += "Статус заявки номер " + orderNumber + "изменен\n";
    }

    if (!string.IsNullOrEmpty(orderUpdate.ProblemDesc))
    {
        orderToUpdate.ProblemDesc = orderUpdate.ProblemDesc;
    }

    if (!string.IsNullOrEmpty(orderUpdate.Master))
    {
        orderToUpdate.Master = orderUpdate.Master;
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

class order
{
    public int Number { get; set; }
    public DateTime DateAdd { get; set; }
    public string Device { get; set; }
    public string ProblemType { get; set; }
    public string ProblemDesc { get; set; }
    public string Client { get; set; }
    public string Status { get; set; } = "в_ожидании";
    public string Master { get; set; } = "Не назначен";

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
record class OrderUpdate(string Status, string ProblemDesc, string Master);

record class OrderUpdateStatus(List<order> bd, string message);