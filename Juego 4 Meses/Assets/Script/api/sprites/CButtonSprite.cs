using UnityEngine;
using System.Collections;

public class CButtonSprite : CAnimatedSprite
{
	public bool mIsMouseOver = false;

	public CButtonSprite()
	{
	}

	public override void destroy()
	{
		base.destroy ();
	}

	public override void update()
	{
		base.update ();

		float scale = 1.0f;
		int frame = 1;

		Vector3 mousePos = CMouse.getPos ();
		mIsMouseOver = CMath.pointInRect (mousePos.x, mousePos.y, getX () - getWidth () / 2, getY () - getHeight () / 2, getWidth (), getHeight ());

		if (CMouse.pressed ()) 
		{
			if (mIsMouseOver) 
			{
				scale = 0.9f;
				frame = 2;
			}
		} 
		else 
		{
			if (mIsMouseOver)
			{
				scale = 1.1f;
				frame = 1;
			}
			else
			{
				scale = 1.0f;
				frame = 1;
			}
		}

		setScale (scale);
		gotoAndStop (frame);
	}

	public override void render()
	{
		base.render ();
	}

	public bool isMouseOver()
	{
		return mIsMouseOver;
	}

	public bool pressed()
	{
		return (CMouse.pressed () && mIsMouseOver);
	}

	public bool clicked()
	{
		Vector3 mousePos = CMouse.getPos ();

		if (CMouse.release ()) 
		{
			if (CMath.pointInRect (mousePos.x, mousePos.y, getX () - getWidth () / 2, getY () - getHeight () / 2, getWidth (), getHeight ()))
			{
				return true;
			}
		}

		return false;
	}
}