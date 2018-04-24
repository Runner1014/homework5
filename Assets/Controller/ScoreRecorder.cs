using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRecorder : MonoBehaviour
{

    public int score; //总分

    private Dictionary<Color, int> scoreTable = new Dictionary<Color, int>(); //得分规则表，每种飞碟的颜色对应一个分数 

    // Use this for initialization  
    void Start()
    {
        score = 0;
        scoreTable.Add(Color.blue, 1);
        scoreTable.Add(Color.red, 2);
        scoreTable.Add(Color.black, 4);
    }

    public void Record(GameObject disk)
    {
        score += scoreTable[disk.GetComponent<DiskData>().color];
    }

    public void Reset()
    {
        score = 0;
    }
}