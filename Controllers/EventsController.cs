using Microsoft.AspNetCore.Mvc;
using SchuifJeAanAPI.Models;

namespace SchuifJeAanAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private static List<Event> _events = new List<Event>();
        private static int _nextEventId = 1;

        [HttpPost("Create")]
        public ActionResult Create([FromForm] Event eventModel)
        {
            eventModel.EventId = _nextEventId++;
            _events.Add(eventModel);
            return Ok();
        }

        [HttpGet("GetMine")]
        public ActionResult<List<Event>> GetMine([FromQuery] int ownerId)
        {
            var myEvents = _events.Where(e => e.UserId == ownerId).ToList();
            return Ok(myEvents);
        }

        [HttpGet("GetJoined")]
        public ActionResult<List<Event>> GetJoined([FromQuery] int userId)
        {
            var joinedEvents = _events.Where(e => e.JoinedUserIds.Contains(userId)).ToList();
            return Ok(joinedEvents);
        }

        [HttpGet("GetOthers")]
        public ActionResult<List<Event>> GetOthers([FromQuery] int userId)
        {
            var otherEvents = _events.Where(e => 
                e.UserId != userId && 
                !e.JoinedUserIds.Contains(userId)
            ).ToList();
            return Ok(otherEvents);
        }

        [HttpPost("Join")]
        public ActionResult Join([FromForm] int userId, [FromForm] int eventId)
        {
            var eventModel = _events.FirstOrDefault(e => e.EventId == eventId);
            if (eventModel == null)
                return NotFound();

            if (eventModel.JoinedUserIds.Count >= eventModel.MaxGuestCount)
                return BadRequest("Event is full");

            if (!eventModel.JoinedUserIds.Contains(userId))
            {
                eventModel.JoinedUserIds.Add(userId);
            }

            return Ok();
        }

        [HttpDelete("Delete")]
        public ActionResult Delete([FromQuery] int id)
        {
            var eventModel = _events.FirstOrDefault(e => e.EventId == id);
            if (eventModel == null)
                return NotFound();

            _events.Remove(eventModel);
            return Ok();
        }

        [HttpGet("Leave")]
        public ActionResult Leave([FromQuery] int userId, [FromQuery] int eventId)
        {
            var eventModel = _events.FirstOrDefault(e => e.EventId == eventId);
            if (eventModel == null)
                return NotFound();

            if (eventModel.JoinedUserIds.Contains(userId))
            {
                eventModel.JoinedUserIds.Remove(userId);
            }

            return Ok();
        }
    }
}
