using System.Collections;
using UnityEngine;

public class WarpGate : MonoBehaviour
{
    public int warpNum = 0;

    public int groupId;
    public Vector3 myGridPosition;

    public Transform parentObj;

    int _warpID;

    bool coolDown;


    private void Start()
    {
        GameObject bombParentObj = GameObject.Find("WarpGateGenerate");
        parentObj = bombParentObj.transform; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "TeamOne" || other.tag == "TeamTwo")
        {
            if(!other.GetComponent<PlayerBase>().isWarpCoolDown)
            {
                if (groupId % 2 == 0)
                {
                    _warpID = groupId + 1;
                }
                else if (groupId % 2 == 1)
                {
                    _warpID = groupId - 1;
                }
                for (int i = 0; i < parentObj.childCount; i++)
                {
                    WarpGate gate = parentObj.GetChild(i).GetComponent<WarpGate>();
                    if (gate.groupId == _warpID)
                    {
                        // プレイヤーの座標をそのオブジェクトの座標に飛ばす
                        Debug.Log("わーーーーーぷ");
                        other.GetComponent<PlayerBase>().WarpPosition(gate.myGridPosition);
                        other.GetComponent<PlayerBase>().isWarpCoolDown = true;
                        StartCoroutine(CoolDown(other.GetComponent<PlayerBase>().isWarpCoolDown));

                        break;
                    }
                }
            }
        }
    }


    IEnumerator CoolDown(bool flag)
    {
        yield return new WaitForSeconds(5f);
        flag = false;
    }
}
