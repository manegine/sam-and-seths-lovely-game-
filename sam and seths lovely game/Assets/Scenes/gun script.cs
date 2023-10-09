using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunscript : MonoBehaviour
{
    public GameObject bullet;
    //this is unfinished 
    //this is unfinished
    //this is unfinished 
    public float shootforce, upwardforce;

    public float timebetweenshooting, spread, reloadtime, timebetweenshots;
    public int magazinesize, bulletspertap;
    public bool FullAuto;
    int bulletsleft, bulletsshot;

    bool shooting, readytoshoot, reloading,

    public Camera maincamera;
    public Transform attackpoint


        //placeholders for stuff we will need to do later 
        public GameObject muzzleflash; 
        public textmeshprougui 

    private void Awake()
    {
        bulletsleft = magazinesize;
        readytoshoot = true;

    }
    private void Update()
    {
        myinput();
    }
    private void myinput() 
    {
        if (FullAuto) shooting = Input..GetKey(KeyCode.Mouse0);
        else shooting = Input.Getkeydown(KeyCode.Mouse0);
        
        if (Input.getkeydown(KeyCode.R) && bulletsleft < magazinesize !reloading) reload();

        if (readytoshoot && shooting !reloading && bulletsleft <= 0) reload();

        if (readytoshoot && shooting !reloading && bulletsleft >0)
        {
            bulletsshot = 0;

            shoot();
        }
    }
   
    private void shoot()
    {
        readytoshoot = false;
        //maincamera is a placeholder unless it works might need some tweeking 
        Ray ray = maincamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0))
        raycastHit hit;
        Vector3 targetpoint;
        if (Physics.Raycast(ray, out hit))
            targetpoint = hit.point;
        else
            targetpoint = ray.getpoint(75);

        Vector3 directwithoutspread = targetpoint - attackpoint.position;

        //spread 
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //calculate new direction+ spread like above 

        Vector3 directionwithspread = directionwithoutspread + new Vector3 (x, y , 0)
            Gameobject currentBullet = Instantiate(bullet, attackpoint.position, Quaternion.identity);
        currentBullet.transform.forward = directionwithspread.normalized;
        currentBullet.Getcomponet<Rigidbody>().addforce(directionwithspread.normalized * shootforce, ForceMode.Impulse);
        currentBullet.Getcomponet<Rigidbody>().addforce(maincamera.transform.up * upwardforce, ForceMode.Impulse);
        bulletsleft--;
        bulletssho++;

        if (allowinvoke)
        {
            invoke("resetshot", timebetweenshooting);
            allowinvoke = false
        }

        //shotgun code 

        if (bulletsshot < bulletspertap && bulletsleft > 0)
            invoke("shoot", timebetweenshots); 

    }
    private void resetshot()
    {
        readytoshoot = true;
        allow invoke = true;
    }
    private void reload()
    {
        reloading = true
            invoke("reloadfinished", reloadtime);
    }
    private void reloadfinished()
    {
        bulletsleft = magazinesize; 
        reloading = false 
    }
}

