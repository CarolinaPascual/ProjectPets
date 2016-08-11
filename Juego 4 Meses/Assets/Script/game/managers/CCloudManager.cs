using UnityEngine;
using System.Collections;

public class CCloudManager : CManager
{
    private static CCloudManager mInst = null;

    public CCloudManager()
    {
        registerSingleton();
    }

    public static CCloudManager inst()
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
            throw new UnityException("ERROR: Cannot create another instance of singleton class CCloudManager.");
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