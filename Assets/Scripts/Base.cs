using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public enum BoxStat
{
    None,//空
    Number,//数字
    Bomb,//炸弹
    Flag,//棋子
}
public enum ClickStat
{
    None,//没点击的
    flag,//点成旗子的
    open,//打开了的
}

public class Base : MonoBehaviour,IPointerClickHandler {
    public int x;
    public int y;
    Image image;
    Text text;
    int BomNum;
    bool isClick=false;

    public BoxStat Bs=BoxStat.None;
    public ClickStat Cs = ClickStat.None;
    BoxStat TempBs;
    public List<Base> aroundBaseList = new List<Base>();
    private void Awake()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
        InitManger.Instance.BackFunc += Init;
    }
    // Use this for initialization
    void Start () {
	
	}
    void Init()
    {
        BomNum = 0;
        //设置默认图片
        //关闭文字显示
        text.enabled = false;
        //获取周围格子
        if (Bs!=BoxStat.Bomb)
        {
            aroundBaseList = InitManger.Instance.ReturnBaseList(x, y);
            foreach (var item in aroundBaseList)
            {
                if (item.Bs == BoxStat.Bomb)
                {
                    BomNum++;
                }
            }
            //根据雷数改变格子状态
            if (BomNum != 0)
            {
                Bs = BoxStat.Number;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //TODO 点击事件
        if (PointerEventData.InputButton.Left==eventData.button&&Bs!=BoxStat.Flag)
        {
            //左键单击
            Cs = ClickStat.open;
            StartCoroutine(ClickEvent());
        }
        else if (PointerEventData.InputButton.Right == eventData.button)
        {
            //右键点击
            if (Cs == ClickStat.flag)
            {
                //TODO 回复成为点击图片
                image.color = Color.white;
                InitManger.Instance.FlagList.Remove(this);
            }
            else if (Cs == ClickStat.None)
            {
                Cs = ClickStat.flag;
                //TODO 变成棋子图片
                image.color = Color.blue;
                InitManger.Instance.FlagList.Add(this);
            }
        }
        

    }
    public IEnumerator ClickEvent()
    {
        yield return new WaitForFixedUpdate();
        if (isClick==false)
        {
            isClick = true;
            if (Bs == BoxStat.None)
            {
                //周围格子都没有雷 周围所有都格子都打开
                foreach (var item in aroundBaseList)
                {
                    StartCoroutine(item.ClickEvent());
                }
                //TODO 图片
                image.color = Color.black;
            }
            else if (Bs == BoxStat.Number)
            {
                //如果点中数字 TODO 
                text.enabled = true;
                text.text = BomNum.ToString();
                image.color=Color.white;
            }
            else
            {
                //如果是雷
                //TODO 游戏结束
                image.color = Color.red;
            }
        }
        
        
    }

}
