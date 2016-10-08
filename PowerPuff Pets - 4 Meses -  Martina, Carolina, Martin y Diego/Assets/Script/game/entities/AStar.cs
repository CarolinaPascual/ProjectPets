using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AStar  {

    List<CNode> mOpen;
    List<CNode> mClosed;
    List<CNode> mPath;
    private CGrid mGrid;
    private CNode mStartNode;
    private CNode mEndNode;
    private float mStraightCost = 1.0f;
    private float mDiagCost = CMath.sqrt(2);


    public AStar()
    {
        
    }

    public bool findPath(CVector aPos,float aX, float aY)
    {
        Debug.Log("find path");
        mGrid = new CGrid(CTileMap.MAP_WIDTH, CTileMap.MAP_HEIGHT);
        mOpen = new List<CNode>();
        mClosed = new List<CNode>();

        int aStartX = (int)(aPos.x / CTileMap.TILE_WIDTH);
        int aStartY = (int)(aPos.y / CTileMap.TILE_HEIGHT); 

        int pX = (int)(aX / CTileMap.TILE_WIDTH);
        int pY = (int)(aY / CTileMap.TILE_HEIGHT);
        mGrid.setStartNode(aStartX, aStartY);
        mGrid.setEndNode(pX, pY);
        Debug.Log("START NODE: " + aStartX + " " + aStartY);
        Debug.Log("END NODE: " + pX + " " + pY);

        mStartNode = mGrid.getStartNode();
        mEndNode = mGrid.getEndNode();

        mStartNode.setG(0);
        mStartNode.setH(manhattan(mStartNode));
        mStartNode.setF(mStartNode.getG() + mStartNode.getH());
        
        return search();
    }

    public bool search()
    {
        Debug.Log("search");
        CNode node = mStartNode;
        while(!node.isIdentical(mEndNode))
        {
            int startX = CMath.max(0, node.getX() - 1);
            int endX = CMath.min(mGrid.getCols() - 1, node.getX() + 1);
            int startY = CMath.max(0, node.getY() - 1);
            int endY = CMath.min(mGrid.getRows() - 1, node.getY() + 1);
            for(int i = startX; i <= endX; i++)
            {
                for(int j = startY; j <= endY; j++)
                {
                    CNode test = mGrid.getNode(i, j);
                    if(test.isIdentical(node) || !test.getWalkable())
                    {
                        continue;
                    }
                    float cost = mStraightCost;

                    if (!((node.getX() == test.getX()) || (node.getY() == test.getY())))
                    {
                        //We implement this here and not on the test.identical if for optimization reasons, this is checked only when the validation is diagonal, to make sure we don't slip through the cracks
                        if(!mGrid.getNode(node.getX(), test.getY()).getWalkable() || !mGrid.getNode(test.getX(), node.getY()).getWalkable())
                        {
                            continue;
                        }
                        cost = mDiagCost;
                    }

                    float g = node.getG() + cost;
                    float h = manhattan(test);
                    float f = g + h;
                    if(isOpen(test) || isClosed(test))
                    {
                        if(test.getF() > f)
                        {
                            test.setF(f);
                            test.setG(g);
                            test.setH(h);
                            test.setParent(node);
                        }
                        
                    }
                    else
                    {
                        
                        test.setF(f);
                        test.setG(g);
                        test.setH(h);
                        test.setParent(node);
                        mOpen.Add(test);
                        
                    }
                    
                }
                
            }
            
            mClosed.Add(node);
            if (mOpen.Count == 0)
            {
                Debug.Log("empty open");
                return false;
            }

            //Magic code to sort the list by F
            mOpen = mOpen.OrderBy(o => o.getF()).ToList();
            node = mOpen[0];
            mOpen.RemoveAt(0);
        }
        buildPath();
        Debug.Log("search ended");
        return true;
    }

    private void buildPath()
    {
        mPath = new List<CNode>();
        CNode node = mEndNode;
        mPath.Add(node);
        
        while (!node.isIdentical(mStartNode))
        {
            node = node.getParent();
            mPath.Insert(0, node);
        }
        Debug.Log("path count build: " + mPath.Count);
    }

    public List<CNode> getPath()
    {
        return mPath;
    }

    private bool isOpen(CNode aNode)
    {
        for(int i= 0; i < mOpen.Count; i++)
        {
            if(mOpen[i].isIdentical(aNode))
            {
                return true;
            }
        }
        return false;
    }

    private bool isClosed(CNode aNode)
    {
        for (int i = 0; i < mClosed.Count; i++)
        {
            if (mClosed[i].isIdentical(aNode))
            {
                return true;
            }
        }
        return false;
    }
    
    private float manhattan(CNode aNode)
    {
        return ((CMath.abs(aNode.getX() - mEndNode.getX()) * mStraightCost) + (CMath.abs(aNode.getY() + mEndNode.getY()) * mStraightCost));
    }

    private float euclidian(CNode aNode)
    {
        float dx = aNode.getX() - mEndNode.getX();
        float dy = aNode.getY() - mEndNode.getY();
        return CMath.sqrt(dx * dx + dy * dy) * mStraightCost;
    }

    private float diagonal(CNode aNode)
    {
        float dx = CMath.abs(aNode.getX() - mEndNode.getX());
        float dy = CMath.abs(aNode.getY() - mEndNode.getY());
        float diag = CMath.min(dx, dy);
        float straight = dx + dy;
        return mDiagCost * diag + mStraightCost * (straight - 2 * diag);
    }

    public List<CNode> getVisited()
    {
        List<CNode> aList = new List<CNode>();
        aList.AddRange(mClosed);
        aList.AddRange(mOpen);
        return aList;
    }

    
}
