using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class InitManger : MonoBehaviour {

    public int hig=16;
    public int len=16;
    Base[,] baseArr;
    GameObject BasePre;
    Transform Parent;
    public Action BackFunc;

    List<Base> BomList=new List<Base>();
    public List<Base> FlagList = new List<Base>();
    
    public static InitManger Instance;
    private void Awake()
    {
        Instance = this;
        BasePre = Resources.Load<GameObject>("Base");
        Parent = GameObject.Find("Canvas/Panel/Panel").transform;
        baseArr = new Base[hig, len];
        StartCoroutine(InitBase());
    }

    IEnumerator InitBase()
    {
        yield return new WaitForFixedUpdate();
        for (int i = 0; i < len; i++)
        {
            for (int j = 0; j < hig; j++)
            {
                Base b = Instantiate(BasePre).AddComponent<Base>();
                b.transform.SetParent(Parent);
                b.gameObject.name = i + "--" + j;
                baseArr[j, i] = b;
                b.x = i;
                b.y = j;

                if (UnityEngine.Random.Range(0,5)==0)
                {
                    b.Bs = BoxStat.Bomb;
                    BomList.Add(b);
                }
            }
        }
        BackFunc();
    }

    /// <summary>
    /// 根据格子获得周围格子
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public List<Base> ReturnBaseList(int x,int y)
    {
        List<Base> LB = new List<Base>();
        for (int i = -1; i <=1; i++)
        {
            for (int j = -1; j <=1; j++)
            {
                if (x+i>=0&&x+i<len&&y+j>=0&&y+j<hig)
                {
                    if (i==0&&j==0)
                    {
                        continue;
                    }
                    LB.Add(baseArr[y + j, x + i]);
                }
            }
        }
        return LB;
    }

    /// <summary>
    /// 输赢判断
    /// </summary>
    /// <returns></returns>
    public bool WinOrLose()
    {
        if (FlagList.Count!=BomList.Count)
        {
            return false;
        }
        foreach (var item in FlagList)
        {
            if (!BomList.Contains(item))
            {
                //只要有一个不在列表里面。就不对
                return false;
            }
        }
        return true;
    }


    //TODO 后续 当旗子数量和位置相同时。判断输赢
}
