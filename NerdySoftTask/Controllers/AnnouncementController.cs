using Microsoft.AspNetCore.Mvc;
using NerdySoftTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NerdySoftTask.Controllers
{
    [ApiController]
    [Route("api/announcements")]
    public class AnnouncementController : ControllerBase
    {
        private static List<Announcement> announcements = new List<Announcement>();

        [HttpGet]
        public ActionResult<IEnumerable<Announcement>> GetAnnouncements()
        {
            return Ok(announcements);
        }

        [HttpGet("{id}")]
        public ActionResult<Announcement> GetAnnouncement(int id)
        {
            var announcement = announcements.FirstOrDefault(a => a.Id == id);
            if (announcement == null)
                return NotFound();

            return Ok(announcement);
        }

        [HttpPost]
        public ActionResult<Announcement> AddAnnouncement(Announcement announcement)
        {
            announcement.Id = announcements.Count + 1;
            announcement.DateAdded = DateTime.Now;
            announcements.Add(announcement);

            return CreatedAtAction(nameof(GetAnnouncement), new { id = announcement.Id }, announcement);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAnnouncement(int id, Announcement updatedAnnouncement)
        {
            var announcement = announcements.FirstOrDefault(a => a.Id == id);
            if (announcement == null)
                return NotFound();

            announcement.Title = updatedAnnouncement.Title;
            announcement.Description = updatedAnnouncement.Description;
            announcement.DateAdded = DateTime.Now;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAnnouncement(int id)
        {
            var announcement = announcements.FirstOrDefault(a => a.Id == id);
            if (announcement == null)
                return NotFound();

            announcements.Remove(announcement);
            return NoContent();
        }

        [HttpGet("{id}/similar")]
        public ActionResult<IEnumerable<Announcement>> GetSimilarAnnouncements(int id)
        {
            var announcement = announcements.FirstOrDefault(a => a.Id == id);
            if (announcement == null)
                return NotFound();

            var similarAnnouncements = announcements.Where(a =>
                (a.Title.Contains(announcement.Title, StringComparison.OrdinalIgnoreCase) ||
                 a.Description.Contains(announcement.Description, StringComparison.OrdinalIgnoreCase)) &&
                a.Id != id)
                .Take(3);

            return Ok(similarAnnouncements);
        }
    }

}
