using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class CLine : CGameObject
{
    private GameObject mLine;
    private LineRenderer mLineRenderer;

    //
    public CLine()
    {
        mLine = new GameObject();
        mLineRenderer = mLine.AddComponent<LineRenderer>();
    }

    public void setPositions(Vector2 origin, Vector2 end)
    {
        //hudPosition is set so the line is between the Unity Camera and the Game
        float hudPosition = -5;
        //also invert the Y axis
        Vector3 originFixed = new Vector3(origin.x, -1 * origin.y, hudPosition);
        Vector3 endFixed = new Vector3(end.x, -1 * end.y, hudPosition);
        Vector3[] positions = new Vector3[] { originFixed, endFixed };
        mLineRenderer.SetPositions(positions);
    }

    //it can be set more than one material, in this case we use only the first one
    public void setMaterial(Material mat)
    {
        mLineRenderer.materials[0] = mat;
    }

    public void setWidth(float aWidth)
    {
        mLineRenderer.SetWidth(aWidth, aWidth);
    }
    //sets the begining and ending color, then interpolates all the colors in between them and
    //are asigned to a section of the line. (degradé)
    public void setColors(Color startingColor, Color endColor)
    {
        mLineRenderer.SetColors(startingColor, endColor);
    }

    public void setActive(bool aBool)
    {
        mLine.SetActive(aBool);
    }
}