using UnityEngine;
using System.Collections;

public class CGameObject 
{

    public static CVector GRAVITY = new CVector(0,0,900);
    //Border Behaviours
    private static int NONE = 0;
    private static int STOP = 1;
    private static int WRAP = 2;
    private static int BOUNCE = 3;
    private static int DIE = 4;

    private CVector mPos;
	private CVector mVel;
	private CVector mAccel;

    private string ID;
    
    private float mMinX = 0;
    private float mMaxX = 0;
    private float mMinY = 0;
    private float mMaxY = 0;
    
    private int mBoundAction = CGameObject.NONE;

    private bool mIsDead = false;
	
	private int mState = 0;
	private float mTimeState = 0.0f;
	
	private string mName;

	private int mRadius = 100;
    private float mFriction = 1.0f;

	private int mType;

	private int mWidth = 100;
	private int mHeight = 100;

    private float mMaxSpeed = 500;
    private float mMass = 1.0f;

    private float mInSightDist = CTileMap.TILE_WIDTH*2;
    private float mTooCloseDist = 60;

    private float mLifePoints = 100;
    private int mFireRate = 2;
    private float mDamage = 10;
    private float mSpeed = 10;

    public CGameObject()
	{
		mPos = new CVector ();
		mVel = new CVector ();
		mAccel = new CVector ();
        
	}
	
    public void setID(string aID)
    {
        ID = aID;
        CEntityManager.inst().registerEntity(ID, this);
    }
    public string getID()
    {
        return ID;
    }

    public void handleMessage(CTelegram aMessage)
    {
        OnMessage(aMessage);        
    }

    virtual public void OnMessage(CTelegram aMessage)
    {

    }

    public bool inSight(CGameObject aObject)
    {
        if (getPos().dist(aObject.getPos()) > mInSightDist)
        {
            return false;
        }
        /*CVector pHeading = getVel().clone().normalize();
        CVector pDifference = aObject.getPos() - getPos();
        float pDotProd = pDifference.dotProd(pHeading);
        if (pDotProd < 0)
        {
            return false;
        }*/
        return true;
    }

    public void setInSightDist(float aInSightDist)
    {
        mInSightDist = aInSightDist;
    }

    public float getInSightDist()
    {
        return mInSightDist;
    }

    public void setTooCloseDist(float aTooCloseDist)
    {
        mTooCloseDist = aTooCloseDist;
    }

    public float getTooCloseDist()
    {
        return mTooCloseDist;
    }

    public bool tooClose(CGameObject aObject)
    {
        return getPos().dist(aObject.getPos()) < mTooCloseDist;
    }

    public void setMass(float aMass)
    {
        mMass = aMass;
    }

    public float getMass()
    {
        return mMass;
    }

    public void setFriction(float aFriction)
    {
        mFriction = aFriction;
    }

    public float getFriction()
    {
        return mFriction;
    }

    public void setMaxSpeed(float aMaxSpeed)
    {
        mMaxSpeed = aMaxSpeed;
    }

    public float getMaxSpeed()
    {
        return mMaxSpeed;
    }

    public float getLifePoints()
    {
        return mLifePoints;
    }

    public void setLifePoints(float aLifePoints)
    {
        mLifePoints = aLifePoints;
    }

    public float getFireRate()
    {
        return mFireRate;
    }

    public void setFireRate(int aFireRate)
    {
        mFireRate = aFireRate;
    }

    public float getDamage()
    {
        return mDamage;
    }

    public void setDamage(float aDamage)
    {
        mDamage = aDamage;
    }

    public void setSpeed(float aSpeed)
    {
        mSpeed = aSpeed;
        if(mSpeed > getMaxSpeed())
        {
            mSpeed = getMaxSpeed();
        }
    }

    public float getSpeed()
    {
        return mSpeed;
    }
	public void setX(float aX)
	{
		mPos.x = aX;
	}
	
	public void setY(float aY)
	{
		mPos.y = aY;
	}
	
	public void setZ(float aZ)
	{
		mPos.z = aZ;
	}

