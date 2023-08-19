namespace Services;

public class Scheduler
{
    public string CronExpression { get; set; } = "";
    public DateOnly? StartDate { get; set; }

    /*[StartDateTimeValidator]*/
    public TimeOnly? StartTime { get; set; }
}