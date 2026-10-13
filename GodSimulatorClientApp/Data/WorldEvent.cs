using System.ComponentModel.DataAnnotations;

namespace GodSimulatorClientApp.Data
{
    public class WorldEvent
    {
        [Key]
        public int Id { get; set; }
        public string EventName { get; set; } = "";
        public string EventDescription { get; set; } = "";
        public DateTime EventBeginning { get; set; }
        public int EventDuration {  get; set; }

        public DateTime EventEnding => EventBeginning.AddDays(EventDuration);
    }
}
