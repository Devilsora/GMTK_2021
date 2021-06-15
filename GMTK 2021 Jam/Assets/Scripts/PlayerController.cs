using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 40f;
    public float jump = 40f;
    public float slideFactor = 5f;
    public float reductionFactor = 4f;

    public int weightVal = 1;

    public bool grounded = false;
    public bool onWall = false;
    public bool turnBackToCam = false;

    float maxSpeed = 20f;
    float wallTimer = 0.0f;
    float wallTime = 3f;

  
    public Rigidbody rb;
    public MeshRenderer playerMesh;
    public Animator animator;
    BlobManager blobManager;
  
    Material playerMat;
    Color playerColor;

    public bool isMainPlayer = true;

    public AudioSource source;

    public AudioClip jump_sound;
  

  // Start is called before the first frame update
  void Awake()
  {
      rb = GetComponent<Rigidbody>();
      animator = GetComponent<Animator>();
      source = GetComponent<AudioSource>();

      if (playerMesh == null && gameObject.GetComponent<MeshRenderer>() != null)
      {
        playerMesh = gameObject.GetComponent<MeshRenderer>();

        playerMat = playerMesh.material;
        playerColor = playerMat.color;
      }

      blobManager = GameObject.FindObjectOfType<BlobManager>();
        
  }

    public void Update()
    {

      if (Input.GetKeyDown(KeyCode.S))
      {
        Debug.Log("Pressing S");
        blobManager.RemoveBlob();
      }

    }

  // Update is called once per frame
  void FixedUpdate()
    {
      PlayerMovement();
      Gravity();

      if (!grounded)
      {
        //Gravity();
      }

    if (onWall)
      {
        wallTimer += Time.deltaTime;
        if(wallTimer > wallTime)
          WallSlide();
      }


      if(turnBackToCam)
      {
        Debug.Log("Turning back to cam");
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 270f, 0), 0.5f);
        if (transform.eulerAngles.y == 270f)
          turnBackToCam = false;
      }
    }

    public void OnCollisionEnter(Collision collision)
    {
      if(collision.gameObject.tag == "Ground")
      {
        grounded = true;
      }

      if(collision.gameObject.tag == "Wall")
      {
        onWall = true;
      }

      if(collision.gameObject.tag == "Blob")
      {
        int weight = collision.gameObject.GetComponent<PlayerController>().weightVal;

        if(weight >= 1 && weight < 5)
        {
          blobManager.AddBlob(weight);
          Destroy(collision.gameObject);
        }
        else
        {
          if(weight == 1)
          {
            blobManager.AddBlob();
            Destroy(collision.gameObject);
          }
            
        }
      
        //MergeBlobs(collision.gameObject);
        //replace model w/ another one of the stack, color top one the color of the one that stacked on top?
      }
      else
      {
        Debug.Log("Colliding with " + collision.gameObject.tag);
      }
    }

    public void WallSlide()
    {
      rb.AddForce(0, -slideFactor, 0);
    }

    public void PlayerMovement()
    {
      float modifiedSpeed = (speed - (weightVal * reductionFactor));
      float modifiedJump = (jump - (weightVal * reductionFactor));

      Debug.Log("Modified jump: " + jump + " -  (" + weightVal + " * " + reductionFactor + ")");

      //if (Input.GetKey(KeyCode.W) && grounded)
      //{
      //  rb.AddForce(Vector3.forward * modifiedSpeed);
      //  transform.eulerAngles = new Vector3(0f, 90f, 0f);
      //}

      bool movementKeyPressed = false;
      bool inAir = false;

      

    if (Input.GetKey(KeyCode.A))
    {
      rb.AddForce(Vector3.left * modifiedSpeed);
      rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
      
      transform.eulerAngles = new Vector3(0f, 285f, 0f);
      Debug.Log("euler angles: " + transform.eulerAngles);

      if(grounded)
        movementKeyPressed = true;
    }

    if (Input.GetKey(KeyCode.D))
    {
      rb.AddForce(Vector3.right * modifiedSpeed);
      rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
      
      transform.eulerAngles = new Vector3(0f, 255f, 0f);
      Debug.Log("euler angles: " + transform.eulerAngles);

      if (grounded)
        movementKeyPressed = true;
    }

    
    if (grounded)
      {
        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)))
        {
          source.clip = jump_sound;
          source.Play();
          rb.AddForce(Vector3.up * (modifiedJump * 60));
          rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
          grounded = false;
          inAir = true;
        }
      }
      
      if(onWall)
      {
        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)))
        {
          source.clip = jump_sound;
          source.Play();
          if (Input.GetKey(KeyCode.A))
          {
            rb.AddForce(Vector3.left * modifiedSpeed * 10 + Vector3.up * (modifiedJump * 80));
            transform.eulerAngles = new Vector3(0f, 285f, 0f);
            inAir = true;
            Debug.Log("Wall jumping left");
          }

          if (Input.GetKey(KeyCode.D))
          {
            rb.AddForce(Vector3.right * modifiedSpeed * 10 + Vector3.up * (modifiedJump * 80));
            transform.eulerAngles = new Vector3(0f, 255f, 0f);
            inAir = true;
            Debug.Log("Wall jumping left");
          }

        }
      }

      if ((!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) && grounded)
      {
        Debug.Log("Pulled off directional key, setting back");
        turnBackToCam = true;
      }


    //if (Input.GetKey(KeyCode.A) && grounded)
    //{
    //  rb.AddForce(Vector3.left * modifiedSpeed);
    //  transform.eulerAngles = new Vector3(0f, 0f, 0f);
    //  movementKeyPressed = true;
    //}
    //
    ////if (Input.GetKey(KeyCode.S) && grounded)
    ////{
    ////  rb.AddForce(Vector3.back * modifiedSpeed);
    ////  transform.eulerAngles = new Vector3(0f, -90f, 0f);
    ////}
    //
    //if (Input.GetKey(KeyCode.D) && grounded)
    //{
    //  rb.AddForce(Vector3.right * modifiedSpeed);
    //  transform.eulerAngles = new Vector3(0f, 180f, 0f);
    //  movementKeyPressed = true;
    //}
    //
    //if((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) && grounded)
    //{
    //  rb.AddForce(Vector3.up * (modifiedJump * 50));
    //  grounded = false;
    //}

      animator.SetBool("moving", movementKeyPressed);
      animator.SetBool("in_air", inAir);
      

    //if(rb.velocity.magnitude > maxSpeed)
    //{
    //  rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    //  Debug.Log("Current square magnitude " + rb.velocity.sqrMagnitude);
    //  
    //}
  }

  public void OnCollisionExit(Collision collision)
  {
    if(collision.gameObject.tag == "Wall")
    {
      onWall = false;
      wallTimer = 0.0f;
    }

    if (collision.gameObject.tag == "Ground")
    {
      //grounded = false;

    }
  }

    public void RotateTarget(float degree)
    {
      
    }

    public void Gravity()
    {
      rb.AddForce(Physics.gravity);
    }

    public void DragForce()
    {
      rb.AddForce(new Vector3(-rb.velocity.x, -rb.velocity.y, -rb.velocity.z).normalized);
    }

    //public void MergeBlobs(GameObject collision)
    //{
    //  mergedObjects.Add(collision);
    //
    //  //change material to merge in the color of the absorbed blob
    //  Material blobMat = collision.GetComponent<MeshRenderer>().material;
    //
    //  Color matColor = blobMat.color;
    //  Color newCol = new Color((playerColor.r + matColor.r) / 2, (playerColor.g + matColor.g) / 2, (playerColor.b + playerColor.b) / 2);
    //
    //  playerMat.color = newCol;
    //  gameObject.GetComponent<MeshRenderer>().material = playerMat;
    //
    //  weightVal++;
    //
    //  Destroy(collision.gameObject);
    //}
}
