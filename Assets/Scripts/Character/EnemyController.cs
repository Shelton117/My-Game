/// <summary>
/// ���˿�����
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

    #region ʵ�ֽӿ�

    public void EndNotify()
    {
        //���������ı���
        isChase = false;
        isWalk = false;
        isPlayerDead = true;
        attackTarget = null;

        anim.SetBool("Win", true);
    }

    #endregion
}