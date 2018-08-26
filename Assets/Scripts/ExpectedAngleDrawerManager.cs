using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpectedAngleDrawerManager : Singleton<ExpectedAngleDrawerManager> {

    public GameObject EADClass = null;

	// Use this for initialization
	/*void Start () {
		
	}*/
	
	// Update is called once per frame
	void Update () {
		
	}
     
    //这个变量在重新编译时置空
    public Dictionary<int, ExpectedAngleDrawer> EADs = new Dictionary<int, ExpectedAngleDrawer>();
     
    //public Dictionary<int, >
    public void OnRefreshToShowAngles()
    {
        var table = GameManager.Instance.m_TableHits;
        for(int k = 0; k<table.GetCount(); k++)
        {
            var hitInfo = table.GetValue(k);
             
            if (!EADs.ContainsKey( hitInfo.Id)||EADs[hitInfo.Id]==null)
            {
                string name = "MusicHit" + hitInfo.Id;
                var eadInstance = GameObject.Find(name);
                if (eadInstance == null)
                {
                    eadInstance = Instantiate(EADClass);
                    eadInstance.name = name;
                }
                if (EADs.ContainsKey(hitInfo.Id)) EADs.Remove(hitInfo.Id);
                EADs.Add(hitInfo.Id, eadInstance.GetComponent<ExpectedAngleDrawer>());
            }
            if (EADs.ContainsKey(hitInfo.Id))
            {
                EADs[hitInfo.Id].HitInfo = hitInfo;
                EADs[hitInfo.Id].Refresh();
            }

        }
    }
}
