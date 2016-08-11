using UnityEngine;
using System.Collections;

public class CPlayer : CAnimatedSprite
{
    // STATE MACHINE
    private const int STATE_STAND = 0;
	private const int STATE_WALKING = 1;
    private const int STATE_FREEZE = 2;
    private const int STATE_FALLING_DEAD = 3;
    private const int STATE_STAND_AFTER_DEAD = 4;
    private const int STATE_GAME_OVER = 5;
    private const int STATE_EXPLODING = 6;
    private const int STATE_JUMP = 7;
    private const int STATE_FALL = 8;
    private const int STATE_START_CLIMB_DOWN = 9;
    private const int STATE_CLIMB = 10;
    private const int STATE_END_CLIMB = 11;
    //From above to climb down to the platform, as seen from the side
    private const int STATE_START_HANG = 12;
    //Idle while hanging from the side of a platform
    private const int STATE_HANG = 13;
    //From below to climb up to the platform, as seen from the side
    private const int STATE_END_HANG = 14;
    private const int STATE_START_CRAWL = 15;
    private const int STATE_CRAWL = 16;
    private const int STATE_END_CRAWL = 17;
    private const int STATE_DYING = 18;
    private const int STATE_WINNING = 19;
    
    private const int TIME_FREEZE = 30;
    private const int MAX_BULLETS = 1;

    //private int Y_LIMIT = -CGameConstants.SCREEN_HEIGHT - CPlayer.PLAYER_HEIGHT;
    private const int SPEED = 240;
    private const int SPEED_LADDER = 240;
    private const int TURN_SPEED = 5;
    private const float ACCEL = 0.1f;
    private const float FRICTION = 0.99f;

	public static int PLAYER_WIDTH = 200;
	public static int PLAYER_HEIGHT = 300;
    public static int X_OFFSET_BOUNDING_BOX = 50;
    public static int Y_OFFSET_BOUNDING_BOX = 50;
    private int mBulletCount;
    private CTile tileTopLeft;
    private CTile tileTopRight;
    private CTile tileMiddleLeft;
    private CTile tileMiddleRight;
    private CTile tileDownLeft;
    private CTile tileDownRight;
    private CTile tileFloorLeft;
    private CTile tileFloorRight;
    private int mIgnoreRowFloor = 1;
    private float mOldX;
    private float mOldY;
    private float mOldVelY = 0f;
    


    public CPlayer()
	{
		setFrames (Resources.LoadAll<Sprite> ("Sprites/player"));
        setOldXYPosition();
        setMaxSpeed(CTileMap.TILE_HEIGHT);
        setName("Player");
        setSortingLayerName ("Player");
        setRegistration(CSprite.REG_TOP_LEFT);
        setWidth(PLAYER_WIDTH);
        setHeight(PLAYER_HEIGHT);
        setXY (0, CGameConstants.SCREEN_HEIGHT - getHeight() - CTileMap.TILE_HEIGHT/2.5f);
        mOldX = getX();
        mOldY = getY();
        setState (STATE_STAND);
        render ();
        
	}

