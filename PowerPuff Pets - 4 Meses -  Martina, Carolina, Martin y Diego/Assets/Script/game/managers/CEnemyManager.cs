using UnityEngine;
using System.Collections;

public class CEnemyManager : CManager
{
	private static CEnemyManager mInst = null;
	
	public CEnemyManager()
	{
		registerSingleton ();
	}
	
	public static CEnemyManager inst()
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
			throw new UnityException( "ERROR: Cannot create another instance of singleton class CEnemyManager.");
		}
	}
	
	override public void update()
	{
		base.update ();
	}
	
	override public void render()
	{
		base.render ();
	}
	
	override public void destroy()
	{
		base.destroy ();
		mInst = null;
	}

    override public CGameObject collides(CGameObject aGameObject)
    {
        return base.collides(aGameObject);
        
    }

    override public CGameObject collides(CSprite aSprite)
    {
        return base.collides(aSprite);
       
    }

    public override CGameObject inSight(CGameObject aGameObject)
    {
        return base.inSight(aGameObject);
    }
}