using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Lane
{
    Vector2 stPos;
    Vector2 edPos;
    float per;
    float perSpeed;
    int order;
    GameObject obj;

    public Lane(Vector2 st, Vector2 ed, float speed, int layer)
    {
        stPos = st;
        edPos = ed;
        perSpeed = speed / Vector2.Distance(st, ed);
        order = layer;
    }

    public void StartLane(GameObject passObj)
    {
        if (!obj)
        {
            obj = passObj;
            per = 0;
            passObj.GetComponent<SpriteRenderer>().sortingOrder = order;
        }
    }

    public bool IsEmpty()
    {
        return !obj;
    }

    public bool Step()
    {
        if (obj)
        {
            per += perSpeed;
            obj.transform.position = Vector2.Lerp(stPos, edPos, per);
        }

        if (per > 1)
        {
            obj = null;
            return false;
        }
        return true;
    }
    public void StepRoad()
    {
        if (obj)
        {
            per += perSpeed;
            while (per > 1) per -= 1;
            obj.transform.position = Vector2.Lerp(stPos, edPos, per);
        }
    }
}
public class BackgroundManager : SingletonBehaviour<BackgroundManager>
{
    public float roadAngle;
    public Vector2[] lanePos;
    public Vector2[] roadLanePos;
    public int[] layerOrder;
    public GameObject roadPre;
    public GameObject runnerPre;
    public GameObject backPre;
    public float speed;
    public Vector2[] runnerLane;
    public float runnerLaneWidth;
    public float minRegenTime;
    public float maxRegenTime;
    
    GameObject roadObj;
    GameObject playerObj;
    GameObject[] backObj;
    float[] waitingTime;
    int[] inLane;
    Dictionary<string, GameObject> friendObj;
    Dictionary<string, int> friendLanePos;
    Lane[] lanes;
    Lane roadLane;

    Dictionary<string, Sprite> backSprites;
    Dictionary<string, AnimationClip> runnerAnimations;
    Dictionary<string, Sprite> roadSprites;
    
    Vector2 ResetPosByAngle(Vector2 axis, Vector2 origin, float degree)
    {
        if (origin.x == 0) origin.x = axis.x + (origin.y - axis.y) / Mathf.Tan(Mathf.Deg2Rad * degree);
        else if (origin.y == 0) origin.y = axis.y + (origin.x - axis.x) * Mathf.Tan(Mathf.Deg2Rad * degree);
        return origin;
    }

    void Start()
    {
        friendObj = new Dictionary<string, GameObject>();
        friendLanePos = new Dictionary<string, int>();

        backSprites = new Dictionary<string, Sprite>();
        runnerAnimations = new Dictionary<string, AnimationClip>();
        roadSprites = new Dictionary<string, Sprite>();

        roadObj = Instantiate(roadPre);
        playerObj = Instantiate(runnerPre);
        backObj = new GameObject[5];
        for (int i = 0; i < 5; i++) backObj[i] = Instantiate(backPre);

        lanes = new Lane[lanePos.Length / 2];

        for(int i=0; i<lanes.Length; i++)
        {
            lanes[i] = new Lane(lanePos[i * 2], ResetPosByAngle(lanePos[i * 2], lanePos[i * 2 + 1], roadAngle), speed, layerOrder[i]);
        }

        roadLane = new Lane(roadLanePos[0], ResetPosByAngle(roadLanePos[0], roadLanePos[1], roadAngle), speed, 0);
        roadLane.StartLane(roadObj);

        runnerLane[1] = ResetPosByAngle(runnerLane[0], runnerLane[1], roadAngle);

        waitingTime = new float[5];
        inLane = new int[5];
        for (int i = 0; i < 5; i++)
        {
            waitingTime[i] = UnityEngine.Random.Range(minRegenTime, maxRegenTime);
            inLane[i] = -1;
        }
    }

    void Update()
    {
        for (int i = 0; i < 5; i++)
        {
            if(inLane[i] < 0)
            {
                waitingTime[i] -= Time.deltaTime;
                if(waitingTime[i] <= 0)
                {
                    int seed = UnityEngine.Random.Range(1, 6);
                    int p = 0;
                    for (; seed > 0; seed--)
                    {
                        p = (p + 1) % lanes.Length;
                        if (!lanes[p].IsEmpty())
                        {
                            seed++;
                        }
                        
                    }
                    lanes[p].StartLane(backObj[i]);
                    inLane[i] = p;
                }
            }
            else
            {
                if(!lanes[inLane[i]].Step())
                {
                    inLane[i] = -1;
                    waitingTime[i] = UnityEngine.Random.Range(minRegenTime, maxRegenTime);
                }
            }
        }

        roadLane.StepRoad();

        SetRunner();
        if(AuthManager.Instance.CurrentUserId != null && RunManager.Instance.users.ContainsKey(AuthManager.Instance.CurrentUserId))
            foreach (string id in RunManager.Instance.users[AuthManager.Instance.CurrentUserId].friends)
                if (!friendObj.ContainsKey(id) && RunManager.Instance.users.ContainsKey(id))
                    SetRunnerImage(id);
        foreach (string id in friendObj.Keys) SetRunner(id);
    }

    void SetRunner()
    {
        SpriteRenderer sprt;

        sprt = playerObj.GetComponent<SpriteRenderer>();
        sprt.sortingOrder = 15;
        playerObj.transform.position = Vector2.Lerp(runnerLane[0], runnerLane[1], 0.5f);
        playerObj.GetComponentInChildren<TextMesh>().text = RunManager.Instance.MeterForm((int)RunManager.Instance.users[AuthManager.Instance.CurrentUserId].score);

    }