    public void restartPlayer()
    {
        
        setXY(0, CGameConstants.SCREEN_HEIGHT - getHeight() - CTileMap.TILE_HEIGHT / 2.5f);
        mOldX = getX();
        mOldY = getY();
        if (isDead())
        {
            setState(CPlayer.STATE_STAND_AFTER_DEAD);
            setDead(false);
            render();
            return;
        }
        else if(isGameOver())
        {
            setState(CPlayer.STATE_STAND);
            render();
            return;
        }
        
        
        
    }
    override public void update()
    {
        CGameObject enemy = CEnemyManager.inst().collides(this);

        if (enemy != null)
        {
            //enemy.hit();//TODO: crear la funcion hit en el enemigo
            if (!isDead())
            {
                setDead(true);
            }
                
        }

        CGameObject dreamcatcher = CDreamCatcherManager.inst().collides(this);

        if (dreamcatcher != null)
        {
            setState(CPlayer.STATE_GAME_OVER);
            dreamcatcher.hit();
            return;
                
        }

        if (getState() == CPlayer.STATE_STAND)
        {
            if (CKeyboard.pressed(CKeyboard.UP) && takeLadderUp())
            {
                setState(CPlayer.STATE_CLIMB);
                return;
            }
            if (CKeyboard.pressed(CKeyboard.DOWN) && hangDown())
            {
                setState(CPlayer.STATE_START_HANG);
                return;
            }
            else if (CKeyboard.pressed(CKeyboard.DOWN) && takeLadderDown())
            {
                setState(CPlayer.STATE_START_CLIMB_DOWN);
                return;
            }

            else if (CKeyboard.pressed(CKeyboard.DOWN))
            {
                setState(CPlayer.STATE_START_CRAWL);
                return;
            }
            if (!isFloorBelow(getX(), getY() + 1))
            {
                setState(CPlayer.STATE_FALL);
                return;
            }
            
            if (CKeyboard.firstPress(CKeyboard.SPACE))
            {
                setState(CPlayer.STATE_JUMP);
                return;
            }
            if (((!CKeyboard.pressed(CKeyboard.LEFT) && !CKeyboard.pressed(CKeyboard.RIGHT)) || (CKeyboard.pressed(CKeyboard.LEFT) && CKeyboard.pressed(CKeyboard.RIGHT)))&& getState() != CPlayer.STATE_STAND)
            {
                setState(CPlayer.STATE_STAND);
                return;
            }
            else
            if (CKeyboard.pressed(CKeyboard.LEFT))
            {
                if (!isWallLeft(getX() - CPlayer.SPEED * Time.deltaTime, getY()))
                {
                    setState(CPlayer.STATE_WALKING);
                    return;
                }
            }

            else if (CKeyboard.pressed(CKeyboard.RIGHT))
            {
                if (!isWallRight(getX() + CPlayer.SPEED * Time.deltaTime, getY()))
                {
                    setState(CPlayer.STATE_WALKING);
                    return;
                }

            }

        }
        else
        {
            if (getState() == STATE_WALKING)
            {
                if (CKeyboard.pressed(CKeyboard.UP) && takeLadderUp())
                {
                    setState(CPlayer.STATE_CLIMB);
                    return;
                }
                if (CKeyboard.pressed(CKeyboard.DOWN) && takeLadderDown())
                {
                    setState(CPlayer.STATE_START_CLIMB_DOWN);
                    return;
                }
                if (CKeyboard.pressed(CKeyboard.DOWN) && hangDown())
                {
                    setState(CPlayer.STATE_START_HANG);
                    return;
                }
                if (!isFloorBelow(getX(), getY() + 1))
                {
                    setState(CPlayer.STATE_FALL);
                    return;
                }

                if (CKeyboard.firstPress(CKeyboard.SPACE))
                {
                    setState(CPlayer.STATE_JUMP);
                    return;
                }


                if ((!CKeyboard.pressed(CKeyboard.LEFT) && !CKeyboard.pressed(CKeyboard.RIGHT)) || (CKeyboard.pressed(CKeyboard.LEFT) && CKeyboard.pressed(CKeyboard.RIGHT)))
                {
                    setState(CPlayer.STATE_STAND);
                    return;
                }
                else
                {
                    if (CKeyboard.pressed(CKeyboard.LEFT))
                    {

                        setFlipH(true);
                        if (!isWallLeft(getX() - CPlayer.SPEED * Time.deltaTime, getY()))
                        {
                            setVelX(-CPlayer.SPEED);
                        }
                        else
                        {
                            setX((((int)((getX() + CPlayer.X_OFFSET_BOUNDING_BOX - CPlayer.SPEED * Time.deltaTime) / CTileMap.TILE_WIDTH) + 1) * CTileMap.TILE_WIDTH) - CPlayer.X_OFFSET_BOUNDING_BOX);
                            setState(CPlayer.STATE_STAND);
                            return;

                        }

                    }

                    if (CKeyboard.pressed(CKeyboard.RIGHT))
                    {
                        setFlipH(false);
                        if (!isWallRight(getX() + CPlayer.SPEED * Time.deltaTime, getY()))
                        {
                            setVelX(CPlayer.SPEED);
                        }
                        else
                        {
                            setX((((int)((getX() - CPlayer.X_OFFSET_BOUNDING_BOX + getWidth() + CPlayer.SPEED * Time.deltaTime) / CTileMap.TILE_WIDTH) * CTileMap.TILE_WIDTH) - getWidth()) + CPlayer.X_OFFSET_BOUNDING_BOX);
                            setState(CPlayer.STATE_STAND);
                            return;
                        }

                    }


                }
            }
            else if (getState() == CPlayer.STATE_CLIMB)
            {
                if (isWallBelow(getX(), getY() + 1) && CKeyboard.pressed(CKeyboard.DOWN))
                {
                    setState(CPlayer.STATE_STAND);
                    return;
                }
                if (CKeyboard.firstPress(CKeyboard.SPACE))
                {
                    setState(CPlayer.STATE_FALL);
                    return;
                }
                if (CKeyboard.pressed(CKeyboard.LEFT))
                {
                    setFlipH(true);
                }
                else if (CKeyboard.pressed(CKeyboard.RIGHT))
                {
                    setFlipH(false);
                }
                if (!CKeyboard.pressed(CKeyboard.UP) && !CKeyboard.pressed(CKeyboard.DOWN))
                {
                    pauseAnimation();
                    setVelY(0);
                }
                else if (CKeyboard.pressed(CKeyboard.UP))
                {
                    if (isWallAbove(getX(), getY() - CPlayer.SPEED_LADDER * Time.deltaTime))
                    {
                        setY(((((int)((getY() + CPlayer.Y_OFFSET_BOUNDING_BOX - CPlayer.SPEED * Time.deltaTime) / CTileMap.TILE_HEIGHT)) + 1) * CTileMap.TILE_HEIGHT) - CPlayer.Y_OFFSET_BOUNDING_BOX);
                        pauseAnimation();
                        return;
                    }
                    else
                    {
                        if (!isOnLadder(getX(), getY() - CPlayer.SPEED_LADDER * Time.deltaTime))
                        {
                            setState(CPlayer.STATE_END_CLIMB);
                            return;
                        }
                        else
                        {
                            setVelY(-CPlayer.SPEED_LADDER);
                            continueAnimation();
                        }
                    }
                }
                else if (CKeyboard.pressed(CKeyboard.DOWN))
                {
                    if (isWallBelow(getX(), getY() + CPlayer.SPEED_LADDER * Time.deltaTime))
                    {
                        setY((((int)((getY() + getHeight() - 1 + CPlayer.SPEED * Time.deltaTime) / CTileMap.TILE_HEIGHT)) * CTileMap.TILE_HEIGHT) - getHeight());
                        setState(CPlayer.STATE_STAND);
                        return;
                    }
                    else
                    {
                        if (!isOnLadder(getX(), getY() + CPlayer.SPEED_LADDER * Time.deltaTime))
                        {
                            setState(CPlayer.STATE_FALL);
                            return;
                        }
                        else
                        {
                            setVelY(CPlayer.SPEED_LADDER);
                            continueAnimation();
                        }
                    }
                }
            }
            else if (getState() == CPlayer.STATE_END_CLIMB)
            {
                if (isEnded())
                {
                    setY(((int)(getY() / CTileMap.TILE_WIDTH) * CTileMap.TILE_WIDTH) - getHeight() / 3);
                    setX((int)(getX() / CTileMap.TILE_WIDTH) * CTileMap.TILE_WIDTH);
                    setState(CPlayer.STATE_STAND);
                    return;
                }
            }

            else if (getState() == CPlayer.STATE_START_CLIMB_DOWN)
            {
                if (isEnded())
                {
                    setY(getY() + getHeight() / 2);
                    setState(CPlayer.STATE_CLIMB);
                    return;
                }
            }
            else if (getState() == CPlayer.STATE_START_HANG)
            {
                if (isEnded())
                {
                    setState(CPlayer.STATE_HANG);
                    return;
                }
            }
            else if (getState() == CPlayer.STATE_HANG)
            {
                if (CKeyboard.pressed(CKeyboard.UP))
                {
                    setState(CPlayer.STATE_END_HANG);
                    return;
                }
                if (CKeyboard.pressed(CKeyboard.DOWN))
                {
                    setState(CPlayer.STATE_FALL);
                    mIgnoreRowFloor = (int)((getY()) / CTileMap.TILE_HEIGHT);
                    return;
                }
            }
            else if (getState() == CPlayer.STATE_END_HANG)
            {
                if (isEnded())
                {
                    int x = (int)getX();
                    int y = (int)getY();
                    x = (int)(x / CTileMap.TILE_WIDTH);
                    y = (int)(y / CTileMap.TILE_HEIGHT);
                    x = (x * CTileMap.TILE_WIDTH) + CTileMap.TILE_WIDTH;
                    y = (y * CTileMap.TILE_HEIGHT) - CTileMap.TILE_HEIGHT /*+ getHeight()*/;
                    setX(x);
                    setY(y);
                    setState(CPlayer.STATE_STAND);
                    return;
                }
            }
            else if (getState() == CPlayer.STATE_START_CRAWL)
            {
                if (isEnded())
                {
                    setState(CPlayer.STATE_CRAWL);
                    return;
                }
            }
            else if (getState() == CPlayer.STATE_CRAWL)
            {
                if (!isFloorBelow(getX(), getY() + 1))
                {
                    setState(CPlayer.STATE_FALL);
                    return;
                }
                if (isTunnelTop(getX() - CPlayer.SPEED * Time.deltaTime, getY()) && CKeyboard.pressed(CKeyboard.UP))
                {
                    setState(CPlayer.STATE_END_CRAWL);
                    return;
                }
                if ((!CKeyboard.pressed(CKeyboard.LEFT) && !CKeyboard.pressed(CKeyboard.RIGHT)) || (CKeyboard.pressed(CKeyboard.LEFT) && CKeyboard.pressed(CKeyboard.RIGHT)))
                {
                    stopMove();
                    pauseAnimation();
                    
                }
                else
                {
                    continueAnimation();
                    if (CKeyboard.pressed(CKeyboard.LEFT))
                    {
                        
                        setFlipH(true);
                        if (isTunnelLeft(getX() - CPlayer.SPEED * Time.deltaTime, getY()) || !isWallLeft(getX() - CPlayer.SPEED * Time.deltaTime, getY()))
                        {
                            setVelX(-CPlayer.SPEED);
                        }
                        else
                        {
                            setX((((int)((getX() + CPlayer.X_OFFSET_BOUNDING_BOX - CPlayer.SPEED * Time.deltaTime) / CTileMap.TILE_WIDTH) + 1) * CTileMap.TILE_WIDTH) - CPlayer.X_OFFSET_BOUNDING_BOX);
                            setState(CPlayer.STATE_STAND);
                            return;

                        }

                    }

                    if (CKeyboard.pressed(CKeyboard.RIGHT))
                    {
                        
                        setFlipH(false);
                        if (isTunnelRight(getX() - CPlayer.SPEED * Time.deltaTime, getY()) || !isWallRight(getX() - CPlayer.SPEED * Time.deltaTime, getY()))
                        {
                            setVelX(CPlayer.SPEED);
                        }
                        else
                        {
                            setX((((int)((getX() - CPlayer.X_OFFSET_BOUNDING_BOX + getWidth() + CPlayer.SPEED * Time.deltaTime) / CTileMap.TILE_WIDTH) * CTileMap.TILE_WIDTH) - getWidth()) + CPlayer.X_OFFSET_BOUNDING_BOX);
                            setState(CPlayer.STATE_STAND);
                            return;
                        }

                    }
                    
                }
            }
            else if (getState() == CPlayer.STATE_END_CRAWL)
            {
                setState(CPlayer.STATE_STAND);
                return;
            }else if (getState() == CPlayer.STATE_DYING)
            {
                if (isEnded())
                {
                    setDead(true);
                    
                }
            }
            else if (getState() == CPlayer.STATE_STAND_AFTER_DEAD)
            {
                Debug.Log(getTimeState()*10);
                if (getTimeState() > 3)
                {
                    setState(CPlayer.STATE_STAND);
                    return;
                }
            
            }
    }

        checkBorders();
        base.update();
        

        if (getState() == CPlayer.STATE_JUMP)
        {
            //Control which row to ignore when checking collisions against the floor
            if (getVelY() >= 0 && mOldVelY < 0)
            {
                mIgnoreRowFloor = (int)((getY() + getHeight() - 1) / CTileMap.TILE_HEIGHT);
            }
            controlMoveHorizontal();
            int row = (int)(getY()) / CTileMap.TILE_HEIGHT;
            if (row != mIgnoreRowFloor)
            {
                if (catchBorder())
                {
                    setState(CPlayer.STATE_HANG);
                    return;
                }
            }
            
            if (getVelY() < 0)
            {
                if(isWallAbove(getX(), getY() - 1))
                {
                    setY(((int)((getY() + CPlayer.Y_OFFSET_BOUNDING_BOX) / CTileMap.TILE_HEIGHT + 1)*CTileMap.TILE_HEIGHT) - CPlayer.Y_OFFSET_BOUNDING_BOX);
                    setState(CPlayer.STATE_FALL);
                    return;
                }
            }
            if(getVelY() > 0)
            {
                row = (int)(getY() + getHeight() - 1) / CTileMap.TILE_HEIGHT;
                if(row != mIgnoreRowFloor)
                {
                    if (isFloorBelow(getX(), getY() + 1))
                    {
                        setY((((int)((getY() + getHeight())/CTileMap.TILE_HEIGHT)*CTileMap.TILE_HEIGHT) - getHeight()));
                        setState(CPlayer.STATE_STAND);
                        return;
                        
                    }
                }
            }
        } else if (getState() == CPlayer.STATE_FALL)
        {
            int row = (int)(getY()/CTileMap.TILE_HEIGHT);
            if (row != mIgnoreRowFloor)
            {
                if (catchBorder())
                {
                    setState(CPlayer.STATE_HANG);
                    return;
                }
            }
           
            controlMoveHorizontal();
            row = (int)(getY() + getHeight() - 1) / CTileMap.TILE_HEIGHT;
            if (row != mIgnoreRowFloor)
            {
                if (isFloorBelow(getX(), getY() + 1))
                {
                    setY((((int)((getY() + getHeight()) / CTileMap.TILE_HEIGHT) * CTileMap.TILE_HEIGHT) - getHeight()));
                    setState(CPlayer.STATE_STAND);
                    return;
                }
            }
        }
        setOldXYPosition();

    }
	
