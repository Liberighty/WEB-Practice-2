using GodSimulatorAPI.Data;
using GodSimulatorAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GodSimulatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsServerLogic : ControllerBase
    {
        private readonly MyApplicationContext _context;
        public EventsServerLogic(MyApplicationContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetWorldEvent")]          /*отображение имеющегося списка из бдшки*/
        public List<WorldEvent> Get()
        {
            return _context.WorldEvents.OrderByDescending(x => x.Id).ToList();
        }

        [HttpGet("search")]                                                                     /*АКТУАЛЬНЫЙ поиск по бдшке*/
        public List<WorldEvent> Get([FromQuery] string? descriptionFinder, [FromQuery] string? nameFinder)
        {
            return _context.WorldEvents
                .Where(x => (descriptionFinder == null || x.EventDescription.Contains(descriptionFinder)))
                .Where(x => (nameFinder == null || x.EventName.Contains(nameFinder)))
                .OrderByDescending(x => x.Id)
                .ToList();
        }


        [HttpPut(Name = "PutWorldEvent")]          /*добавление нового*/
        public WorldEvent Put(EventsAdd requestEvent)
        {

            var newEvent = new WorldEvent
            {
                EventDescription = requestEvent.EventDescription,
                EventName = requestEvent.EventName,
                EventBeginning = requestEvent.EventBeginning,
                EventDuration = requestEvent.EventDuration,
            };
            _context.WorldEvents.Add(newEvent);
            _context.SaveChanges();

            return newEvent;
        }

        [HttpDelete(Name = "DeleteWorldEvent")]          /*удаление из бдшки*/
        public IActionResult Delete(int eventId)
        {
            var deleteEvent = _context.WorldEvents.FirstOrDefault(x => x.Id == eventId);
            if (deleteEvent != null)
            {
                _context.Remove(deleteEvent);
                _context.SaveChanges();

                return Ok();
            }

            return NotFound();
        }

        [HttpPatch(Name = "PatchWorldEvent")]        /*редактирование данных в бд*/
        public IActionResult Patch(WorldEvent redactedEvent)
        {
            var proceedRedaction = _context.WorldEvents.FirstOrDefault(x => x.Id == redactedEvent.Id);
            if (proceedRedaction != null)
            {
                proceedRedaction.EventName = redactedEvent.EventName;
                proceedRedaction.EventDescription = redactedEvent.EventDescription;
                proceedRedaction.EventBeginning = redactedEvent.EventBeginning;
                proceedRedaction.EventDuration = redactedEvent.EventDuration;

                _context.Update(proceedRedaction);
                _context.SaveChanges();

                return Ok();
            }

            return NotFound();
        }

        [HttpGet("{id}")]                          /*редактирование конкретного пользователя в бд*/
        public ActionResult<WorldEvent> Get(int id)
        {
            var reqId = _context.WorldEvents.FirstOrDefault(x => x.Id == id);
            if (reqId == null)
                return NotFound();
            return reqId;
        }
    }
}
