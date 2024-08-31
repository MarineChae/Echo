using System;
using UnityEngine;

//��尡 ���� �� �� �ִ��� Ȯ���ϴ� �Լ�, �����Ȯ��

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
