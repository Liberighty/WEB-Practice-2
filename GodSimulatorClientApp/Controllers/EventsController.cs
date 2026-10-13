using GodSimulatorClientApp.Data;
using GodSimulatorClientApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Diagnostics;
using System.Text.Json;

namespace GodSimulatorClientApp.Controllers
{
    public class EventsController : Controller
    {
        #region движок. НЕ ТРОГАЙ.
        private readonly ILogger<EventsController> _logger;                         //подгрузка логгера. не используется, боюсь удалить//

        public EventsController(ILogger<EventsController> logger)
        {
            _logger = logger;
        }
        #endregion


        #region Action. Тут основная логика работы

        [HttpGet]                                                     //основная страница событий//
        public async Task<IActionResult> Action()
        {

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:5288/api/EventsServerLogic"); 
            var responseText = await response.Content.ReadAsStringAsync();

            var jsonOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var responseData = JsonSerializer.Deserialize<List<WorldEvent>>(responseText, jsonOptions);

            return View(responseData);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string name, string description)
        {

            var baseUrl = "http://localhost:5288/api/EventsServerLogic/search";    
            var queryParams = new Dictionary<string, string?>
               {
                    { "descriptionFinder", description },
                    { "nameFinder", name },
                };

            var finalUri = QueryHelpers.AddQueryString(baseUrl, queryParams);
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, finalUri);
            var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(requestMessage);
            var responseText = await response.Content.ReadAsStringAsync();

            var jsonOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var responseData = JsonSerializer.Deserialize<List<WorldEvent>>(responseText, jsonOptions);


            return View("Action", responseData);
        }
        #endregion

        #region ADD.
        [HttpPost]                                                              //Добавление новых записей//
        public async Task<IActionResult> Add(WorldEventAdd request)
        {

            var httpClient = new HttpClient();
            var response = await httpClient.PutAsJsonAsync("http://localhost:5288/api/EventsServerLogic", request);    

            return RedirectToAction(nameof(Action));
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        #endregion

        #region  REMOVE...
        [HttpGet]                                                              //Удаление записей//
        public async Task<IActionResult> Remove(int Id)
        {

            var httpClient = new HttpClient();
            var response = await httpClient.DeleteAsync($"http://localhost:5288/api/EventsServerLogic?eventId={Id}");    

            return RedirectToAction(nameof(Action));
        }
        #endregion

        #region EDIT.
        //редактирование//
        [HttpGet]                                                 //открытие окна редактируемого обновления//
        public async Task<IActionResult> Edit(int Id)
        {

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"http://localhost:5288/api/EventsServerLogic/{Id}");    
            var responseText = await response.Content.ReadAsStringAsync();

            var jsonOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var responseData = JsonSerializer.Deserialize<WorldEvent>(responseText, jsonOptions);
            return View(responseData);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(WorldEventEdit request)
        {

            var httpClient = new HttpClient();
            var response = await httpClient.PatchAsJsonAsync("http://localhost:5288/api/EventsServerLogic", request);

            ;
            return RedirectToAction(nameof(Action));
        }

        #endregion

        #region Перворождённые методы Privacy и Error.
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion



    }
}
