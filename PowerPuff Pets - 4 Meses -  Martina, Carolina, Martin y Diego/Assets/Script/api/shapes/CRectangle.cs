using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class CRectangle
{
    CLine topLine;
    CLine rightLine;
    CLine bottomLine;
    CLine leftLine;

    public CRectangle(Vector2 corner1, Vector2 corner2)
    {
        topLine = new CLine();
        topLine.setPositions(corner1, new Vector2(corner2.x, corner1.y));
        rightLine = new CLine();
        rightLine.setPositions(corner1, new Vector2(corner1.x, corner2.y));
        bottomLine = new CLine();
        bottomLine.setPositions(new Vector2(corner1.x, corner2.y), corner2);
        leftLine = new CLine();
        leftLine.setPositions(new Vector2(corner2.x, corner1.y), corner2);
    }

    public void toggle(bool aBool)
    {
        topLine.setActive(aBool);
        rightLine.setActive(aBool);
        bottomLine.setActive(aBool);
        leftLine.setActive(aBool);
    }

    public void setWidth(float aFloat)
    {
        topLine.setWidth(aFloat);
        rightLine.setWidth(aFloat);
        bottomLine.setWidth(aFloat);
        leftLine.setWidth(aFloat);
    }
}
