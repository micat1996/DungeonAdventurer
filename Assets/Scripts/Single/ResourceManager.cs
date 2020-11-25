using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

// 게임 실행중 로드한 리소스들을 관리하는 클래스입니다.
public sealed class ResourceManager : ManagerClassBase<ResourceManager>
{
	// Json 파일을 저장하기 위한 경로를 저장할 변수입니다.
	private string _JsonFolderPath;

	private Dictionary<string, Object> _LoadedResources;

	public Object this[string resourceName] => _LoadedResources[resourceName];

	public List<ItemInfo> items;


	public override void InitializeManagerClass()
	{
		_LoadedResources = new Dictionary<string, Object>();
		_JsonFolderPath = $"{Application.dataPath}/Resources/Json/";
	}

	// 특정한 형식으로 리소스를 로드하여 반환합니다.
	public T LoadResource<T>(string resourceName, string resourcePath = null) where T : Object
	{
		// 만약 이미 resourceName 으로 되어있는 리소스가 로드되어있다면
		if (_LoadedResources.ContainsKey(resourceName))
			return _LoadedResources[resourceName] as T;

		// 만약 로드되어있지 않다면
		else
		{
			// 리소스를 로드합니다.
			T loadedResource = Resources.Load<T>(resourcePath);

			// 만약 리소스가 로드 되었다면
			if (loadedResource)
			{
				_LoadedResources.Add(resourceName, loadedResource);
				return loadedResource as T;
			}
			// 만약 리소스가 로드되지 않았다면
			else
			{
				// 에디터의 경우에만 로그를 띄웁니다.
#if UNITY_EDITOR
				Debug.LogError($"{resourceName} is not loaded! (path : {resourcePath})\n");
#endif

				// 정상적으로 로드되지 않았으므로 null 을 반환합니다.
				return null;
			}
		}
	}



	// Json 파일을 읽습니다.
	public T LoadJson<T>(string filePath, out bool fileNotFound) where T : struct
	{
		string jsonData = null;

		try
		{
			jsonData = File.ReadAllText(_JsonFolderPath + filePath);
		}
		catch (DirectoryNotFoundException)
		{
			fileNotFound = true;
			return new T();
		}
		catch (FileNotFoundException)
		{
			fileNotFound = true;
			return new T();
		}

		fileNotFound = false;
		return JsonUtility.FromJson<T>(jsonData);
	}

	// json 파일로 저장합니다.
	public void SaveJson<T>(T data, string folderPath, string fileName) where T : struct
	{
		Directory.CreateDirectory(_JsonFolderPath + folderPath);

		string jsonString = JsonUtility.ToJson(data, true);
		File.WriteAllText(_JsonFolderPath + folderPath + "/" + fileName, jsonString);
	}


	private void Awake()
	{
		string[] itemCodes =
		{
			"10000",
			"10001",
			"10002",

			"11000",
			"11001",
			"11002",
			"11003",
			"11004",
			"11005",
			"11006",

			"12000",
			"12001",
			"12002",
			"12003",

			"13000",
			"13001",
			"13002",
			"13003",
			"13004",
			"13005",

			"14000",
			"14001",
			"14002",
			"14003",
			"14004",

			"15000",
			"15001",
			"15002",
			"15003",
			"15004",
			"15005",

			"16000",
			"16001",
			"16002",
			"16003",
			"16004",
			"16005",
			"16006",
			"16007",
			"16008",
			"16009",

			"17000",
			"17001",
			"17002",
			"17003",
			"17004",
			"17005",

			// 가방
			"18000",
			"18001",
			"18002",

			// 검
			"19000",
			"19001",
			"19002",
			"19003",
		};

		foreach (string code in itemCodes)
		{
			bool filenotfound;
			items.Add(LoadJson<ItemInfo>($"ItemInfos/{code}.json", out filenotfound));
		}
		
	}

	private void OnApplicationQuit()
	{
		foreach (var i in items)
			SaveJson(i, "ItemInfos", $"{i.itemCode}.json");
	}


}
