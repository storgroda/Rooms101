#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rooms101.Data;
using Rooms101.Models;
using System.Text.Json;
using Newtonsoft.Json;
using Rooms101.Services;
using Rooms101.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Rooms101.Controllers
{
    public class MeetingController : Controller
    {
        private readonly IMeetingsService _meetingsService;
        private readonly UserManager<ApplicationUser> _userManager;

        public MeetingController(IMeetingsService meetingsService,
                                UserManager<ApplicationUser> userManager)
        {
            _meetingsService = meetingsService;
            _userManager = userManager;
        }


        // GET: Meeting
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {

            var meetings = await _meetingsService.GetMeetingsAsync();

            List<MeetingViewModel> lmvm = new List<MeetingViewModel>();

            var myUser = await _userManager.GetUserAsync(User);

            foreach (var meeting in meetings)
            {
                var fullName = "";
                if(meeting.Owner != null)
                {
                    fullName = (await _userManager.FindByIdAsync(meeting.Owner)).FullName;
                }

                MeetingViewModel mvm = new MeetingViewModel
                {
                    MeetingId = meeting.MeetingId,
                    MeetingDescription = meeting.MeetingDescription,
                    MeetingRoomId = meeting.MeetingRoomId,
                    CreateMoment = meeting.CreateMoment,
                    StartMoment = meeting.StartMoment,
                    EndMoment = meeting.EndMoment,
                    Owner = fullName,
                    OwnerId = meeting.Owner,
                    Cancelled = meeting.Cancelled,
                    Attendees = meeting.Attendees,
                    MeetingRoom = meeting.MeetingRoom,
                    OkToEdit = (meeting.Owner == myUser.Id)
                };

                lmvm.Add(mvm);
            }

            return View(lmvm);
        }

        // Get GetMeetings
        [HttpGet]
        [AllowAnonymous]
        public async Task<string> GetMeetings()
        {
            var meetings = await _meetingsService.GetMeetingsJsonAsync();

            return meetings;
        }

        // GET: Meeting/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meeting = await _meetingsService.GetMeetingAsync(id.GetValueOrDefault());

            if (meeting == null)
            {
                return NotFound();
            }

            var myUser = await _userManager.GetUserAsync(User);

            var fullName = "";
            if (meeting.Owner != null)
            {
                fullName = (await _userManager.FindByIdAsync(meeting.Owner)).FullName;
            }

            foreach (var user in meeting.Attendees)
            {
                user.User = await _userManager.FindByIdAsync(user.UserId);
            }

            MeetingViewModel mvm = new MeetingViewModel
            {
                MeetingId = meeting.MeetingId,
                MeetingDescription = meeting.MeetingDescription,
                MeetingRoomId = meeting.MeetingRoomId,
                CreateMoment = meeting.CreateMoment,
                StartMoment = meeting.StartMoment,
                EndMoment = meeting.EndMoment,
                Owner = fullName,
                OwnerId = meeting.Owner,
                 Cancelled = meeting.Cancelled,
                 Attendees = meeting.Attendees,
                 MeetingRoom = meeting.MeetingRoom,
                OkToEdit = (meeting.Owner == myUser.Id)
            };

            return View(mvm);
        }

        // GET: Meeting/Create
        public async Task<IActionResult> Create()
        {
            ViewData["AttendeeId"] = new SelectList(await _meetingsService.GetAllUsersAysnc(), "Id", "FullName");
            ViewData["MeetingRoomId"] = new SelectList(await _meetingsService.GetRoomsAsync(), "MeetingRoomId", "MeetingRoomName");
            return View();
        }

        // POST: Meeting/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MeetingCreateViewModel meetingvm)
        {
            ViewData["UserMessage"] = "";

            if (ModelState.IsValid)
            {
                var myUser = await _userManager.GetUserAsync(User);

                List<MeetingAttendee> attList = new List<MeetingAttendee>();

                foreach(string att in meetingvm.AttendeeId)
                {
                     var ma = new MeetingAttendee {
//                        MeetingId = meetingvm.MeetingRoomId,
                        UserId = att,
                        User = await _userManager.FindByIdAsync(att)
                    };

                    attList.Add(ma);
                }

                Meeting meeting = new Meeting
                {
                    MeetingDescription = meetingvm.MeetingDescription,
                    MeetingRoomId = meetingvm.MeetingRoomId,
                    StartMoment = meetingvm.StartMoment,
                    EndMoment = meetingvm.EndMoment,
                    Owner = myUser.Id,
                    Cancelled = false,
                    Attendees = attList
                };

                try
                {
                    await _meetingsService.AddMeetingAsync(meeting, myUser);
                }
                catch (NotYourMeetingException nymex)
                {
                    ViewData["UserMessage"] = nymex.Message;
                    ViewData["AttendeeId"] = new SelectList(await _meetingsService.GetAllUsersAysnc(), "Id", "FullName");
                    ViewData["MeetingRoomId"] = new SelectList(await _meetingsService.GetRoomsAsync(), "MeetingRoomId", "MeetingRoomName", meetingvm.MeetingRoomId);
                    return View(meetingvm);
                }
                catch (MeetingConflictException mex)
                {
                    ViewData["UserMessage"] = mex.Message;
                    ViewData["AttendeeId"] = new SelectList(await _meetingsService.GetAllUsersAysnc(), "Id", "FullName");
                    ViewData["MeetingRoomId"] = new SelectList(await _meetingsService.GetRoomsAsync(), "MeetingRoomId", "MeetingRoomName", meetingvm.MeetingRoomId);
                    return View(meetingvm);
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["AttendeeId"] = new SelectList(await _meetingsService.GetAllUsersAysnc(), "Id", "FullName");
            ViewData["MeetingRoomId"] = new SelectList(await _meetingsService.GetRoomsAsync(), "MeetingRoomId", "MeetingRoomName", meetingvm.MeetingRoomId);
            return View(meetingvm);
        }

        // GET: Meeting/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meeting = await _meetingsService.GetMeetingAsync(id.GetValueOrDefault());
            if (meeting == null)
            {
                return NotFound();
            }

            var myUser = await _userManager.GetUserAsync(User);

            var fullName = "";
            if (meeting.Owner != null)
            {
                fullName = (await _userManager.FindByIdAsync(meeting.Owner)).FullName;
            }

            var attendees = meeting.Attendees.Select(a => a.UserId).ToArray<string>();

            MeetingAmendViewModel mvm = new MeetingAmendViewModel
            {
                MeetingId = meeting.MeetingId,
                MeetingDescription = meeting.MeetingDescription,
                MeetingRoomId = meeting.MeetingRoomId,
                StartMoment = meeting.StartMoment,
                EndMoment = meeting.EndMoment,
                Owner = fullName,
                AttendeeId = attendees,
                OkToEdit = (meeting.Owner == myUser.Id)
            };

            ViewData["MeetingRoomId"] = new SelectList(await _meetingsService.GetRoomsAsync(), "MeetingRoomId", "MeetingRoomName", meeting.MeetingRoom); //, meeting.MeetingRoomId
            ViewData["Attendees"] = new SelectList(await _meetingsService.GetAllUsersAysnc(), "Id", "FullName");
            ViewData["AttendeeId"] = new SelectList(await _meetingsService.GetAllUsersAysnc(), "Id", "FullName", attendees);
            return View(mvm);
        }

        // POST: Meeting/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MeetingId,MeetingRoomId,MeetingDescription,StartMoment,EndMoment,AttendeeId")] MeetingAmendViewModel meetingvm)
        {
            if (id != meetingvm.MeetingId)
            {
                return NotFound();
            }

            var meetingRoom = await _meetingsService.GetRoomEntityAsync(meetingvm.MeetingRoomId);

            if (ModelState.IsValid)
            {
                await _meetingsService.DeleteAttendeesAsync(meetingvm.MeetingId);

                List<MeetingAttendee> attList = new List<MeetingAttendee>();

                foreach (string att in meetingvm.AttendeeId)
                {
                     var ma = new MeetingAttendee
                    {
                        MeetingId = meetingvm.MeetingId,
                        UserId = att,
                        User = await _userManager.FindByIdAsync(att)
                    };

                    attList.Add(ma);
                }


                Meeting meeting = new Meeting
                {
                    MeetingId = meetingvm.MeetingId,
                    MeetingDescription = meetingvm.MeetingDescription,
                    MeetingRoomId = meetingvm.MeetingRoomId,
                    MeetingRoom = meetingRoom,
                    StartMoment = meetingvm.StartMoment,
                    EndMoment = meetingvm.EndMoment,
                    Attendees = attList
                };

                try
                {
                    var myUser = await _userManager.GetUserAsync(User);

                    await _meetingsService.AmendMeetingAsync(meeting, myUser);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeetingExists(meeting.MeetingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (NotYourMeetingException nymex)
                {
                    ViewData["UserMessage"] = nymex.Message;
                    ViewData["MeetingRoomId"] = new SelectList(await _meetingsService.GetRoomsAsync(), "MeetingRoomId", "MeetingRoomName", meetingRoom); //, meeting.MeetingRoomId
                    ViewData["AttendeeId"] = new SelectList(await _meetingsService.GetAllUsersAysnc(), "Id", "FullName", meetingvm.AttendeeId);
                    return View(meetingvm);
                }
                catch (MeetingConflictException mex)
                {
                    ViewData["UserMessage"] = mex.Message;
                    ViewData["MeetingRoomId"] = new SelectList(await _meetingsService.GetRoomsAsync(), "MeetingRoomId", "MeetingRoomName", meetingRoom); //, meeting.MeetingRoomId
                    ViewData["AttendeeId"] = new SelectList(await _meetingsService.GetAllUsersAysnc(), "Id", "FullName", meetingvm.AttendeeId);
                    return View(meetingvm);
                }

                return RedirectToAction(nameof(Index));
            }
            
            ViewData["MeetingRoomId"] = new SelectList(await _meetingsService.GetRoomsAsync(), "MeetingRoomId", "MeetingRoomName", meetingRoom);
            ViewData["Attendees"] = new SelectList(await _meetingsService.GetAllUsersAysnc(), "Id", "FullName", meetingvm.AttendeeId);

            return View(meetingvm);
        }

        // GET: Meeting/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meeting = await _meetingsService.GetMeetingAsync(id.GetValueOrDefault());

            if (meeting == null)
            {
                return NotFound();
            }

            var myUser = await _userManager.GetUserAsync(User);

            var fullName = "";
            if (meeting.Owner != null)
            {
                fullName = (await _userManager.FindByIdAsync(meeting.Owner)).FullName;
            }

            MeetingViewModel mvm = new MeetingViewModel
            {
                MeetingId = meeting.MeetingId,
                MeetingDescription = meeting.MeetingDescription,
                MeetingRoomId = meeting.MeetingRoomId,
                CreateMoment = meeting.CreateMoment,
                StartMoment = meeting.StartMoment,
                EndMoment = meeting.EndMoment,
                Owner = fullName,
                OwnerId = meeting.Owner,
                Cancelled = meeting.Cancelled,
                Attendees = meeting.Attendees,
                MeetingRoom = meeting.MeetingRoom,
                OkToEdit = (meeting.Owner == myUser.Id)
            };

            return View(mvm);
        }

        // POST: Meeting/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var myUser = await _userManager.GetUserAsync(User);

            await _meetingsService.DeleteMeetingAsync(id, myUser);

            return RedirectToAction(nameof(Index));
        }

        private bool MeetingExists(int id)
        {
            return (_meetingsService.GetMeetingAsync(id)) == null ? false : true;
        }
    }
}