	public void setXY(float aX, float aY)
	{
		mPos.x = aX;
		mPos.y = aY;
	}

    public void setXYZ(float aX, float aY, float aZ)
    {
        mPos.x = aX;
        mPos.y = aY;
        mPos.z = aZ;
    }

    public float getX()
	{
		return mPos.x;
	}
	
	public float getY()
	{
		return mPos.y;
	}
	
	public float getZ()
	{
		return mPos.z;
	}
        
    public CVector getPos()
    {
        return mPos;
    }

    public void setPos(CVector aPos)
    {
        mPos = aPos;
    }
    
    public CVector getVel()
    {
        return mVel;
    }

    public void setVel(CVector aVel)
    {
        mVel = aVel;
    }

    public CVector getAccel()
    {
        return mAccel;
    }

    public void setAccel(CVector aAccel)
    {
        mAccel = aAccel;
    }

    public void setVelX(float aVelX)
	{
		mVel.x = aVelX;
	}
	
	public void setVelY(float aVelY)
	{
		mVel.y = aVelY;
	}

	public void setVelXY(float aVelX, float aVelY)
	{
		mVel.x = aVelX;
		mVel.y = aVelY;
	}

    public void setVelXYZ(float aVelX, float aVelY, float aVelZ)
    {
        mVel.x = aVelX;
        mVel.y = aVelY;
        mVel.z = aVelZ;
    }

    public void setVelZ(float aVelZ)
	{
		mVel.z = aVelZ;
	}
	
	public float getVelX()
	{
		return mVel.x;
	}
	
	public float getVelY()
	{
		return mVel.y;
	}
	
	public float getVelZ()
	{
		return mVel.z;
	}
	
	public void setAccelX(float aAccelX)
	{
		mAccel.x = aAccelX;
	}
	
	public void setAccelY(float aAccelY)
	{
		mAccel.y = aAccelY;
	}

    public void setAccelXY(float aAccelX, float aAccelY)
    {
        mAccel.x = aAccelX;
        mAccel.y = aAccelY;
    }

    public void setAccelXYZ(float aAccelX, float aAccelY, float aAccelZ)
    {
        mAccel.x = aAccelX;
        mAccel.y = aAccelY;
        mAccel.z = aAccelZ;
    }

    public void setAccelZ(float aAccelZ)
	{
		mAccel.z = aAccelZ;
	}
	
	public float getAccelX()
	{
		return mAccel.x;
	}
	
	public float getAccelY()
	{
		return mAccel.y;
	}
	
	public float getAccelZ()
	{
		return mAccel.z;
	}
	
	virtual public void update()
	{

        setVelXY(CMath.Round(getVelX(), 2), CMath.Round(getVelY(), 2));
        setAccelXY(CMath.Round(getAccelX(), 2), CMath.Round(getAccelY(), 2));
        mTimeState = mTimeState + Time.deltaTime;
        mVel = mVel + mAccel * Time.deltaTime;
        mVel = mVel * mFriction;
		mPos = mPos + mVel * Time.deltaTime;
        checkBounds();
	}
	
	virtual public void render()
	{
	}
	
	virtual public void destroy()
	{
		mPos.destroy ();
		mPos = null;
		mVel.destroy ();
		mVel = null;
		mAccel.destroy ();
		mAccel = null;
	}

	virtual public void setState(int aState)
	{
        if(mState != aState)
        {
            mState = aState;
            mTimeState = 0.0f;
        }
             
	}

    public void impact(CGameObject aAttacker)
    {
        setLifePoints(getLifePoints() - aAttacker.getDamage());
        if (getLifePoints() <= 0)
        {
            setDead(true);
        }

    }

    virtual public void hit()
    {
        
    }

    public int getState()
	{
		return mState;
	}

    public void setTimeState(float aTimeState)
    {
        mTimeState = aTimeState;
    }

	public float getTimeState()
	{
		return mTimeState;
	}

	public void setDead(bool aIsDead)
	{
		mIsDead = aIsDead;
	}

	public bool isDead()
	{
		return mIsDead;
	}

	public void setRadius(int aRadius)
	{
		mRadius = aRadius;
	}

