using UnityEngine;
using System.Collections;

public class CKeyboard
{
	static private bool mInitialized = false;

    public const KeyCode UP = KeyCode.UpArrow;
    public const KeyCode DOWN = KeyCode.DownArrow;
    public const KeyCode RIGHT = KeyCode.RightArrow;
	public const KeyCode LEFT = KeyCode.LeftArrow;
	public const KeyCode SPACE = KeyCode.Space;
	public const KeyCode ESCAPE = KeyCode.Escape;
	
	public CKeyboard()
	{
		throw new UnityException ("Error in CKeyboard(). You're not supposed to instantiate this class.");
	}
	
	public static void init()
	{
		if (mInitialized) 
		{
			return;
		}
		mInitialized = true;
	}
	
	public static void update()
	{
	}
	
	public static void destroy()
	{
		if (mInitialized) 
		{
			mInitialized = false;
		}
	}
	
	public static bool pressed(KeyCode aKey)
	{
		return Input.GetKey(aKey);
	}

	public static bool firstPress(KeyCode aKey)
	{
		return Input.GetKeyDown (aKey);
	}

    public static bool keyUp(KeyCode aKey)
    {
        return Input.GetKeyUp(aKey);
    }

    public static bool fire()
    {
        return Input.GetKeyUp(KeyCode.Space);
    }
}