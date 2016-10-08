using UnityEngine;
using System.Collections;

public class CPlayerManager : CManager
{
    private static CPlayerManager mInst = null;

    public CPlayerManager()
    {
        registerSingleton();
    }

    public static CPlayerManager inst()
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
            throw new UnityException("ERROR: Cannot create another instance of singleton class CPlayerManager.");
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
        CGameObject player = base.collides(aGameObject);
        return player;
    }

    override public CGameObject collides(CSprite aSprite)
    {
        CGameObject player = base.collides(aSprite);
        return player;
    }
}