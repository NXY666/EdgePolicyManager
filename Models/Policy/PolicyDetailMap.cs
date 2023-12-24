using System.Collections.Generic;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace PolicyManager.Models.Policy;

public class PolicyDetailMap : Dictionary<string, PolicyDetail>;

public class PolicyDetail
{
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public string SupportVersion { get; set; }
    public string DataType { get; set; }
    public List<PolicyDataOption> DataOptions { get; set; }
    public string DynamicRefresh { get; set; }
    public PolicyRegistry Registry { get; set; }
}

public class PolicyDataOption
{
    public string Name { get; set; }
    public string Value { get; set; }
}

public class PolicyRegistry
{
    public bool CanMandatory { get; set; }
    public bool CanRecommended { get; set; }
    public string MandatoryPath { get; set; }
    public string RecommendedPath { get; set; }
    public string Name { get; set; }
    public object DefaultValue { get; set; }
    public string ValueKind { get; set; }
}