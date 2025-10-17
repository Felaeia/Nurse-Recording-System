using NurseRecordingSystem.DTO.NurseServiceDTOs.NurseAppointmentScheduleDTOs;

namespace NurseRecordingSystem.Contracts.ServiceContracts.INurseServices
{
    public interface ICreateAppointmentSchedule
    {
        // The common approach here is a DTO in and a generic Result/bool out, or a DTO out on success.
        Task<bool> CreateAppointmentAsync(CreateAppointmentScheduleRequestDTO request);
    }

    public interface IViewAppointmentScheduleList
    {
        Task<IEnumerable<ViewAppointmentScheduleListResponseDTO>> ViewAppointmentScheduleListAsync();
    }

    public interface IViewAppointmentSchedule
    {
        Task<ViewAppointmentScheduleResponseDTO> ViewAppointmentScheduleAsync(int appointmentId);
    }

    public interface IUpdateAppointmentSchedule
    {
        Task<bool> UpdateAppointmentAsync(int appointmentId,UpdateAppointmentScheduleRequestDTO request);
    }

    public interface IDeleteAppointmentSchedule
    {
        Task<bool> DeleteAppointmentAsync(int appointmentId, DeleteAppointmentScheduleRequestDTO request);
    }
}
