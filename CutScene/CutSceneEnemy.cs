using UnityEngine;

public class CutSceneEnemy : MonoBehaviour, IUpdateable
{
    private float time;
    [SerializeField]
    private PoolingType EnemyEcho;
    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, true, false, false);
    }

    private void OnDisable()
    {
        UpdateManager.OffSubscribe(this, true, false, false);
    }

    public void FixedUpdateWork() { }
    public void UpdateWork()
    {
        time += Time.deltaTime;
        //�����ð����� ����ó�� �ƽſ��� ����ϴ� ai �� �÷��̾��
        //�ڿ������� ������ ���� �������ٰ� �ƴ� �ð������� ���
        if (time > 1.0f)
        {
            time = 0.0f;      
            var t1 = GameManager.Instance.ObjectPool.Get(EnemyEcho);
            t1.Activate(transform);
            GameManager.Instance.ObjectPool.Return(t1);
        }
    }
    public void LateUpdateWork() { }

    //�ƽſ��� ���ڸ� �����Ҷ� ���
    public void ChangeEco()
    {
        if (EnemyEcho == PoolingType.PlayerEcho)
        {
            EnemyEcho = PoolingType.NPCEcho;
        }
        else
        {
            EnemyEcho = PoolingType.PlayerEcho;
        }


    }
    public void FootSound()
    {

        GameManager.Instance.SoundManager.PlaySound3D("Sound/walk", transform);

    }
    public void BodyFallSound()
    {

        GameManager.Instance.SoundManager.PlaySound3D("Sound/BodyFall", transform, SoundType.SFX, 0.2f);

    }

}