	public int getRadius()
	{
		return mRadius;
	}

	public void setType(int aType)
	{
		mType = aType;
	}

	public int getType()
	{
		return mType;
	}

	virtual public void setName(string aName)
	{
		mName = aName;
	}

	virtual public string getName()
	{
		return mName;
	}

	public void setWidth(int aWidth)
	{
		mWidth = aWidth;
	}

	public int getWidth()
	{
		return mWidth;
	}

	public void setHeight(int aHeight)
	{
		mHeight = aHeight;
	}
	
	public int getHeight()
	{
		return mHeight;
	}

	public bool collides(CGameObject aGameObject)
	{
        
		if (CMath.dist (getX (), getY (), aGameObject.getX (), aGameObject.getY ()) < (getRadius () + aGameObject.getRadius ()))
		{
			return true;
		}
		else 
		{
			return false;
		}
	}

    public bool collides(CSprite aSprite)
    {
        if (aSprite.getRegistration() == CSprite.REG_TOP_LEFT)
        {
            float x1 = getX();
            float y1 = getY();
            float w1 = getWidth();
            float h1 = getHeight();
            float x2 = aSprite.getX();
            float y2 = aSprite.getY();
            float w2 = aSprite.getWidth();
            float h2 = aSprite.getHeight();
            return CMath.rectRectCollision(x1, y1, w1, h1, x2, y2, w2, h2);
        }else if(aSprite.getRegistration() == CSprite.REG_CENTER)
        {
            float x1 = getX();
            float y1 = getY();
            float r1 = getRadius();
            float x2 = aSprite.getX();
            float y2 = aSprite.getY();
            float r2 = aSprite.getRadius();
            return CMath.circleCircleCollision(x1,y1,r1,x2,y2,r2);
        }
        return false;
    }

    public void setBoundAction(int aBoundAction)
    {
        mBoundAction = aBoundAction;
    }

    public void setBounds(int aMinX, int aMinY, int aMaxX, int aMaxY)
    {
        mMinX = aMinX;
        mMinY = aMinY;
        mMaxX = aMaxX;
        mMaxY = aMaxY;
    }

    public void stopMove()
    {
        mVel.zero();
        mAccel.zero();
    }

    public void checkBounds()
    {
        //if NONE then we don't check border collision
        if (mBoundAction == CGameObject.NONE)
        {
            return;
        }
        
        //Which borders we are touching
        bool left = getX() < mMinX;
        bool right = getX() > mMaxX;
        bool up = getY() > mMinY;//inverting the comparison to adjust to negative Y
        bool down = getY() < mMaxY;

        //If no collision, we don't do anything
        if (!(left||right||up||down))
        {
            return;
        }

        if(mBoundAction == CGameObject.WRAP)
        {
            if (left)
            {
                setX(mMaxX);
            }

            if(right)
            {
                setX(mMinX);
            }

            if (up)
            {
                setY(mMaxY);
            }

            if (down)
            {
                setY(mMinY);
            }
        }
        //if Action is STOP,BOUNCE or DIE we correct position, otherwise object remains out of bounds
        else
        {
            if (left)
            {
                setX(mMinX);
            }

            if (right)
            {
                setX(mMaxX);
            }

            if (up)
            {
                setY(mMinY);
            }

            if (down)
            {
                setY(mMaxY);
            }
        }
        if (mBoundAction == CGameObject.STOP || mBoundAction == CGameObject.DIE)
        {
            setVelXY(0, 0);
        }
        else if(mBoundAction == CGameObject.BOUNCE)
        {
            if (left || right)
            {
                setVelX(getVelX() * -1);
            }

            if (up || down)
            {
                setVelY(getVelY() * -1);
            }
            
        }
        if (mBoundAction == CGameObject.DIE)
        {
            mIsDead = true;
            return;
        }

    }

    public void setAccelAndMag(float aAng, float aMag)
    {
        mAccel.setAngMag(aAng, aMag);
    }

    public void setVelAndMag(float aAng, float aMag)
    {
        mVel.setAngMag(aAng, aMag);
    }

    
}