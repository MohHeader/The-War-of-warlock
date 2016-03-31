using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;




public class NodeObject : Object
{

	[MenuItem( "Assets/Create/Wuxingogo/NodeObject" )]
	static void init()
	{
		var no = new NodeObject();
		AssetDatabase.CreateAsset( no, GetAssetBrowserPath() + "wuxingogo.n" );
		
	}

	static string GetAssetBrowserPath()
	{
		string path = AssetDatabase.GetAssetPath( Selection.activeObject );
		if( path == "" ) {
			path = "Assets";
		} else if( Path.GetExtension( path ) != "" ) {
			path = path.Replace( Path.GetFileName( AssetDatabase.GetAssetPath( Selection.activeObject ) ), "" );
		}
		return path;
	}
}


