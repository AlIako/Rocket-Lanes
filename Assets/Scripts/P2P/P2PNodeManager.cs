using System.Collections;
using System.Collections.Generic;

// tracks incoming and leaving nodes (clients) and save their ip and id
public class P2PNodeManager
{
	public static int MyNodeId = -1;
	public static List<P2PNode> nodes = new List<P2PNode>();

    public static string GetIpForNodeId(int id)
    {
        string ip = "";

        // find node with the right id and return its ip address

        return ip;
    }

    public static bool StillOffline()
    {
        return MyNodeId == -1;
    }

    public static int ComputeIdForNewPlayer()
    {
        // stay between 0, 1, 2, 3 ? for now I'll make it simple and increment highest id
        int newId = MyNodeId;

        foreach(P2PNode node in nodes)
        {
            if(node.id >= newId)
                newId = node.id;
        }

        return newId + 1;
    }

    public static void ComputeIdForMyself()
    {
        MyNodeId = 0;
    }

    public static bool GameFull()
    {
        return nodes.Count >= 3;
    }
}
