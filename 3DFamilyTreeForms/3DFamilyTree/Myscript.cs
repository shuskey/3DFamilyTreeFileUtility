﻿using System.Collections;
using System.Collections.Generic;

#if NOTNOW
public class Myscript : MonoBehaviour {

	bool isActive = false;
	
	public GameObject mycube;
	public int currentYear = 0;
	public Matchmaker myMatchMaker;
	public MyPeople myPeople;
	public Color BrideColor = new Color(0.855f, 0.439f, 0.839f);
	public Color GroomColor = new Color(0.118f, 0.565f, 1.0f);
	public Color GirlColor = new Color(0.847f, 0.749f, 0.847f);
	public Color BoyColor = new Color(0.392f, 0.584f, 0.929f);
	public Color BlankColor = new Color(.467f, 0.533f, 0.600f);
	
	//public MySecondScript secondScriptqqqqqqwe;

	void Awake()
	{
		Debug.Log ("Awake");
		myMatchMaker = new Matchmaker();
		myPeople = new MyPeople();
		int lastYear = 0;
		currentYear = 0;

		Person Adam = new Person(Person.PersonType.Adam, currentYear.ToString());
		Person Eve = new Person(Person.PersonType.Eve, currentYear.ToString());
		var personAIndex = myPeople.addToAllPeople(Adam);
		var personEIndex = myPeople.addToAllPeople(Eve);
		myMatchMaker.addToSinglesList(personAIndex, Adam.Sex);
		myMatchMaker.addToSinglesList(personEIndex, Eve.Sex);

		//Debug.Log ("Hello " + Adam.Name + " and " + Eve.Name + " !");
		//Debug.Log (Adam.GetSex() + " and " + Eve.GetSex());

		/// TODO
		/// implement the House cluster Display List Data type
		/// Draw a sample Cluster
		/// put in Marriage portals
		/// put in tomb stones
		/// add in Divorce and side winds
		/// 
		for (currentYear = 0 ; currentYear < 200 ; currentYear++)
		{

			Debug.Log ("Happy New Year!!.  It is year: " + currentYear.ToString());
			Debug.Log ("We have " + myMatchMaker.BachelorPersonIndexList.Count + " Bachelors, and " +
			           myMatchMaker.BachelorettePersonIndexList.Count + " Bachelorettes");
			Debug.Log ("Our people count is " + myPeople.allPeople.Count + ", with alive count =" + myPeople.livingCount());
			Debug.Log ("We have " + myMatchMaker.allFamilies.Count + " Families <-----------------------");

			myMatchMaker.doWeddings(currentYear);
			myMatchMaker.beFruitfullAndMultiply(currentYear);
			myPeople.mortality(currentYear);

			lastYear = currentYear;
		}
		Debug.Log("We are done with populating the earth!");

		myMatchMaker.displayAllFamilies(lastYear);

	}
	// Use this for initialization
	void Start () {
		Debug.Log ("I just started!");
		//secondScript.SortMyList();

		int[] counterPerGeneration = new int [25];
		for (int i = 0; i < 25; i++) counterPerGeneration[i] = 0;
		Person.timeSpan[] personTimeSpans = new Person.timeSpan[25];

		foreach (Family familyHome in myMatchMaker.allFamilies)
		{
			for (int i = 0; i < 25; i++) personTimeSpans[i].Start = personTimeSpans[i].End = 0;
			int generation = familyHome.Generation;

			int sequence = counterPerGeneration[generation]++;
			int startDate = familyHome.marriageDateInt();
			int endDate = startDate;
			int peopleCount = familyHome.ChildrenPersonIndexList.Count + 2;
			if (generation != 0)
			{
				for (int p = 0; p < peopleCount; p++)
				{
					if (p == 0) // Bride
					{
						Person mySpouse = myPeople.allPeople[familyHome.BridePersonIndex];
						personTimeSpans[p].Start = startDate;
						if (mySpouse.Death == "")
						{ // Not Dead
							personTimeSpans[p].End = currentYear; // I am ALIVE, and not married
						}
						else
						{ // Dead
							personTimeSpans[p].End = mySpouse.deathDateInt();
						}
					}
					else if (p == 1) // Groom
					{
						Person mySpouse = myPeople.allPeople[familyHome.GroomPersonIndex];
						personTimeSpans[p].Start = startDate;
						if (mySpouse.Death == "")
						{ // Not Dead
							personTimeSpans[p].End = currentYear; // I am ALIVE, and not married
						}
						else
						{ // Dead
							personTimeSpans[p].End = mySpouse.deathDateInt();
						}
					}
					else //Children get their timespan in the house
					{
						Person mychild = myPeople.allPeople[familyHome.ChildrenPersonIndexList[p-2]];
						personTimeSpans[p].Start = mychild.birthDateInt();
						if (!mychild.isMarried())
						{
							if (mychild.Death == "")
							{ // Not Married, Not Dead
								personTimeSpans[p].End = currentYear; // I am ALIVE, and not married
							}
							else
							{ //Not Married, But Dead
								personTimeSpans[p].End = mychild.deathDateInt();
							}

						} 
						else
						{ // Married!
							personTimeSpans[p].End = mychild.marriageDateInt();
						}
					}
					// who is the last left in the house?
					if (personTimeSpans[p].End > endDate) endDate = personTimeSpans[p].End;
				}
	
				float Y = generation * 3;
				float X = sequence * 60;
				float Z = startDate;

				float Zscale = (float)(endDate - startDate);
				float Xscale = (float)peopleCount*5;

				//GameObject newHouse = GameObject.CreatePrimitive(PrimitiveType.Cube);
				GameObject newHouse = (GameObject) Instantiate(mycube, new Vector3(0.0f, 0.0f, 0.0f), transform.rotation);

				newHouse.transform.position=new Vector3(X,Y,Z);  // + 5*(peopleCount-1)
				newHouse.transform.localScale=new Vector3(Xscale, 0.1f, -Zscale);
				newHouse.GetComponent<Renderer>().material.color = BlankColor;
			

				Xscale = 5.0f;
				Y = Y + 0.1f;

				for (int p = 0; p < peopleCount; p++)
				{
					if (p == 0) // Bride
					{
						//GameObject newBride = GameObject.CreatePrimitive(PrimitiveType.Cube);
						GameObject newBride = (GameObject) Instantiate(mycube, new Vector3(0.0f, 0.0f, 0.0f), transform.rotation);
						Zscale = (float)(personTimeSpans[p].End - startDate);
						
						newBride.transform.position=new Vector3(X,Y,Z);
						newBride.transform.localScale=new Vector3(Xscale, 0.2f, -Zscale);
						newBride.GetComponent<Renderer>().material.color = BrideColor;
				
					}
					else if (p == 1) // Groom
					{
						//GameObject newGroom = GameObject.CreatePrimitive(PrimitiveType.Cube);
						GameObject newGroom = (GameObject) Instantiate(mycube, new Vector3(0.0f, 0.0f, 0.0f), transform.rotation);

						Zscale = (float)(personTimeSpans[p].End - startDate);
						
						newGroom.transform.position=new Vector3(X + 5,Y,Z);
						newGroom.transform.localScale=new Vector3(Xscale, 0.2f, -Zscale);
						newGroom.GetComponent<Renderer>().material.color = GroomColor;

						
					}
					else //Children get their timespan in the house
					{
						//GameObject newChild = GameObject.CreatePrimitive(PrimitiveType.Cube);
						GameObject newChild = (GameObject) Instantiate(mycube, new Vector3(0.0f, 0.0f, 0.0f), transform.rotation);

						Zscale = (float)(personTimeSpans[p].End - personTimeSpans[p].Start);

						newChild.transform.position=new Vector3(X+(5*p),Y,personTimeSpans[p].Start);
						newChild.transform.localScale=new Vector3(Xscale, 0.2f, -Zscale);
						if (myPeople.allPeople[familyHome.ChildrenPersonIndexList[p-2]].Sex == Person.PersonSex.Female)
						{
							newChild.GetComponent<Renderer>().material.color = GirlColor;
						}
						else
						{
							newChild.GetComponent<Renderer>().material.color = BoyColor;
						}
						
					}

				}


				//transform.localScale = new Vector3(transform.localScale.x  2, transform.localScale.y  2, transform.localScale.z * 2);
			} // skip gen 0
		
		}


//		for (int y = 0; y < 5; y++) {
//			for (int x = -5; x < 5; x++) {
//
//				Instantiate(brick, new Vector3(x, y + 3, 0 ), Quaternion.identity);
//			}
//		}

		
	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log ("Updating!!");
	}
	
}

#endif