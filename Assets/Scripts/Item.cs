using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {


    public int n_TypeIndex = 0;
    public int n_ItemRank = 10;
    public string Item_Type;
    public GameObject EnableMapObj;
    public ControlledMap[] ControlMap;
    public Transform ChangeRespawnPoint;


	// Use this for initialization
	void Start () {

        Item_Type = GeneralManager.instance.ItemType[n_TypeIndex];

        if (Item_Type == GeneralManager.instance.ItemType[0] && EnableMapObj != null)
        {
            EnableMapObj.SetActive(false);
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
                MapEnable();
            }else if(Item_Type == GeneralManager.instance.ItemType[1])
            {
                RankUp();
            }else if(Item_Type == GeneralManager.instance.ItemType[2])
            {
                MapDisAble();
            }

            if(ChangeRespawnPoint!= null)
            {
                GeneralManager.instance.Respawn = ChangeRespawnPoint;
            }
            for(int i=0; i < ControlMap.Length; i++)
            {
                ControlMap[i].b_Start = true;
            }
            Destroy(this.gameObject);
        }
    }
    void MapEnable()
    {
        if(EnableMapObj != null)
        {
            EnableMapObj.SetActive(true);

        }
    }
    void RankUp()
    {
        GeneralManager.instance.rank += n_ItemRank;

    }
    void MapDisAble()
    {
        if(EnableMapObj != null)
        {
            EnableMapObj.SetActive(false);
        }
    }

}
