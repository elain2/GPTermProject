using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisBright : MonoBehaviour {

    Transform Player;
    Transform Map;



    public float f_Xsize = 0;
    public float f_Zsize = 0;

    public Material mat;
    public float f_Distance = 0;
	// Use this for initialization
	void Start () {
        Map = transform;
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        

        for(int i=0; i < transform.childCount; i++)
        {
            float tempX = Mathf.Abs(transform.GetChild(i).position.x - transform.position.x);
            float tempZ = Mathf.Abs(transform.GetChild(i).position.z - transform.position.z);

            tempX += transform.GetChild(i).GetComponent<MeshRenderer>().bounds.size.x/2;
            tempZ += transform.GetChild(i).GetComponent<MeshRenderer>().bounds.size.z/2;
            if(f_Xsize < tempX)
            {
                f_Xsize = tempX;
            }
            if(f_Zsize < tempZ)
            {
                f_Zsize = tempZ;
            }

            
        }
        
	}
	
	// Update is called once per frame
	void Update () {
        
        f_Distance = Vector3.Distance(Player.position, Camera.main.transform.position);
        mat.SetFloat("_CamDistanceValue", f_Distance);

	}
}
