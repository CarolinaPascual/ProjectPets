using UnityEngine;

public class CAnimatedSprite : CSprite
{
	private Sprite[] mFrame;

	private CAnim mAnim;

	public CAnimatedSprite()
	{
		mAnim = new CAnim ();
	}

	public void setFrames(Sprite[] aFramesArray)
	{
		mFrame = aFramesArray;
	}

	override public void update()
	{
		base.update ();
		mAnim.update ();
	}

    override public void hit()
    {
        base.hit();
    }

    override public void render()
	{
		base.render ();

		int frame = mAnim.getCurrentFrame () - 1;

		if (frame < 0 || frame >= mFrame.Length) 
		{
			Debug.Log ("ERROR: Animation out of range: " + frame); 
		}
		else 
		{
			setImage(mFrame[frame]);			    
		}
	}

	public void gotoAndStop(int aFrame)
	{
		mAnim.gotoAndStop (aFrame);
	}

	public void gotoAndPlay(int aFrame)
	{
		mAnim.gotoAndPlay (aFrame);
	}

    public void setDelay(float aDelay)
    {
        mAnim.setDelay(aDelay);
    }

	public void initAnimation(int aStartFrame, int aEndFrame, int aFPS, bool aIsLoop)
	{
		mAnim.init (aStartFrame, aEndFrame, aFPS, aIsLoop);
	}

    override public void setState(int aState)
    {
        base.setState(aState);
    }

    public bool isEnded()
    {
        return mAnim.isEnded();
    }

    public void pauseAnimation()
    {
        mAnim.pauseAnimation();
    }

    public void continueAnimation()
    {
        mAnim.continueAnimation();
    }

    public override void destroy()
    {
        mFrame = null;
        mAnim = null;
        base.destroy();
    }
}