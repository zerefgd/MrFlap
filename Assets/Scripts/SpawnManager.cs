using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _spokePrefab;

    private List<GameObject> _spokes = new List<GameObject>();

    private void OnEnable()
    {
        EventManager.StartListening(Constants.EventNames.GAME_START, SpawnSpokes);
        EventManager.StartListening(Constants.EventNames.SPAWN_SPOKE, SpawnSpoke);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Constants.EventNames.GAME_START, SpawnSpokes);
        EventManager.StopListening(Constants.EventNames.SPAWN_SPOKE, SpawnSpoke);
    }

    private void SpawnSpokes(Dictionary<string,object> message)
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject tempSpoke = Instantiate(_spokePrefab, Vector3.zero, Quaternion.identity);
            tempSpoke.transform.rotation = Quaternion.Euler(0,0,-90f * i);
            _spokes.Add(tempSpoke);
            tempSpoke.GetComponent<SpokeMovement>().SetSpokeParams(((i + 4) % 5) * 0.2f);
        }

        _spokes[0].GetComponent<SpokeMovement>().SetFirstSpoke();
    }

    private void SpawnSpoke(Dictionary<string, object> message)
    {
        GameObject tempSpoke = Instantiate(_spokePrefab, Vector3.zero, Quaternion.identity);
        tempSpoke.GetComponent<SpokeMovement>().SetSpokeParams(5f);
        _spokes.Add(tempSpoke);


        float rotationOffset = 360 / _spokes.Count;
        Vector3 spokeAngle = _spokes[0].transform.rotation.eulerAngles;

        for (int i = 0; i < _spokes.Count; i++)
        {
            tempSpoke = _spokes[i];
            Vector3 tempOffset = new Vector3(0, 0, -rotationOffset * i);
            tempSpoke.transform.rotation = Quaternion.Euler(spokeAngle + tempOffset);
        }
    }

}
