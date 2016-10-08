using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSteeredVehicle : CAnimatedSprite {

    private float mMaxForce = 1.0f;
    private CVector mSteeringForce;
    private int mBehavior;
    private CGameObject mTarget;
    private CVector mDestination;
    private float mWanderAngle = 0;
    private float mWanderDistance = 10;
    private float mWanderRadius = 5;
    private float mWanderRange = 1;
    private float mAvoidDistance = 300;
    private float mAvoidBuffer = 20;
    
    List<CNode> mPath;
    private int mArrivalThreshold = 500;
    public const int NONE = 0;
    public const int SEEK = 1;
    public const int FLEE = 2;
    public const int ARRIVE = 3;
    public const int PURSUE = 4;
    public const int EVADE = 5;
    public const int WANDER = 6;




    public CSteeredVehicle()
    {
        mSteeringForce = new CVector();
        //mDestination = new CVector(80 * CTileMap.TILE_WIDTH, 20 * CTileMap.TILE_HEIGHT);
        //mPath = findPath();
    }

    public void findPath()
    {
        AStar mAStar = new AStar();
        Debug.Log("Dest X: " + mDestination.x + "Dest Y: " + mDestination.y);
        if (mAStar.findPath(getPos(),mDestination.x,mDestination.y))
        {
            mPath = mAStar.getPath();
        }
        
    }

    public void setBehavior(int aBehavior)
    {
        mBehavior = aBehavior;
    }

    public int getBehavior()
    {
        return mBehavior;
    }

    public void setSteeringForce(CVector aForce)
    {
        mSteeringForce = aForce;
    }

    public CVector getSteeringForce()
    {
        return mSteeringForce;
    }

    public void setMaxForce(float aMaxForce)
    {
        mMaxForce = aMaxForce;
    }

    public float getMaxForce()
    {
        return mMaxForce;
    }


    public void seekNext()
    {
        if (mPath != null && mPath.Count > 0)
        {
            mDestination = new CVector(mPath[0].getX() * CTileMap.TILE_WIDTH, mPath[0].getY() * CTileMap.TILE_HEIGHT);
            seek(mDestination);
            mPath.RemoveAt(0);
            setState(CPlayer.STATE_WALKING);
            return;
        }
        else if(mPath == null)
        {
            Debug.Log("no paths possible");
        }
        else
        {
            Debug.Log("Destination Reached");
            //Behavior on destination
            setState(CPlayer.STATE_STAND);
            return;
        }
    }

    public void arriveNext()
    {
        if (mPath.Count > 0)
        {
            if(mPath.Count == 1)
            {
                mDestination = new CVector(mPath[0].getX() * CTileMap.TILE_WIDTH, mPath[0].getY() * CTileMap.TILE_HEIGHT);
                arrive(mDestination);
                mPath.RemoveAt(0);
                setState(CPlayer.STATE_WALKING);
                return;
            }
            else
            {
                mDestination = new CVector(mPath[0].getX() * CTileMap.TILE_WIDTH, mPath[0].getY() * CTileMap.TILE_HEIGHT);
                seek(mDestination);
                mPath.RemoveAt(0);
                setState(CPlayer.STATE_WALKING);
                return;
            }
            
        }
        else
        {
            Debug.Log("Destination Reached");
            //Behavior on destination
            setState(CPlayer.STATE_STAND);
            return;
        }
    }

    public CGameObject getTarget()
    {
        return mTarget;
    }

    public void setTarget(CGameObject aTarget)
    {
        mTarget = aTarget;
        mDestination = mTarget.getPos();
    }
   
    public CVector getDestination()
    {
        return mDestination;
    }
   

    public override void update()
    {
        mSteeringForce.truncate(mMaxForce);
        mSteeringForce.div(getMass());
        setVel(getVel() + mSteeringForce);
        mSteeringForce = new CVector();
        base.update();
    }

    public void seek(CVector aTarget)
    {
        //Calculate the velocity that would put you in your destination right now
        CVector pDesiredVelocity = aTarget - getPos();
        //normalize vector to get unit vector in the right direction
        pDesiredVelocity.normalize();
        //multiply it by max speed to get there as fast as possible
        pDesiredVelocity.mul(getMaxSpeed());
        //substract your current speed to get what you need on top of what you have
        CVector pForce = pDesiredVelocity - getVel();
        //add that speed you need to your force(fuck physics, we can add speed to force), this will later be capped by maxForce
        setSteeringForce(getSteeringForce() + pForce);
        
    }

    public void flee(CVector aTarget)
    {
        //Calculate the velocity that would put you in your destination right now
        CVector pDesiredVelocity = aTarget - getPos();
        //normalize vector to get unit vector in the right direction
        pDesiredVelocity.normalize();
        //multiply it by max speed to get there as fast as possible
        pDesiredVelocity.mul(getMaxSpeed());
        //substract your current speed to get what you need on top of what you have
        CVector pForce = pDesiredVelocity - getVel();
        //add that speed you need to your force(fuck physics, we can substract speed to force), this will later be capped by maxForce
        setSteeringForce(getSteeringForce() - pForce);
    }

    public void arrive(CVector aTarget)
    {
        //Calculate the velocity that would put you in your destination right now
        CVector pDesiredVelocity = aTarget - getPos();
        //normalize vector to get unit vector in the right direction
        pDesiredVelocity.normalize();
        //calculate distance until destination
        float dist = getPos().dist(aTarget);
        //multiply it by max speed to get there as fast as possible
        pDesiredVelocity.mul(getMaxSpeed()*dist/mArrivalThreshold);
        //substract your current speed to get what you need on top of what you have
        CVector pForce = pDesiredVelocity - getVel();
        //add that speed you need to your force(fuck physics, we can add speed to force), this will later be capped by maxForce
        setSteeringForce(getSteeringForce() + pForce);
    }

    public void pursue(CGameObject aTarget)
    {
        float pLookAheadTime = getPos().dist(aTarget.getPos())/getMaxSpeed();
        CVector pPredictedTarget = aTarget.getPos() + aTarget.getVel()* pLookAheadTime;
        seek(pPredictedTarget);
    }

    public void evade(CGameObject aTarget)
    {
        float pLookAheadTime = getPos().dist(aTarget.getPos()) / getMaxSpeed();
        CVector pPredictedTarget = aTarget.getPos() + aTarget.getVel() * pLookAheadTime;
        flee(pPredictedTarget);
    }

    public void wander()
    {
        CVector pCenter = getVel().clone().normalize() * mWanderDistance;
        CVector pOffset = new CVector();
        pOffset.setLenght(getWanderRadius());
        pOffset.setAngle(mWanderAngle);
        mWanderAngle += CMath.randomFloatBetween(0,1)*mWanderRange - mWanderRange*.5f;
        CVector pForce = pCenter + pOffset;
        setSteeringForce(getSteeringForce() + pForce);
    }

    public void avoid(List<CGameObject> aObstacles)
    {
        for(int i = 0; i < aObstacles.Count; i++)
        {
            CGameObject pObstacle = aObstacles[i];
            CVector pHeading = getVel().clone().normalize();

            //Vector between obstacle and vehicle
            CVector pDifference = pObstacle.getPos() - getPos();
            float pDotProd = pDifference.dotProd(pHeading);

            //If circle is in front of vehicle
            if(pDotProd > 0)
            {
                //vector to represent "feeler" arm
                CVector pFeeler = pHeading * mAvoidDistance;
                //project difference vector onto feeler
                CVector pProjection = pHeading*pDotProd;
                //distance from circle to feeler
                float pDist = (pProjection - pDifference).getLenght();

                //if feeler intersects circle (plus buffer)
                //and projection is less than feeler length,
                //we will collide, so need to steer
                if(pDist < pObstacle.getRadius() + mAvoidBuffer && pProjection.getLenght() < pFeeler.getLenght())
                {
                    //calculate a force +/- 90 degrees
                    //from vector to circle
                    CVector pForce = pHeading * getMaxSpeed();
                    pForce.setAngle(pForce.getAngle() + pDifference.sign(getVel())*Mathf.PI/2);
                    
                    //scale this force by distance to circle.
                    //the farther away, the smaller the force
                    pForce = pForce * (1.0f - pProjection.getLenght()/pFeeler.getLenght());

                    //add to the steering force
                    setSteeringForce(getSteeringForce() + pForce);

                    //braking force - slows vehicle down so it has
                    //time to turn. The closer it is, the harder it brakes
                    setVel(getVel() * (pProjection.getLenght()/pFeeler.getLenght()));

                }
            }
        }
    }

    public void flock(List<CSteeredVehicle> aVehicles)
    {
        CVector pAverageVelocity = getVel().clone();
        CVector pAveragePosition = new CVector();
        int pInSightCount = 0;

        for (int i = 0; i < aVehicles.Count; i++)
        {
            CSteeredVehicle pVehicle = aVehicles[i];
            if (pVehicle != this && inSight(pVehicle))
            {
                pAverageVelocity = pAverageVelocity + pVehicle.getVel();
                pAveragePosition = pAveragePosition + pVehicle.getPos();
                if (tooClose(pVehicle))
                {
                    flee(pVehicle.getPos());
                }
                pInSightCount++;
            }
        }

        if (pInSightCount > 0)
        {
            pAverageVelocity.div(pInSightCount);
            pAveragePosition.div(pInSightCount);
            seek(pAveragePosition);
            setSteeringForce(getSteeringForce() + pAverageVelocity - getVel());
        }
    }

    public void setWanderDistance(float aWanderDistance)
    {
        mWanderDistance = aWanderDistance;
    }

    public float getWanderDistance()
    {
        return mWanderDistance;
    }

    public void setWanderRadius(float aWanderRadius)
    {
        mWanderRadius = aWanderRadius;
    }

    public float getWanderRadius()
    {
        return mWanderRadius;
    }

    public void setWanderRange(float aWanderRange)
    {
        mWanderRange = aWanderRange;
    }

    public float getWanderRange()
    {
        return mWanderRange;
    }

    public override void render()
    {
        base.render();
    }

    public override void destroy()
    {
        base.destroy();
    }



}
