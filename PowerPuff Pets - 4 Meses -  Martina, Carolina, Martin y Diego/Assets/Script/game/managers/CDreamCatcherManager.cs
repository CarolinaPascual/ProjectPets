using UnityEngine;
using System.Collections;

public class CDreamCatcherManager : CManager {

    private static CDreamCatcherManager mInst = null;

    public CDreamCatcherManager()
    {
        registerSingleton();
    }

    public static CDreamCatcherManager inst()
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
            throw new UnityException("ERROR: Cannot create another instance of singleton class CDreamCatcherManager.");
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
