using UnityEngine;
using System.Collections;
using System;

public struct CTelegram{

    //Who sent it
    public string mSender;

    //who is set to receive it
    public string mReceiver;

    //Message itself
    public string mMessage;

    //message delay, 0 if it will be sent right away

    public DateTime mDispatchTime;

    public CTelegram(string aSender, string aReceiver, string aMessage, DateTime aDispatchTime)
    {
        mSender = aSender;
        mReceiver = aReceiver;
        mMessage = aMessage;
        mDispatchTime = aDispatchTime;
    }
}
