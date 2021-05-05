/// <summary>
/// 敌人控制器
/// </summary>
public class EnemyController : BaseController, IEndGameObserver
{
    protected override void Start()
    {
        base.Start();
        GameManager.Instance.AddObserver(this);
    }

    //void OnEnable()
    //{
    //    GameManager.Instance.AddObserver(this);
    //}

    void OnDisable()
    {
        if (!GameManager.F_isInitialized) return;
        GameManager.Instance.RemoveObserver(this);
    }

    #region 实现接口

    public void EndNotify()
    {
        //玩家阵亡后的表现
        isChase = false;
        isWalk = false;
        isPlayerDead = true;
        attackTarget = null;

        anim.SetBool("Win", true);
    }

    #endregion
}