using UnityEngine;

public class ShootAnim : MonoBehaviour
{
    private EnemyScript enemyScript;
    public GameObject enemyObject;

    void Start()
    {
        enemyScript = enemyObject.GetComponent<EnemyScript>();
        if (enemyScript == null)
        {
            Debug.Log("enemy script is null");
        }
    }

    public void CallShoot()
    {
        //Debug.Log("shot is called");
        enemyScript.StartShooting();
    }
    public void CallStart()
    {
        enemyScript.OnShootAnimationStart();
    }
    public void CallEnd()
    {
        enemyScript.OnShootAnimationEnd();
    }
    public void CallLunge()
    {
        enemyScript.Lunge();
    }
    public void CallDaggerStab()
    {
        enemyScript.DaggerStab();
    }

    public void CallDaggerEnd()
    {
        //enemyScript.DaggerStab();
        CallEnd();
    }
}
