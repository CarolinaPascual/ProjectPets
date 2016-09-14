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
        for (int i = 0; i < mCols; i++)
        {
            mNodes.Add(new List<CNode>());
            for (int j = 0; j < mRows; j++)
            {
                //mNodes[i][j] = new CNode(i, j);
                mNodes[i].Add(new CNode(i, j));
            }
        }
        
    }

    public CNode getNode(int aX,int aY)
    {
        return mNodes[aX][aY];
    }

    public void setEndNode(int aX, int aY)
    {
        mEndNode = mNodes[aX][aY];
    }

    public void setStartNode(int aX, int aY)
    {
        mStartNode = mNodes[aX][aY];
    }

    public void setWalkable(int aX, int aY, bool aValue)
    {
        mNodes[aX][aY].setWalkable(aValue);
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
