using UnityEngine;

public class BehaviorTree : MonoBehaviour
{
    //Ʈ���� ��Ʈ ���� �׻� �귱ġ��忡�� �Ļ� �Ǿ����
    public BranchNode rootNode;
    private bool isRun = true;
    public void RunTree()
    {
        if(isRun)
        rootNode.Tick();
    }

    public void ChangeTreeState()
    {
        rootNode.currentChild = 0;
        isRun = !isRun;
    }
    public bool GetRunState()
    {
        return isRun;
    }

}
