using UnityEngine;
using System.Collections;

public class CGame : MonoBehaviour 
{
	static private CGame mInstance;
	private CGameState mState;
    private CCamera mCamera;

	void Awake()
	{
		if (mInstance != null) 
		{
			throw new UnityException ("Error in CGame(). You are not allowed to instantiate it more than once.");
		}

		mInstance = this;

		CMouse.init();
		CKeyboard.init ();

		setState(new CLevelState ());
		//setState(new CMainMenuState ());
	}

	static public CGame inst()
	{
		return mInstance;
	}

	public void setCamera(CCamera aCamera)
    {
        mCamera = aCamera;
    }

    public CCamera getCamera()
    {
        return mCamera;
    }
	
	// Update is called once per frame
	void Update () 
	{
		update ();
	}

	void LateUpdate()
	{
		render ();
	}

	private void update()
	{
		CMouse.update ();
		CKeyboard.update ();
		mState.update ();
	}

	private void render()
	{
		mState.render ();
	}

	public void destroy()
	{
		CMouse.destroy ();
		CKeyboard.destroy ();
		if (mState != null) 
		{
			mState.destroy ();
			mState = null;
		}
		mInstance = null;
	}

	public void setState(CGameState aState)
	{
		if (mState != null) 
		{
			mState.destroy();
			mState = null;
		}

		mState = aState;
        Debug.Log("Level State Started");
        mState.init ();
	}

	public CGameState getState()
	{
		return mState;
	}
}
