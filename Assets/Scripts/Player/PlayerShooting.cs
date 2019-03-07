using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;


    float timer;
    Ray shootRay = new Ray(); // Used to RayCast out to figure out what is hit with bullets
    RaycastHit shootHit; // This will return back to us whatever we have hit with our shootRay.
    int shootableMask; // So that we can hit only shootable items
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f; // how long are the above effects able to remain in view


    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
    }


    void Update ()
    {
        timer += Time.deltaTime;

		if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            Shoot ();
        }

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
    }


    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot ()
    {
        timer = 0f;

        gunAudio.Play ();

        gunLight.enabled = true;

        gunParticles.Stop ();
        gunParticles.Play ();

        gunLine.enabled = true;
        // Lines have 2 points - start and end (0 - start, 1 - end)
        gunLine.SetPosition (0, transform.position); // set the 0th vertex (start vertex) of the line to transform.position
        // 2nd point requires calculation

        shootRay.origin = transform.position; // set the start of ray to current position of the line
        shootRay.direction = transform.forward; // set the direction of the ray which will be forward since the gun is pointing directly away from the player in the forward direction (relative to the player)

        // Now we use physics to hit this ray (shootRay)    
        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
        {
            // get the EnemyHealth component from the collider of whatever GameObject is hit.
            EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
            if(enemyHealth != null)
            {
                // The gameObject hit indeed had an EnemyHealth component (Thus, it is an actual enemy)
                enemyHealth.TakeDamage (damagePerShot, shootHit.point);
            } 
            // We now have the gunLine
            gunLine.SetPosition (1, shootHit.point);
        }
        else
        {
            // if we did not hit anything (Range is only 100 units)
            // so we add second point as 1st point + shoot direction * range
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
        }
    }
}
