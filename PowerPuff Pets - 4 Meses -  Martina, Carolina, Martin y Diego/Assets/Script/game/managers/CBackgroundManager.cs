using UnityEngine;
using System.Collections;

public class CBackgroundManager : CManager
{
    private static CBackgroundManager mInst = null;

    public CBackgroundManager()
    {
        registerSingleton();
    }

    public static CBackgroundManager inst()
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
            throw new UnityException("ERROR: Cannot create another instance of singleton class CBackgroundManager.");
        }
    }

    override public void update()
    {
        base.update();
    }

    override public void render()
    {
        base.render();
    }

    override public void destroy()
    {
        base.destroy();
        mInst = null;
    }
}