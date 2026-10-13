using System.ComponentModel.DataAnnotations;

namespace GodSimulatorClientApp.Data
{
    public class WorldEventEdit
    {
        public int Id { get; set; }
        public string EventName { get; set; } = "";
        public string EventDescription { get; set; } = "";
        public DateTime EventBeginning { get; set; }
        public int EventDuration {  get; set; }

    }
}
