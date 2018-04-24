/** 
 * 这个文件是用来控制主游戏场景的 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction
{
    public ActionMode mode { get; set; }

    public IActionManager actionManager { get; set; }

    public ScoreRecorder scoreRecorder { get; set; }

    private int diskNumber; //一回合要发射的飞碟总数
    public Queue<GameObject> diskQueue = new Queue<GameObject>(); //要发射的飞碟队列

    public int round = 3; //总回合数
    private int currentRound = -1; //当前回合

    private float time = 0;

    private GameState gameState = GameState.START;

    void Awake()
    {
        SSDirector director = SSDirector.getInstance();
        director.currentSceneController = this;
        diskNumber = 10;
        this.gameObject.AddComponent<ScoreRecorder>();
        this.gameObject.AddComponent<DiskFactory>();
        this.gameObject.AddComponent<CCFlyActionFactory>();
        mode = ActionMode.NOTSET;
        scoreRecorder = Singleton<ScoreRecorder>.Instance;
        director.currentSceneController.LoadResources();
    }

    private void Update()
    {
        if (mode != ActionMode.NOTSET && actionManager != null)
        {
            if (actionManager.getDiskNumber() == 0 && gameState == GameState.RUNNING)
            {
                gameState = GameState.ROUND_FINISH;
            }
            if (actionManager.getDiskNumber() == 0 && gameState == GameState.ROUND_START)
            {
                if (currentRound == 2)
                {
                    scoreRecorder.Reset();
                }
                NextRound();
                actionManager.setDiskNumber(10);
                gameState = GameState.RUNNING;
            }

            time += Time.deltaTime; //每个round飞碟发射的时间间隔不同
            if (time > 2.5 - currentRound)
            {
                ThrowDisk();
                time = 0;
            }
        }
    }

    private void NextRound()
    {
        currentRound = (currentRound + 1) % round;
        DiskFactory df = Singleton<DiskFactory>.Instance;
        for (int i = 0; i < diskNumber; i++)
        {
            diskQueue.Enqueue(df.GetDisk(currentRound, mode)); //获得该回合的飞碟队列
        }
        actionManager.StartThrow(diskQueue);
    }

    void ThrowDisk()
    {
        if (diskQueue.Count != 0)
        {
            GameObject disk = diskQueue.Dequeue();

            //随机确定飞碟出现的位置 
            Vector3 position = new Vector3(0, 0, 0);
            float y = UnityEngine.Random.Range(0f, 5f);
            position = new Vector3(-disk.GetComponent<DiskData>().direction.x * 8, y, 0);
            disk.transform.position = position;

            disk.SetActive(true);
        }

    }

    public void LoadResources()
    {
        GameObject ground = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Ground"));
    }

    public int GetScore()
    {
        return scoreRecorder.score;
    }

    public GameState getGameState()
    {
        return gameState;
    }

    public void setGameState(GameState gs)
    {
        gameState = gs;
    }

    public void hit(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);

        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];

            if (hit.collider.gameObject.GetComponent<DiskData>() != null)
            {
                scoreRecorder.Record(hit.collider.gameObject);
                hit.collider.gameObject.transform.position = new Vector3(0, -5, 0); //飞碟被击中，就移到地面之下
            }
        }
    }

    public ActionMode getMode()
    {
        return mode;
    }

    public void setMode(ActionMode m)
    {

        if (m == ActionMode.KINEMATIC)
        {
            this.gameObject.AddComponent<KinematicActionManager>();
        }
        else
        {
            this.gameObject.AddComponent<PhysicsActionManager>();
        }
        mode = m;
    }

}