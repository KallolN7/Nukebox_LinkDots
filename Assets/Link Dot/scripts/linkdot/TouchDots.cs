using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hitcode_linkDots { 
	public class TouchDots : MonoBehaviour {
		public int tx,ty;
		// Use this for initialization
		int id;
		void Start () {
          
			id = ty * GameData.bsize + tx;
            

        }

		// Update is called once per frame
		void Update () {
    
		}

        bool colorPath = false;
		void OnMouseDown () {
			if (GameData.instance.isWin)
				return;
			if (GameData.instance.isHolding)
				return;
			Transform dotChild = transform.Find ("dot");

			int tdotColor = GameData.getInstance().DotColorData [id];
			if (tdotColor != 0) {//got a dot here
                if (GameData.instance.tipUsed > 0 && tdotColor <= GameData.instance.tipUsed)
                {
                    return;
                }
                clearOldPath(tdotColor);

				GameData.instance.pickColor = tdotColor;
//				print ("firstpick:dot:" + tdotColor);

				Color tcolor = GameData.instance.colors [tdotColor];
				tcolor.a = .5f;
                if (colorPath)
                {
                    transform.GetComponent<SpriteRenderer>().color = tcolor;
                }
				GameData.instance.ColorData [id] = tdotColor;

				GameData.instance.paths[tdotColor] = new List<int> ();//overwrite the old path
//				GameData.instance.paths[tdotColor].Add (id);
				addPath(tdotColor,id);
			} else {
				int cBlockColor = GameData.instance.ColorData [id];
//				print("cblockcolor"+cBlockColor);
				if (cBlockColor == 0) {//tap on a empty block,nothing should happen
					return;
				} else {
                    if (GameData.instance.tipUsed > 0 && cBlockColor <= GameData.instance.tipUsed)
                    {
                        return;
                    }


                    //continue drawing on an already path block
                    GameData.instance.pickColor = cBlockColor;
					//reopen the linkage
					GameData.instance.linkedLines[cBlockColor] = 0;
					for(int i = GameData.instance.paths[cBlockColor].Count - 1;i> 0;i--){
						int oldid = GameData.instance.paths[cBlockColor] [i];
						if (oldid != id) {//remove all paths until find the start one
							//and clear the useless path
							int oldx = Mathf.FloorToInt (oldid % GameData.bsize) ;
							int oldy = Mathf.FloorToInt (oldid / GameData.bsize);

							GameObject oldBg = GameObject.Find ("bg" + oldx + "_" + oldy);

							Color tcolor = GameData.instance.colors [0];
							//tcolor.a = 1f;
							oldBg.transform.GetComponent<SpriteRenderer> ().color = tcolor;
							GameData.instance.ColorData [oldid] = 0;


//							GameData.instance.paths[cBlockColor].RemoveAt (i);
							removePath(cBlockColor,i);
//							GameData.instance.paths[cBlockColor].Add (id);
							addPath(cBlockColor,id);


						} else {
							break;
						}

					}

				}
			}




			GameData.instance.isHolding = true;
			GameData.instance.startId = id;
			GameData.instance.lasttx = tx;
			GameData.instance.lastty = ty;

            checkWin();

        }


		void clearOldPath(int tcolorid){

			int tlen = GameData.instance.paths[tcolorid].Count;



			while(GameData.instance.paths[tcolorid].Count > 0){
				int oldid = GameData.instance.paths[tcolorid] [GameData.instance.paths[tcolorid].Count - 1];


					//and clear the old path
				int oldx = Mathf.FloorToInt (oldid % GameData.bsize) ;
				int oldy = Mathf.FloorToInt (oldid / GameData.bsize);

					GameObject oldBg = GameObject.Find ("bg" + oldx + "_" + oldy);

					Color tcolor = GameData.instance.colors [0];
					oldBg.transform.GetComponent<SpriteRenderer> ().color = tcolor;
					GameData.instance.ColorData [oldid] = 0;


//				GameData.instance.paths[tcolorid].RemoveAt(GameData.instance.paths[tcolorid].Count-1);
				removePath(tcolorid,GameData.instance.paths[tcolorid].Count-1);
				//reopen the linkage
				GameData.instance.linkedLines[tcolorid] = 0;


			}
		}



		void OnMouseOver(){
			if (GameData.instance.isWin)
				return;
			if (GameData.instance.isHolding) {

                


                if (GameData.instance.pickColor != 0) {
					//if dot here,get dot color
					Transform dotChild = transform.Find ("dot");
					int tdotColor = GameData.instance.DotColorData [id];
					//current block color;
					int tColorid = GameData.instance.ColorData [id];
                    

                    if ((tdotColor == 0 && tColorid == 0) || GameData.instance.pickColor == tdotColor && GameData.instance.paths[tdotColor][0] != id) {//all places which can be draw and have not draw on something
						
						//exclude not nearby blocks
						if ((Mathf.Abs (tx - GameData.instance.lasttx) == 1 && ty == GameData.instance.lastty) ||
							(Mathf.Abs (ty - GameData.instance.lastty) == 1 && tx == GameData.instance.lasttx)) {

							addColor ();
						}
					} else {//draw on an already exist self color path
                        
                        if (tColorid != 0) {
							int tlen = GameData.instance.paths[tColorid].Count;


							//exclude not nearby blocks
							if ((Mathf.Abs (tx - GameData.instance.lasttx) == 1 && ty == GameData.instance.lastty) ||
							    (Mathf.Abs (ty - GameData.instance.lastty) == 1 && tx == GameData.instance.lasttx)) {
								if (tColorid == GameData.instance.pickColor) {//draw on self old blocks

									//and clear the old path
									int oldId = GameData.instance.paths [tColorid] [GameData.instance.paths [tColorid].Count-1];
									while (oldId!= id) {
										int oldx = Mathf.FloorToInt (oldId % GameData.bsize) ;
										int oldy = Mathf.FloorToInt (oldId / GameData.bsize);
										GameObject oldBg = GameObject.Find ("bg" + oldx + "_" + oldy);

										Color tcolor = GameData.instance.colors [0];
										oldBg.transform.GetComponent<SpriteRenderer> ().color = tcolor;
										removePath(tColorid,GameData.instance.paths[tColorid].Count-1);
											GameData.instance.ColorData [oldId] = 0;

											//next prev id
											oldId = GameData.instance.paths [tColorid] [GameData.instance.paths [tColorid].Count - 1];


											GameData.instance.lasttx = tx;
											GameData.instance.lastty = ty;

								
									}

								} else {//draw on other block lines,cut them
                                        //clear the being cutted other color old paths

                                    
									int tOtherColorId = GameData.instance.ColorData[id];

                                 

                                    if (GameData.instance.tipUsed > 0 && tOtherColorId <= GameData.instance.tipUsed)
                                    {
                                        return;
                                    }

                                    int oldId = GameData.instance.paths [tOtherColorId] [GameData.instance.paths [tOtherColorId].Count-1];
									
									if (GameData.instance.DotColorData[oldId] == 0 || tOtherColorId != GameData.instance.pickColor) {//if this place doesnt have a dot or this place is a different color
										if (GameData.instance.DotColorData [id] == 0) {	//make wont draw on other color dots
											while (oldId != id) {
									
												int oldx = Mathf.FloorToInt (oldId % GameData.bsize);
												int oldy = Mathf.FloorToInt (oldId / GameData.bsize);
												GameObject oldBg = GameObject.Find ("bg" + oldx + "_" + oldy);

												Color tcolor = GameData.instance.colors [0];
												oldBg.transform.GetComponent<SpriteRenderer> ().color = tcolor;
//												GameData.instance.paths [tOtherColorId].RemoveAt (GameData.instance.paths [tOtherColorId].Count - 1);
												removePath(tOtherColorId,GameData.instance.paths[tOtherColorId].Count-1);
												GameData.instance.ColorData [oldId] = 0;

												//next prev id
												oldId = GameData.instance.paths [tOtherColorId] [GameData.instance.paths [tOtherColorId].Count - 1];

											}

											if (oldId == id) {
												int oldx = Mathf.FloorToInt (oldId % GameData.bsize);
												int oldy = Mathf.FloorToInt (oldId / GameData.bsize);
												GameObject oldBg = GameObject.Find ("bg" + oldx + "_" + oldy);

												Color tcolor = GameData.instance.colors [0];
												oldBg.transform.GetComponent<SpriteRenderer> ().color = tcolor;
//												GameData.instance.paths [tOtherColorId].RemoveAt (GameData.instance.paths [tOtherColorId].Count - 1);
												removePath(tOtherColorId,GameData.instance.paths[tOtherColorId].Count-1);
												GameData.instance.ColorData [oldId] = 0;

									
											}
										}
									}

					


								}
							}
						}
					}

				}
				checkWin ();
			}//if holding
		}



		void addColor(){
			int tdotColor = GameData.instance.DotColorData [id];

//			if (GameData.instance.linkedLines [GameData.instance.pickColor] == 1 && GameData.instance.pickColor == GameData.instance.ColorData[id])//this linkage is closed,unless the linkage being breaked or restart,wont be able to continue drawing
//				return;

			if ((tdotColor!= 0 && tdotColor != GameData.instance.pickColor)//draw on other color dots(not block)
				
			) {
				GameData.instance.isHolding = false;
				GameData.instance.pickColor = 0;
				return;
			} 




			Color tcolor = GameData.instance.colors [GameData.instance.pickColor];
			tcolor.a = .5f;
            if (colorPath)
            {
                transform.GetComponent<SpriteRenderer>().color = tcolor;
            }
//			GameData.instance.paths[GameData.instance.pickColor].Add (id);
			addPath(GameData.instance.pickColor,id);
//			print ("added"+GameData.instance.pickColor);

			GameData.instance.ColorData [id] = GameData.instance.pickColor;//write color to data


			if (tdotColor != 0 && tdotColor == GameData.instance.pickColor && GameData.instance.paths [tdotColor].Count > 1) {
				GameData.instance.linkedLines [tdotColor] = 1;
				GameData.instance.pickColor = 0;
			}


			GameData.instance.lasttx = tx;
			GameData.instance.lastty = ty;
		}


		void OnMouseUp(){
			if (GameData.instance.isWin)
				return;
			GameData.instance.isHolding = false;


		}


        static bool canPlatDotSfx = true;//make draw sound effect not be too frequent;
        IEnumerator sfxGap()
        {
            yield return new WaitForSeconds(.2f);
            canPlatDotSfx = true;
        }
		void addPath(int colorId,int placeId){


            if (canPlatDotSfx)
            {
                string tsfx = "d" + Mathf.FloorToInt(Random.Range(0, 6));
                GameManager.getInstance().playSfx(tsfx);
                canPlatDotSfx = false;
                StartCoroutine("sfxGap");
            }
           

            GameData.instance.paths[colorId].Add (placeId);
			int tlen = GameData.instance.paths [colorId].Count;
			if (tlen > 1) {
				int tlastId1 = GameData.instance.paths [colorId] [tlen - 2];
				int tlastId2 = GameData.instance.paths [colorId] [tlen - 1];


				int tx = Mathf.FloorToInt (tlastId1 % GameData.bsize);
				int ty = Mathf.FloorToInt (tlastId1 / GameData.bsize);

				int placeOffset = tlastId2 - tlastId1;
//				print ("placeOffset" + placeOffset);

				int tRight = 1;
				int tLeft = -1;
				int tUp = GameData.bsize;//paths go up
				int tDown = -GameData.bsize;
				GameObject tlink = null;
				if (placeOffset == tRight) {//right
					tlink = GameObject.Find("linkr"+tx+"_"+ty);
				} else if (placeOffset == tLeft) {
					tlink = GameObject.Find("linkl"+tx+"_"+ty);
				} else if (placeOffset == tUp) {
					tlink = GameObject.Find("linku"+tx+"_"+ty);
				} else if (placeOffset == tDown) {
					tlink = GameObject.Find("linkd"+tx+"_"+ty);
				}
				if (tlink != null) {
					tlink.GetComponent<SpriteRenderer> ().color = GameData.instance.colors [colorId];
				}
			}

		}

		void removePath(int colorId,int index){

          

			//clear all linkage on this node
			int tlastId = GameData.instance.paths [colorId] [index];
							int tx = Mathf.FloorToInt (tlastId % GameData.bsize);
							int ty = Mathf.FloorToInt (tlastId / GameData.bsize);

                            
                            
                            
							GameObject tlink = null;
							tlink = GameObject.Find ("linkr" + tx + "_" + ty);
							tlink.GetComponent<SpriteRenderer> ().color = GameData.instance.colors [0];
							tlink = GameObject.Find ("linkl" + tx + "_" + ty);
							tlink.GetComponent<SpriteRenderer> ().color = GameData.instance.colors [0];
							tlink = GameObject.Find ("linku" + tx + "_" + ty);
							tlink.GetComponent<SpriteRenderer> ().color = GameData.instance.colors [0];
							tlink = GameObject.Find ("linkd" + tx + "_" + ty);
							tlink.GetComponent<SpriteRenderer> ().color = GameData.instance.colors [0];


			GameData.instance.paths[colorId].RemoveAt (index);

			if (index > 0) {
				tlastId = GameData.instance.paths [colorId] [index - 1];
				tx = Mathf.FloorToInt (tlastId % GameData.bsize);
				ty = Mathf.FloorToInt (tlastId / GameData.bsize);
	
				tlink = GameObject.Find ("linkr" + tx + "_" + ty);
				tlink.GetComponent<SpriteRenderer> ().color = GameData.instance.colors [0];
				tlink = GameObject.Find ("linkl" + tx + "_" + ty);
				tlink.GetComponent<SpriteRenderer> ().color = GameData.instance.colors [0];
				tlink = GameObject.Find ("linku" + tx + "_" + ty);
				tlink.GetComponent<SpriteRenderer> ().color = GameData.instance.colors [0];
				tlink = GameObject.Find ("linkd" + tx + "_" + ty);
				tlink.GetComponent<SpriteRenderer> ().color = GameData.instance.colors [0];
			}


            GameData.instance.linkedLines[colorId] = 0;//reopen the linkage
        }

   
		void checkWin(){
			int nwin = 0;
			for (int k = 0; k < GameData.instance.linkedLines.Length; k++) {
				if (GameData.instance.linkedLines [k] == 1) {
					nwin++;
				}

			}
            //print(nwin + "_____" + GameData.instance.winLinkCount);
			if(nwin >= GameData.instance.winLinkCount){//enough linkage
				GameData.instance.isHolding = false;
				GameData.instance.isWin = true;
                GameObject reciever = GameObject.Find("linkdot");
                if (reciever != null)
                {
                    reciever.GetComponent<MainScript>().gameWin();
                    print("game win!!");
                }


                else
                {
                    GameObject[] allgameObject = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
                    foreach(GameObject g in allgameObject)
                    {
                        g.BroadcastMessage("linkDotWin",SendMessageOptions.DontRequireReceiver);
                    }
                }
                
			}
		}

		
	}
}
