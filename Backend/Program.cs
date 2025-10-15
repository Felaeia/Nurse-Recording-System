using NurseRecordingSystem.Class.Repository;
using NurseRecordingSystem.Class.Services.Authentication;
using NurseRecordingSystem.Class.Services.UserServices.UserForms;
using NurseRecordingSystem.Class.Services.UserServices.Users;
using NurseRecordingSystem.Contracts.ControllerContracts;
using NurseRecordingSystem.Contracts.HelperContracts.IHelperUserForm;
using NurseRecordingSystem.Contracts.RepositoryContracts.User;
using NurseRecordingSystem.Contracts.ServiceContracts.Auth;
using NurseRecordingSystem.Contracts.ServiceContracts.IAdminServices.IAdminUser;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseUserForms;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseUsers;
using NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.IUserForms;
using NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.UserForms;
using NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.Users;
using NurseRecordingSystem.Contracts.ServiceContracts.User;
using NurseRecordingSystem.Controllers.AuthenticationControllers;

var builder = WebApplication.CreateBuilder(args);
// Services
// Add services to the container.
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Authentication:
// Authentication services
builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();

//Admin:
//User services
builder.Services.AddScoped<IDeleteUser, DeleteUser>();

//Helper:
//UserForm services
builder.Services.AddScoped<IViewUserForm, ViewUserForm>();

//Nurse:
//User services
builder.Services.AddScoped<IViewAllUsers, ViewAllUsers>();
//Form Services
builder.Services.AddScoped<IViewUserFormList, ViewUserFormList>();

//User:
//User services
builder.Services.AddScoped<ICreateUsers, CreateUser>();
builder.Services.AddScoped<IViewUserProfile, ViewUserProfile>();
builder.Services.AddScoped<IUpdateUser, UpdateUser>();
// Form Services
builder.Services.AddScoped<ICreateUserForm, CreateUserForm>();
builder.Services.AddScoped<IUpdateUserForm, UpdateUserForm>();
builder.Services.AddScoped<IDeleteUserForm, DeleteUserForm>();


// Controllers
builder.Services.AddScoped<IAuthController, AuthController>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseDefaultFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
