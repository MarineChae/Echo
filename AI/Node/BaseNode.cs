

//behaviortree�� ����ϱ����ؼ� �� �̳Ѱ��� �����ؾ߸� ��밡��
public enum ReturnCode { FAILURE, SUCCESS, RUNNING };

//���� ���� ��ũ��Ʈ�� ���� �ʿ䰡 ���� ������ �⺻Ŭ������ ����
public class BaseNode 
{
    // Start is called before the first frame update

    public virtual ReturnCode Tick()
    {
        return ReturnCode.SUCCESS;
    }


}
