using Palmmedia.ReportGenerator.Core.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class GameManager : MonoBehaviour
{
    public string ApiLink;


    private void Start()
    {
        StartCoroutine(GetRequest(ApiLink));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    List<BlockData> allBlockDatas = JsonConvert.DeserializeObject<List<BlockData>>(webRequest.downloadHandler.text);
                    StackManager.Instance.CreateStacks(allBlockDatas);
                    break;
            }
        }
    }
}

[System.Serializable]
public class BlockData
{
    public int id;
    public string subject;
    public string grade;
    public int mastery;
    public string domainid;
    public string domain;
    public string cluster;
    public string standardid;
    public string standarddescription;
}