	override public void render()
	{
        if (getState() == CPlayer.STATE_GAME_OVER)
        {
            return;
        }

        else if (getState() == CPlayer.STATE_FREEZE || getState() == CPlayer.STATE_STAND_AFTER_DEAD)
        {
            if ((int)(getTimeState()*10) % 2 == 0)
            {
                setVisible(true);
            }
            else
            {
                setVisible(false);
            }
        }
        
        base.render ();
		
		
	}
	
	override public void destroy()
	{
        base.destroy();
	}


    
    private void checkBorders()
    {
        if (getX() < 0)
        {
            setX(0);
        }
        if (getX() + getWidth() > CGameConstants.WORLD_WIDTH)
        {
            setX(CGameConstants.WORLD_WIDTH - getWidth());
        }
        if(getY() > CGameConstants.SCREEN_HEIGHT)
        {
            setDead(true);
        }
    }
    private void controlMoveHorizontal()
    {
        if (!CKeyboard.pressed(CKeyboard.LEFT) && !CKeyboard.pressed(CKeyboard.RIGHT))
        {
            setVelX(0);
        }
        else
        {
            if (isWallLeft(getX(), mOldY))
            {
                setX((((int)((getX() + CPlayer.X_OFFSET_BOUNDING_BOX) / CTileMap.TILE_WIDTH) + 1) * CTileMap.TILE_WIDTH) - CPlayer.X_OFFSET_BOUNDING_BOX);
                setVelX(0);
            }
            else if (isWallRight(getX(), mOldY))
            {
                setX(((((int)((getX() - CPlayer.X_OFFSET_BOUNDING_BOX + getWidth()) / CTileMap.TILE_WIDTH)) * CTileMap.TILE_WIDTH) - getWidth()) + CPlayer.X_OFFSET_BOUNDING_BOX);
                setVelX(0);
            } else if (CKeyboard.pressed(CKeyboard.LEFT))
            {
                if (!isWallLeft(getX() - CPlayer.SPEED * Time.deltaTime,mOldY))
                {
                    setVelX(-CPlayer.SPEED);
                    setFlipH(true);
                }
            }
            else if(CKeyboard.pressed(CKeyboard.RIGHT))
            {
                if (!isWallRight(getX() + CPlayer.SPEED * Time.deltaTime, mOldY))
                {
                    setVelX(CPlayer.SPEED);
                    setFlipH(false);
                }
            }
        }
    }

 
    //TODO: REDO THE STATEMENTS SINCE ITS ALL COPY PASTE
    override public void setState(int aState)
	{
        base.setState (aState);
        
        setVisible (true);
        if (getState () == STATE_STAND) 
		{
            Debug.Log("STATE STAND");
            initAnimation (1, 6, 3, true);
			stopMove();
            
		}
        else if (getState () == STATE_WALKING) 
		{
            Debug.Log("STATE WALKING");
            initAnimation (7, 14, 10, true);
		}
        else if (getState() == STATE_FREEZE)
        {
            Debug.Log("STATE FREEZE");
            //initAnimation(2, 9, 10, true);
        }
        else if (getState() == STATE_FALLING_DEAD)
        {
            Debug.Log("STATE FALLING DEAD");
            //initAnimation(2, 9, 10, true);
        }
        else if (getState() == STATE_STAND_AFTER_DEAD)
        {
            Debug.Log("STATE STAND AFTER DEAD");
            stopMove();
            initAnimation(1, 6, 3, true);
        }
        else if (getState() == STATE_GAME_OVER)
        {
            Debug.Log("STATE GAME OVER");
            //initAnimation(2, 9, 10, true);
        }
        else if (getState() == STATE_EXPLODING)
        {
            Debug.Log("STATE EXPLODING");
            //initAnimation(2, 9, 10, true);
        }
        else if (getState() == STATE_JUMP)
        {
            Debug.Log("STATE JUMP");
            setVelY(-450);
            setAccelY(450);
            //initAnimation(15, 20, 7, false);
            initAnimation(15, 18, 7, false);

        }
        else if (getState() == STATE_FALL)
        {
            Debug.Log("STATE FALL");
            setVelY(0);
            setAccelY(900);
            gotoAndStop(18);
            mIgnoreRowFloor = (int)((getY() + getHeight()) / CTileMap.TILE_HEIGHT);
        }
        else if (getState() == STATE_CLIMB)
        {
            Debug.Log("STATE CLIMB");
            stopMove();
            initAnimation(42, 43, 10, true);

        }
        else if (getState() == STATE_END_CLIMB)
        {
            Debug.Log("STATE END CLIMB");
            stopMove();
            setY(getY() - 40);//adjust number so the climbing animation is done in the right place
            initAnimation(21, 25, 10, false);
        }
        else if (getState() == STATE_START_CLIMB_DOWN)
        {
            Debug.Log("STATE START CLIMB DOWN");
            stopMove();
            setY(getY() + 40);
            initAnimation(7, 14, 10, false);
        }
        else if (getState() == STATE_START_HANG)
        {
            Debug.Log("STATE START HANG");
            stopMove();
            setY(getY() + 40);
            initAnimation(22, 25, 10, false);
        }
        else if (getState() == STATE_HANG)
        {
            Debug.Log("STATE HANG");
            stopMove();
            setY(getY() + 50 + getHeight() / 2);
            initAnimation(38, 41, 5, true);

        }
        else if (getState() == STATE_END_HANG)
        {
            Debug.Log("STATE END HANG");
            stopMove();
            setY(getY() - 80);
            initAnimation(22, 25, 10, false);
        }
        else if (getState() == STATE_START_CRAWL)
        {
            Debug.Log("STATE START CRAWL");
            stopMove();
            initAnimation(26, 30, 10, false);
        }
        else if (getState() == STATE_CRAWL)
        {
            Debug.Log("STATE CRAWL");
            stopMove();
            initAnimation(36, 37, 10, true);
        }
        else if (getState() == STATE_END_CRAWL)
        {
            Debug.Log("STATE END CRAWL");
            stopMove();
            initAnimation(31, 35, 10, false);
        }
        else if (getState() == STATE_DYING)
        {
            Debug.Log("STATE DYING");
            stopMove();
            initAnimation(44, 45, 10, false);
        }
       
    }
    

