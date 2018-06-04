using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {


    public int n_TypeIndex = 0;
    public int n_ItemRank = 10;
    string Item_Type;
    public GameObject ControlledMap;


	// Use this for initialization
	void Start () {
        if(n_TypeIndex == 0)
        {
            Item_Type = GeneralManager.instance.ItemType[0];
            if (ControlledMap != null)
            {
                ControlledMap.SetActive(false);
            }
        }
        else
        {
            Item_Type = GeneralManager.instance.ItemType[1];
        }
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(Item_Type == GeneralManager.instance.ItemType[0])
            {
                MapControl();
            }else if(Item_Type == GeneralManager.instance.ItemType[1])
            {
                RankUp();
            }
            Destroy(this.gameObject);
        }
    }
    void MapControl()
    {
        if(ControlledMap != null)
        {
            ControlledMap.SetActive(true);

        }
    }
    void RankUp()
    {
        GeneralManager.instance.rank += n_ItemRank;

    }

}