    public void SetRunner(string id)
    {
        if (RunManager.Instance.users.ContainsKey(id))
        {
            float realScore = RunManager.Instance.users[id].score;
            float realSpeed = 3f;
            float runTime = 0f;
            foreach (string item in RunManager.Instance.users[id].items)
                if (ItemManager.Instance.itemlist.ContainsKey(item)) realSpeed += ItemManager.Instance.itemlist[item].PlusRSpeed;

            string lastOnline = RunManager.Instance.users[id].lastOnline;


            DateTime last = DateTime.Parse(lastOnline);
            DateTime present = DateTime.Parse(DateTime.Now.ToString("s"));

            TimeSpan span = present.Subtract(last);

            if (span.TotalSeconds >= 300) runTime =  300;

            else runTime = (float)span.TotalSeconds;

            realScore += realSpeed * runTime;

            float per = (realScore - RunManager.Instance.users[AuthManager.Instance.CurrentUserId].score) / RunManager.Instance.FriendViewDist / 2 + 0.5f;
            SpriteRenderer sprt;

            sprt = friendObj[id].GetComponent<SpriteRenderer>();

            if (per >= 0 && per <= 1)
            {
                friendObj[id].SetActive(true);
                sprt.sortingOrder = friendLanePos[id] + 10;
                friendObj[id].transform.position = Vector2.Lerp(runnerLane[0], runnerLane[1], per) -
                    new Vector2(Mathf.Cos(Mathf.Deg2Rad * (roadAngle + 90)), Mathf.Sin(Mathf.Deg2Rad * (roadAngle + 90)))
                    * runnerLaneWidth / 10 * (friendLanePos[id] - 5);
                

                friendObj[id].GetComponentInChildren<TextMesh>().text = RunManager.Instance.MeterForm((int)realScore);
            }
            else
            {
                friendObj[id].SetActive(false);
            }
        }
    }

    void AddFriendObj(string id)
    {
        if(!friendObj.ContainsKey(id))
        {
            friendObj.Add(id, Instantiate(runnerPre));
            int rnd = UnityEngine.Random.Range(1, 9);
            if (rnd > 4) rnd++;
            friendLanePos.Add(id, rnd);
        }
    }
    
    public void SetRunnerImage()
    {
        string target = "";

        if (ItemManager.Instance.ClothQ.Count > 0) target = ItemManager.Instance.ClothQ[0];
        else target = "Normal";

        if (!runnerAnimations.ContainsKey(target)) runnerAnimations.Add(target, Resources.Load<AnimationClip>("Sprites/Runner/" + target));

        AnimatorOverrideController aoc = new AnimatorOverrideController(playerObj.GetComponent<Animator>().runtimeAnimatorController);
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        foreach (var a in aoc.animationClips)
            anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, runnerAnimations[target]));
        aoc.ApplyOverrides(anims);
        playerObj.GetComponent<Animator>().runtimeAnimatorController = aoc;

        foreach(MeshRenderer msh in playerObj.GetComponentsInChildren<MeshRenderer>()) msh.sortingOrder = 100;
    }
    public void SetRunnerImage(string id)
    {
        if (RunManager.Instance.users.ContainsKey(id))
        {
            AddFriendObj(id);
            string target = "";

            foreach (string item in RunManager.Instance.users[id].equippedItems) if (ItemManager.Instance.itemlist[item].property == Item.Property.Cloth) target = item;
            else target = "Normal";

            if (!runnerAnimations.ContainsKey(target)) runnerAnimations.Add(target, Resources.Load<AnimationClip>("Sprites/Runner/" + target));

            AnimatorOverrideController aoc = new AnimatorOverrideController(friendObj[id].GetComponent<Animator>().runtimeAnimatorController);
            var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
            foreach (var a in aoc.animationClips)
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, runnerAnimations[target]));
            aoc.ApplyOverrides(anims);
            friendObj[id].GetComponent<Animator>().runtimeAnimatorController = aoc;

            foreach (MeshRenderer msh in friendObj[id].GetComponentsInChildren<MeshRenderer>()) msh.sortingOrder = 100;
        }
    }
    public void SetBackgroundImage()
    {
        string target = "";
        for (int i = 0; i < Mathf.Min(ItemManager.Instance.BackGroundQ.Count, 5); i++)
        {
            target = ItemManager.Instance.BackGroundQ[i];
            if (!backSprites.ContainsKey(target)) backSprites.Add(target, Resources.Load<Sprite>("Sprites/Background/" + target));
            Debug.Log(backSprites[target]);
            backObj[i].GetComponent<SpriteRenderer>().sprite = backSprites[target];
            backObj[i].GetComponent<SpriteRenderer>().enabled = true;
        }
        for(int i = ItemManager.Instance.BackGroundQ.Count; i < 5; i++)
        {
            backObj[i].GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    public void SetRoadImage()
    {
        string target = "";

        if (ItemManager.Instance.RoadQ.Count > 0) target = ItemManager.Instance.RoadQ[0];
        else target = "Normal";

        if (!roadSprites.ContainsKey(target)) roadSprites.Add(target, Resources.Load<Sprite>("Sprites/Road/" + target));
        roadObj.GetComponent<SpriteRenderer>().sprite = roadSprites[target];
    }
}
