using System.Collections.Generic;

//������ ���� �������� �ڽĳ�带 ���� ����
//�ڽ��� �����Ѱ�� �� ���� ���� ����ó��
public class SequenceNode : BranchNode
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
            //������ �ڽ��� �����üũ�� ���� �� ��� �������� ����ó��
            //������ �������� ������ 0�� �ڽĺ��� �����ϱ� ����
            else if (State == ReturnCode.FAILURE)
            {
                currentChild = 0;
                return ReturnCode.FAILURE;
            }

        }
        currentChild = 0;
        return ReturnCode.SUCCESS;
    }






}