    public void checkPoints(float aX, float aY){
		int leftX = (int)((aX + CPlayer.X_OFFSET_BOUNDING_BOX) / CTileMap.TILE_WIDTH);
		int upY = (int)((aY + CPlayer.Y_OFFSET_BOUNDING_BOX) / CTileMap.TILE_HEIGHT);
		int rightX = (int)((aX + getWidth () - 1 - CPlayer.X_OFFSET_BOUNDING_BOX) / CTileMap.TILE_WIDTH);
		int downY = (int)((aY + getHeight () - 1 - CPlayer.Y_OFFSET_BOUNDING_BOX) / CTileMap.TILE_HEIGHT);
        int floorY = (int)((aY + getHeight() - 1) / CTileMap.TILE_HEIGHT);
        tileTopLeft = CTileMap.inst().getTile(leftX, upY);
		tileTopRight = CTileMap.inst().getTile(rightX, upY);
		tileDownLeft = CTileMap.inst().getTile(leftX, downY);
		tileDownRight = CTileMap.inst().getTile(rightX, downY);
        tileFloorLeft = CTileMap.inst().getTile(leftX, floorY);
        tileFloorRight = CTileMap.inst().getTile(rightX,floorY);
        //Debug.Log("tileTopLeft " + tileTopLeft.isWalkable());
        //Debug.Log("tileTopRight " + tileTopRight.isWalkable());
        //Debug.Log("tileDownLeft " + tileDownLeft.isWalkable());
        //Debug.Log("tileDownRight" + tileDownRight.isWalkable());
        //Debug.Log("tileFloorLeft" + tileFloorLeft.isWalkable());
        //Debug.Log("tileFloorRight" + tileFloorRight.isWalkable());
        
    }

