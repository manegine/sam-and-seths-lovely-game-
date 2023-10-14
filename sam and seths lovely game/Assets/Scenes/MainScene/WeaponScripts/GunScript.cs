using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class gunscript : MonoBehaviour
{
    public GameObject bulletPrefab;
    //this is unfinished 
    //this is unfinished
    //this is unfinished 
    public float ShootForce, UpwardForce;

    public float Spread, bRecoil, hRecoil, vRecoil, ReloadTime, RPM;
    public int MagazineSize, BulletsPerShot;
    public bool FullAuto;
    public int BurstSize;
    int BulletsLeft, BulletsShot;

    public bool Shooting, ReadyToShoot, Reloading;
    private bool AllowInvoke = true;
    private float TimeBetweenShots;
    // the camera that controls the gun and the point the bullet spawns on
    public Camera MainCamera;
    public Transform AttackSource;


        //placeholders for stuff we will need to do later 
    public GameObject MuzzleFlash;
    public TextMeshProUGUI text;

    private void Awake()
    {
        BulletsLeft = MagazineSize;
        ReadyToShoot = true;
        TimeBetweenShots = 60/RPM;

    }
    private void Update()
    {
        myinput();
    }
    private void myinput() 
    {
        // check if we are trying to fire
        if (FullAuto) Shooting = Input.GetKey(KeyCode.Mouse0);
        else Shooting = Input.GetKeyDown(KeyCode.Mouse0);
        
        // reload if they press R, and we are not already reloading and the mag is not full.
        if (Input.GetKeyDown(KeyCode.R) && BulletsLeft < MagazineSize && !Reloading)
        {
            Reload();
        }

        // reload automatically if the gun is empty? Not sure if we should keep this.
        //if (ReadyToShoot && Shooting && !Reloading && BulletsLeft <= 0)
        //{
        //    Reload();
        //}

        // if we are trying to shoot, not reloading and the gun has bullets, fire.
        if (ReadyToShoot && Shooting && !Reloading && BulletsLeft > 0)
        {
            BulletsShot = 0;
            Shoot();
        }
    }
   
    private void Shoot()
    {
        ReadyToShoot = false;
        //maincamera is a placeholder unless it works might need some tweaking 
        Ray ray = new Ray(this.transform.position, this.transform.forward);
        RaycastHit hit;
        Vector3 TargetPoint;
        if (Physics.Raycast(ray, out hit))
            TargetPoint = hit.point;
        else
            TargetPoint = ray.GetPoint(75);

        Vector3 VectorToTarget = TargetPoint - AttackSource.position;

        // determine spread 
        float xDeviation = Random.Range(-Spread, Spread);
        float yDeviation = Random.Range(-Spread, Spread);

        // calculate new direction of the bullet with spread
        Vector3 BulletDirection = VectorToTarget + new Vector3(xDeviation, yDeviation, 0);
        GameObject currentBulletInstance = Instantiate(bulletPrefab, AttackSource.position, Quaternion.identity);
        Transform currentBullet = currentBulletInstance.transform.Find("Bullet");

        // apply forces to the bullet
        currentBullet.transform.forward = BulletDirection.normalized;
        currentBullet.GetComponent<Rigidbody>().AddForce(BulletDirection.normalized * ShootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(AttackSource.transform.up * UpwardForce, ForceMode.Impulse);

        // add recoil forces to the gun
        this.GetComponent<Rigidbody>().AddForce(this.transform.forward * -1 * bRecoil);
        this.GetComponent<Rigidbody>().AddForce(this.transform.right * Random.Range(-hRecoil, hRecoil));
        this.GetComponent<Rigidbody>().AddForce(this.transform.up * -1 * Random.Range(0.5f*vRecoil, vRecoil));
        BulletsLeft--;
        BulletsShot++;

        // 
        if (AllowInvoke)
        {
            AllowInvoke = false;
            Invoke("ResetShot", TimeBetweenShots);
        }

        // shotgun code - keep calling shoot() if we have more bullets to shoot in the burst
        if (BulletsShot < BulletsPerShot && BulletsLeft > 0)
        {
            Invoke("Shoot", 0);
        }

        // burst weapon code
        if (BulletsShot < BurstSize && BulletsLeft > 0)
        {
            Invoke("Shoot", TimeBetweenShots);
        }
    }

    // allow the gun to fire another bullet after invoking
    private void ResetShot()
    {
        ReadyToShoot = true;
        AllowInvoke = true;
    }

    // when invoked, reloading is true and reloadFinished is invoked after the reload time
    private void Reload()
    {
        Reloading = true;
        Invoke("ReloadFinished", ReloadTime);
    }

    // when this is invoked, the gun is reloaded and can fire again
    private void ReloadFinished()
    {
        BulletsLeft = MagazineSize;
        Reloading = false;
    }
}

