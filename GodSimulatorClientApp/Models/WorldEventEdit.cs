using System.ComponentModel.DataAnnotations;

namespace GodSimulatorClientApp.Data
{
    public class WorldEventAdd
    {
        public string EventName { get; set; } = "";
        public string EventDescription { get; set; } = "";
        public DateTime EventBeginning { get; set; }
        public int EventDuration {  get; set; }

    }
}
