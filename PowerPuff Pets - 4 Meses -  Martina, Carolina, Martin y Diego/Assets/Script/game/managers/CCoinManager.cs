using UnityEngine;
using System.Collections;

public class CCoinManager : CManager
{
    private static CCoinManager mInst = null;

    public CCoinManager()
    {
        registerSingleton();
    }

    public static CCoinManager inst()
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
            throw new UnityException("ERROR: Cannot create another instance of singleton class CCoinManager.");
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
        CGameObject coin = base.collides(aGameObject);
        return coin;
    }

    override public CGameObject collides(CSprite aSprite)
    {
        CGameObject coin = base.collides(aSprite);
        return coin;
    }

    public void spawnCoins(int aAmount,float aX,float aY)
    {
        CCoin a = new CCoin();
        a.setXYZ(5 * CTileMap.TILE_HEIGHT, 5 * CTileMap.TILE_HEIGHT, 0);
        a.setVelXYZ(0, 0, 800);
        a.setAccelXYZ(0, 0, -1000);
    }
}