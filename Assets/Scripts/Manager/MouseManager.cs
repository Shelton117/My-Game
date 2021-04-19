using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel;

/// <summary>
/// 鼠标管理类
/// </summary>
public class MouseManager : Singleton<MouseManager>
{
    /// <summary>
    /// 屏幕射线触碰的信息
    /// </summary>
    private RaycastHit hitInfo;
    /// <summary>
    /// 存放玩家（点击地面）信息的事件
    /// </summary>
    public event Action<Vector3> OnMouseClicked;
    /// <summary>
    /// 存放玩家（点击敌人时）的事件
    /// </summary>
    public event Action<GameObject> OnEnemyClicked;
    /// <summary>
    /// 游戏中的鼠标光标图片
    /// </summary>
    public Texture2D point, doorWay, attack, target, arrow;

    protected override void Awake()
    {
        base.Awake();
        //DontDestroyOnLoad(this);
    }

    void Update()
    {
        SetCursorTexture();
        MouseControl();
    }

    #region 私有方法

    /// <summary>
    /// 切换鼠标贴图
    /// </summary>
    private void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo))
        {
            //切换鼠标光标
            switch (hitInfo.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Enemy":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Attackable":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }
    }

    /// <summary>
    /// 监听按钮
    /// </summary>
    private void MouseControl()
    {
        if (Input.GetMouseButtonDown(0) && hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.CompareTag("Ground"))
            {
                OnMouseClicked?.Invoke(hitInfo.point);
            }

            if (hitInfo.collider.gameObject.CompareTag("Enemy"))
            {
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
            }

            if (hitInfo.collider.gameObject.CompareTag("Attckable"))
            {
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
            }
        }
    }

    #endregion
}
