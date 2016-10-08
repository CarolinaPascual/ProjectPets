using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CGrid  {

    private CNode mStartNode;
    private CNode mEndNode;
    List<List<CNode>> mNodes = new List<List<CNode>>();
    private int mCols;
    private int mRows;


    public CGrid(int aCols, int aRows)
    {
        
        mCols = aCols;
        mRows = aRows;
        for (int y = 0; y < mRows; y++)
        {
            mNodes.Add(new List<CNode>());
            for (int x = 0; x < mCols; x++)
            {
                mNodes[y].Add(new CNode(x, y));
                int index = CTileMap.LEVEL_001[y][x];
                setWalkable(x, y, CTileMap.inst().mWalkable[index]);
            }
        }
        
    }

    public CNode getNode(int aX,int aY)
    {
        return mNodes[aY][aX];
    }

    public void setEndNode(int aX, int aY)
    {
        mEndNode = mNodes[aY][aX];
    }

    public void setStartNode(int aX, int aY)
    {
        mStartNode = mNodes[aY][aX];
    }

    public void setWalkable(int aX, int aY, bool aValue)
    {
        mNodes[aY][aX].setWalkable(aValue);
    }

    public CNode getEndNode()
    {
        return mEndNode;
    }

    public CNode getStartNode()
    {
        return mStartNode;
    }

    public int getCols()
    {
        return mCols;
    }

    public int getRows()
    {
        return mRows;
    }

}
