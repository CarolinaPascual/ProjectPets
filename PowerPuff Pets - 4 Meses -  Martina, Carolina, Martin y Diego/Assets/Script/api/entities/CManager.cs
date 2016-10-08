using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CManager
{
	private List<CGameObject> mArray;

	public CManager()
	{
		mArray = new List<CGameObject> ();
	}

	public void add(CGameObject aGameObject)
	{
		mArray.Add (aGameObject);
	}

	virtual public void update()
	{
		for (int i = mArray.Count - 1; i >= 0; i --) 
		{
			mArray[i].update();
		}

		for (int i = mArray.Count - 1; i >= 0; i --) 
		{
			if (mArray[i].isDead())
			{
				removeObjectWithIndex(i);
			}
		}
	}

	virtual public void render()
	{
		for (int i = mArray.Count - 1; i >= 0; i --) 
		{
			mArray[i].render();
		}
	}

    public int length()
    {
        return mArray.Count;
    }
	private void removeObjectWithIndex(int aIndex)
	{
		if (aIndex < mArray.Count) 
		{
			mArray[aIndex].destroy();
			mArray[aIndex] = null;
			mArray.RemoveAt(aIndex);
		}
	}

	virtual public void destroy()
	{
		for (int i = mArray.Count - 1; i >= 0; i --) 
		{
			removeObjectWithIndex(i);
		}
		mArray = null;
	}

	virtual public CGameObject collides(CGameObject aGameObject)
	{
		for (int i = mArray.Count - 1; i >= 0; i --) 
		{
			if (aGameObject.collides(mArray[i]))
			{
				return mArray[i];
			}
		}

		return null;
	}

    virtual public CGameObject inSight(CGameObject aGameObject)
    {
        for (int i = mArray.Count - 1; i >= 0; i--)
        {
            if (aGameObject.inSight(mArray[i]))
            {
                return mArray[i];
            }
        }

        return null;
    }

    virtual public CGameObject collides(CSprite aSprite)
    {
        for (int i = mArray.Count - 1; i >= 0; i--)
        {
            if (aSprite.collides(mArray[i]))
            {
                return mArray[i];
            }
        }

        return null;
    }

    public List<CGameObject> getArray()
    {
        return mArray;
    }

}