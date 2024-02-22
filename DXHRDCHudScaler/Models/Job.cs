using System;

namespace DXHRDCHudScaler.Models;

public class Job(string name, Action action)
{
    public string Name { get; } = name;
    public Action Action { get; } = action;
}
