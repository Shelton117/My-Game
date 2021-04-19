using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel;

/// <summary>
/// ��������
/// </summary>
public class MouseManager : Singleton<MouseManager>
{
    /// <summary>
    /// ��Ļ���ߴ�������Ϣ
    /// </summary>
    private RaycastHit hitInfo;
    /// <summary>
    /// �����ң�������棩��Ϣ���¼�
    /// </summary>
    public event Action<Vector3> OnMouseClicked;
    /// <summary>
    /// �����ң��������ʱ�����¼�
    /// </summary>
    public event Action<GameObject> OnEnemyClicked;
    /// <summary>
    /// ��Ϸ�е������ͼƬ
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

    #region ˽�з���

    /// <summary>
    /// �л������ͼ
    /// </summary>
    private void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo))
        {
            //�л������
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
    /// ������ť
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
