﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CTileMap
{
	private static CTileMap mInst = null;

	public const int MAP_WIDTH = 64;
	public const int MAP_HEIGHT = 6;

	public const int TILE_WIDTH = 200;
	public const int TILE_HEIGHT = 200;

	List<List<CTile>> mMap;
    private CTile mEmptyTile;
    
    private bool[] mWalkable = new bool[] {  true, false,  true,  true,  true, false, false, false,  true, false, false, true,  true, true, true, true, true, true,false};
    private bool[] mPlatform = new bool[] { false, false, false, false, false, false, false, false, false, false, false, false, true, true, true,false,false, true,false};
    private bool[] mLadder = new bool[]   { false, false, false, false, false, false, false, false, false, false, false, false,false,false,false, true, true, true,false};
    private bool[] mHangable = new bool[] { false, false, false, false, false, false, false, false, false, false, false, false, true,false, true,false,false,false,false};
    
    // Amount of Tiles + 1 since it starts in 0
    private const int NUM_TILES = 19;

	// Array con los sprites de los tiles.
	private Sprite[] mTiles;

	// 9 Height by 5 Width TileMap
	public static int[][] LEVEL_001 = {
		new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 0, 0, 0, 0, 0, 0},
		new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,12,13,14, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0,18,18,18, 0, 0, 0, 0, 0, 0},
		new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,17,18,18,18,18,17, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,17,12,13,13,14, 0, 0, 0, 0, 0, 0,18, 0, 0, 0, 0, 0, 0, 0, 0, 0,18,18,18, 0, 0, 0, 0, 0, 0},
		new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,16,18,18,18,18,16, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,16, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,18, 0, 0, 0, 0, 0, 0, 0, 0, 0,18,18,18, 0, 0, 0, 0, 0, 0},
		new int[] {2, 2, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2,15,18,18,18,18,15, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,15, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,18, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
        new int[] {1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
    };
    public static int[][] LEVEL_002 = {
        new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 0, 0, 0, 0, 0, 0},
        new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,12,13,14, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0,18,18,18, 0, 0, 0, 0, 0, 0},
        new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,17,12,13,13,14, 0, 0, 0, 0, 0, 0,18, 0, 0, 0, 0, 0, 0, 0, 0, 0,18,18,18, 0, 0, 0, 0, 0, 0},
        new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,16, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,18, 0, 0, 0, 0, 0, 0, 0, 0, 0,18,18,18, 0, 0, 0, 0, 0, 0},
        new int[] {2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,15, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,18, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
        new int[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
    };

    public CTileMap(int aLevel)
	{
		registerSingleton ();

        
        mTiles = new Sprite [NUM_TILES];
        for(int i = 0; i < CTileMap.NUM_TILES; i++)
        {
            if (i < 10)
            {
                mTiles[i] = Resources.Load<Sprite>("Sprites/tiles/tile00" + i.ToString());
            }
            else
            {
                mTiles[i] = Resources.Load<Sprite>("Sprites/tiles/tile0" + i.ToString());
            }
            
        }
        mEmptyTile = new CTile(0,0,0,mTiles[0]);
        mEmptyTile.setWalkable(true);
        
        loadLevel (aLevel);
        
    }

	public static CTileMap inst()
	{
		return mInst;
	}

	private void registerSingleton()
	{
		if (mInst == null) 
		{
			mInst = this;
		}
		else 
		{
			throw new UnityException( "ERROR: Cannot create another instance of singleton class CTileMap.");
		}
	}

	public void loadLevel(int aLevel)
	{
        if(aLevel == 1) {
            CLevelState.mLevel = 1;
            mMap = new List<List<CTile>>();

            for (int y = 0; y < MAP_HEIGHT; y++)
            {
                mMap.Add(new List<CTile>());

                for (int x = 0; x < MAP_WIDTH; x++)
                {
                    int index = LEVEL_001[y][x];
                    CTile tile = new CTile(x * TILE_WIDTH, y * TILE_HEIGHT, index, mTiles[index]);
                    mMap[y].Add(tile);
                    mMap[y][x].setWalkable(mWalkable[index]);
                    mMap[y][x].setPlatform(mPlatform[index]);
                    mMap[y][x].setLadder(mLadder[index]);
                    mMap[y][x].setHangable(mHangable[index]);

                }
            }
        }
        //sacar el hardcodeo
        if (aLevel >= 2)
        {
            CLevelState.mLevel = 2;
            mMap = new List<List<CTile>>();

            for (int y = 0; y < MAP_HEIGHT; y++)
            {
                mMap.Add(new List<CTile>());

                for (int x = 0; x < MAP_WIDTH; x++)
                {
                    int index = LEVEL_002[y][x];
                    CTile tile = new CTile(x * TILE_WIDTH, y * TILE_HEIGHT, index, mTiles[index]);
                    mMap[y].Add(tile);
                    mMap[y][x].setWalkable(mWalkable[index]);
                    mMap[y][x].setPlatform(mPlatform[index]);
                    mMap[y][x].setLadder(mLadder[index]);
                    mMap[y][x].setHangable(mHangable[index]);

                }
            }
        }
    }

	public void update()
	{
	}

	public void render()
	{
        if(mMap != null)
        {
            for (int y = 0; y < MAP_HEIGHT; y++)
            {
                for (int x = 0; x < MAP_WIDTH; x++)
                {
                    mMap[y][x].render();
                }
            }
        }
        
	}

	public void destroy()
	{
        if (mMap != null)
        {
            for (int y = MAP_HEIGHT - 1; y >= 0; y--)
            {
                for (int x = MAP_WIDTH - 1; x > 0; x--)
                {
                    mMap[y][x].destroy();
                    mMap[y][x] = null;
                }
                mMap.RemoveAt(y);
            }
            mMap = null;
        }
        
	}

	public bool isWalkable(int aX, int aY)
	{
		return mMap [aY] [aX].isWalkable ();
	}

    public bool isHangable(int aX, int aY)
    {
        return mMap[aY][aX].isHangable();
    }

    public int getTileIndex(int aX, int aY)
	{
        if (aX < 0 || aX >= CTileMap.MAP_WIDTH || aY < 0 || aY >= CTileMap.MAP_HEIGHT)
        {
            return 0;
        }
        else
        {
            return mMap[aY][aX].getTileIndex();
        }
        
	}

    public CTile getTile(int aX,int aY)
    {
        if (aX < 0 || aX >= CTileMap.MAP_WIDTH || aY < 0 || aY >= CTileMap.MAP_HEIGHT)
        {
            return mEmptyTile;
        }
        else
        {
            return mMap[aY][aX];
        }
        
    }
}
