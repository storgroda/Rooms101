using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Rooms101.Areas.Identity.Data;
using Rooms101.Data;
using Rooms101.Models;
using System.Drawing;

namespace Rooms101.Services
{
    public class MeetingsService : IMeetingsService
    {
        private readonly ILogger<MeetingsService> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MeetingsService(ILogger<MeetingsService> logger,
                                ApplicationDbContext context,
                                UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // **************************************
        // * Meeting Service Actions
        // **************************************

        /// <summary>
        /// Get A single Meeting By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task<Meeting></returns>
        public async Task<Meeting> GetMeetingAsync(int id)
        {
            var meeting = await _context.Meetings
                            .Include(m => m.MeetingRoom)
                            .Include(m => m.Attendees)
                            .FirstOrDefaultAsync(m => m.MeetingId == id);

            return meeting;
        }

        /// <summary>
        /// Get All Meetings
        /// </summary>
        /// <returns>Task<ICollection<Meeting>></returns>
        public async Task<ICollection<Meeting>> GetMeetingsAsync()
        {
            var meetings = await _context.Meetings.Include(m => m.MeetingRoom).Where(m => m.EndMoment > DateTime.Now && m.Cancelled == false).ToListAsync();

            return meetings;
        }

        public async Task<ICollection<Meeting>> GetAllMeetingsForUserAsync(ApplicationUser user)
        {
            var meetings = await _context.Meetings.Include(m => m.MeetingRoom).Where(m => m.EndMoment > DateTime.Now && m.Cancelled == false).ToListAsync(); //  && m.Owner == user.Id

            return meetings;
        }

        public async Task<bool> AddMeetingAsync(Meeting meeting, ApplicationUser user)
        {
            if (! await this.FindMeetingConflict(meeting, user))
            {
                var room = await _context.Rooms.FirstOrDefaultAsync(m => m.MeetingRoomId == meeting.MeetingRoomId);

                meeting.Owner = user.Id;
                if (room != null) meeting.MeetingRoom = room;
                _context.Add(meeting);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> AmendMeetingAsync(Meeting meeting, ApplicationUser user)
        {
            System.Console.WriteLine($"******** meeting.MeetingRoomId {meeting.MeetingRoomId}");

            var current = await _context.Meetings
                         .Include(m => m.MeetingRoom)
                        .FirstOrDefaultAsync(m => m.MeetingId == meeting.MeetingId);

            if (current != null)
            {
                if (current.Owner == user.Id)
                {
                    if (! await this.FindMeetingConflict(meeting, user))
                    {
                        var room = await _context.Rooms.FirstOrDefaultAsync(m => m.MeetingRoomId == meeting.MeetingRoomId);

                        current.MeetingDescription = meeting.MeetingDescription;
                        current.MeetingId = meeting.MeetingId;
                        current.MeetingRoomId = meeting.MeetingRoomId;
                        if (room != null) current.MeetingRoom = room;
                        current.StartMoment = meeting.StartMoment;
                        current.EndMoment = meeting.EndMoment;
                        current.Attendees = meeting.Attendees;
                        //current.Cancelled = meeting.Cancelled;

                        _context.Update(current);
                        await _context.SaveChangesAsync();
                    }
                } 
                else
                {
                    throw new NotYourMeetingException("This is not your meeting.");
                }
            }

            return true;
        }

        public async Task<bool> DeleteMeetingAsync(int id, ApplicationUser user)
        {
            var meeting = await _context.Meetings.FindAsync(id);

            if(meeting != null)
            {
                if (meeting.Owner == user.Id) // Check OK to delete
                {

                    meeting.Cancelled = true;
                    _context.Update(meeting);
                    //                _context.Meetings.Remove(meeting); // Logical delete = cancel
                    await _context.SaveChangesAsync();

                }
                else
                {
                    throw new NotYourMeetingException("This is not your meeting.");
                }
            }

            return true;
        }

        public async Task<string> GetMeetingsJsonAsync()
        {
            var meetings = await this.GetMeetingsAsync();

            List<CalendarEvent> calList = new List<CalendarEvent>();

            foreach(var m in meetings)
            {
                var myJsonObject = new CalendarEvent
                {
                    id = m.MeetingId,
                    title = $"Meeting in {m.MeetingRoom.MeetingRoomName}",
                    description = m.MeetingDescription == null ? "" : m.MeetingDescription,
                    start = m.StartMoment,
                    end = m.EndMoment,
                    url = $"Meeting/Details/{m.MeetingId}"
                };
                calList.Add(myJsonObject);
            }

            return JsonConvert.SerializeObject(calList);
        }

        public async Task<bool> FindMeetingConflict(Meeting meeting, ApplicationUser user)
        {
            System.Console.WriteLine($"*** FindMeetingConflict()");

            var query = from c in _context.Meetings.ToList()
                        where c.Cancelled.GetValueOrDefault(false) == false
                        && c.MeetingRoomId == meeting.MeetingRoomId
                        && c.StartMoment <= meeting.EndMoment.AddMinutes(-1)
                        && c.EndMoment.AddMinutes(-1) >= meeting.StartMoment
                        && c.MeetingId != meeting.MeetingId
                        select c;

            if (query.Any())
            {
                System.Console.WriteLine($"*** THROW MeetingConflictException");

                throw new MeetingConflictException("This room already booked for this time");
            }

            var newquery = from c in _context.Meetings.Include(c => c.Attendees).ToList()
                           where c.Cancelled.GetValueOrDefault(false) == false
                        && c.StartMoment <= meeting.EndMoment.AddMinutes(-1)
                        && c.EndMoment.AddMinutes(-1) >= meeting.StartMoment
                        && c.MeetingId != meeting.MeetingId
                           where c.Attendees.Any(z => (from a in meeting.Attendees where a.UserId == z.UserId select a).Any())
                           select c;

            if (newquery.Any())
            {
                List<MeetingAttendee> found = new List<MeetingAttendee>();
                string msg = "";

                foreach (var m in newquery)
                {
                    found.AddRange(m.Attendees);
                }

                var dist = (from att in found
                            where meeting.Attendees.Any(z => z.UserId == att.UserId) 
                            select att.UserId).Distinct();

                foreach (var z in dist)
                {
                    msg += $" {(await _userManager.FindByIdAsync(z)).FullName}";
                }
                System.Console.WriteLine($"*** USER QUERY CONFLICT FOR {msg}");

                System.Console.WriteLine($"*** THROW MeetingConflictException {msg}");

                throw new MeetingConflictException($"Attendee(s) {msg} cannot be assigned to your meeting!");
            }

            return false;
        }

        public async Task<bool> DeleteAttendeesAsync(int meetingId)
        {
            var atts = await _context.MeetingAttendees.Where(m => m.MeetingId == meetingId).ToListAsync();

            if (atts != null)
            {
                foreach (var att in atts)
                {
                    _context.MeetingAttendees.Remove(att);
                    await _context.SaveChangesAsync();
                }
            }

            return true;
        }

        // **************************************
        // * Room Service Actions
        // **************************************
        public async Task<RoomViewModel> GetRoomAsync(int roomId)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(m => m.MeetingRoomId == roomId);

            RoomViewModel roomView = null;

            if(room != null)
            {
                roomView = new RoomViewModel
                {
                    MeetingRoomId = room.MeetingRoomId,
                    MeetingRoomName = room.MeetingRoomName,
                    MeetingRoomDescription = room.MeetingRoomDescription,
                    BackgroundColour = room.BackgroundColour == null ? "" : room.BackgroundColour,
                    Active = room.Active
                };
            }

            return roomView;
        }

        public async Task<Room> GetRoomEntityAsync(int roomId)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(m => m.MeetingRoomId == roomId);
            return room;
        }

        public async Task<ICollection<RoomViewModel>> GetActiveRoomsAsync()
        {
            List<RoomViewModel> rvms = new List<RoomViewModel>();

            foreach (var r in await _context.Rooms.Where(r => r.Active == true).ToListAsync())
            {
                rvms.Add(new RoomViewModel
                {
                    MeetingRoomId = r.MeetingRoomId,
                    MeetingRoomName = r.MeetingRoomName,
                    MeetingRoomDescription = r.MeetingRoomDescription,
                    BackgroundColour = r.BackgroundColour == null ? "" : r.BackgroundColour,
                    Active = r.Active
                });
            }

            return rvms;
        }

        public async Task<ICollection<RoomViewModel>> GetRoomsAsync()
        {
            List<RoomViewModel> rvms = new List<RoomViewModel>();

            foreach (var r in await _context.Rooms.ToListAsync())
            {
                rvms.Add(new RoomViewModel
                {
                    MeetingRoomId = r.MeetingRoomId,
                    MeetingRoomName = r.MeetingRoomName,
                    MeetingRoomDescription = r.MeetingRoomDescription,
                    BackgroundColour = r.BackgroundColour == null ? "" : r.BackgroundColour,
                    Active = r.Active
                });
            }

            return rvms;
        }

        public async Task AddMeetingRoomAsync(Room room)
        {
            // Check if room already exists

            //Random r = new Random();
            //var color = Color.FromArgb(r.Next(0, 256),r.Next(0, 256), r.Next(0, 256));
            //var colorName = color.ToKnownColor();

            _context.Add(room);
            await _context.SaveChangesAsync();

        }

        public async Task AmendMeetingRoomAsync(Room room)
        {
            // Check if room name change there is no conflict

            _context.Update(room);
            await _context.SaveChangesAsync();
        }

        // **************************************
        // * User Service Actions
        // **************************************
        public async Task<ICollection<ApplicationUser>> GetAllUsersAysnc()
        {
            var users = await _context.Users.ToListAsync();

            return users;
        }
    }
}
