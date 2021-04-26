using UnityEngine;

/// <summary>
/// ����/�������ݵĹ�����
/// </summary>
public class SaveManager : Singleton<SaveManager>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePlayerData();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadPlayerData();
        }
    }

    #region ���ŷ����ӿ�

    /// <summary>
    /// ��������
    /// </summary>
    public void SavePlayerData()
    {
        Save(GameManager.Instance.PlayerStats.characterData, GameManager.Instance.PlayerStats.characterData.name);
    }

    /// <summary>
    /// ��������
    /// </summary>
    public void LoadPlayerData()
    {
        Load(GameManager.Instance.PlayerStats.characterData, GameManager.Instance.PlayerStats.characterData.name);
    }

    #endregion

    #region ˽�з���

    private void Save(Object data, string key)
    {
        var jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.Save();
    }

    private void Load(Object data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }

    #endregion


}
