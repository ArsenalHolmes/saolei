using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public enum BoxStat
{
    None,
    Number,
    Bomb,
    Flag,
}

public class Base : MonoBehaviour,IPointerClickHandler {
    public int x;
    public int y;
    Image image;
    Text text;
    int BomNum;
    bool isClick=false;

    public BoxStat Bs=BoxStat.None;
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
        TempBs = Bs;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //TODO 点击事件
        if (PointerEventData.InputButton.Left==eventData.button&&Bs!=BoxStat.Flag)
        {
            //左键单击
            StartCoroutine(ClickEvent());
        }
        else if (PointerEventData.InputButton.Right == eventData.button)
        {
            //右键点击
            if (Bs==BoxStat.Flag)
            {
                Bs = TempBs;
                //TODO 回复图片
                image.color = Color.white;
            }
            else
            {
                Bs = BoxStat.Flag;
                //TODO 旗帜图片
                image.color = Color.blue;
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
                //周围所有都格子都点雷
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
