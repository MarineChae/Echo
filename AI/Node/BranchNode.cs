using System.Collections.Generic;

//�б⸦ ���� �� �ִ� ���� �� ��带 �Ļ��Ͽ� ����ؾ��Ѵ�
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
