using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CEntityManager : CManager
{

    private static CEntityManager mInst = null;
    private Dictionary<string,CGameObject> entityList = new Dictionary<string, CGameObject>();
    
    public CEntityManager()
    {
        registerSingleton();
    }
    
    public static CEntityManager inst()
    {
        return mInst;
    }

    public void registerEntity(string str,CGameObject obj)
    {
        entityList.Add(str, obj);
    }

    public CGameObject getEntityFromID(string ID)
    {
        if (entityList.ContainsKey(ID))
        {
            return entityList[ID];
        }
        else
        {
            throw new UnityException("ERROR: Object ID doesn't exist on Entity Manager.");
        }
        
    }

    public void removeEntity(string ID)
    {
        entityList.Remove(ID);
    }

    private void registerSingleton()
    {
        if (mInst == null)
        {
            mInst = this;
        }
        else
        {
            throw new UnityException("ERROR: Cannot create another instance of singleton class CEntityManager.");
        }
    }

}