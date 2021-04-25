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
    /// 读取CSV文件
    /// </summary>
    void readCSV(TextAsset textAsset)
    {
        //读取csv二进制文件
        if (textAsset == null) return;

        string[] data = textAsset.text.Split(","[0]);
        foreach (var dat in data)
        {
            Debug.Log(dat);
        }

        ////读取每一行的内容
        string[] lineArray = textAsset.text.Split("\r"[0]);
        ////按‘,’进行拆分
        string[] lineArray1 = textAsset.text.Split(","[0]);

        //创建二维数组
        string[][] Array = new string[lineArray.Length][];

        //把csv中的数据储存在二位数组中
        for (int i = 0; i < lineArray.Length; i++)
        {
            Array[i] = lineArray[i].Split("\r"[0]);
            Debug.Log(Array[i][0]);
        }
    }
}
