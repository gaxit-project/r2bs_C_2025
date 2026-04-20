using UnityEngine;

public class CollisionDeath : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("aaa");
        if(collision.gameObject.tag == "TeamOne" || collision.gameObject.tag == "TeamTwo")
        {
            Debug.Log("bbb");
            //collision.gameObject.GetComponent<PlayerController>().RespawnPlayer();
        }
    }
}
