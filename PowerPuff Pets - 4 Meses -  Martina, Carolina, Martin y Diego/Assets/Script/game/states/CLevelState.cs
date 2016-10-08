using UnityEngine;
using System.Collections;

public class CLevelState : CGameState
{
    //State Machine to define Tiles based on where you are located(garden, house, etc..)
    public const int HOME = 0;
    private CPlayer mPlayer;
    //private CBulletManager mBulletManager;
    private CEnemyManager mEnemyManager;
    private CPlayerManager mPlayerManager;
    //private CCloudManager mCloudManager;
    //private CBackgroundManager mBackgroundManager;
    private CWallManager mWallManager;
    private CEntityManager mEntityManager;
    private CCoinManager mCoinManager;
    //private MessageDispatcher mMessageDispatcher;
    //private static int mCloudSpeed = 15;
    private CTileMap mMap;
    private CCamera mCamera;
    public static int mLevel = 1;
    private CTower mCatTower;
    private CTower mCatTower2;
    private CTower mCatTower3;
    //This can be changed so each unit has a spawning point
    private int mStartPositionX = 15;
    private int mStartPositionY = 20;
       

    public CLevelState()
	{
        mEntityManager = new CEntityManager();
        //mMessageDispatcher = new MessageDispatcher();
        //mBulletManager = new CBulletManager ();
        mEnemyManager = new CEnemyManager ();
        mPlayerManager = new CPlayerManager();
        // mCloudManager = new CCloudManager();
        //  mBackgroundManager = new CBackgroundManager();
        mWallManager = new CWallManager();
        mCoinManager = new CCoinManager();
        //Debug.Log("wall ready");
        mMap = new CTileMap(1);
        //Debug.Log("Map ready");
        mPlayer = new CPlayer(mStartPositionX * CTileMap.TILE_WIDTH, mStartPositionY * CTileMap.TILE_HEIGHT,CPlayer.RANGE);
        //Debug.Log("Player ready");
        mCatTower = new CTower(0);
        mCatTower.setXY(mStartPositionX * CTileMap.TILE_WIDTH, mStartPositionY * CTileMap.TILE_HEIGHT);
        mCatTower2 = new CTower(0);
        mCatTower2.setXY(80 * CTileMap.TILE_WIDTH, 20 * CTileMap.TILE_HEIGHT);
        mCatTower3 = new CTower(0);
        mCatTower3.setXY(60 * CTileMap.TILE_WIDTH, 10 * CTileMap.TILE_HEIGHT);
        mCamera = new CCamera();
        //Debug.Log("Camera ready");
        mCamera.setXY(0,0);
        mCamera.setGameObjectToFollow(mPlayer);
        CGame.inst().setCamera(mCamera);
        CPlayerManager.inst().add(mCatTower);
        CEnemyManager.inst().add(mCatTower2);
        CEnemyManager.inst().add(mCatTower3);
        //Debug.Log("Level state constructed");
        //CAudioManager.Inst.PlayMusic("Song1");


    }


    override public void init()
	{
		base.init ();
        //setBackground();
        setWall();
	}

    public CPlayer getPlayer()
    {
        return mPlayer;
    }

    private void setWall()
    {
        int wallsPerScreen = 1;
        int screenWidth = wallsPerScreen*CGameConstants.WALL_WIDTH;
        int screensPerWorld = (int)(CGameConstants.WORLD_WIDTH / screenWidth);
        for(int i = 0; i < screensPerWorld; i++)
        {
            int index = 0;
            for (int j = 0; j < wallsPerScreen; j++)
            {
                CWall a;
                a = new CWall(CGameConstants.WALL_CONFIGURATIONS[0][index]);
                a.setXYZ(screenWidth*i + j * CGameConstants.WALL_WIDTH, 0, -2);
                a.setSortingLayerName("Background");
                a.setName("Wall");
                CWallManager.inst().add(a);
                index++;
                
            }
        }
        
    }
    private void setBackground()
    {
        int backgroundAmountWidth = (int)(CGameConstants.WORLD_WIDTH / CGameConstants.BACKGROUND_WIDTH) + 1;//+1 because 12.8 rounded as int leaves 12 and you miss that .8
        int backgroundAmountHeight = (int)(CGameConstants.WORLD_WIDTH / CGameConstants.BACKGROUND_WIDTH) + 1;//+1 because 12.8 rounded as int leaves 12 and you miss that .8
        for (int i=0;i < backgroundAmountWidth; i++)
        {
            CSprite a;
            a = new CSprite();
            a.setImage(Resources.Load<Sprite>("Sprites/Background/GoodSkyLower"));
            a.setXY(i*CGameConstants.BACKGROUND_WIDTH, 80);
            a.setSortingLayerName("Background");
            a.setName("background");
            CBackgroundManager.inst().add(a);
        }
        for(int j=0; j <= backgroundAmountHeight; j++)
        {
            for (int i = 0; i < backgroundAmountWidth; i++)
            {
                CSprite a;
                a = new CSprite();
                a.setImage(Resources.Load<Sprite>("Sprites/Background/GoodSkyHigher"));
                a.setXY(i * CGameConstants.BACKGROUND_WIDTH, -920 - j*CGameConstants.BACKGROUND_HEIGHT);
                a.setSortingLayerName("Background");
                a.setName("background");
                CBackgroundManager.inst().add(a);
            }
        }
    }

