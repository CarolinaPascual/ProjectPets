using UnityEngine;
using System.Collections;

public class CMath
{
    static float RAD_TO_DEG = 180 / Mathf.PI;
    static float DEG_TO_RAD = Mathf.PI / 180;

    //Random Integer between aMin[inclusive] and aMax[inclusive]
    public static int randomIntBetween(int aMin, int aMax)
	{
		return Random.Range (aMin, aMax + 1);
	}

	public static float randomFloatBetween(float aMin, float aMax)
	{
		return Random.Range (aMin, aMax);
	}

	public static float clampDeg(float aDeg)
	{
		aDeg = aDeg % 360.0f;
		if (aDeg < 0.0f) 
		{
			aDeg += 360.0f;
		}

		return aDeg;
	}

    public static float sqrt(float aNum)
    {
        return Mathf.Sqrt(aNum);
    }

    public static float sqrt(int aNum)
    {
        return Mathf.Sqrt(aNum);
    }

    public static float abs(float aNum)
    {
        return Mathf.Abs(aNum);
    }

    public static int abs(int aNum)
    {
        return Mathf.Abs(aNum);
    }

    public static bool pointInRect(float aX, float aY, float aRectX, float aRectY, float aRectWidth, float aRectHeight)
	{
		return (aX >= aRectX && aX <= aRectX + aRectWidth && aY >= aRectY && aY <= aRectY + aRectHeight);
	}

	public static float dist(float aX1, float aY1, float aX2, float aY2)
	{
		return Mathf.Sqrt((aX2 - aX1) * (aX2 - aX1) + (aY2 - aY1) * (aY2 - aY1));
	}

    public static float dist2(float aX1, float aY1, float aX2, float aY2)
    {
        return (aX2 - aX1) * (aX2 - aX1) + (aY2 - aY1) * (aY2 - aY1);
    }

    public static float min(float aValue1, float aValue2)
	{
		if (aValue1 < aValue2)
		{
			return aValue1;
		}
		
		return aValue2;
	}

    public static int min(int aValue1, int aValue2)
    {
        if (aValue1 < aValue2)
        {
            return aValue1;
        }

        return aValue2;
    }
    public static float max(float aValue1, float aValue2)
    {
        if (aValue1 > aValue2)
        {
            return aValue1;
        }

        return aValue2;
    }

    public static int max(int aValue1, int aValue2)
    {
        if (aValue1 > aValue2)
        {
            return aValue1;
        }

        return aValue2;
    }

    // Convert from radians to degrees.
    public static float radToDeg(float aAngle)
	{
		return aAngle * CMath.RAD_TO_DEG; 
		
	}
	
	// Convert from degrees to radians.
	public static float degToRad(float aAngle)
	{
		return aAngle * CMath.DEG_TO_RAD;
		
	}

    public static float Round(float value, int digits)
    {
        float rounded = (float)System.Math.Round(value, digits);
        return rounded;
    }

    public static bool rectRectCollision(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2)
    {
        return ((((x1 + w1) > x2) && (x1 < (x2 + w2))) && (((y1 + h1) > y2) && (y1 < (y2 + h2))));
    }

    public static bool circleCircleCollision(float aX1, float aY1, float aRadius1, float aX2, float aY2, float aRadius2)
    {
        return CMath.dist(aX1, aY1, aX2, aY2) <= (aRadius1 + aRadius2);
    }
        
}
