using UnityEngine;
using System.Collections;

public class CEnemy : CAnimatedSprite
{
	
	public CEnemy()
	{
        
		setFrames(Resources.LoadAll<Sprite>("Sprites/enemies/"));
        initAnimation(1, 9, 10, true);
        setName ("Enemy");
        setSortingLayerName ("Enemies");
        CEnemyManager.inst().add(this);
        render ();
	}
	
	override public void update()
	{
		base.update ();
		

		
	}
	
	override public void render()
	{
        base.render ();
		
		
	}

    override public void hit()
    {
        setDead(true);
        base.hit();
    }

    override public void destroy()
	{
		base.destroy ();	
		
	}
}