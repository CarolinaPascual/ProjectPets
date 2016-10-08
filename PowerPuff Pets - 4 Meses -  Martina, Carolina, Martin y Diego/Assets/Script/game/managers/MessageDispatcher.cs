using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class MessageDispatcher : CManager {


    private static MessageDispatcher mInst = null;
    private List<CTelegram> PriorityQ = new List<CTelegram>();

    public MessageDispatcher()
    {
        registerSingleton();
    }

    public static MessageDispatcher inst()
    {
        return mInst;
    }

    private void registerSingleton()
    {
        if (mInst == null)
        {
            mInst = this;
        }
        else
        {
            throw new UnityException("ERROR: Cannot create another instance of singleton class MessageDispatcher.");
        }
    }

    public void discharge(CGameObject pReceiver, CTelegram msg)
    {
        pReceiver.handleMessage(msg);
    }

    //Sender ID, Receiver ID and Message strings,aDelay in seconds
    public void dispatchMessage(string aSender, string aReceiver, string aMessage,float aDelay)
    {
        //Get the object that matches the ID
        CGameObject pReceiver = CEntityManager.inst().getEntityFromID(aReceiver);

        //Create the telegram
        CTelegram pTelegram = new CTelegram(aSender,aReceiver,aMessage,System.DateTime.Now);

        if(aDelay <= 0.0f)
        {
            discharge(pReceiver, pTelegram);
        }
        else
        {
            DateTime pCurrentTime = System.DateTime.Now;

            pTelegram.mDispatchTime = pCurrentTime.AddSeconds(aDelay);

            PriorityQ.Add(pTelegram);
        }
    }

    public void dispatchDelayedMessage()
    {
        DateTime pCurrentTime = System.DateTime.Now;

        while (PriorityQ.First<CTelegram>().mDispatchTime < pCurrentTime)
        {
            //Read the telegramfrom the front of the queue.
            CTelegram pTelegram = PriorityQ.First<CTelegram>();

            //Find the recipient
            CGameObject pReceiver = CEntityManager.inst().getEntityFromID(pTelegram.mReceiver);

            //send the telegram to the recipient
            discharge(pReceiver, pTelegram);

            //Remove it from queue
            PriorityQ.Remove(pTelegram);
        }
    }

    override public void update()
    {
        dispatchDelayedMessage();
    }
        
}
