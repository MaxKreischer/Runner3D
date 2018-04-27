using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAndAim : MonoBehaviour {
    
    public GameObject target;
    public GameObject projectile;
    public float velocityMagn;
    public float variance;
    public float spawnDelay;
    public int numOfProjectiles = 3;
    public bool active = true;
   
    private Vector3 direction;
    private Quaternion spawnRotation;
    
    private List<GameObject> projectiles;
    private List<Rigidbody> rigis;
    private float timeCounter = 0f;
    private int projectileIdx = 0;

    // Use this for initialization
    void Start () {
        direction = (target.transform.position - transform.position) + new Vector3(Random.Range(-variance, variance), Random.Range(-variance, variance),0f);
        projectiles = new List<GameObject>();
        rigis = new List<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= spawnDelay && projectiles.Count < numOfProjectiles)
            {
                projectiles.Add(spawnProjectile());
                rigis.Add(projectiles[projectiles.Count - 1].GetComponent<Rigidbody>());
                timeCounter = 0f;
            }
        }  
	}

    GameObject spawnProjectile()
    {
        GameObject obj;
        Rigidbody rigi;
        spawnRotation = new Quaternion(Random.Range(-variance, variance), Random.Range(-variance, variance), Random.Range(-variance, variance), Random.Range(-variance, variance));
        obj = Instantiate(projectile, transform.position, spawnRotation);
        rigi = obj.GetComponent<Rigidbody>();
        rigi.velocity = direction;
        return obj;
    }

    void DestroyOldestProjectile()
    {
        Destroy(rigis[projectileIdx]);
        Destroy(projectiles[projectileIdx]);
        projectileIdx--;
    }


}
