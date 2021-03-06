using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


// ui enter
public class UIEnter : ScreenBaseHandler
{
	public UI_Event mBtnStart;
    public System.Action mStartFunction;

    public override void Init()
    {
        base.Init();
        mBtnStart.onClick = BtnStartOnClick;
    }

    public override void CloseScreen()
    {
        base.CloseScreen();
    }

    public override void OpenScreen()
    {
        base.OpenScreen();
    }

    void BtnStartOnClick(PointerEventData eventData , UI_Event ev)
    {
        if(mStartFunction != null)
        {
            mStartFunction();
        }
    }
}