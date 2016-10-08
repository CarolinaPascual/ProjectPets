using UnityEngine;
using System.Collections;

public class CWallManager : CManager
{

    
    private static CWallManager mInst = null;

    public CWallManager()
    {
        registerSingleton();
    }

    public static CWallManager inst()
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