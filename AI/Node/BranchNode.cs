using System.Collections.Generic;

//분기를 나눌 수 있는 노드는 이 노드를 파생하여 사용해야한다
public class BranchNode : BaseNode
{

    public int currentChild;
    public List<BaseNode> childList;


    public override ReturnCode Tick()
    {
        return ReturnCode.SUCCESS;
    }

    public BranchNode()
    {
        childList = new List<BaseNode>();
    }

}
