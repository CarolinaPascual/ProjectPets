using UnityEngine;
using System.Collections;

public class CBulletManager : CManager
{
	private static CBulletManager mInst = null;

	public CBulletManager()
	{
		registerSingleton ();
	}

	public static CBulletManager inst()
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
			throw new UnityException( "ERROR: Cannot create another instance of singleton class CBulletManager.");
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
}