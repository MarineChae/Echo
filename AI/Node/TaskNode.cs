using System;
using UnityEngine;

//���� �׼��� ó���ϴ� ���
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
