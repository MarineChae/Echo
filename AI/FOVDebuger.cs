using UnityEditor;
using UnityEngine;


#if DEBUG
 //������ �󿡼� AI�� �þ߹����� Ȯ���ϱ����� ������ ��ũ��Ʈ
[CustomEditor(typeof(Enemy))]
public class FOVDebuger : Editor
{
    void OnSceneGUI()
    {
        Enemy fow = (Enemy)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.ViewRadius);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.ViewRadius / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.ViewRadius / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.ViewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.ViewRadius);

        Handles.color = Color.red;
        foreach (Transform visible in fow.TargetPlayer.transform)
        {
            Handles.DrawLine(fow.transform.position, visible.transform.position);
        }
    }
}
#else
public class FOVDebuger 
{
}
#endif