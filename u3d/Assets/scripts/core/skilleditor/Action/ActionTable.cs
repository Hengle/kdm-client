
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


//	ActionTable.cs
//	Author: Lu Zexi
//	2015-06-03




public class ActionTable
{
	public ActionObject m_ActionObject = null;

	public void CopyFrom( ActionObject src )
    {
		// m_ActionObjects = new List<ActionObject>();
		// orderingActionTable	= src.orderingActionTable;
		// m_IsLookTarget = src.m_IsLookTarget;
		m_ActionObject = new ActionObject();
		m_ActionObject.CopyFrom( src );
    }

    public void Init()
    {
    	m_ActionObject = new ActionObject();
    	// m_IsLookTarget = false;
    }

#if UNITY_EDITOR

	public GameObject previewCharacterSource;
	public ActionCustomObject player;
	public void Draw()
	{
		GUILayout.BeginVertical();
		{
			previewCharacterSource = (GameObject)EditorGUILayout.ObjectField( previewCharacterSource, typeof(GameObject), false);

			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Create"))
			{
				if(this.player != null)
				{
					GameObject.Destroy(this.player.gameObject);
					this.player = null;
				}
				this.player = (GameObject.Instantiate(previewCharacterSource) as GameObject).AddComponent<ActionCustomObject>();
				GameObject cam = GameObject.Find("Main Camera");
				if(cam == null)
				{
					cam = new GameObject("Main Camera");
				}
				TempSkillEditorCamera game_c = cam.GetComponent<TempSkillEditorCamera>();
				game_c.m_Target = this.player.transform;
				// GameObject ingameObj = GameObject.Instantiate(Resources.Load("Scene/InGame")) as GameObject;
				// InGameManager.I.GameCamera = game_c.GetComponent<Camera>();
			}
			if (GUILayout.Button("Reset"))
			{
				this.player.transform.localPosition = Vector3.zero;
				this.player.transform.localRotation = Quaternion.identity;
			}
			if (GUILayout.Button("Play"))
			{
				if(this.player != null)
				{
					// ActionExcute.Create(this , this.player);
					this.player.transform.localPosition = Vector3.zero;
					this.player.transform.localRotation = Quaternion.identity;
					// this.player.PlayAction(this);
					ActionExcuteManager.instance.StartAction(m_ActionObject,this.player);
				}
			}
			GUILayout.EndHorizontal();
		}
		{
			GUILayout.BeginVertical();
			if(m_ActionObject != null)
			{
				m_ActionObject.Draw(this);
			}
			GUILayout.EndVertical();

			// GUILayout.BeginVertical();
			// if(m_ActionObjects != null)
			// {
			// 	for( int i = 0 ; i<this.m_ActionObjects.Count ; i++ )
			// 	{
			// 		ActionObject ao = this.m_ActionObjects[i];
			// 		ao.Draw( this );
			// 	}
			// }
			// GUILayout.EndVertical();
		}
		GUILayout.EndVertical();
	}

	public void Read(string path)
    {
    	FileStream fs = new FileStream(path, FileMode.Open);
        BinaryReader br = new BinaryReader(fs);
        m_ActionObject = new ActionObject();
        m_ActionObject.Read(br);
        br.Close();
    }

    public void Write(string path)
    {
    	FileStream fs = new FileStream(path, FileMode.Create);
        BinaryWriter bw = new BinaryWriter(fs);
        m_ActionObject.Write(bw);
        bw.Close();
        fs.Close();
        AssetDatabase.Refresh();
    }

    public void OnLoad( string path )
    {
    	string json_str = File.ReadAllText(path);
    	object obj = MiniJSON.Json.Deserialize(json_str);
    	m_ActionObject = new ActionObject();
        m_ActionObject.ReadJson((Dictionary<string,object>)obj);
    }

    public void OnSave( string path )
    {
    	Dictionary<string, object> obj = new Dictionary<string, object>();
    	m_ActionObject.WriteJson(obj);
    	string json_str = MiniJSON.Json.Serialize(obj);
    	File.WriteAllText(path,json_str);
    	AssetDatabase.Refresh();
    }

#endif
}
