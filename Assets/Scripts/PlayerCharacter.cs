using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private AudioSource projectileShotAudio;
    [SerializeField] private AudioSource launchActiveAudio;
    [SerializeField] private AudioSource launchShotAudio;

    [SerializeField] private ParticleSystem hitPsPrefab;
    [SerializeField] private ParticleSystem legLPsPrefab;
    [SerializeField] private ParticleSystem legRPsPrefab;
    [SerializeField] private ParticleSystem hazardPsPrefab;

    private ParticleSystem hitPs;    
    private ParticleSystem legLPs;    
    private ParticleSystem legRPs;
    private ParticleSystem hazardPs;

    [SerializeField] private Projectile projectilePrefab;
    private Projectile projectile;
    
    [SerializeField] private Transform camBoomTransform;
    [SerializeField] private Transform camTransform;

    [SerializeField] private Transform projectileSpawnPos;
    [SerializeField] private Transform projectileSpawnRoot;

    [SerializeField] private Transform LaunchStartPos;
    [SerializeField] private Transform legRTransform;

    [SerializeField] private float gravityModifier;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float jumpForce;

    private Rigidbody playerRb;
    private Animator playerAnimator;
    //private bool isOnGround;
    private bool isCharacterMoving;

    private bool isJumping;
    private bool canJump;

    private bool isCharacterDashing;
    private bool isCharacterFloatingUp;
    //private bool isZeroGravity;

    private Vector3 projectileHitPosition;
    private Vector3 hitDirection;
    //private Vector3 enterJetPosition;

    private Quaternion projectileHitRotation;
    private float distanceDelayFactor;
    //private float projectileFrequency;
    //private int projectileStopperBasic;
    private int projectileStopperHeavy;

    private GameObject launchObject;
    //private GameObject hitObjectPrefab;
    private GameObject hazardObject;
    private Rigidbody hitObjectRb;

    private bool isLaunchActive;
    private bool isKeyDown;
    //private bool isPlayerInJet;
    //private bool hasSecondPassed;

    private float hInput;
    private float vInput;

    //private Vector3 collisionNormal;


    private void Awake()
    {
        Physics.gravity = new Vector3(0f, -9.8f, 0f) * gravityModifier;
    }

    
    void Start()
    {   
        //collisionNormal = new Vector3(0f, 0f, 0f);
        //projectileShotAudio.volume = 0.05f;
        //launchActiveAudio.volume = 0.1f;
        //launchShotAudio.volume = 0.75f;
        MainManager.Instance.playerLegRTransform = legRTransform;        

        legLPs = legLPsPrefab;
        legRPs = legRPsPrefab;

        //hitDirection = transform.forward;
        //projectileStopperBasic = 1;
        projectileStopperHeavy = 1;
        playerAnimator = GetComponentInChildren<Animator>();
        playerRb = GetComponent<Rigidbody>();        
        //isOnGround = true;
        isCharacterMoving = false;
        isJumping = false;
        isCharacterDashing = false;
        isCharacterFloatingUp = false;
        //isZeroGravity = false;
        isLaunchActive = false;
        isKeyDown = false;
        //Physics.gravity *= gravityModifier;
        //hasSecondPassed = false;        

        //InvokeRepeating("ProjectileBasicEnable", 0, 0.5f);
        InvokeRepeating("ProjectileHeavyEnable", 0, 0.1f);        
    }
    

    
    void Update()
    {
        
        if (MainManager.Instance.isGameActive)
        {            

            if (!MainManager.Instance.isPlayerInJet)
            {
                //hInput = Input.GetAxis("Horizontal");
                //vInput = Input.GetAxis("Vertical");

                projectileShotAudio.volume = (MainManager.Instance.soundFXVolume * 0.05f) * 5;
                launchActiveAudio.volume = (MainManager.Instance.soundFXVolume * 0.1f) * 5;
                launchShotAudio.volume = (MainManager.Instance.soundFXVolume * 0.75f) * 5;

                /*
                if (!MainManager.Instance.isPlayerOnGround)
                {
                    playerRb.drag = playerRb.velocity.magnitude / 30f;

                    if (isZeroGravity)
                    {
                        playerRb.drag = playerRb.velocity.magnitude / 20f;
                    }
                }
                else
                {
                    playerRb.drag = playerRb.velocity.magnitude / 60f;
                }
                */

                playerAnimator.enabled = true;

                //Walk();
                //FastWalk();
                //Jump();
                //Dash();
                //GoZeroGravity();
                //FireProjectileBasic();
                FireProjectileHeavy();
                //Launch();                


                if (!MainManager.Instance.isPlayerOnGround)
                {
                    playerSpeed = 7.5f;
                    playerAnimator.SetBool("Walk_b", false);
                    playerAnimator.SetBool("FastWalk_b", false);
                    isCharacterMoving = false;
                }


                if (MainManager.Instance.isPlayerOnHoverboard)
                {
                    //playerAnimator.enabled = true;
                    playerAnimator.SetBool("Walk_b", false);
                    playerAnimator.SetBool("FastWalk_b", false);
                    //playerAnimator.SetBool("Jump_b", false);
                }

                if (Input.GetKeyDown(KeyCode.C))
                {
                    if (!MainManager.Instance.isPlayerOnHoverboard)
                    {
                        MainManager.Instance.isPlayerOnHoverboard = true;
                        MainManager.Instance.offHoverboard = true;
                        legRPs.Play();
                    }
                    else
                    {
                        MainManager.Instance.isPlayerOnHoverboard = false;
                        MainManager.Instance.offHoverboard = false;
                        legRPs.Stop();
                    }

                }


                if (hitPs != null)
                {
                    hitPs.Play();
                }

                if (legLPs.isPaused)                
                {
                    legLPs.Play();                    
                    //StartCoroutine(ZeroGravityTimer());
                }
                if (legRPs.isPaused)
                {
                    legRPs.Play();
                }
                
            }

        }
        else
        {
            playerAnimator.enabled = false;

            if (hitPs != null)
            {
                hitPs.Pause();
            }

            if (legLPs.isPlaying)
            {
                legLPs.Pause();
            }
            if (legRPs.isPlaying)
            {
                legRPs.Pause();
            }
        }
    }



    private void FixedUpdate()
    {
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");

        if (MainManager.Instance.isGameActive)
        {
            Walk();
            FastWalk();
            Jump();
            Dash();
            Launch();
            GoZeroGravity();
        }
    }



    private void Walk()
    {
        Vector3 camBoomRotEuler = camBoomTransform.rotation.eulerAngles;

        if (Mathf.Abs(hInput) > Mathf.Epsilon || Mathf.Abs(vInput) > Mathf.Epsilon)
        {
            transform.forward = new Vector3(hInput, 0f, vInput);
            transform.eulerAngles += new Vector3(0f, camBoomRotEuler.y, 0f);
            playerRb.AddRelativeForce(Vector3.forward * playerSpeed, ForceMode.Impulse);

            isCharacterMoving = true;

            if (playerAnimator.GetBool("LaunchHoldIdle_b"))
            {
                playerAnimator.SetBool("LaunchHoldWalk_b", true);
                playerAnimator.SetBool("Walk_b", true);
                playerAnimator.SetBool("LaunchHoldIdle_b", false);
            }
            else
            {
                playerAnimator.SetBool("Walk_b", true);
            }

            if (playerAnimator.GetBool("ShotIdle_b"))
            {
                playerAnimator.SetBool("ShotWalking_b", true);
                playerAnimator.SetBool("Walk_b", true);
                playerAnimator.SetBool("ShotIdle_b", false);
            }
            else
            {
                playerAnimator.SetBool("Walk_b", true);
            }
        }
        else
        {
            isCharacterMoving = false;

            if (playerAnimator.GetBool("Walk_b"))
            {
                playerAnimator.SetBool("Walk_b", false);
            }

            if (playerAnimator.GetBool("LaunchHoldWalk_b"))
            {
                playerAnimator.SetBool("LaunchHoldWalk_b", false);
                playerAnimator.SetBool("LaunchHoldIdle_b", true);
            }

            if (playerAnimator.GetBool("ShotWalking_b"))
            {
                playerAnimator.SetBool("ShotWalking_b", false);
                playerAnimator.SetBool("ShotIdle_b", true);
            }
        }

        /*
        Vector3 camBoomRotEuler = camBoomTransform.rotation.eulerAngles;

        if (vInput > 0.9f)//Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || vInput > 0)
        {
            if (hInput > 0.9f)//Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || hInput > 0)
            {
                transform.forward = new Vector3(hInput, 0f, vInput);
                //transform.eulerAngles = new Vector3(0f, camBoomRotEuler.y + 45f, 0f);
            }
            else if (hInput < -0.9f)//Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || hInput < 0)
            {
                transform.forward = new Vector3(hInput, 0f, vInput);
                //transform.eulerAngles = new Vector3(0f, camBoomRotEuler.y - 45f, 0f);
            }
            else
            {
                transform.rotation = new Quaternion(transform.rotation.x, camBoomTransform.rotation.y,
                    transform.rotation.z, camBoomTransform.rotation.w);//camBoomTransform.rotation;                
            }
            
            //transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed);
            //playerRb.AddRelativeForce((Vector3.Cross(Vector3.right, collisionNormal)) * playerSpeed, ForceMode.Impulse);
            playerRb.AddRelativeForce(Vector3.forward * playerSpeed, ForceMode.Impulse);

            isCharacterMoving = true;

            if (playerAnimator.GetBool("LaunchHoldIdle_b"))
            {
                playerAnimator.SetBool("LaunchHoldWalk_b", true);
                playerAnimator.SetBool("Walk_b", true);
                playerAnimator.SetBool("LaunchHoldIdle_b", false);
            }
            else
            {
                playerAnimator.SetBool("Walk_b", true);
            }

            if (playerAnimator.GetBool("ShotIdle_b"))
            {
                playerAnimator.SetBool("ShotWalking_b", true);
                playerAnimator.SetBool("Walk_b", true);
                playerAnimator.SetBool("ShotIdle_b", false);
            }
            else
            {
                playerAnimator.SetBool("Walk_b", true);
            }

        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || vInput < 0)
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || hInput > 0)
            {
                transform.eulerAngles = new Vector3(0f, camBoomRotEuler.y + 135f, 0f);                
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || hInput < 0)
            {
                transform.eulerAngles = new Vector3(0f, camBoomRotEuler.y - 135f, 0f);                
            }
            else
            {
                transform.eulerAngles = new Vector3(0f, camBoomRotEuler.y + 180f, 0f);                
            }

            //transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed);
            playerRb.AddRelativeForce(Vector3.forward * playerSpeed, ForceMode.Impulse);

            isCharacterMoving = true;

            if (playerAnimator.GetBool("LaunchHoldIdle_b"))
            {
                playerAnimator.SetBool("LaunchHoldWalk_b", true);
                playerAnimator.SetBool("Walk_b", true);
                playerAnimator.SetBool("LaunchHoldIdle_b", false);
            }
            else
            {
                playerAnimator.SetBool("Walk_b", true);
            }

            if (playerAnimator.GetBool("ShotIdle_b"))
            {
                playerAnimator.SetBool("ShotWalking_b", true);
                playerAnimator.SetBool("Walk_b", true);
                playerAnimator.SetBool("ShotIdle_b", false);
            }
            else
            {
                playerAnimator.SetBool("Walk_b", true);
            }            
            
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || hInput < 0)
        {
            transform.eulerAngles = new Vector3(0f, camBoomRotEuler.y - 90f, 0f);
            //transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed);
            playerRb.AddRelativeForce(Vector3.forward * playerSpeed, ForceMode.Impulse);

            isCharacterMoving = true;

            if (playerAnimator.GetBool("LaunchHoldIdle_b"))
            {
                playerAnimator.SetBool("LaunchHoldWalk_b", true);
                playerAnimator.SetBool("Walk_b", true);
                playerAnimator.SetBool("LaunchHoldIdle_b", false);
            }
            else
            {
                playerAnimator.SetBool("Walk_b", true);
            }

            if (playerAnimator.GetBool("ShotIdle_b"))
            {
                playerAnimator.SetBool("ShotWalking_b", true);
                playerAnimator.SetBool("Walk_b", true);
                playerAnimator.SetBool("ShotIdle_b", false);
            }
            else
            {
                playerAnimator.SetBool("Walk_b", true);
            }
            
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || hInput > 0)
        {
            transform.eulerAngles = new Vector3(0f, camBoomRotEuler.y + 90f, 0f);
            //transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed);
            playerRb.AddRelativeForce(Vector3.forward * playerSpeed, ForceMode.Impulse);

            isCharacterMoving = true;

            if (playerAnimator.GetBool("LaunchHoldIdle_b"))
            {
                playerAnimator.SetBool("LaunchHoldWalk_b", true);
                playerAnimator.SetBool("Walk_b", true);
                playerAnimator.SetBool("LaunchHoldIdle_b", false);
            }
            else
            {
                playerAnimator.SetBool("Walk_b", true);
            }

            if (playerAnimator.GetBool("ShotIdle_b"))
            {
                playerAnimator.SetBool("ShotWalking_b", true);
                playerAnimator.SetBool("Walk_b", true);
                playerAnimator.SetBool("ShotIdle_b", false);
            }
            else
            {
                playerAnimator.SetBool("Walk_b", true);
            }            
        }
        else
        {
            isCharacterMoving = false;

            if (playerAnimator.GetBool("Walk_b"))
            {
                playerAnimator.SetBool("Walk_b", false);
            }

            if (playerAnimator.GetBool("LaunchHoldWalk_b"))
            {
                playerAnimator.SetBool("LaunchHoldWalk_b", false);                
                playerAnimator.SetBool("LaunchHoldIdle_b", true);
            }

            if (playerAnimator.GetBool("ShotWalking_b"))
            {
                playerAnimator.SetBool("ShotWalking_b", false);
                playerAnimator.SetBool("ShotIdle_b", true);
            }
        }
        */

    }


    private void Launch()
    {

        RaycastHit hit;
        //var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var ray = new Ray();
        ray.origin = camTransform.position;
        ray.direction = camTransform.forward;

        Vector3 hitDirection = new Vector3(0f, 0f, 0f);

        if (Physics.Raycast(ray, out hit))
        {            
            if (launchObject != null)
            {
                if (hit.collider.gameObject != launchObject)
                {
                    hitDirection = (hit.point - launchObject.transform.position).normalized;
                }
                else
                {                    
                    RaycastHit[] hits = Physics.RaycastAll(ray);
                    hitDirection = (hits[1].point - launchObject.transform.position).normalized;
                }
                
            }


            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (!isLaunchActive)
                {

                    if (hit.rigidbody != null && hit.collider.CompareTag("Launchable"))
                    {
                        launchObject = hit.collider.gameObject;
                        launchActiveAudio.Play();
                        isLaunchActive = true;                        
                        launchObject.GetComponent<Rigidbody>().useGravity = false;

                        if (isCharacterMoving)
                        {
                            playerAnimator.SetBool("LaunchHoldWalk_b", true);
                            playerAnimator.SetBool("Walk_b", true);
                        }
                        else
                        {
                            playerAnimator.SetBool("LaunchHoldIdle_b", true);
                            playerAnimator.SetBool("LaunchHoldWalk_b", false);
                        }

                    }
                }
                else
                {
                    launchActiveAudio.Stop();

                    isLaunchActive = false;

                    if (playerAnimator.GetBool("LaunchHoldWalk_b"))
                    {
                        playerAnimator.SetBool("LaunchHoldWalk_b", false);
                        playerAnimator.SetBool("Walk_b", true);
                    }

                    if (playerAnimator.GetBool("LaunchHoldIdle_b"))
                    {
                        playerAnimator.SetBool("LaunchHoldIdle_b", false);
                        playerAnimator.SetBool("Walk_b", false);
                    }


                    if (launchObject != null)
                    {
                        launchObject.GetComponent<Rigidbody>().useGravity = true;
                        launchObject = null;
                    }
                }

            }

        }


        if (launchObject != null)
        {
            launchObject.transform.position = Vector3.Lerp(launchObject.transform.position,
                    LaunchStartPos.position, Time.deltaTime * 2f);
            launchObject.GetComponent<Rigidbody>().AddRelativeTorque(Vector3.one * 0.001f, ForceMode.Impulse);
            /*
            if (Vector3.Distance(launchObject.transform.position, projectileSpawnPos.position) > 5.5f && isLaunchActive)
            {
                
            }
            */
            if (Input.GetKeyDown(KeyCode.E))
            {
                launchActiveAudio.Stop();
                launchShotAudio.Play();

                if (isCharacterMoving)
                {
                    playerAnimator.SetBool("Launch2Walk_b", true);

                    StartCoroutine(LaunchWalkTimer());
                }
                else
                {
                    playerAnimator.SetBool("Launch2Idle_b", true);

                    StartCoroutine(LaunchIdleTimer());

                }


                launchObject.GetComponent<Rigidbody>().AddForce(hitDirection * 250f, ForceMode.VelocityChange);
                launchObject.GetComponent<Rigidbody>().useGravity = true;
                isLaunchActive = false;
                launchObject = null;

            }

        }

    }


    IEnumerator LaunchIdleTimer()
    {
        yield return new WaitForEndOfFrame();

        playerAnimator.SetBool("Launch2Idle_b", false);
        playerAnimator.SetBool("LaunchHoldIdle_b", false);
        //playerAnimator.SetBool("Walk_b", false);

    }

    IEnumerator LaunchWalkTimer()
    {
        yield return new WaitForEndOfFrame();

        playerAnimator.SetBool("Launch2Walk_b", false);
        playerAnimator.SetBool("LaunchHoldWalk_b", false);
        playerAnimator.SetBool("Walk_b", true);

    }


    private void FastWalk()
    {
        if (Input.GetKey(KeyCode.LeftShift) && MainManager.Instance.isPlayerOnGround)
        {
            playerSpeed = 15f;

            if (isCharacterMoving)
            {
                playerAnimator.SetBool("FastWalk_b", true);
            }
            else
            {
                playerAnimator.SetBool("FastWalk_b", false);
            }

        }
        else
        {
            playerSpeed = 10f;

            playerAnimator.SetBool("FastWalk_b", false);
        }
    }


    IEnumerator JumpTimer()
    {
        yield return new WaitForSeconds(0.5f);

        //playerAnimator.SetBool("Jump_b", false);
        isJumping = false;
    }


    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space)) //|| Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            if (canJump)
            {
                isJumping = true;
                canJump = false;

                StartCoroutine(JumpTimer());
            }
        }
        else
        {
            isJumping = false;

            if (MainManager.Instance.isPlayerOnGround)
            {   
                canJump = true;
            }
        }

        if (isJumping)
        {
            playerAnimator.SetBool("Jump_b", true);

            if (Mathf.Abs(hInput) > 0 || Mathf.Abs(vInput) > 0)
            {
                playerRb.AddForce((Vector3.up + (transform.forward / 2)) * jumpForce, ForceMode.Impulse);
            }
            else
            {
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }   



    private void Dash()
    {        
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isCharacterDashing)
        {
            //playerAnimator.SetBool("Dash_b", true);
            StartCoroutine(DashTimer());
            isCharacterDashing = true;

            if (playerAnimator.GetBool("ShotIdle_b") || playerAnimator.GetBool("ShotWalking_b"))
            {
                float fac = 350f;

                if (vInput < 0)
                {
                    if (hInput < 0)
                    {
                        fac /= 2;
                        playerRb.AddRelativeForce(Vector3.left * fac, ForceMode.Impulse);
                    }

                    if (hInput > 0)
                    {
                        fac /= 2;
                        playerRb.AddRelativeForce(Vector3.right * fac, ForceMode.Impulse);
                    }

                    playerRb.AddRelativeForce(Vector3.back * fac, ForceMode.Impulse);
                }

                if (vInput > 0)
                {
                    if (hInput < 0)
                    {
                        fac /= 2;
                        playerRb.AddRelativeForce(Vector3.left * fac, ForceMode.Impulse);
                    }

                    if (hInput > 0)
                    {
                        fac /= 2;
                        playerRb.AddRelativeForce(Vector3.right * fac, ForceMode.Impulse);
                    }

                    playerRb.AddRelativeForce(Vector3.forward * fac, ForceMode.Impulse);
                }

                if (vInput == 0 && hInput == 0)
                {                    
                    playerRb.AddRelativeForce(Vector3.forward * fac, ForceMode.Impulse);
                }

                if (hInput < 0)
                {                    
                    playerRb.AddRelativeForce(Vector3.left * fac, ForceMode.Impulse);
                }

                if (hInput > 0)
                {
                    playerRb.AddRelativeForce(Vector3.right * fac, ForceMode.Impulse);
                }
            }
            else
            {
                playerAnimator.SetBool("Dash_b", true);
                playerRb.AddRelativeForce(Vector3.forward * 350f, ForceMode.Impulse);
            }            
        }

    }


    IEnumerator DashTimer()
    {
        yield return new WaitForSeconds(0.5f);

        //playerRb.velocity = new Vector3(0f, playerRb.velocity.y, 0f);
        playerAnimator.SetBool("Dash_b", false);
        isCharacterDashing = false;
    }



    private void GoZeroGravity()
    {

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isKeyDown)
            {                
                legLPs.Play();
                legRPs.Play();
                isKeyDown = true;
                StartCoroutine(FloatUpTimer());                
            }
            else
            {                
                legLPs.Stop();
                legRPs.Stop();
                playerRb.useGravity = true;
                StopAllCoroutines();
                isKeyDown = false;
            }
        }

        if (isKeyDown && isCharacterFloatingUp)
        {
            playerRb.AddRelativeForce(Vector3.up * Time.deltaTime * 200f, ForceMode.Impulse);
        }
    }


    IEnumerator FloatUpTimer()
    {
        MainManager.Instance.isPlayerOnGround = false;
        isCharacterFloatingUp = true;
        playerRb.useGravity = false;

        yield return new WaitForSeconds(1);

        playerAnimator.SetBool("Jump_b", false);        
        isCharacterFloatingUp = false;

        StartCoroutine(ZeroGravityTimer());

    }
    


    IEnumerator ZeroGravityTimer()
    {
        playerRb.velocity = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);
        //isZeroGravity = true;

        yield return new WaitForSeconds(5);

        legLPs.Stop();
        legRPs.Stop();
        playerRb.useGravity = true;
        //isZeroGravity = false;
        isKeyDown = false;
    }
    
    /*
    private void ProjectileBasicEnable()
    {        
        projectileStopperBasic = 0;
    }*/

    private void ProjectileHeavyEnable()
    {
        projectileStopperHeavy = 0;
    }


    private void SpawnProjectile()
    {
        projectileShotAudio.Play();
        projectile = Instantiate(projectilePrefab, projectileSpawnPos.position,
                    transform.rotation);
    }

    /*
    private void FireProjectileBasic()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {

            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && !hit.collider.CompareTag("Player"))
            {
                projectileHitPosition = hit.point;
                projectileHitRotation = hit.transform.rotation;

                Vector3 hitDirection = hit.point - projectileSpawnPos.position;
                projectileSpawnRoot.rotation = Quaternion.LookRotation(hitDirection);

                if (projectileStopperBasic < 1)
                {
                    if (projectile == null)
                    {
                        SpawnProjectile();

                        distanceDelayFactor = (Vector3.Distance(hit.point, transform.position) /
                            projectile.projectileSpeed);

                        projectileStopperBasic = 1;
                    }

                    if (hitPs == null)
                    {
                        StartCoroutine(DistanceDelayTimer(distanceDelayFactor));
                    }

                    if (hit.rigidbody != null)
                    {
                        hit.rigidbody.AddForce(hitDirection * 0.5f, ForceMode.Impulse);
                    }
                }                    
                

            }
            else
            {
                Vector3 camBoomRotEuler = camBoomTransform.rotation.eulerAngles;
                Vector3 camRotEuler = camTransform.rotation.eulerAngles;
                projectileSpawnRoot.eulerAngles = new Vector3(camRotEuler.x, camBoomRotEuler.y, 0f);
            }

        }

    }
    */

    IEnumerator ShotIdleTimer()
    {
        if (playerAnimator.GetBool("Walk_b"))
        {

            playerAnimator.SetBool("ShotWalking_b", true);
        }
        else
        {
            playerAnimator.SetBool("ShotIdle_b", true);
        }

        yield return new WaitForSeconds(2);

        if (Input.GetKey(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            //StartCoroutine(ShotIdleTimer());
        }
        else
        {
            if (playerAnimator.GetBool("ShotWalking_b"))
            {
                playerAnimator.SetBool("ShotWalking_b", false);
            }

            if (playerAnimator.GetBool("ShotIdle_b"))
            {
                playerAnimator.SetBool("ShotIdle_b", false);
            }
        }
        
    }

    

    private void FireProjectileHeavy()
    {
        if (Input.GetKey(KeyCode.Mouse0) && !isLaunchActive) // || Input.GetKeyDown(KeyCode.Joystick1Button3))
        {
            if (playerAnimator.GetBool("Walk_b"))
            {
                playerAnimator.SetBool("ShotWalking_b", true);
            }
            else
            {
                playerAnimator.SetBool("ShotIdle_b", true);
            }

            
            RaycastHit hit;
            var ray = new Ray();
            ray.origin = camTransform.position;
            ray.direction = camTransform.forward;

            if (Physics.Raycast(ray, out hit) && !hit.collider.CompareTag("Player"))
            {                
                projectileHitPosition = hit.point;
                projectileHitRotation = hit.transform.rotation;

                hitDirection = hit.point - projectileSpawnPos.position;
                projectileSpawnRoot.rotation = Quaternion.LookRotation(hitDirection);
                transform.rotation = new Quaternion(0f, projectileSpawnRoot.rotation.y, 0f, projectileSpawnRoot.rotation.w);                

                if (projectileStopperHeavy < 1)
                {
                    if (projectile == null)
                    {
                        projectileShotAudio.Play();
                        projectile = Instantiate(projectilePrefab, projectileSpawnPos.position,
                                    Quaternion.LookRotation(hitDirection));
                        //Debug.DrawLine(projectileSpawnPos.position, hit.point, Color.red, 1f);

                        distanceDelayFactor = (Vector3.Distance(hit.point, transform.position) /
                            projectile.projectileSpeed);

                        projectileStopperHeavy = 1;
                    }

                    if (hitPs == null)
                    {
                        StartCoroutine(DistanceDelayTimer(distanceDelayFactor));
                    }

                    if (hit.rigidbody != null)
                    {
                        hitObjectRb = hit.rigidbody;
                        hit.rigidbody.AddForce(hitDirection * 0.5f, ForceMode.Impulse);
                    }

                    if (hit.collider.gameObject.CompareTag("Hazard"))
                    {
                        hazardObject = hit.collider.gameObject;
                    }
                    
                }


            }
            else
            {
                Vector3 camBoomRotEuler = camBoomTransform.rotation.eulerAngles;
                Vector3 camRotEuler = camTransform.rotation.eulerAngles;
                projectileSpawnRoot.eulerAngles = new Vector3(camRotEuler.x, camBoomRotEuler.y, 0f);
            }

        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && !isLaunchActive)
        {
            playerAnimator.SetBool("ShotWalking_b", false);
            playerAnimator.SetBool("ShotIdle_b", false);            
        }

    }


    IEnumerator DistanceDelayTimer(float sec)
    {
        yield return new WaitForSeconds(sec);
        
        hitPs = Instantiate(hitPsPrefab, projectileHitPosition, projectileHitRotation);

        if (hazardObject != null)
        {            
            hazardPs = Instantiate(hazardPsPrefab, hazardObject.transform.position, hazardObject.transform.rotation);
            Destroy(hazardObject);
            MainManager.Instance.score += 100;
        }

        if (hitObjectRb != null)
        {
            hitObjectRb.AddForce(hitDirection.normalized * 5f, ForceMode.Impulse);
        }

    }
    


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            MainManager.Instance.isPlayerOnGround = true;
            playerAnimator.SetBool("Jump_b", false);

            //collisionNormal = collision.GetContact(0).normal;
        }

        /*
        if (Vector3.Dot(collision.GetContact(0).normal, Vector3.up) > 0.75f)
        {
            MainManager.Instance.isPlayerOnGround = true;
            playerAnimator.SetBool("Jump_b", false);            
        }
        */
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            MainManager.Instance.isPlayerOnGround = false;
        }
    }

}
