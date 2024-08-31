using System;
using UnityEngine;

//실제 액션을 처리하는 노드
public class TaskNode : BaseNode
{

    [SerializeField]
    public Func<ReturnCode> action;



    public override ReturnCode Tick()
    {
        return action();
    }
    public TaskNode(Func<ReturnCode> action)
    {
        this.action = action;
    }
}
