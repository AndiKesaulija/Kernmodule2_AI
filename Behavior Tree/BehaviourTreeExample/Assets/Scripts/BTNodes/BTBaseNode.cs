using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskStatus { Success, Failed, Running }
public abstract class BTBaseNode
{
    public string _name { get; set; }
    public BTBaseNode currNode;
    public abstract TaskStatus Run();
}
