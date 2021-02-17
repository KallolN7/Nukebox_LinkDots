using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace nukebox
{
    public class DotsController : MonoBehaviour
    {
		public SpriteRenderer spriteRenderer;

		private int tx, ty;
		// Use this for initialization
		int id;
		void Start()
		{

			id = ty * GameManager.bsize + tx;


		}

		public SpriteRenderer GetSpriteRenderer()
        {
			return spriteRenderer;
        }

		public int GetTx()
        {
			return tx;
        }

		public int GetTy()
        {
			return ty;
        }

		public void SetData(int tx, int ty)
        {
			this.tx = tx;
			this.ty = ty;
        }

		bool colorPath = false;

		void OnMouseDown()
		{
			Debug.Log("DotsController, OnMouseDown");

			if (GameManager.instance.isWin)
				return;
			if (GameManager.instance.isHolding)
				return;



		//	Transform dotChild = transform.Find("dot");

			int tdotColor = GameManager.instance.DotColorData[id];
			if (tdotColor != 0)
			{//got a dot here
				if (GameManager.instance.tipUsed > 0 && tdotColor <= GameManager.instance.tipUsed)
				{
					return;
				}
				clearOldPath(tdotColor);

				GameManager.instance.pickColor = tdotColor;
				//				print ("firstpick:dot:" + tdotColor);

				Color tcolor = GameManager.instance.colors[tdotColor];
				tcolor.a = .5f;
				if (colorPath)
				{
					transform.GetComponent<SpriteRenderer>().color = tcolor;
				}
				GameManager.instance.ColorData[id] = tdotColor;

				GameManager.instance.paths[tdotColor] = new List<int>();//overwrite the old path
																	 //				GameManager.instance.paths[tdotColor].Add (id);
				addPath(tdotColor, id);
			}
			else
			{
				int cBlockColor = GameManager.instance.ColorData[id];
				//				print("cblockcolor"+cBlockColor);
				if (cBlockColor == 0)
				{//tap on a empty block,nothing should happen
					return;
				}
				else
				{
					if (GameManager.instance.tipUsed > 0 && cBlockColor <= GameManager.instance.tipUsed)
					{
						return;
					}


					//continue drawing on an already path block
					GameManager.instance.pickColor = cBlockColor;
					//reopen the linkage
					GameManager.instance.linkedLines[cBlockColor] = 0;
					for (int i = GameManager.instance.paths[cBlockColor].Count - 1; i > 0; i--)
					{
						int oldid = GameManager.instance.paths[cBlockColor][i];
						if (oldid != id)
						{//remove all paths until find the start one
						 //and clear the useless path
							int oldx = Mathf.FloorToInt(oldid % GameManager.bsize);
							int oldy = Mathf.FloorToInt(oldid / GameManager.bsize);

							GameObject oldBg = GameObject.Find("bg" + oldx + "_" + oldy);

							Color tcolor = GameManager.instance.colors[0];
							//tcolor.a = 1f;
							oldBg.transform.GetComponent<SpriteRenderer>().color = tcolor;
							GameManager.instance.ColorData[oldid] = 0;


							//							GameManager.instance.paths[cBlockColor].RemoveAt (i);
							removePath(cBlockColor, i);
							//							GameManager.instance.paths[cBlockColor].Add (id);
							addPath(cBlockColor, id);


						}
						else
						{
							break;
						}

					}

				}
			}




			GameManager.instance.isHolding = true;
			GameManager.instance.startId = id;
			GameManager.instance.lasttx = tx;
			GameManager.instance.lastty = ty;

			checkWin();

		}


		void clearOldPath(int tcolorid)
		{

			int tlen = GameManager.instance.paths[tcolorid].Count;



			while (GameManager.instance.paths[tcolorid].Count > 0)
			{
				int oldid = GameManager.instance.paths[tcolorid][GameManager.instance.paths[tcolorid].Count - 1];


				//and clear the old path
				int oldx = Mathf.FloorToInt(oldid % GameManager.bsize);
				int oldy = Mathf.FloorToInt(oldid / GameManager.bsize);

				GameObject oldBg = GameObject.Find("bg" + oldx + "_" + oldy);

				Color tcolor = GameManager.instance.colors[0];
				oldBg.transform.GetComponent<SpriteRenderer>().color = tcolor;
				GameManager.instance.ColorData[oldid] = 0;


				//				GameManager.instance.paths[tcolorid].RemoveAt(GameManager.instance.paths[tcolorid].Count-1);
				removePath(tcolorid, GameManager.instance.paths[tcolorid].Count - 1);
				//reopen the linkage
				GameManager.instance.linkedLines[tcolorid] = 0;


			}
		}



		void OnMouseOver()
		{
			Debug.Log("DotsController, OnMouseOver");

			if (GameManager.instance.isWin)
				return;
			if (GameManager.instance.isHolding)
			{




				if (GameManager.instance.pickColor != 0)
				{
					//if dot here,get dot color
					//Transform dotChild = transform.Find("dot");

					int tdotColor = GameManager.instance.DotColorData[id];
					//current block color;
					int tColorid = GameManager.instance.ColorData[id];


					if ((tdotColor == 0 && tColorid == 0) || GameManager.instance.pickColor == tdotColor && GameManager.instance.paths[tdotColor][0] != id)
					{//all places which can be draw and have not draw on something

						//exclude not nearby blocks
						if ((Mathf.Abs(tx - GameManager.instance.lasttx) == 1 && ty == GameManager.instance.lastty) ||
							(Mathf.Abs(ty - GameManager.instance.lastty) == 1 && tx == GameManager.instance.lasttx))
						{

							addColor();
						}
					}
					else
					{//draw on an already exist self color path

						if (tColorid != 0)
						{
							int tlen = GameManager.instance.paths[tColorid].Count;


							//exclude not nearby blocks
							if ((Mathf.Abs(tx - GameManager.instance.lasttx) == 1 && ty == GameManager.instance.lastty) ||
								(Mathf.Abs(ty - GameManager.instance.lastty) == 1 && tx == GameManager.instance.lasttx))
							{
								if (tColorid == GameManager.instance.pickColor)
								{//draw on self old blocks

									//and clear the old path
									int oldId = GameManager.instance.paths[tColorid][GameManager.instance.paths[tColorid].Count - 1];
									while (oldId != id)
									{
										int oldx = Mathf.FloorToInt(oldId % GameManager.bsize);
										int oldy = Mathf.FloorToInt(oldId / GameManager.bsize);
										GameObject oldBg = GameObject.Find("bg" + oldx + "_" + oldy);

										Color tcolor = GameManager.instance.colors[0];
										oldBg.transform.GetComponent<SpriteRenderer>().color = tcolor;
										removePath(tColorid, GameManager.instance.paths[tColorid].Count - 1);
										GameManager.instance.ColorData[oldId] = 0;

										//next prev id
										oldId = GameManager.instance.paths[tColorid][GameManager.instance.paths[tColorid].Count - 1];


										GameManager.instance.lasttx = tx;
										GameManager.instance.lastty = ty;


									}

								}
								else
								{//draw on other block lines,cut them
								 //clear the being cutted other color old paths


									int tOtherColorId = GameManager.instance.ColorData[id];



									if (GameManager.instance.tipUsed > 0 && tOtherColorId <= GameManager.instance.tipUsed)
									{
										return;
									}

									int oldId = GameManager.instance.paths[tOtherColorId][GameManager.instance.paths[tOtherColorId].Count - 1];

									if (GameManager.instance.DotColorData[oldId] == 0 || tOtherColorId != GameManager.instance.pickColor)
									{//if this place doesnt have a dot or this place is a different color
										if (GameManager.instance.DotColorData[id] == 0)
										{   //make wont draw on other color dots
											while (oldId != id)
											{

												int oldx = Mathf.FloorToInt(oldId % GameManager.bsize);
												int oldy = Mathf.FloorToInt(oldId / GameManager.bsize);
												GameObject oldBg = GameObject.Find("bg" + oldx + "_" + oldy);

												Color tcolor = GameManager.instance.colors[0];
												oldBg.transform.GetComponent<SpriteRenderer>().color = tcolor;
												//												GameManager.instance.paths [tOtherColorId].RemoveAt (GameManager.instance.paths [tOtherColorId].Count - 1);
												removePath(tOtherColorId, GameManager.instance.paths[tOtherColorId].Count - 1);
												GameManager.instance.ColorData[oldId] = 0;

												//next prev id
												oldId = GameManager.instance.paths[tOtherColorId][GameManager.instance.paths[tOtherColorId].Count - 1];

											}

											if (oldId == id)
											{
												int oldx = Mathf.FloorToInt(oldId % GameManager.bsize);
												int oldy = Mathf.FloorToInt(oldId / GameManager.bsize);
												GameObject oldBg = GameObject.Find("bg" + oldx + "_" + oldy);

												Color tcolor = GameManager.instance.colors[0];
												oldBg.transform.GetComponent<SpriteRenderer>().color = tcolor;
												//												GameManager.instance.paths [tOtherColorId].RemoveAt (GameManager.instance.paths [tOtherColorId].Count - 1);
												removePath(tOtherColorId, GameManager.instance.paths[tOtherColorId].Count - 1);
												GameManager.instance.ColorData[oldId] = 0;


											}
										}
									}




								}
							}
						}
					}

				}
				checkWin();
			}//if holding
		}



		void addColor()
		{
			int tdotColor = GameManager.instance.DotColorData[id];

			//			if (GameManager.instance.linkedLines [GameManager.instance.pickColor] == 1 && GameManager.instance.pickColor == GameManager.instance.ColorData[id])//this linkage is closed,unless the linkage being breaked or restart,wont be able to continue drawing
			//				return;

			if ((tdotColor != 0 && tdotColor != GameManager.instance.pickColor)//draw on other color dots(not block)

			)
			{
				GameManager.instance.isHolding = false;
				GameManager.instance.pickColor = 0;
				return;
			}




			Color tcolor = GameManager.instance.colors[GameManager.instance.pickColor];
			tcolor.a = .5f;
			if (colorPath)
			{
				transform.GetComponent<SpriteRenderer>().color = tcolor;
			}
			//			GameManager.instance.paths[GameManager.instance.pickColor].Add (id);
			addPath(GameManager.instance.pickColor, id);
			//			print ("added"+GameManager.instance.pickColor);

			GameManager.instance.ColorData[id] = GameManager.instance.pickColor;//write color to data


			if (tdotColor != 0 && tdotColor == GameManager.instance.pickColor && GameManager.instance.paths[tdotColor].Count > 1)
			{
				GameManager.instance.linkedLines[tdotColor] = 1;
				GameManager.instance.pickColor = 0;
			}


			GameManager.instance.lasttx = tx;
			GameManager.instance.lastty = ty;
		}


		void OnMouseUp()
		{
			Debug.Log("DotsController, OnMouseUp");

			if (GameManager.instance.isWin)
				return;
			GameManager.instance.isHolding = false;


		}


		static bool canPlatDotSfx = true;//make draw sound effect not be too frequent;
		IEnumerator sfxGap()
		{
			yield return new WaitForSeconds(.2f);
			canPlatDotSfx = true;
		}
		void addPath(int colorId, int placeId)
		{


			if (canPlatDotSfx)
			{
				string tsfx = "d" + Mathf.FloorToInt(Random.Range(0, 6));
				//GameManager.instance.playSfx(tsfx);
				canPlatDotSfx = false;
				StartCoroutine("sfxGap");
			}


			GameManager.instance.paths[colorId].Add(placeId);
			int tlen = GameManager.instance.paths[colorId].Count;
			if (tlen > 1)
			{
				int tlastId1 = GameManager.instance.paths[colorId][tlen - 2];
				int tlastId2 = GameManager.instance.paths[colorId][tlen - 1];


				int tx = Mathf.FloorToInt(tlastId1 % GameManager.bsize);
				int ty = Mathf.FloorToInt(tlastId1 / GameManager.bsize);

				int placeOffset = tlastId2 - tlastId1;
				//				print ("placeOffset" + placeOffset);

				int tRight = 1;
				int tLeft = -1;
				int tUp = GameManager.bsize;//paths go up
				int tDown = -GameManager.bsize;
				GameObject tlink = null;
				if (placeOffset == tRight)
				{//right
					tlink = GameObject.Find("linkr" + tx + "_" + ty);
				}
				else if (placeOffset == tLeft)
				{
					tlink = GameObject.Find("linkl" + tx + "_" + ty);
				}
				else if (placeOffset == tUp)
				{
					tlink = GameObject.Find("linku" + tx + "_" + ty);
				}
				else if (placeOffset == tDown)
				{
					tlink = GameObject.Find("linkd" + tx + "_" + ty);
				}
				if (tlink != null)
				{
					tlink.GetComponent<SpriteRenderer>().color = GameManager.instance.colors[colorId];
				}
			}

		}

		void removePath(int colorId, int index)
		{



			//clear all linkage on this node
			int tlastId = GameManager.instance.paths[colorId][index];
			int tx = Mathf.FloorToInt(tlastId % GameManager.bsize);
			int ty = Mathf.FloorToInt(tlastId / GameManager.bsize);




			GameObject tlink = null;
			tlink = GameObject.Find("linkr" + tx + "_" + ty);
			tlink.GetComponent<SpriteRenderer>().color = GameManager.instance.colors[0];
			tlink = GameObject.Find("linkl" + tx + "_" + ty);
			tlink.GetComponent<SpriteRenderer>().color = GameManager.instance.colors[0];
			tlink = GameObject.Find("linku" + tx + "_" + ty);
			tlink.GetComponent<SpriteRenderer>().color = GameManager.instance.colors[0];
			tlink = GameObject.Find("linkd" + tx + "_" + ty);
			tlink.GetComponent<SpriteRenderer>().color = GameManager.instance.colors[0];


			GameManager.instance.paths[colorId].RemoveAt(index);

			if (index > 0)
			{
				tlastId = GameManager.instance.paths[colorId][index - 1];
				tx = Mathf.FloorToInt(tlastId % GameManager.bsize);
				ty = Mathf.FloorToInt(tlastId / GameManager.bsize);

				tlink = GameObject.Find("linkr" + tx + "_" + ty);
				tlink.GetComponent<SpriteRenderer>().color = GameManager.instance.colors[0];
				tlink = GameObject.Find("linkl" + tx + "_" + ty);
				tlink.GetComponent<SpriteRenderer>().color = GameManager.instance.colors[0];
				tlink = GameObject.Find("linku" + tx + "_" + ty);
				tlink.GetComponent<SpriteRenderer>().color = GameManager.instance.colors[0];
				tlink = GameObject.Find("linkd" + tx + "_" + ty);
				tlink.GetComponent<SpriteRenderer>().color = GameManager.instance.colors[0];
			}


			GameManager.instance.linkedLines[colorId] = 0;//reopen the linkage
		}


		void checkWin()
		{
			int nwin = 0;
			for (int k = 0; k < GameManager.instance.linkedLines.Length; k++)
			{
				if (GameManager.instance.linkedLines[k] == 1)
				{
					nwin++;
				}

			}
			//print(nwin + "_____" + GameManager.instance.winLinkCount);
			if (nwin >= GameManager.instance.winLinkCount)
			{//enough linkage
				GameManager.instance.isHolding = false;
				GameManager.instance.isWin = true;
				//GameManager.instance.gameWin();
				print("game win!!");

			}
			else
			{
				GameObject[] allgameObject = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
				foreach (GameObject g in allgameObject)
				{
					g.BroadcastMessage("linkDotWin", SendMessageOptions.DontRequireReceiver);
				}
			}

		}
	}

	
}

