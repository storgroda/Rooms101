using Rooms101.Areas.Identity.Data;
using Rooms101.Models;

namespace Rooms101.Services
{
    public interface IMeetingsService
    {
        Task<Meeting> GetMeetingAsync(int id);
        Task<ICollection<Meeting>> GetMeetingsAsync();
        Task<string> GetMeetingsJsonAsync();
        Task<ICollection<Meeting>> GetAllMeetingsForUserAsync(ApplicationUser user);
        Task<bool> AddMeetingAsync(Meeting meeting, ApplicationUser user);
        Task<bool> AmendMeetingAsync(Meeting meeting, ApplicationUser user);

        Task<bool> DeleteMeetingAsync(int id, ApplicationUser user);

        Task<bool> FindMeetingConflict(Meeting meeting, ApplicationUser user);

        Task<bool> DeleteAttendeesAsync(int meetingId);

        // Room Services
        Task<ICollection<RoomViewModel>> GetRoomsAsync();
        Task<ICollection<RoomViewModel>> GetActiveRoomsAsync();
        Task<RoomViewModel> GetRoomAsync(int roomId);
        Task<Room> GetRoomEntityAsync(int roomId);
        Task AddMeetingRoomAsync(Room room);
        Task AmendMeetingRoomAsync(Room room);

        // User Services
        Task<ICollection<ApplicationUser>> GetAllUsersAysnc();
    }
}
