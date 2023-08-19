using System.ComponentModel.DataAnnotations;

namespace Services;

public class AppSettings
{
    [Required(ErrorMessage = "The \"Program\" field is required.")]
    public string AppPath { get; set; } = "";
    
    [Required(ErrorMessage = "The \"Working directory\" field is required.")]
    public string WorkingDirectory { get; set; } = "";
    public string Arguments { get; set; } = "";
}