namespace GodSimulatorAPI.Models
{
    public class EventsAdd
    {
        public string EventName { get; set; } = "";
        public string EventDescription { get; set; } = "";
        public DateTime EventBeginning { get; set; }
        public int EventDuration { get; set; }
    }
}
