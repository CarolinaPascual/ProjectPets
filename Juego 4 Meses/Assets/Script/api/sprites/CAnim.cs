using UnityEngine;

public class CAnim 
{	
	private int mCurrentFrame = 0;
	private int mStartFrame = 0;
	private int mEndFrame = 0;
	private float mDelay = 0.0f;
	private bool mIsLoop = false;
	private float mTimeFrame = 0.0f;
	private bool mEnded = false;	
	private bool mPaused = false;
	// Indicates if frame has changed since the previous update().
	private bool mFrameHasChanged = false;
	// Used to calculate if the previous frame is the same as the current frame.
	private int mOldFrame = 0;

	public CAnim()
	{
        //CUtils.log ("constructor CAnimatedSprite");
	}

	// Update is called once per frame
	public void update () 
	{
        if (mPaused)
			return;
		
		mTimeFrame += Time.deltaTime;
		if (mTimeFrame >= mDelay)
		{
            if (!mEnded)
			{
				mCurrentFrame += 1;
				mTimeFrame = 0.0f;
				if (mCurrentFrame > mEndFrame)
				{
					if (mIsLoop)
					{
						mCurrentFrame = mStartFrame;
					}
					else
					{
						mCurrentFrame = mEndFrame;
						mEnded = true;
					}
				}
			}
		}
		
		// TODO: Ojo que se cambia por delta time, podria pasar que se salte a otro frame no contiguo. Tener en cuenta esto para la logica de cambio de frame de las unidades que disparan en cierto frame.
		if (mOldFrame != mCurrentFrame)
			mFrameHasChanged = true;
		else
			mFrameHasChanged = false;
		
		mOldFrame = mCurrentFrame;
	}
	
	public void init(int aStartFrame, int aEndFrame, int aFPS, bool aIsLoop)
	{
		// If animation is made in Flash, rest one to the numbers (Flash starts counting the frames in 1) but
		// in the array, we need to start counting from zero. Comment if the animations were not done in Flash.
		aStartFrame -= 1;
		aEndFrame -= 1;
	
		mStartFrame = aStartFrame;
		mEndFrame = aEndFrame;
		mDelay = 1.0f / aFPS;  
		mIsLoop = aIsLoop;
		mPaused = false;
		mCurrentFrame = aStartFrame;
		mTimeFrame = 0;
		mEnded = false;
		mOldFrame = aStartFrame;
		mFrameHasChanged = false;
	}

    public void setFPS(int aFPS)
    {
        mDelay = 1.0f / aFPS;  
    }
	
	public void setCurrentFrame(int aCurrentFrame)
	{
		// If animation is made in Flash, rest one to the frame (Flash starts counting the frames in 1) but
		// in the array, we need to start counting from zero. Comment if the animations were not done in Flash.
		mCurrentFrame = aCurrentFrame - 1;
	}
	
	public int getCurrentFrame()
	{
		// If animation is made in Flash, sum one to the current frame (Flash starts counting the frames in 1) but
		// in the array, we need to start counting from zero. Comment if the animations were not done in Flash.
		return mCurrentFrame + 1;
	}
	
	public bool isEnded()
	{
		return mEnded;
	}
	
	public bool frameHasChanged()
	{
		return mFrameHasChanged;
	}
	
	public void gotoAndStop(int aFrame)
	{
		// If animation is made in Flash, rest one to the frame (Flash starts counting the frames in 1) but
		// in the array, we need to start counting from zero. Comment if the animations were not done in Flash.
		mCurrentFrame = aFrame - 1;
		mEnded = true;
		mTimeFrame = 0.0f;
	}
	
	public void gotoAndPlay(int aFrame)
	{
		// If animation is made in Flash, rest one to the frame (Flash starts counting the frames in 1) but
		// in the array, we need to start counting from zero. Comment if the animations were not done in Flash.
		mCurrentFrame = aFrame - 1;
		mEnded = false;
		mTimeFrame = 0.0f;
	}
	
	public void pauseAnimation()
	{
		mPaused = true;
	}
	
	public void continueAnimation()
	{
		mPaused = false;
	}

    public void setDelay(float aDelay)
    {
        mDelay = aDelay;
    }
}
