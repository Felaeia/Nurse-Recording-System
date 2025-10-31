using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Class.Repository;
using NurseRecordingSystem.Class.Services.AdminServices.AdminAppointmentSchedule;
using NurseRecordingSystem.Class.Services.Authentication;
using NurseRecordingSystem.Class.Services.ClinicStatusServices;
using NurseRecordingSystem.Class.Services.MedecineStockServices;
using NurseRecordingSystem.Class.Services.NurseServices.AppointmentSchedules;
using NurseRecordingSystem.Class.Services.NurseServices.NurseAppointmentSchedules;
using NurseRecordingSystem.Class.Services.NurseServices.NurseCreation;
using NurseRecordingSystem.Class.Services.NurseServices.PatientRecords;
using NurseRecordingSystem.Class.Services.UserServices.UserForms;
using NurseRecordingSystem.Class.Services.UserServices.Users;
using NurseRecordingSystem.Contracts.ControllerContracts;
using NurseRecordingSystem.Contracts.RepositoryContracts.User;
using NurseRecordingSystem.Contracts.ServiceContracts.Auth;
using NurseRecordingSystem.Contracts.ServiceContracts.HelperContracts.IHelperUserForm;
using NurseRecordingSystem.Contracts.ServiceContracts.IAdminServices.IAdminClinicStatus;
using NurseRecordingSystem.Contracts.ServiceContracts.IAdminServices.IAdminPatientRecords;
using NurseRecordingSystem.Contracts.ServiceContracts.IAdminServices.IAdminUser;
using NurseRecordingSystem.Contracts.ServiceContracts.IHelperServices.IHelperClinicStatus;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseClinicStatus;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseMedecineStock;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INursePatientRecords;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseUserForms;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseUsers;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.NurseCreation;
using NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.IUserForms;
using NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.UserForms;
using NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.Users;
using NurseRecordingSystem.Contracts.ServiceContracts.User;
using NurseRecordingSystem.Controllers.AuthenticationControllers;
using System.Data;


var builder = WebApplication.CreateBuilder(args);
// Services
// Add services to the container.
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Authentication:
// Authentication services
builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

//Admin:
//User services
builder.Services.AddScoped<IDeleteUser, DeleteUser>();
//Patient Records
builder.Services.AddScoped<IDeletedPatientRecord, DeletePatientRecord>();
//Nurse Services
builder.Services.AddScoped<ICreateNurse, CreateNurse>();
//Clinic Status Services
builder.Services.AddScoped<ICreateClinicStatus, CreateClinicStatus>();
builder.Services.AddScoped<IDeleteClinicStatus, DeleteClinicStatus>();


//Helper:
//UserForm services
builder.Services.AddScoped<IViewUserForm, ViewUserForm>();
//Clinic Status Services
builder.Services.AddScoped<IViewClinicStatus, ViewClinicStatus>();
//Session Token Services
builder.Services.AddScoped<ISessionTokenService, SessionTokenService>();


//Nurse:
//User services
builder.Services.AddScoped<IViewAllUsers, ViewAllUsers>();
//Form Services
builder.Services.AddScoped<IViewUserFormList, ViewUserFormList>();
//Patient Record
builder.Services.AddScoped<ICreatePatientRecord, CreatePatientRecord>();
builder.Services.AddScoped<IViewPatientRecord, ViewPatientRecord>();
builder.Services.AddScoped<IViewPatientRecordList, ViewPatientRecordList>();
builder.Services.AddScoped<IUpdatePatientRecord, UpdatePatientRecord>();
//MedecineStock services
builder.Services.AddScoped<ICreateMedecineStock, CreateMedecineStock>();
builder.Services.AddScoped<IUpdateMedecineStock, UpdateMedecineStock>();
builder.Services.AddScoped<IDeleteMedecineStock, DeleteMedecineStock>();
builder.Services.AddScoped<IViewAllMedecineStocks, ViewAllMedecineStocks>();
//Clinic Status Services
builder.Services.AddScoped<IUpdateClinicStatus, UpdateClinicStatus>();
// Appointment Schedule Services Registration
builder.Services.AddScoped<ICreateAppointmentSchedule, CreateAppointmentSchedule>();
builder.Services.AddScoped<IViewAppointmentScheduleList, ViewAppointmentScheduleList>();
builder.Services.AddScoped<IViewAppointmentSchedule, ViewAppointmentSchedule>();
builder.Services.AddScoped<IUpdateAppointmentSchedule, UpdateAppointmentSchedule>();
builder.Services.AddScoped<IDeleteAppointmentSchedule, DeleteAppointmentSchedule>();


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
