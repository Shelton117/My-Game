using UnityEngine;

public class Read : MonoBehaviour
{
    [SerializeField]
    private TextAsset dataAsset;
    [SerializeField]
    private TextAsset attackDataAsset;

    void Start()
    {
        readCSV(dataAsset);
        readCSV(attackDataAsset);
    }

    /// <summary>
    /// ��ȡCSV�ļ�
    /// </summary>
    void readCSV(TextAsset textAsset)
    {
        //��ȡcsv�������ļ�
        if (textAsset == null) return;

        string[] data = textAsset.text.Split(","[0]);
        foreach (var dat in data)
        {
            Debug.Log(dat);
        }

        ////��ȡÿһ�е�����
        string[] lineArray = textAsset.text.Split("\r"[0]);
        ////����,�����в��
        string[] lineArray1 = textAsset.text.Split(","[0]);

        //������ά����
        string[][] Array = new string[lineArray.Length][];

        //��csv�е����ݴ����ڶ�λ������
        for (int i = 0; i < lineArray.Length; i++)
        {
            Array[i] = lineArray[i].Split("\r"[0]);
            Debug.Log(Array[i][0]);
        }
    }
}
