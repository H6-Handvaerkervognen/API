
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using HåndværkervognenAPI.Database;
using HåndværkervognenAPI.Managers;
using HåndværkervognenAPI.Notifiacation;
using HåndværkervognenAPI.Security;

namespace HåndværkervognenAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddScoped<ILoginService, loginManager>();
            builder.Services.AddScoped<IAppService, AppManager>();
            builder.Services.AddScoped<IAlarmService, NotificationAlarmManager>();
            builder.Services.AddScoped<IDatabase, DataManager>();
            builder.Services.AddScoped<IHashing, Hasher>();
            builder.Services.AddScoped<IEncryption, RSAEncrypter>();
            builder.Services.AddScoped<INotification, FcmNotification>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //builder.Services.AddHttpClient<FcmSender>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("private_key.json")
            });            

            app.MapControllers();

            app.Run();
        }
    }
}