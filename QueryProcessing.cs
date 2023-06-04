namespace ConsoleTeleBot;

public class ParsingRule
{
    public Guid Id { get; set; }
    public string ConsoleOutput { get; set; }
    public string RegexPattern { get; set; }
    public string Result { get; set; }
    public bool QuietMessage { get; set; }
}