    /*private void createClouds()
    {
        //How many pixels there is between clouds(aprox since it's random)
        int frequency = 1000;
        int totalClouds = CGameConstants.WORLD_WIDTH/1000;
        for (int i = 0; i < totalClouds; i++)
        {
            CCloud a;
            int aCloudType = CMath.randomIntBetween(3, 5);
            int cloudPosition;
            cloudPosition = CMath.randomIntBetween(i * frequency, ((i + 1)*frequency));
            int aX = (int)(cloudPosition);
            int aY = CMath.randomIntBetween(0, CGameConstants.SCREEN_HEIGHT/2);
            a = new CCloud(mCloudSpeed, aCloudType, aX, aY);
            CCloudManager.inst().add(a);
        }
        
    }*/
    /*
    private void checkCloudPosition()
    {
        if(CCloudManager.inst().length() != 0)
        {
            for (int i = 0; i < CCloudManager.inst().length(); i++)
            {
                if(CCloudManager.inst().getArray()[i].getX() < -CCloudManager.inst().getArray()[i].getWidth())
                {
                    CCloudManager.inst().getArray()[i].setX(CGameConstants.WORLD_WIDTH);
                    CCloudManager.inst().getArray()[i].setY(CMath.randomIntBetween(0, CGameConstants.SCREEN_HEIGHT / 2)); 
                }
            }
        }
        
    }*/

    private void nextLevel() {
        
        mMap.destroy();
        mLevel += 1;
        mMap.loadLevel(mLevel);
        //mPlayer.restartPlayer();
        /*mCloudManager.destroy();
        mCloudManager = null;
        mCloudManager = new CCloudManager();
        mWallManager.destroy();
        mWallManager = null;
        mWallManager = new CWallManager();
        createClouds();
        mDreamCatcher = new CDreamCatcher(CGameConstants.WORLD_WIDTH - 2*CTileMap.TILE_WIDTH, CGameConstants.SCREEN_HEIGHT - 2 * CTileMap.TILE_HEIGHT);
        */
        mCamera.setXY(0, 0);
        CEnemyManager.inst().destroy();
        


    }
	override public void update()
	{
        
		base.update ();
        
        //mMessageDispatcher.update();
        //checkLoseCondition();
        //checkWinCondition();
        if (CKeyboard.firstPress (CKeyboard.ESCAPE)) 
		{
			CGame.inst().setState(new CMainMenuState());
			return;
		}
        //mBackgroundManager.update();
        mWallManager.update();
        mPlayerManager.update();
        mCoinManager.update();
        //mBulletManager.update();
		mEnemyManager.update();
        //mCloudManager.update();
        //checkCloudPosition();
		mMap.update();
        mCamera.update();
        
        
	}

    /*private void checkLoseCondition()
    {
        if (mPlayer.isDead())
        {
            restartLevel();
        }
    }*/

    /*private void checkWinCondition()
    {
        if (mPlayer.isGameOver())
        {
            nextLevel();
        }
    }

    private void restartLevel()
    {
        mPlayer.restartPlayer();
        mCamera.setXY(0, 0);
    }
    */
    override public void render()
	{
		base.render ();

        //mBackgroundManager.render();
        mWallManager.render();
        //mCloudManager.render();
		mPlayerManager.render();
        mCoinManager.render();
        //mBulletManager.render();
		mEnemyManager.render();
        mMap.render();
	}

	override public void destroy()
	{
		base.destroy();

        //mBackgroundManager.destroy();
        //mBackgroundManager = null;
        mWallManager.destroy();
        mWallManager = null;
        mPlayerManager.destroy();
        mPlayerManager = null;
        //mBulletManager.destroy();
        //mBulletManager = null;
        mEnemyManager.destroy();
        mEnemyManager = null;
        mCoinManager.destroy();
        mCoinManager = null;
        //mCloudManager.destroy();
        //mCloudManager = null;
        mMap.destroy();
		mMap = null;
	}

	
}

