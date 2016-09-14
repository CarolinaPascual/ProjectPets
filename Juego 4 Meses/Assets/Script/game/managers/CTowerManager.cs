using UnityEngine;
using System.Collections;

public class CTowerManager : CManager
{
    private static CTowerManager mInst = null;

    public CTowerManager()
    {
        registerSingleton();
    }

    public static CTowerManager inst()
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
            throw new UnityException("ERROR: Cannot create another instance of singleton class CTowerManager.");
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

    override public CGameObject collides(CGameObject aGameObject)
    {
        CGameObject tower = base.collides(aGameObject);
        return tower;
    }

    override public CGameObject collides(CSprite aSprite)
    {
        CGameObject tower = base.collides(aSprite);
        return tower;
    }
}