    public bool isWallLeft(float aX, float aY)
    {
        checkPoints(aX, aY);
        //Debug.Log("isWallLeft " + (!tileTopLeft.isWalkable() || !tileDownLeft.isWalkable()).ToString());
        return !tileTopLeft.isWalkable() || !tileDownLeft.isWalkable() || !tileFloorLeft.isWalkable();
    }

    public bool isWallRight(float aX, float aY)
    {
        checkPoints(aX, aY);
        //Debug.Log("isWallRight " +(!tileTopRight.isWalkable() || !tileDownRight.isWalkable() || !tileFloorRight.isWalkable()));
        return !tileTopRight.isWalkable() || !tileDownRight.isWalkable() || !tileFloorRight.isWalkable();
    }
    
    public bool isFloorBelow(float aX, float aY)
    {
        checkPoints(aX, aY);
        //Debug.Log("isFloorBelow " + (tileDownLeft.isFloor() || tileDownRight.isFloor()).ToString());
        return tileFloorLeft.isFloor() || tileFloorRight.isFloor();
    }
        

    public bool isWallAbove(float aX, float aY)
    {
        checkPoints(aX, aY);
        //Debug.Log("isWallAbove " +(!tileTopLeft.isWalkable() || !tileTopRight.isWalkable().ToString());
        return !tileTopLeft.isWalkable() || !tileTopRight.isWalkable();
    }

