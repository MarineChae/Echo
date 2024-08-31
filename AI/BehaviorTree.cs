using UnityEngine;

public class BehaviorTree : MonoBehaviour
{
    //트리의 루트 노드는 항상 브런치노드에서 파생 되어야함
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
