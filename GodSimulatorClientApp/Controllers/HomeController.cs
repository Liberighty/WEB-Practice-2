using GodSimulatorClientApp.Data;
using GodSimulatorClientApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Diagnostics;
using System.Text.Json;
using WorldUpdate = GodSimulatorClientApp.Data.Update;

namespace GodSimulatorClientApp.Controllers
{
    public class HomeController : Controller
    {
        #region движок. НЕ ТРОГАЙ.
        private readonly ILogger<HomeController> _logger;                         //подгрузка логгера. не используется, боюсь удалить//

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        #endregion

        #region Index. Первая страница, привествие и экспозиция.

        public IActionResult Index()                             //Приветствие//
        {
            return View();
        }

        #endregion

        #region Action. Тут основная логика работы

        [HttpGet]                                                     //основная страница обновлений//
        public async Task<IActionResult> Action()
        {

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:5288/UpdatesServerLogic");
            var responseText = await response.Content.ReadAsStringAsync();

            var jsonOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var responseData = JsonSerializer.Deserialize<List<WorldUpdate>>(responseText, jsonOptions);
               
            return View(responseData);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string version, string description)
        {

            var baseUrl = "http://localhost:5288/UpdatesServerLogic/search";
            var queryParams = new Dictionary<string, string?>
               {
                    { "descriptionFinder", description },
                    { "versionFinder", version },
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

            var responseData = JsonSerializer.Deserialize<List<WorldUpdate>>(responseText, jsonOptions);


            return View("Action", responseData);
        }
        #endregion

        #region ADD.
        [HttpPost]                                                              //Добавление новых записей//
        public async Task<IActionResult> Add(UpdateClientAdd request)
        {

            var httpClient = new HttpClient();
            var response = await httpClient.PutAsJsonAsync("http://localhost:5288/UpdatesServerLogic", request);

            return RedirectToAction(nameof(Action));
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        #endregion

        #region  REMOVE. Допустим, общественность не должна знать о каком-то острове...
        [HttpGet]                                                              //Удаление записей//
        public async Task<IActionResult> Remove(int Id)
        {

            var httpClient = new HttpClient();
            var response = await httpClient.DeleteAsync($"http://localhost:5288/UpdatesServerLogic?updateId={Id}");

            return RedirectToAction(nameof(Action));
        }
        #endregion

        #region EDIT.
                                                                    //редактирование//
        [HttpGet]                                                 //открытие окна редактируемого обновления//
        public async Task<IActionResult> Edit(int Id)               
        {

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"http://localhost:5288/UpdatesServerLogic/{Id}");
            var responseText = await response.Content.ReadAsStringAsync();

            var jsonOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var responseData = JsonSerializer.Deserialize<WorldUpdate>(responseText, jsonOptions);
            return View(responseData);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateClientEdit request)
        {

            var httpClient = new HttpClient();
            var response = await httpClient.PatchAsJsonAsync("http://localhost:5288/UpdatesServerLogic", request);

;
            return RedirectToAction(nameof(Action));
        }

        #endregion

        #region Очень (!!!) важные мнения

        [HttpGet]
        public IActionResult InternetInsanity()
        {
            var post = MessagesData.GetRandom();
            return Json(new { post });
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