    public bool isWallBelow(float aX, float aY)
    {
        checkPoints(aX, aY);
        //Debug.Log("isWallBelow " +(!tileFloorLeft.isWalkable() || !tileFloorRight.isWalkable()).ToString());
        return !tileFloorLeft.isWalkable() || !tileFloorRight.isWalkable();
    }

    public bool isOnLadder(float aX, float aY)
    {
        aX = (int)(aX);
        aY = (int)(aY);
        int leftX = (int)((aX + CPlayer.X_OFFSET_BOUNDING_BOX) / CTileMap.TILE_WIDTH);
        int rightX = (int)((aX + getWidth() - 1 - CPlayer.X_OFFSET_BOUNDING_BOX) / CTileMap.TILE_WIDTH);
        int middleY = (int)((aY + getHeight() / 2) / CTileMap.TILE_HEIGHT);
        tileMiddleLeft = CTileMap.inst().getTile(leftX, middleY);
        tileMiddleRight = CTileMap.inst().getTile(rightX, middleY);
        return tileMiddleLeft.isLadder() || tileMiddleRight.isLadder();
    }

    public bool isLadderBelow(float aX, float aY)
    {
        aX = (int)(aX);
        aY = (int)(aY);
        int leftX = (int)((aX + CPlayer.X_OFFSET_BOUNDING_BOX) / CTileMap.TILE_WIDTH);
        int rightX = (int)((aX + getWidth() - 1 - CPlayer.X_OFFSET_BOUNDING_BOX) / CTileMap.TILE_WIDTH);
        int downY = (int)((aY + getHeight() - 1 - CPlayer.Y_OFFSET_BOUNDING_BOX - 10) / CTileMap.TILE_HEIGHT);
        tileDownLeft = CTileMap.inst().getTile(leftX, downY);
        tileDownRight = CTileMap.inst().getTile(rightX, downY);
        return tileDownLeft.isLadder() || tileDownRight.isLadder();
    }

