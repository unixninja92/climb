﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vine : MonoBehaviour {
    private List<VineLevel> _vineLevels;
    private List<LeafPlatform> _leafPlatforms;

	private VineLevel _lastLevel;
	private int _firstLevelIndex;
	private int _firstPlatformIndex;

	private int _nextPlatform;

	private bool _move = false;
	private bool _left = false;

	void Awake() {
        _vineLevels = new List<VineLevel>();
        _leafPlatforms = new List<LeafPlatform>();

		_firstLevelIndex = 0;
		_firstPlatformIndex = 0;

		_nextPlatform = Random.Range(2, 5);

		while(_lastLevel == null || _lastLevel.transform.position.y < Camera.main.orthographicSize - 2)
			Grow();
	}
	
	void Update () {
		if(Input.GetKey(KeyCode.Space))
			_move = true;

		if(!_move) return;

		transform.position -= new Vector3(0, 0.005f);

		if(_lastLevel.transform.position.y < Camera.main.orthographicSize - 2)
			Grow();

		if(_vineLevels[_firstLevelIndex].transform.position.y < -Camera.main.orthographicSize - 1) {
			VineLevel firstLevel = _vineLevels[_firstLevelIndex];

			Destroy(firstLevel.gameObject);
			_vineLevels[_firstLevelIndex] = null;

			_firstLevelIndex++;
		}
		
		if(_leafPlatforms[_firstPlatformIndex].transform.position.y < -Camera.main.orthographicSize - 1) {
			LeafPlatform firstPlatform = _leafPlatforms[_firstPlatformIndex];

			Destroy(firstPlatform.gameObject);
			_leafPlatforms[_firstPlatformIndex] = null;

            _firstPlatformIndex++;
        }
    }
    
    private void Grow() {
        float positionXSample = (Mathf.PerlinNoise(_vineLevels.Count / 10.0f, 0) - 0.5f) * 4.0f;
		float scaleSample = Mathf.Max(2.0f, Mathf.PerlinNoise((_vineLevels.Count + 200) / 10.0f, 0) * 4.0f);

		Vector3 oldPos = (_lastLevel == null ? new Vector3(0, 0) : _lastLevel.transform.localPosition);


        GameObject newLevelGo = UnityUtils.LoadResource<GameObject>("Prefabs/VineLevel", true);
        newLevelGo.transform.parent = transform;
		newLevelGo.transform.localPosition = new Vector3(positionXSample, oldPos.y + 0.5f, 0);
		newLevelGo.transform.localScale = new Vector3(scaleSample, 0.5f, 1.0f);

        VineLevel newLevel = newLevelGo.GetComponent<VineLevel>();

        _vineLevels.Add(newLevel);
		_lastLevel = newLevel;

        newLevel.Grow(scaleSample);


		if(--_nextPlatform == 0) {
			GrowPlatform(newLevelGo.transform.localPosition.y);
			_nextPlatform = Random.Range(3, 5);
        }
    }

    private void GrowPlatform(float vineHeight) {
		float position;
		float max = 4.0f;
		float min = 0.8f;
		if(_left)
			position = Random.Range(-max, -min);
		else
			position = Random.Range(min, max);

		_left = !_left;

		float hModifier = Random.Range (-0.4f, 1.8f);

		int blocks = Random.Range(5, 8);

        GameObject leafPlatformGo = UnityUtils.LoadResource<GameObject>("Prefabs/LeafPlatform", true);
        leafPlatformGo.transform.parent = transform;

		LeafPlatform leafPlatform = leafPlatformGo.GetComponent<LeafPlatform>();
		leafPlatform.Grow(blocks);

//		float platformTilt = Random.Range(-10.0f, 10.0f);

		leafPlatformGo.transform.localPosition = new Vector2(position, vineHeight + hModifier);
//		leafPlatformGo.transform.localEulerAngles = new Vector3(0, 0, platformTilt);

		_leafPlatforms.Add(leafPlatform);
    }
}
