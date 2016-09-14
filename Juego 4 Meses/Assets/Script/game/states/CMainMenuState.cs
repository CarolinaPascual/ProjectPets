using UnityEngine;
using System.Collections;

public class CMainMenuState : CGameState
{
	private CAnimatedBackground mBackground;

	private CButtonSprite mButtonPlay;
    private CButtonSprite mButtonMemories;
    private CButtonSprite mButtonCredits;
    private CButtonSprite mButtonExit;

    public CMainMenuState()
	{

	}
	
	override public void init()
	{
		base.init ();

		mBackground = new CAnimatedBackground ();
        mBackground.initAnimation(1, 8, 5, true);
        //Play Button
        mButtonPlay = new CButtonSprite ();
		mButtonPlay.setFrames (Resources.LoadAll<Sprite> ("Sprites/ui/button/start"));
		mButtonPlay.gotoAndStop (1);
		mButtonPlay.setXY (1524, 260);
		mButtonPlay.setWidth (225);
		mButtonPlay.setHeight (125);
		mButtonPlay.setSortingLayerName ("UI");
        mButtonPlay.setName ("Play Button");
        //Memories Button
        mButtonMemories = new CButtonSprite();
        mButtonMemories.setFrames(Resources.LoadAll<Sprite>("Sprites/ui/button/memories"));
        mButtonMemories.gotoAndStop(1);
        mButtonMemories.setXY(1524, 460);
        mButtonMemories.setWidth(525);
        mButtonMemories.setHeight(125);
        mButtonMemories.setSortingLayerName("UI");
        mButtonMemories.setName("Memories Button");
        //Credits Button
        mButtonCredits = new CButtonSprite();
        mButtonCredits.setFrames(Resources.LoadAll<Sprite>("Sprites/ui/button/credits"));
        mButtonCredits.gotoAndStop(1);
        mButtonCredits.setXY(1524, 660);
        mButtonCredits.setWidth(275);
        mButtonCredits.setHeight(125);
        mButtonCredits.setSortingLayerName("UI");
        mButtonCredits.setName("Credits Button");
        //Exit Button
        mButtonExit = new CButtonSprite();
        mButtonExit.setFrames(Resources.LoadAll<Sprite>("Sprites/ui/button/exit"));
        mButtonExit.gotoAndStop(1);
        mButtonExit.setXY(1524, 860);
        mButtonExit.setWidth(225);
        mButtonExit.setHeight(125);
        mButtonExit.setSortingLayerName("UI");
        mButtonExit.setName("Exit Button");
    }
	
	override public void update()
	{
		base.update ();

		mButtonPlay.update ();
        mButtonMemories.update();
        mButtonCredits.update();
        mButtonExit.update();
        mBackground.update();

		if (mButtonPlay.clicked ()) 
		{
            Debug.Log("click");
            CGame.inst ().setState(new CLevelState ());
            Debug.Log("after setting state");
            return;
		}
	}
	
	override public void render()
	{
		base.render ();
        mBackground.render();
		mButtonPlay.render ();
        mButtonMemories.render();
        mButtonCredits.render();
        mButtonExit.render();
    }
	
	override public void destroy()
	{
		base.destroy ();
		
		mBackground.destroy ();
		mBackground = null;

		mButtonPlay.destroy ();
        mButtonPlay = null;
        mButtonMemories.destroy();
        mButtonMemories = null;
        mButtonCredits.destroy();
        mButtonCredits = null;
        mButtonExit.destroy();
        mButtonExit = null;
               
    }
	
}


