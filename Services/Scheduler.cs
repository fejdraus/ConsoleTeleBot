namespace Services;

public class Scheduler
{
    public string CronExpression { get; set; } = "";
    public DateTime? StartDate { get; set; }
}