

//����Ʈ ���� �Ѱ����� �����ߴٸ� ���ڸ����� ���� ����
//������ ��� ���� �������� �ݺ��Ѵ�
public class SelectNode : BranchNode
{

    public override ReturnCode Tick()
    {

        int icur = currentChild;
        int iListSize = childList.Count;

        for (int iSize = icur; iSize < iListSize; ++iSize)
        {
            ReturnCode State = childList[iSize].Tick();

            currentChild = iSize;

            //�̹� ƽ���� �׼��� �����߰� ������ �ʾҴٸ� 
            //���� ƽ���� �̾ �����ϱ�����
            if (State == ReturnCode.RUNNING)
            {
                return ReturnCode.RUNNING;
            }
            //�������� �ڽ��� ���������� �����ٸ� ���� ƽ���� �������� ù ��° ���� ����
            else if (State == ReturnCode.SUCCESS)
            {
                currentChild = 0;
                return ReturnCode.SUCCESS;
            }

        }

        currentChild = 0;
        return ReturnCode.FAILURE;
    }
}
