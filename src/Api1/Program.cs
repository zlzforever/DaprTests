var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddDapr(x =>
{
    x.UseGrpcEndpoint("http://localhost:3501");
    x.UseHttpEndpoint("http://localhost:50001");
});
builder.Services.AddDaprClient();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Dapr configurations
app.UseCloudEvents();
app.MapSubscribeHandler();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();