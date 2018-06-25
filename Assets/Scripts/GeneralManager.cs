using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralManager : MonoBehaviour {

    public static GeneralManager instance = null;
    public string[] ItemType = new[] { "KeyItem", "RankItem", "MapDisableItem", "Default" };
    public int rank = 0;
    public Transform Respawn;
    public bool b_IsTimeLine = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else if(instance != this)
        {
            Destroy(gameObject);

        }
        
        DontDestroyOnLoad(gameObject);
    }
    // Use this for initialization
    
	// Use this for initialization
	void Start () {
        InitParameter();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitParameter()
    {
        if(Respawn == null && GameObject.FindWithTag("Respawn") != null)
        {
            Respawn = GameObject.FindWithTag("Respawn").transform;
        }

    }
}
