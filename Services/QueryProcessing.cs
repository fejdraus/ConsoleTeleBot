﻿using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Services;

[Index("ConsoleOutput")]
public class ParsingRule
{
    public Guid Id { get; set; }
    public string? ConsoleOutput { get; set; }
    public string? RegexPattern { get; set; }
    public string? Result { get; set; }
    public bool QuietMessage { get; set; }
    
    [NotMapped]
    public bool IsDelete { get; set; }
}