using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace nukebox
{
	/// <summary>
	/// Controls OnMouse related functionalities on each dot and creates new link based on that
	/// </summary>
    public class DotsController : MonoBehaviour
    {
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		private int tx, ty;
		int id;
		bool colorPath = false;

		#region OnMouse Methods

		void OnMouseDown()
		{
			HandleOnMouseDown();
		}

		private void OnMouseOver()
		{
			HandleOnMouseOver();
		}

		private void OnMouseUp()
		{
			HandleOnMouseUp();
		}

		#endregion

		#region Public Methods

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
			id = ty * Config.rowCount + tx;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Handles functionalities for OnMouseDown event
		/// </summary>
		private void HandleOnMouseDown()
		{
			if (Config.isWin)
				return;
			if (Config.isHolding)
				return;

			Debug.Log("DotsController, OnMouseDown");

			int tdotColor = Config.DotColorData[id];
			if (tdotColor != 0)
			{

				ClearOldPath(tdotColor);

				Config.pickColor = tdotColor;

				Color tcolor = Config.colors[tdotColor];
				tcolor.a = .5f;
				if (colorPath)
				{
					spriteRenderer.color = tcolor;
				}
				Config.ColorData[id] = tdotColor;

				Config.paths[tdotColor] = new List<int>();

				AddPath(tdotColor, id);
			}
			else
			{
				int cBlockColor = Config.ColorData[id];

				if (cBlockColor == 0)
				{
					return;
				}
				else
				{
					Config.pickColor = cBlockColor;
					Config.linkedLines[cBlockColor] = 0;
					for (int i = Config.paths[cBlockColor].Count - 1; i > 0; i--)
					{
						int oldid = Config.paths[cBlockColor][i];
						if (oldid != id)
						{
							int oldx = Mathf.FloorToInt(oldid % Config.rowCount);
							int oldy = Mathf.FloorToInt(oldid / Config.rowCount);

							DotsController oldBg = Config.dotsDict["bg" + oldx + "_" + oldy];

							Color tcolor = Config.colors[0];
							oldBg.GetSpriteRenderer().color = tcolor;
							Config.ColorData[oldid] = 0;

							RemovePath(cBlockColor, i);
							AddPath(cBlockColor, id);
						}
						else
						{
							break;
						}
					}

				}
			}

			Config.isHolding = true;
			Config.startId = id;
			Config.lasttx = tx;
			Config.lastty = ty;
			CheckForWin();

		}

		/// <summary>
		/// Handles functionalities for OnMouseUp event
		/// </summary>
		private void HandleOnMouseUp()
		{

			if (Config.isWin)
				return;
			Config.isHolding = false;

			Debug.Log("DotsController, OnMouseUp");
		}

		/// <summary>
		/// Handles functionalities for OnMouseOver event
		/// </summary>
		private void HandleOnMouseOver()
		{

			if (Config.isWin)
				return;
			if (Config.isHolding)
			{

				if (Config.pickColor != 0)
				{

					int tdotColor = Config.DotColorData[id];
					int tColorid = Config.ColorData[id];


					if ((tdotColor == 0 && tColorid == 0) || Config.pickColor == tdotColor && Config.paths[tdotColor][0] != id)
					{

						if ((Mathf.Abs(tx - Config.lasttx) == 1 && ty == Config.lastty) ||
							(Mathf.Abs(ty - Config.lastty) == 1 && tx == Config.lasttx))
						{

							SetColor();
						}
					}
					else
					{

						if (tColorid != 0)
						{
							int tlen = Config.paths[tColorid].Count;

							if ((Mathf.Abs(tx - Config.lasttx) == 1 && ty == Config.lastty) ||
								(Mathf.Abs(ty - Config.lastty) == 1 && tx == Config.lasttx))
							{
								if (tColorid == Config.pickColor)
								{

									int oldId = Config.paths[tColorid][Config.paths[tColorid].Count - 1];
									while (oldId != id)
									{
										int oldx = Mathf.FloorToInt(oldId % Config.rowCount);
										int oldy = Mathf.FloorToInt(oldId / Config.rowCount);
										DotsController oldBg = Config.dotsDict["bg" + oldx + "_" + oldy];

										Color tcolor = Config.colors[0];
										oldBg.GetSpriteRenderer().color = tcolor;
										RemovePath(tColorid, Config.paths[tColorid].Count - 1);
										Config.ColorData[oldId] = 0;
										oldId = Config.paths[tColorid][Config.paths[tColorid].Count - 1];
										Config.lasttx = tx;
										Config.lastty = ty;
									}

								}
								else
								{

									int tOtherColorId = Config.ColorData[id];

									int oldId = Config.paths[tOtherColorId][Config.paths[tOtherColorId].Count - 1];

									if (Config.DotColorData[oldId] == 0 || tOtherColorId != Config.pickColor)
									{
										if (Config.DotColorData[id] == 0)
										{
											while (oldId != id)
											{
												int oldx = Mathf.FloorToInt(oldId % Config.rowCount);
												int oldy = Mathf.FloorToInt(oldId / Config.rowCount);
												DotsController oldBg = Config.dotsDict["bg" + oldx + "_" + oldy];

												Color tcolor = Config.colors[0];
												oldBg.GetSpriteRenderer().color = tcolor;
												RemovePath(tOtherColorId, Config.paths[tOtherColorId].Count - 1);
												Config.ColorData[oldId] = 0;
												oldId = Config.paths[tOtherColorId][Config.paths[tOtherColorId].Count - 1];

											}

											if (oldId == id)
											{
												int oldx = Mathf.FloorToInt(oldId % Config.rowCount);
												int oldy = Mathf.FloorToInt(oldId / Config.rowCount);
												DotsController oldBg = Config.dotsDict["bg" + oldx + "_" + oldy];

												Color tcolor = Config.colors[0];
												oldBg.GetSpriteRenderer().color = tcolor;
												RemovePath(tOtherColorId, Config.paths[tOtherColorId].Count - 1);
												Config.ColorData[oldId] = 0;
											}
										}
									}
								}
							}
						}
					}
				}
				CheckForWin();
			}
		}



		/// <summary>
		/// Clears old path on clicking  old dot or being intersected by other link
		/// </summary>
		/// <param name="tcolorid"></param>
		private void ClearOldPath(int tcolorid)
		{

			int tlen = Config.paths[tcolorid].Count;

			while (Config.paths[tcolorid].Count > 0)
			{
				int oldid = Config.paths[tcolorid][Config.paths[tcolorid].Count - 1];

				int oldx = Mathf.FloorToInt(oldid % Config.rowCount);
				int oldy = Mathf.FloorToInt(oldid / Config.rowCount);

				DotsController oldBg = Config.dotsDict["bg" + oldx + "_" + oldy];

				Color tcolor = Config.colors[0];
				oldBg.GetSpriteRenderer().color = tcolor;
				Config.ColorData[oldid] = 0;

				RemovePath(tcolorid, Config.paths[tcolorid].Count - 1);
				Config.linkedLines[tcolorid] = 0;

			}
		}


		/// <summary>
		/// Setting color to link
		/// </summary>
		private void SetColor()
		{
			int tdotColor = Config.DotColorData[id];
			if ((tdotColor != 0 && tdotColor != Config.pickColor))
			{
				Config.isHolding = false;
				Config.pickColor = 0;
				return;
			}

			Color tcolor = Config.colors[Config.pickColor];
			tcolor.a = .5f;
			if (colorPath)
			{
				transform.GetComponent<SpriteRenderer>().color = tcolor;
			}

			AddPath(Config.pickColor, id);
			Config.ColorData[id] = Config.pickColor;

			if (tdotColor != 0 && tdotColor == Config.pickColor && Config.paths[tdotColor].Count > 1)
			{
				Config.linkedLines[tdotColor] = 1;
				Config.pickColor = 0;
			}

			Config.lasttx = tx;
			Config.lastty = ty;
		}


		/// <summary>
		/// Add link based on color and index
		/// </summary>
		/// <param name="colorId"></param>
		/// <param name="placeId"></param>
		void AddPath(int colorId, int placeId)
		{

			Debug.Log("DotsController, AddPath");
			EventManager.TriggerEvent(EventID.Event_AddPath); //Triggering AddPath Event

			Config.paths[colorId].Add(placeId);
			int tlen = Config.paths[colorId].Count;
			if (tlen > 1)
			{
				int tlastId1 = Config.paths[colorId][tlen - 2];
				int tlastId2 = Config.paths[colorId][tlen - 1];


				int tx = Mathf.FloorToInt(tlastId1 % Config.rowCount);
				int ty = Mathf.FloorToInt(tlastId1 / Config.rowCount);

				int placeOffset = tlastId2 - tlastId1;

				int tRight = 1;
				int tLeft = -1;
				int tUp = Config.rowCount;
				int tDown = -Config.rowCount;
				SpriteRenderer tlink = null;
				if (placeOffset == tRight)
				{
					tlink = Config.linDict["linkr" + tx + "_" + ty];
				}
				else if (placeOffset == tLeft)
				{
					tlink = Config.linDict["linkl" + tx + "_" + ty];
				}
				else if (placeOffset == tUp)
				{
					tlink = Config.linDict["linku" + tx + "_" + ty];
				}
				else if (placeOffset == tDown)
				{
					tlink = Config.linDict["linkd" + tx + "_" + ty];
				}
				if (tlink != null)
				{
					tlink.color = Config.colors[colorId];
				}
			}
		}

		/// <summary>
		/// Removes link based on index and color
		/// </summary>
		/// <param name="colorId"></param>
		/// <param name="index"></param>
		private void RemovePath(int colorId, int index)
		{
			int tlastId = Config.paths[colorId][index];
			int tx = Mathf.FloorToInt(tlastId % Config.rowCount);
			int ty = Mathf.FloorToInt(tlastId / Config.rowCount);

			SpriteRenderer tlink = null;
			tlink = Config.linDict["linkr" + tx + "_" + ty];
			tlink.color = Config.colors[0];
			tlink = Config.linDict["linkl" + tx + "_" + ty];
			tlink.color = Config.colors[0];
			tlink = Config.linDict["linku" + tx + "_" + ty];
			tlink.color = Config.colors[0];
			tlink = Config.linDict["linkd" + tx + "_" + ty];
			tlink.color = Config.colors[0];

			Config.paths[colorId].RemoveAt(index);

			if (index > 0)
			{
				tlastId = Config.paths[colorId][index - 1];
				tx = Mathf.FloorToInt(tlastId % Config.rowCount);
				ty = Mathf.FloorToInt(tlastId / Config.rowCount);

				tlink = Config.linDict["linkr" + tx + "_" + ty];
				tlink.color = Config.colors[0];
				tlink = Config.linDict["linkl" + tx + "_" + ty];
				tlink.color = Config.colors[0];
				tlink = Config.linDict["linku" + tx + "_" + ty];
				tlink.color = Config.colors[0];
				tlink = Config.linDict["linkd" + tx + "_" + ty];
				tlink.color = Config.colors[0];
			}

			Config.linkedLines[colorId] = 0;
		}

		/// <summary>
		///  Checks for win. If win triggers GameOver Event
		/// </summary>
		private void CheckForWin()
		{
			int nwin = 0;
			for (int k = 0; k < Config.linkedLines.Length; k++)
			{
				if (Config.linkedLines[k] == 1)
				{
					nwin++;
				}

			}

			if (nwin >= Config.winLinkCount)
			{
				Config.isHolding = false;
				Config.isWin = true;
				EventManager.TriggerEvent(EventID.Event_GameOver); //Triggering GAmeOver event on game win
			}
		}

		#endregion


	}

	
}

