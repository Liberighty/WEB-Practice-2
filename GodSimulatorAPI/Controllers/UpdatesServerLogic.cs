using GodSimulatorAPI.Data;
using GodSimulatorAPI.Models;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

using WorldUpdate = GodSimulatorAPI.Data.Update;
/*а ведь во время работы над MVC я даже задумался, стоит ли называть объект "Update"... с таким же успехом мог назвать его "String"...
 вообщем теперь этот костыль будет висеть тут как неоднозначное напоминание о том, что интуиция дана человеку не просто так*/

namespace GodSimulatorAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UpdatesServerLogic : ControllerBase
    {

        private readonly ILogger<UpdatesServerLogic> _logger;
       
        private readonly MyApplicationContext _context;

        public UpdatesServerLogic(ILogger<UpdatesServerLogic> logger, MyApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }
    

        [HttpGet(Name = "GetWorldUpdate")]          /*отображение имеющегося списка из бдшки*/
        public List<WorldUpdate> Get()
        {
            _logger.LogInformation("Получаю список обновлений...");

            return _context.Updates.OrderByDescending(x => x.Id).ToList();
        }

        //[HttpPost(Name = "PostUpdates")]        /*УСТАРЕВШИЙ поиск по бдшке*/
        //public IEnumerable<WorldUpdate> Post(string? description, string? ver)
        //{

        //    _logger.LogInformation("Поиск по описанию и версии...");
            
        //    var query = _context.Updates.AsQueryable();

        //    if (!string.IsNullOrEmpty(ver))
        //    {
        //        query = query.Where(x => x.Version.Contains(ver));  /*АХХАХАХА ИМЯ "version" ТОЖЕ ЗАРЕЗЕРВИРОВАННО УЖЕ В VS... теперь это "ver"*/
        //    }
        //    if (!string.IsNullOrEmpty(description))
        //    {
        //        query = query.Where(x => x.Description.Contains(description));
        //    }

        //    var updates = query.OrderByDescending(x => x.Id).ToList(); 

        //    return updates;
        //}


        [HttpGet("search")]                                                                     /*АКТУАЛЬНЫЙ поиск по бдшке*/
        public List<WorldUpdate> Get([FromQuery] string? descriptionFinder, [FromQuery] string? versionFinder)
        {
            return _context.Updates
                .Where(x => (descriptionFinder == null || x.Description.Contains(descriptionFinder)))
                .Where(x => (versionFinder == null || x.Version.Contains(versionFinder)))
                .OrderByDescending(x => x.Id)
                .ToList();
        }
        

        [HttpPut(Name = "PutWorldUpdate")]          /*добавление нового*/
        public WorldUpdate Put(UpdatesAdd requestUpdate)
        {
            _logger.LogInformation("Добавить обновление...");

            var newUpdate = new WorldUpdate
            {
                Description = requestUpdate.Description,
                Version = requestUpdate.Version
            };
           _context.Updates.Add(newUpdate);
           _context.SaveChanges();

           return newUpdate;
        }

        [HttpDelete(Name = "DeleteWorldUpdate")]          /*удаление из бдшки*/
        public IActionResult Delete(int updateId)
        {
            _logger.LogInformation("Удалить обновление...");

            var deleteUpdate = _context.Updates.FirstOrDefault(x => x.Id == updateId);
            if (deleteUpdate != null)
            {
               _context.Remove(deleteUpdate);
               _context.SaveChanges();

              return Ok();
             }

           return NotFound();
        }

        [HttpPatch(Name = "PatchWorldUpdate")]        /*редактирование данных в бд*/
        public IActionResult Patch(WorldUpdate redactedUpdate)
        {
            _logger.LogInformation("Редактирование данных в базе...");

            var proceedRedaction = _context.Updates.FirstOrDefault(x => x.Id == redactedUpdate.Id);
            if (proceedRedaction != null)
               {
                proceedRedaction.Version = redactedUpdate.Version;
                proceedRedaction.Description = redactedUpdate.Description;


                _context.Update(proceedRedaction);
                _context.SaveChanges();

                return Ok();
               }

            return NotFound();
        }
        
        [HttpGet("{id}")]                          /*редактирование конкретного пользователя в бд*/
        public ActionResult<WorldUpdate> Get(int id)
        {
            var reqId = _context.Updates.FirstOrDefault(x => x.Id == id);
            if (reqId == null)
                return NotFound();
            return reqId;
        }
    }
}