    public bool takeLadderUp()
    {
        int aX = (int)(getX());
        int aY = (int)(getY());
        int leftX = (int)((aX + CPlayer.X_OFFSET_BOUNDING_BOX + 8) / CTileMap.TILE_WIDTH);
        int rightX = (int)((aX + getWidth() - 1 - CPlayer.X_OFFSET_BOUNDING_BOX - 8) / CTileMap.TILE_WIDTH);
        int upY = (int)((aY + CPlayer.Y_OFFSET_BOUNDING_BOX) / CTileMap.TILE_HEIGHT);
        tileTopLeft = CTileMap.inst().getTile(leftX, upY);
        tileTopRight = CTileMap.inst().getTile(rightX, upY);
        if (tileTopLeft.isLadder() && tileTopRight.isLadder())
            {
                int x = leftX * CTileMap.TILE_WIDTH;
                x = x - (int)((getWidth() - CTileMap.TILE_WIDTH) / 2);
                setX(x);
                return true;
            }
        return false;
    }

    public bool takeLadderDown()
    {
        int aX = (int)(getX());
        int aY = (int)(getY());
        int leftX = (int)((aX + CPlayer.X_OFFSET_BOUNDING_BOX + 8) / CTileMap.TILE_WIDTH);
        int rightX = (int)((aX + getWidth() - 1 - CPlayer.X_OFFSET_BOUNDING_BOX - 8) / CTileMap.TILE_WIDTH);
        int floorY = (int)((aY + getHeight()) / CTileMap.TILE_HEIGHT);
        tileFloorLeft = CTileMap.inst().getTile(leftX,floorY);
        tileFloorRight = CTileMap.inst().getTile(rightX, floorY);
        if (tileFloorLeft.isLadder() && tileFloorRight.isLadder())
        {
            int x = leftX * CTileMap.TILE_WIDTH;
            x = x - ((getWidth() - CTileMap.TILE_WIDTH) / 2);
            setX(x);
            return true;
        }
        return false;
    }

    //public int getTile
    public bool hangDown()
    {
        int aX = (int)(getX());
        int aY = (int)(getY());
        int leftX = (int)((aX + CPlayer.X_OFFSET_BOUNDING_BOX/2) / CTileMap.TILE_WIDTH);
        int rightX = (int)((aX + getWidth() - 1 - CPlayer.X_OFFSET_BOUNDING_BOX/2) / CTileMap.TILE_WIDTH);
        int floorY = (int)((aY + getHeight()) / CTileMap.TILE_HEIGHT);
        tileFloorLeft = CTileMap.inst().getTile(leftX, floorY);
        tileFloorRight = CTileMap.inst().getTile(rightX, floorY);
        if (tileFloorLeft.isHangable() && !tileFloorRight.isPlatform() && !isFloorBelow((getX() + getWidth() / 2), getY() + getHeight() + 1))
        {
            Debug.Log("True");
            setFlipH(true);
            int x = leftX * CTileMap.TILE_WIDTH;
            x += CTileMap.TILE_WIDTH;
            setX(x - 40/*distancia entre las manos del pj y el borde del frame*/);
            return true;
        }
        else if (tileFloorRight.isHangable() && !tileFloorLeft.isPlatform() && !isFloorBelow((getX() + getWidth() / 2), getY() + getHeight() + 1))
        {
            Debug.Log("False");
            setFlipH(false);
            int x = rightX * CTileMap.TILE_WIDTH;
            x -= CTileMap.TILE_WIDTH;
            setX(x + 40/*distancia entre las manos del pj y el borde del frame*/);
            return true;
        }
        return false;
    }

    public bool isTunnelLeft(float aX, float aY)
    {
        checkPoints(aX, aY);
        //Debug.Log("isTunnelLeft " + (!tileTopLeft.isWalkable() || !tileDownLeft.isWalkable()).ToString());
        return !tileTopLeft.isWalkable() && tileDownLeft.isWalkable();
    }

