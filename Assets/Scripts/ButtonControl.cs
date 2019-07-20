using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    public GameObject GrabHandSuccess;
    public Text GrabHandFriendName;
    public Text GrabHandDistance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GrabHand(int index)
    {
        int AverageDistance = (FriendManage.Instance.MyDistance + FriendManage.Instance.FriendList[index].distance) / 2;
        FriendManage.Instance.MyDistance = AverageDistance;
        FriendManage.Instance.FriendList[index].distance = AverageDistance;
        FriendManage.Instance.FriendDisplay();
        GrabHandSuccess.gameObject.SetActive(true);
        GrabHandFriendName.text = FriendManage.Instance.FriendList[index].name;
        GrabHandDistance.text = AverageDistance.ToString();
    }
}
