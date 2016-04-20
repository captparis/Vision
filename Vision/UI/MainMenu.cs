using UnityEngine;
using System.Collections;
using System;

namespace Vision.Menu
{
	public class MainMenu : MonoBehaviour
	{
		[System.Serializable]
		public struct LevelDisplayStruct
		{
			public string Name;
			public Texture2D Image;
			public string Desc;
		};
		public LevelDisplayStruct[] LevelInfo;

		public int Rows;
		public int Columns;
		public float PaddingR;
		public float PaddingC;
		//public LevelDisplayStruct[] LevelInfo;
		public Rect LevelBoxDim;
		float RowHeight;
		float ColWidth;

		// Use this for initialization
		void Start()
		{
			ColWidth = (LevelBoxDim.width - (Columns - 1) * PaddingC) / Columns;
			RowHeight = (LevelBoxDim.height - (Columns - 1) * PaddingR) / Rows;
		}
		
		void OnGUI()
		{
			float Rowinc = LevelBoxDim.y;
			float Rownum = 1;
			float Colinc = LevelBoxDim.x;
			float Colnum = 1;
			LevelManager LM = LevelManager.LM;

			for (int i = 0; i < LM.LevelDataTable.Length; i++)
			{
				LevelManager.LevelData Level = LM.LevelDataTable[i];
				if (Level.locked || Level.name == "")
				{
					GUI.enabled = false;
				}
				GUI.Label(new Rect(Colinc + 20, Rowinc + (RowHeight - 50), ColWidth - 40, 30), LevelInfo[i].Desc);
				if (GUI.Button(new Rect(Colinc, Rowinc, ColWidth, RowHeight), LevelInfo[i].Image))
				{
					LM.LoadLevel(i);
				}
				Colinc += ColWidth + PaddingC;
				Colnum++;
				if (Colnum > Columns)
				{
					Rownum++;
					Rowinc += RowHeight + PaddingR;
					Colinc = LevelBoxDim.x;
					Colnum = 1;
				}
				if (!GUI.enabled)
				{
					GUI.enabled = true;
				}
			}
			if (GUI.Button(new Rect(0, 0, 100, 50), "Reset Progress"))
			{
				LM.ResetLevelInfo();
			}
		}
	}
}