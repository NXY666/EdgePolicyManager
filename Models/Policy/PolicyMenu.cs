using System.Collections.Generic;

namespace PolicyManager.Models.Policy;

public class PolicyMenu : List<PolicyMenuItem>
{
}

public class PolicyMenuItem
{
    public string Identifier { get; set; }
    public string Name { get; set; }
    public string Icon { get; set; }
    public List<string> Items { get; set; }
}