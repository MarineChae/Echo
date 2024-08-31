using System;
using UnityEngine;

//노드가 실행 될 수 있는지 확인하는 함수, 컨디션확인

public class DecoratorNode : BaseNode
{
    public Func<ReturnCode> condition;


    public override ReturnCode Tick()
    {
        return condition();
    }

    public DecoratorNode(Func<ReturnCode> condition)
    {
        this.condition = condition;
    }

}
