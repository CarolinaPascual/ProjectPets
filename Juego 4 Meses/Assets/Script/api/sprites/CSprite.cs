using UnityEngine;
using System.Collections;

public class CSprite : CGameObject 
{
	private GameObject mSprite;
	private SpriteRenderer mSpriteRenderer;

	// Caching of mSprite.transform.
	private Transform mTransform;

	private bool mFlipH = false;
	private float mRotation = 0.0f;
	private bool mIsRotatingSprite = false;

	public const int REG_CENTER = 0;
	public const int REG_TOP_LEFT = 1;
	private int mRegistration;

	public CSprite()
	{
		mSprite = new GameObject ();
		mSpriteRenderer = mSprite.AddComponent<SpriteRenderer> ();

		mTransform = mSprite.transform;
	}

	override public void update()
	{
		
		base.update ();

	}

    override public void hit()
    {
        base.hit();
    }

    override public void render()
	{
        CCamera Camera = CGame.inst().getCamera();
		base.render ();
		int offset = 0;
		if (getRegistration() == REG_TOP_LEFT) {
			if (mFlipH) {
				offset = getWidth ();
			}
		}
        float xCam = 0;
        float yCam = 0;
        if(Camera != null)
        {
            xCam = Camera.getX();
            yCam = Camera.getY();
        }
        
        Vector3 pos = new Vector3 (getX () + offset - xCam, getY () * -1 + yCam, getZ());
		mTransform.position = pos;

		if (!mIsRotatingSprite) 
		{
			if (mFlipH) 
			{
				mTransform.rotation = Quaternion.Euler (new Vector3 (0, 180, mRotation));
			} 
			else 
			{
				mTransform.rotation = Quaternion.Euler (new Vector3 (0, 0, mRotation));
			}
		} 
		else 
		{
			mTransform.rotation = Quaternion.Euler (new Vector3 (0, 0, mRotation));
		}


	}

	override public void destroy()
	{
        Object.DestroyImmediate(mSprite);
        Object.DestroyImmediate(mSpriteRenderer);
        Object.DestroyImmediate(mTransform);
        base.destroy();
    }

	public void setImage(Sprite aSprite)
	{
		mSpriteRenderer.sprite = aSprite;
	}

	public void setFlipH(bool aFlipH)
	{
		mFlipH = aFlipH;
	}

	public bool getFlipH()
	{
		return mFlipH;
	}

	public void setRotation(float aRotation)
	{
		mIsRotatingSprite = true;
		mRotation = aRotation;
		mRotation = CMath.clampDeg (mRotation);
	}

	public float getRotation()
	{
		return mRotation;
	}

	override public void setName(string aName)
	{
		base.setName (aName);
		mSprite.name = aName;
	}

	public void setSortingLayerName(string aSortingLayerName)
	{
		mSpriteRenderer.sortingLayerName = aSortingLayerName;
	}

	public string getSortingLayerName()
	{
		return mSpriteRenderer.sortingLayerName;
	}

	public void setSortingOrder(int aSortingOrder)
	{
		mSpriteRenderer.sortingOrder = aSortingOrder;
	}

	public int getSortingOrder()
	{
		return mSpriteRenderer.sortingOrder;
	}

	public void setColor(Color aColor)
	{
		mSpriteRenderer.material.color = aColor;
	}

	public Color getColor()
	{
		return mSpriteRenderer.material.color;
	}

	public void setAlpha(float aAlpha)
	{
		Color color = mSpriteRenderer.material.color;
		mSpriteRenderer.material.color = new Color (color.r, color.g, color.b, aAlpha);
	}

	public float getAlpha()
	{
		Color color = mSpriteRenderer.material.color;
		return color.a;
	}

	public void setVisible(bool aIsVisible)
	{
		mSpriteRenderer.enabled = aIsVisible;
	}

	public bool isVisible()
	{
		return mSpriteRenderer.enabled;
	}

	public void setScale(float aScale)
	{
		mSprite.transform.localScale = new Vector3 (aScale, aScale, 0.0f);
	}

	public int getRegistration(){
		return mRegistration;
	}

	public void setRegistration(int aRegistration){
		mRegistration = aRegistration;
	}

    
    public void turnLeft(float aAngle)
    {
        mRotation += aAngle;
        mRotation += CMath.clampDeg(mRotation);
    }

    public void turnRight(float aAngle)
    {
        mRotation -= aAngle;
        mRotation = CMath.clampDeg(mRotation);
    }
}
