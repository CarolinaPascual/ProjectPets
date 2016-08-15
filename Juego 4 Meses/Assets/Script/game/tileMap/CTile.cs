using UnityEngine;
using System.Collections;

public class CTile : CSprite
{
	// Tile index. Starting from 0.
	private int mTileIndex;
	private bool mWalkable;
    private bool mPlatform;
    private bool mLadder;
    private bool mHangable;

	// Parametros: coordenada del tile (x, y) y el indice del tile.
	public CTile(int aX, int aY, int aTileIndex, Sprite aSprite)
	{
		setXY (aX, aY);
		setTileIndex(aTileIndex);

		setImage (aSprite);
		setSortingLayerName ("TileMap");
		mWalkable = true;
        
	}

	public void setTileIndex(int aTileIndex)
	{
		mTileIndex = aTileIndex;
		
	}

	public int getTileIndex()
	{
		return mTileIndex;
	}

	public bool isWalkable()
	{
		return mWalkable;
	}

	public void setWalkable(bool aIsWalkable)
	{
		mWalkable = aIsWalkable;
	}

    /*public bool isHangable()
    {
        return mHangable;
    }

    public void setHangable(bool aIsHangable)
    {
        mHangable = aIsHangable;
    }

    public bool isPlatform()
    {
        return mPlatform;
    }

    public void setPlatform(bool aIsPlatform)
    {
        mPlatform = aIsPlatform;
    }

    public bool isLadder()
    {
        return mLadder;
    }

    public void setLadder(bool aLadder)
    {
        mLadder = aLadder;
    }

    public bool isFloor()
    {
        return isPlatform() || !isWalkable();
    }*/

    override public void render()
	{
		base.render ();
	}

	override public void update()
	{
		base.update ();
	}

	override public void destroy()
	{
        base.destroy();
        
    }
}
