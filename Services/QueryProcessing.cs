using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace Services;

[Index("RegexPattern", IsUnique = true)]
public partial class ParsingRule
{
    public Guid Id { get; set; }
    public string RegexPattern { get; set; } = string.Empty;
    public string? Result { get; set; }
    public bool QuietMessage { get; set; }
    
    [NotMapped]
    public bool IsDelete { get; set; }
    
    [NotMapped]
    private Regex? _compiledRegex;
    
    [NotMapped]
    public Regex CompiledRegex => _compiledRegex ??= new Regex(RegexPattern, RegexOptions.Compiled);
}
