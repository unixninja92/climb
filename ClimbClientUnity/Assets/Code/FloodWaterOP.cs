﻿using UnityEngine;
using System.Collections;

public class FloodWaterOP : MonoBehaviour {
	//FIELDS
	int x =0;
	int waterLevel = 0;
	private bool inputDetected = false;
	private bool two= false;
	private bool three= false;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//will need code raise the water 1/6 the way up the screen once there is 
		//input detected from the user
		
		//need to find out what input will look like
		if(x==0 && Input.anyKey ){
			inputDetected =true;
			x+=1;
		}
		if(waterLevel ==0 && inputDetected ==true) {
			//raise the water to the visible part of the screen
			transform.Translate(Vector3.up * Time.deltaTime, Space.World);
			//if(transform.position.y >= -6.91){
			if(transform.position.y >= -3.91){
				transform.Translate(Vector3.down *Time.deltaTime, Space.World);
				waterLevel = 1;
			}
		}
		//to be changed for boolean value likely based on time played
		if (x==1 && Input.GetKey (KeyCode.E)) {
			two = true;		
			x+=1;
		}
		
		if(waterLevel ==1 && two == true) {
			//raise the water to the visible part of the screen
			transform.Translate(Vector3.up * Time.deltaTime, Space.World);
			//if(transform.position.y >= -5.91){
			if(transform.position.y >= -2.91){
				transform.Translate(Vector3.down *Time.deltaTime, Space.World);
				waterLevel = 2;
			}
		}
		if (x==2 && Input.GetKey (KeyCode.R)) {
			three = true;		
			x+=1;
		}
		
		if(waterLevel ==2 && three == true) {
			//raise the water to the visible part of the screen
			transform.Translate(Vector3.up * Time.deltaTime, Space.World);
			//if(transform.position.y >= -4.91){
			if(transform.position.y >= -1.91){
				transform.Translate(Vector3.down *Time.deltaTime, Space.World);
				waterLevel = 3;
			}
		}
		transform.Translate(Vector3.left * Time.deltaTime, Space.World);
		
	}
	
}