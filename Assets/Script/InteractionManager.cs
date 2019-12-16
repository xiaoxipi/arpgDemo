using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : IActorManagerInterface
{


    private CapsuleCollider interCol;

    public List<EventCasterManager> overlapEcastms = new List<EventCasterManager>();

    // Start is called before the first frame update
    void Start()
    {
        interCol=GetComponent<CapsuleCollider>();
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    
    //}

    private void OnTriggerEnter(Collider other)
    {
        //print(other.name);
        EventCasterManager[] ecastms = other.GetComponents<EventCasterManager>();

        foreach(var ecastm in ecastms)
        {
            //print(ecastm.eventName);
            if (!overlapEcastms.Contains(ecastm))
            {
                overlapEcastms.Add(ecastm);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EventCasterManager[] ecastms = other.GetComponents<EventCasterManager>();

        foreach (var ecastm in ecastms)
        {
            //print(ecastm.eventName);
            if (overlapEcastms.Contains(ecastm))
            {
                overlapEcastms.Remove(ecastm);
            }
        }
    }

}
