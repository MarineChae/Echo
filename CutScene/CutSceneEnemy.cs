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
        //일정시간마다 에코처리 컷신에서 사용하는 ai 및 플레이어는
        //자연스러운 연출을 위해 걸음마다가 아닌 시간간격을 사용
        if (time > 1.0f)
        {
            time = 0.0f;      
            var t1 = GameManager.Instance.ObjectPool.Get(EnemyEcho);
            t1.Activate(transform);
            GameManager.Instance.ObjectPool.Return(t1);
        }
    }
    public void LateUpdateWork() { }

    //컷신에서 에코를 변경할때 사용
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
