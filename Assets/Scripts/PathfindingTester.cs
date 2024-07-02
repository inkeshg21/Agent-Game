using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public class PathfindingTester : MonoBehaviour
{
    // ant colony
    private ACOTester ACOTester = new ACOTester();
    // The A* manager.
    private AStarManager AStarManager = new AStarManager();
    // Array of possible waypoints.
    List<GameObject> Waypoints = new List<GameObject>();
    // Array of waypoint map connections. Represents a path.
    List<Connection> ConnectionArray = new List<Connection>();
    // The start and end target point.
    public GameObject start;
    public GameObject end;

    // Debug line offset.
    Vector3 OffSet = new Vector3(0, 0.3f, 0);
    int current;
    Connection aConnection;
    private int countit;

    //For speed
    [HideInInspector]
    public ACOTester speednext;
    float colidespeed;

    //For status label
    public Text speedText;

    // Start is called before the first frame update
    public void Start()
    {   
        speednext = FindObjectOfType<ACOTester>();
        colidespeed = speednext.finalSpeed;
        this.GetComponent<ACOTester>().enabled = false;
        // Find all the waypoints in the level.
        GameObject[] GameObjectsWithWaypointTag;
        GameObjectsWithWaypointTag = GameObject.FindGameObjectsWithTag("Waypoint");
        foreach (GameObject waypoint in GameObjectsWithWaypointTag)
        {
            WaypointCON tmpWaypointCon = waypoint.GetComponent<WaypointCON>();
            if (tmpWaypointCon)
            {
                Waypoints.Add(waypoint);
            }
        }
        // Go through the waypoints and create connections.
        foreach (GameObject waypoint in Waypoints)
        {
            WaypointCON tmpWaypointCon = waypoint.GetComponent<WaypointCON>();
            // Loop through a waypoints connections.
            foreach (GameObject WaypointConNode in tmpWaypointCon.Connections)
            {
                Connection aConnection = new Connection();
                aConnection.SetFromNode(waypoint);
                aConnection.SetToNode(WaypointConNode);
                AStarManager.AddConnection(aConnection);
            }
        }
        // Run A Star...
        ConnectionArray = AStarManager.PathfindAStar(start, end);
        // Debug.Log(ConnectionArray.Count);

        ACOTester.rb = GetComponent<Rigidbody>();
        ACOTester.rb.MovePosition((ConnectionArray[0].GetFromNode().transform.position + ACOTester.OffSet));
        transform.position = ConnectionArray[0].GetFromNode().transform.position;

        countit = 0;
    }
    // Draws debug objects in the editor and during editor play (if option set).
    void OnDrawGizmos()
    {
        // Draw path.
        foreach (Connection aConnection in ConnectionArray)
        {

            Gizmos.color = Color.green;
            Gizmos.DrawLine((aConnection.GetFromNode().transform.position + ACOTester.OffSet),
            (aConnection.GetToNode().transform.position + ACOTester.OffSet));

        }
    }

    // Update is called once per frame
    public void Update()
    {
        speednext = FindObjectOfType<ACOTester>();

        if (transform.position != ConnectionArray[current].GetToNode().transform.position)
        {
            Vector3 pos3 = Vector3.MoveTowards(transform.position, ConnectionArray[current].GetToNode().transform.position, speednext.finalSpeed * Time.deltaTime);
            var LookPos3 = ConnectionArray[current].GetToNode().transform.position - transform.position;
            LookPos3.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(LookPos3), 1);
            GetComponent<Rigidbody>().MovePosition(pos3);
            // Debug.Log(transform.position);
        }
        else
        {
            current = (current + 1) % ((ConnectionArray.Count));
                countit = countit + 1;
                    if(countit==1)
                    {
                        speednext.finalSpeed = 0f;
                    }
                    countit = 0;
                    
            // if (current + 2 == (ConnectionArray.Count - 1) && (transform.position != ConnectionArray[current].GetToNode().transform.position))
            if (current + (ConnectionArray.Count - 1) == (ConnectionArray.Count - 1) && (transform.position != ConnectionArray[current].GetToNode().transform.position))
            {
                if ((transform.position != ConnectionArray[(current + (ConnectionArray.Count - 1))].GetFromNode().transform.position))
                {

                }
            }
            else
            {
                {
                    
                    current = (current) % ((ConnectionArray.Count));
                }
            }
        }
        // speedometer
        speedText.text = (int)speednext.finalSpeed + " km/hr";

    }

    void OnTriggerEnter(Collider other)
    {
        //For returning the speed before avoiding collison
        if (other.gameObject.CompareTag("Van3"))
        {   
            speednext.finalSpeed = 20f;
        }
        if (other.gameObject.CompareTag("Van2"))
        {
            speednext.finalSpeed = 30f;
        }
        if (other.gameObject.CompareTag("Van1"))
        {
            speednext.finalSpeed = 0f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        //For returning the speed before avoiding collison
        if (other.gameObject.CompareTag("Van3"))
        {
            speednext.finalSpeed = colidespeed;
        }
        if (other.gameObject.CompareTag("Van2"))
        {
            speednext.finalSpeed = colidespeed;
        }
        if (other.gameObject.CompareTag("Van1"))
        {
            speednext.finalSpeed = colidespeed;
        }
    }

}