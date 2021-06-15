using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobManager : MonoBehaviour
{
    // Start is called before the first frame update

    public List<GameObject> blob_stacks;
    public PlayerPuppeteer puppeteer;

    const int MAX_WEIGHT = 5;

    public AudioSource source;
    public AudioClip merge;
    public AudioClip split;

    void Start()
    {
      puppeteer = GameObject.FindObjectOfType<PlayerPuppeteer>();
      source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddBlob()
    {
      //adds blob to the current player puppeteer
      PlayerController player = puppeteer.currentBlob.GetComponent<PlayerController>();
      int blobIndex = player.weightVal;

      if(blobIndex < MAX_WEIGHT)
      {
        
        Debug.Log("Index: " + blobIndex);

        source.clip = merge;
        source.Play();

        GameObject newBlob = GameObject.Instantiate(blob_stacks[blobIndex], player.transform.position, player.transform.rotation);
        newBlob.GetComponent<PlayerController>().weightVal++;
        newBlob.tag = "Player";
        Camera.main.transform.gameObject.GetComponent<PlayerCameraFollow>().Target = newBlob.transform;
      
        Destroy(player.gameObject);
        puppeteer.currentBlob = newBlob;
      }
      
    }

    //handles case where player as 1 blob goes to join w/ a 2 to 4 stack
    public void AddBlob(int blobWeightVal)
    {
      //adds blob to the current player puppeteer
      PlayerController player = puppeteer.currentBlob.GetComponent<PlayerController>();
      int blobIndex = player.weightVal;

      if (blobIndex < MAX_WEIGHT)
      {
        source.clip = merge;
        source.Play();

        Debug.Log("Index: " + blobIndex);

        Debug.Log("Contact position: " + player.transform.position);
        GameObject newBlob = GameObject.Instantiate(blob_stacks[blobIndex], player.transform.position, player.transform.rotation);
        
        newBlob.tag = "Player";
        Camera.main.transform.gameObject.GetComponent<PlayerCameraFollow>().Target = newBlob.transform;

        Destroy(player.gameObject);
        puppeteer.currentBlob = newBlob;
        newBlob.transform.GetChild(1).gameObject.tag = "Player";
      }

    }


  public void RemoveBlob()
    {
      PlayerController player = puppeteer.currentBlob.GetComponent<PlayerController>();

      if(player.weightVal <= 1)
      {
        Debug.Log("Can't take off slimes, weight = 1");
        return; //player shouldn't be able to reduce down to 0
      }
      else
      {
        Debug.Log("Weight greater than 1");
      }

      int newWeight = player.weightVal - 1;
      Debug.Log("new weight: " + newWeight);
      GameObject newBlob = blob_stacks[0];
      GameObject newPlayer = blob_stacks[newWeight - 1];

      source.clip = split;
      source.Play();

    //spawn a blob from the 0 index depending if the player is moving or not - if they're not moving it goes off to the side, if they're moving it should move w/ their velocity
    if (player.GetComponent<Rigidbody>().velocity != Vector3.zero)
      {
        Instantiate(newBlob, player.transform.position + player.GetComponent<Rigidbody>().velocity.normalized * 20, player.transform.rotation);
        newBlob.GetComponent<Rigidbody>().AddForce(player.GetComponent<Rigidbody>().velocity);
      }
      else
      {
        newBlob = Instantiate(newBlob, player.transform.position - new Vector3(15, 0, 0), Quaternion.Euler(0, 270, 0));
      }



      newBlob.tag = "Blob";
      newBlob.transform.GetChild(1).gameObject.tag = "Player";
      newBlob.GetComponent<PlayerController>().weightVal = 1;
      newBlob.GetComponent<PlayerController>().enabled = false;

      Destroy(player.gameObject);

      newPlayer = Instantiate(newPlayer, player.transform.position, player.transform.rotation);
      newPlayer.GetComponent<PlayerController>().weightVal = newWeight;
      newPlayer.GetComponent<PlayerController>().enabled = true;
      newPlayer.name = "Player";
      newPlayer.transform.GetChild(1).gameObject.tag = "Player";
      newPlayer.tag = "Player";
      Camera.main.transform.gameObject.GetComponent<PlayerCameraFollow>().Target = newPlayer.transform;

      puppeteer.currentBlob = newPlayer;
  }
}
