using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AP2PController : MonoBehaviour
{
    [SerializeField]
    private GameObject instancePrefab;

    public List<GameObject> instances;

    private List<int> ports;
	float lastJoinTime = 0;
	float cooldownJoin = 2;

    // Start is called before the first frame update
    void Start()
    {
        ports = new List<int>();
        ports.Add(8888);
        ports.Add(8887);
        ports.Add(8886);
        ports.Add(8885);

        instances = new List<GameObject>();
        Join();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastJoinTime > cooldownJoin)
        {
            Join();
        }
    }

    void Join()
    {
        lastJoinTime = Time.time;

        if(instances.Count < 2)
        {
            GameObject instanceGO = Instantiate(instancePrefab);
            instances.Add(instanceGO);
            
            GameController gameController = instanceGO.GetComponent<GameController>();
            gameController.FindLanes();

            P2PController p2PController = instanceGO.GetComponent<P2PController>();
            gameController.SetNetworkController(p2PController);
            p2PController.SetGameController(gameController);
            p2PController.SetMyPort(ports[instances.Count - 1]);

            if(instances.Count == 1)
            {
                p2PController.NewGame();
            }
            else
            {
                p2PController.SetTargetPort(ports[0]);
                p2PController.SetTargetPort(ports[instances.Count - 2]);
                p2PController.JoinGame();
            }
        }
    }
}
