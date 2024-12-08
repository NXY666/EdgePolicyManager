using System.Collections.Generic;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace PolicyManager.Models.Policy;

public partial class PolicyMenu : List<PolicyMenuItem>;

public class PolicyMenuItem
{
    public string Identifier { get; set; }
    public string Name { get; set; }
    public string Icon { get; set; }
    public List<string> Items { get; set; }
}