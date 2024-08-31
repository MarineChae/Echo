using UnityEditor;
using UnityEngine;


#if DEBUG
 //에디터 상에서 AI의 시야범위를 확인하기위한 디버깅용 스크립트
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