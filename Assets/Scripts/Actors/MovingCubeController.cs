using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCubeController : MonoBehaviour {

    enum MovingCubeState {Moving, NotMoving, StartAndWaitForTargets, SearchBestTarget};
    MovingCubeState movingState;
    bool stateJustChanged;
    //Avoir un tableau pour les position transform des char
    Character[] playersCharacter;
    public List<Vector3> playersPositions;
    Vector3 targetPosition;
    Rigidbody rb;

    public string actualState;

    public List<float> playersDistance;
    //Selected direction
    public int intDirections;
    public float maxSpeed;
    public float brakeSpeed;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();



        ChangeState(MovingCubeState.StartAndWaitForTargets);
    }

    // Update is called once per frame
    void Update () {
		
	}

    void FixedUpdate()
    {
        //Debug.Log(PlayersManager.playersManager.charactersPlayedNow.Length);
        actualState = movingState.ToString();

        switch (movingState)
        {
            case MovingCubeState.StartAndWaitForTargets:
                WaitForTargetsOnStart();
                break;

            case MovingCubeState.SearchBestTarget:
                SearchForBestTarget();
                break;

            case MovingCubeState.Moving:

                break;
            case MovingCubeState.NotMoving:
                OnMovingStop();
                break;
        }
    }

    void WaitForTargetsOnStart()
    {
        //When targets are available
        if(PlayersManager.playersManager.charactersPlayedNow.Length > 0)
        {
            playersCharacter = new Character[PlayersManager.playersManager.charactersPlayedNow.Length];
            playersCharacter = PlayersManager.playersManager.charactersPlayedNow;

            if (stateJustChanged)
            {
                ChangeState(MovingCubeState.SearchBestTarget);
            }
        }
    }    
    
    void SearchForBestTarget()
    {
        //SEARCH FOR TARGET
        playersDistance = new List<float>(); //      [playersCharacter.Length];
        foreach (Character charac in playersCharacter)
        {
            playersDistance.Add(Vector3.Distance(charac.transform.position, transform.position));
        }

        //Mettre liste dans l'ordre
        playersDistance.Sort();
        
        //Find corresponding direction




    }


    void OnMovingStart()
    {


    }

    void Moving()
    {
        if (rb.velocity.magnitude > 0.1f)
        {   //Ralentir le depacement jusqu'au freinage					
            rb.AddForce(-rb.velocity * brakeSpeed * Time.deltaTime, ForceMode.Acceleration);
        }
        else
        {   //When the cube is totally stopped				
            rb.velocity = Vector3.zero;
        }
    }

    void OnMovingStop()
    {
        //Only when the method is called; 
        if (stateJustChanged)
        {
            stateJustChanged = true;
            SearchForBestTarget();
        }
        
        
        //StartMoving();

    }



    void ChangeState(MovingCubeState requestedState)
    {


        stateJustChanged = true;
        Debug.Log("CHANGING STATE FROM : " + actualState.ToString() + " TO : " + requestedState.ToString());
        movingState = requestedState;


    }

}