    public bool isTunnelRight(float aX, float aY)
    {
        checkPoints(aX, aY);
        //Debug.Log("isTunnelRight " +(!tileTopRight.isWalkable() || !tileDownRight.isWalkable() || !tileFloorRight.isWalkable()));
        return !tileTopRight.isWalkable() && tileDownRight.isWalkable();
    }

    public bool isTunnelTop(float aX, float aY)
    {
        checkPoints(aX, aY);
        //Debug.Log("isTunnelRight " +(!tileTopRight.isWalkable() || !tileDownRight.isWalkable() || !tileFloorRight.isWalkable()));
        return (tileTopRight.isWalkable() && tileTopLeft.isWalkable());
    }

    public bool catchBorder()
    {
        int x = (int)(getX());
        int y = (int)(getY());
        int leftX = (int)((x + CPlayer.X_OFFSET_BOUNDING_BOX) / CTileMap.TILE_WIDTH);
        int rightX = (int)((x + getWidth() - 1 - CPlayer.X_OFFSET_BOUNDING_BOX) / CTileMap.TILE_WIDTH);
        int middleY = (int)((y + getHeight() / 2) / CTileMap.TILE_HEIGHT);
        int upY = (int)((y + CPlayer.Y_OFFSET_BOUNDING_BOX) / CTileMap.TILE_HEIGHT);
        tileTopLeft = CTileMap.inst().getTile(leftX, upY); ;
        tileTopRight = CTileMap.inst().getTile(rightX, upY); ;
        tileMiddleLeft = CTileMap.inst().getTile(leftX, middleY);
        tileMiddleRight = CTileMap.inst().getTile(rightX, middleY);
        if (((tileMiddleLeft.isHangable() && tileMiddleLeft.isPlatform()) ^ (tileMiddleRight.isHangable() && tileMiddleRight.isPlatform())) && getVelY() > 0)
        {

            if (tileMiddleLeft.isHangable() && tileMiddleLeft.isPlatform() && getFlipH())
            {
                
                x = leftX * CTileMap.TILE_WIDTH;
                x += CTileMap.TILE_WIDTH;
                setX(x - 40/*distancia entre las manos del pj y el borde del frame*/);
                y = (int)(middleY / CTileMap.TILE_HEIGHT);
                y = y * CTileMap.TILE_HEIGHT;
                setY(y);
                return true;
            }
            else if (tileMiddleRight.isHangable() && tileMiddleRight.isPlatform() && !getFlipH())
            {
                x = rightX * CTileMap.TILE_WIDTH;
                x -= CTileMap.TILE_WIDTH;
                setX(x + 40/*distancia entre las manos del pj y el borde del frame*/);
                y = (int)(middleY / CTileMap.TILE_HEIGHT);
                y = y * CTileMap.TILE_HEIGHT;
                setY(y);
                return true;
            }
            return false;

        
            
        }else if (((tileTopLeft.isHangable() && tileTopLeft.isPlatform() && !tileTopRight.isPlatform() && getFlipH()) ^ (tileTopRight.isHangable() && tileTopRight.isPlatform() && !tileTopLeft.isPlatform() && !getFlipH())) && getVelY() < 0)
        {
            if (tileTopLeft.isHangable() && tileTopLeft.isPlatform() && !tileTopRight.isPlatform() && getFlipH())                
                {
                
                    x = leftX * CTileMap.TILE_WIDTH;
                    x += CTileMap.TILE_WIDTH;
                    setX(x - 40/*distancia entre las manos del pj y el borde del frame*/);
                    y = (int)(y / CTileMap.TILE_HEIGHT);
                    y = y * CTileMap.TILE_HEIGHT - CTileMap.TILE_HEIGHT;
                    setY(y);
                    return true;
            }
            else if (tileTopRight.isHangable() && tileTopRight.isPlatform() && !tileTopLeft.isPlatform()  && !getFlipH())
                {
                
                    x = rightX * CTileMap.TILE_WIDTH;
                    x -= CTileMap.TILE_WIDTH;
                    setX(x + 40/*distancia entre las manos del pj y el borde del frame*/);
                    y = (int)(y / CTileMap.TILE_HEIGHT);
                    y = y * CTileMap.TILE_HEIGHT - CTileMap.TILE_HEIGHT;
                    setY(y);
                    return true;
            }
                
             
        }
        return false;
    }
    public void bulletDestroyed()

    {
        mBulletCount -= 1;
    }

    public bool isGameOver()
    {
        return getState() == CPlayer.STATE_GAME_OVER;
    }

    public bool isStateNormal()
    {
        return getState() == CPlayer.STATE_STAND || getState() == CPlayer.STATE_WALKING || getState() == CPlayer.STATE_STAND_AFTER_DEAD || getState() == CPlayer.STATE_JUMP || getState() == CPlayer.STATE_FALL;
    }

    private void setOldXYPosition()
    {
        mOldX = getX();
        mOldY = getY();
        mOldVelY = getVelY();
    }

}
