using Microsoft.AspNetCore.Mvc;
using Serene.Modules.Common.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Serene.Modules.Common.Dashboard
{
    [ApiController]
    [Route("api/[controller]")]
    [IgnoreAntiforgeryToken]
    public class ScheduleController : ControllerBase
    {
        // In-memory list to store events
        private static List<ScheduleEventData> ScheduleEvents = new List<ScheduleEventData>
        {
            new ScheduleEventData
            {
                Id = 1,
                Subject = "Team Meeting",
                StartTime = new DateTime(2025, 1, 26, 10, 0, 0),
                EndTime = new DateTime(2025, 1, 26, 11, 0, 0),
                IsAllDay = false,
                Location = "Conference Room 1",
                Description = "Weekly team sync-up meeting."
            },
            new ScheduleEventData
            {
                Id = 2,
                Subject = "Project Deadline",
                StartTime = new DateTime(2025, 1, 27, 15, 0, 0),
                EndTime = new DateTime(2025, 1, 27, 16, 0, 0),
                IsAllDay = false,
                Location = "Online",
                Description = "Submission of the final project deliverables."
            },
            new ScheduleEventData
            {
                Id = 3,
                Subject = "Client Call",
                StartTime = new DateTime(2025, 1, 28, 9, 30, 0),
                EndTime = new DateTime(2025, 1, 28, 10, 30, 0),
                IsAllDay = false,
                Location = "Zoom",
                Description = "Monthly review call with the client."
            },
            new ScheduleEventData
            {
                Id = 4,
                Subject = "Training Session",
                StartTime = new DateTime(2025, 1, 29, 14, 0, 0),
                EndTime = new DateTime(2025, 1, 29, 16, 0, 0),
                IsAllDay = false,
                Location = "Training Room 2",
                Description = "Technical training session on the new software."
            },
            new ScheduleEventData
            {
                Id = 5,
                Subject = "Annual Review",
                StartTime = new DateTime(2025, 1, 30, 11, 0, 0),
                EndTime = new DateTime(2025, 1, 30, 12, 30, 0),
                IsAllDay = false,
                Location = "CEO's Office",
                Description = "Annual performance review meeting with the CEO."
            },
            new ScheduleEventData
            {
                Id = 6,
                Subject = "Code Review",
                StartTime = new DateTime(2025, 1, 31, 10, 0, 0),
                EndTime = new DateTime(2025, 1, 31, 12, 0, 0),
                IsAllDay = false,
                Location = "Development Lab",
                Description = "Review the latest code updates and suggest improvements."
            },
            new ScheduleEventData
            {
                Id = 7,
                Subject = "Marketing Presentation",
                StartTime = new DateTime(2025, 2, 1, 13, 0, 0),
                EndTime = new DateTime(2025, 2, 1, 14, 30, 0),
                IsAllDay = false,
                Location = "Conference Room 3",
                Description = "Presentation on the upcoming marketing campaign."
            },
            new ScheduleEventData
            {
                Id = 8,
                Subject = "Board Meeting",
                StartTime = new DateTime(2025, 2, 2, 9, 0, 0),
                EndTime = new DateTime(2025, 2, 2, 11, 0, 0),
                IsAllDay = false,
                Location = "Board Room",
                Description = "Quarterly board meeting to discuss company performance."
            },
            new ScheduleEventData
            {
                Id = 9,
                Subject = "Team Lunch",
                StartTime = new DateTime(2025, 2, 3, 12, 0, 0),
                EndTime = new DateTime(2025, 2, 3, 13, 30, 0),
                IsAllDay = false,
                Location = "Downtown Cafe",
                Description = "Team lunch to celebrate project completion."
            },
            new ScheduleEventData
            {
                Id = 10,
                Subject = "Webinar: Tech Trends 2025",
                StartTime = new DateTime(2025, 2, 4, 15, 0, 0),
                EndTime = new DateTime(2025, 2, 4, 16, 30, 0),
                IsAllDay = false,
                Location = "Online",
                Description = "Webinar discussing emerging tech trends in 2025."
            }
        };


        [HttpPost("LoadData")]
        public IActionResult LoadData([FromQuery] Params param)
        {
            // Filter events based on the provided date range
            var data = ScheduleEvents
                .Where(e => e.StartTime >= param.StartDate && e.EndTime <= param.EndDate)
                .ToList();

            return Ok(ScheduleEvents);
        }

        [HttpPost("UpdateData")]
        public IActionResult UpdateData([FromBody] JsonElement jsonPayload)
        {
            try
            {
                // Extract the main fields from the payload
                var param = JsonSerializer.Deserialize<EditParams>(jsonPayload.ToString(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Validate the action field
                if (param == null || string.IsNullOrEmpty(param.action))
                {
                    return BadRequest("Invalid payload structure.");
                }

                // Handle different actions
                if (param.action == "insert" || (param.action == "batch" && param.added != null))
                {
                    var addedEvents = param.added ?? new List<ScheduleEventData>();
                    foreach (var newEvent in addedEvents)
                    {
                        var maxId = ScheduleEvents.Any() ? ScheduleEvents.Max(e => e.Id) : 0;
                        newEvent.Id = maxId + 1; // Assign a new unique ID
                        ScheduleEvents.Add(newEvent);
                    }
                }

                if (param.action == "update" || (param.action == "batch" && param.changed != null))
                {
                    foreach (var changedEvent in param.changed ?? new List<ScheduleEventData>())
                    {
                        var existingEvent = ScheduleEvents.FirstOrDefault(e => e.Id == changedEvent.Id);
                        if (existingEvent != null)
                        {
                            existingEvent.StartTime = changedEvent.StartTime;
                            existingEvent.EndTime = changedEvent.EndTime;
                            existingEvent.Subject = changedEvent.Subject;
                            existingEvent.Location = changedEvent.Location;
                            existingEvent.Description = changedEvent.Description;
                            existingEvent.IsAllDay = changedEvent.IsAllDay;
                            existingEvent.StartTimezone = changedEvent.StartTimezone;
                            existingEvent.EndTimezone = changedEvent.EndTimezone;
                            existingEvent.RecurrenceRule = changedEvent.RecurrenceRule;
                            existingEvent.RecurrenceID = changedEvent.RecurrenceID;
                            existingEvent.RecurrenceException = changedEvent.RecurrenceException;
                        }
                    }
                }

                if (param.action == "remove" || (param.action == "batch" && param.deleted != null))
                {
                    foreach (var deletedEvent in param.deleted ?? new List<ScheduleEventData>())
                    {
                        var eventToRemove = ScheduleEvents.FirstOrDefault(e => e.Id == deletedEvent.Id);
                        if (eventToRemove != null)
                        {
                            ScheduleEvents.Remove(eventToRemove);
                        }
                    }
                }

                // Return the updated list of events
                return Ok(ScheduleEvents);
            }
            catch (Exception ex)
            {
                // Handle errors and return a meaningful message
                return BadRequest($"Error processing the request: {ex.Message}");
            }
        }

        public class Params
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }

        public class EditParams
        {
            public string key { get; set; }
            public string action { get; set; }
            public List<ScheduleEventData> added { get; set; }
            public List<ScheduleEventData> changed { get; set; }
            public List<ScheduleEventData> deleted { get; set; }
            public ScheduleEventData value { get; set; }
        }

        public class ScheduleEventData
        {
            public int Id { get; set; }
            public string Subject { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public bool IsAllDay { get; set; }
            public string Location { get; set; }
            public string Description { get; set; }
            public string StartTimezone { get; set; }
            public string EndTimezone { get; set; }
            public string RecurrenceRule { get; set; }
            public int? RecurrenceID { get; set; }
            public string RecurrenceException { get; set; }
        }
    }
}

