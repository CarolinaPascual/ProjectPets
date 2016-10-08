using UnityEngine;
using System.Collections;

public class CNode {

    private int mX;
    private int mY;
    private float mF;
    private float mG;
    private float mH;
    private bool mWalkable = true;
    private CNode mParent;


    public CNode(int aX, int aY)
    {
        mX = aX;
        mY = aY;
    }

    public bool isIdentical(CNode aNode)
    {
        if ((mX == aNode.getX()) && (mY == aNode.getY()))
        {
            return true;
        }
        return false;
    }

    public int getX()
    {
        return mX;
    }

    public int getY()
    {
        return mY;
    }

    public float getF()
    {
        return mF;
    }

    public float getG()
    {
        return mG;
    }

    public float getH()
    {
        return mH;
    }

    public bool getWalkable()
    {
        return mWalkable;
    }

    public CNode getParent()
    {
        return mParent;
    }

    public void setX(int aX)
    {
        mX = aX;
    }

    public void setY(int aY)
    {
        mY = aY;
    }

    public void setF(float aF)
    {
        mF = aF;
    }

    public void setG(float aG)
    {
        mG = aG;
    }

    public void setH(float aH)
    {
        mH = aH;
    }

    public void setWalkable(bool aWalkable)
    {
        mWalkable = aWalkable;
    }

    public void setParent(CNode aParent)
    {
        mParent = aParent;
    }
}
