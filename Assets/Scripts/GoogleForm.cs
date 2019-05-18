using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoogleForm : MonoBehaviour
{
    public GameObject username;


    private string Name;


    [SerializeField]
    private string BASE_URL = "https://docs.google.com/forms/d/e/1FAIpQLSdrKsrpWHxVqjlmWZfemZSlgonSQ3uty7MVNmIfgfUugPHvOw/formResponse";

    IEnumerator Post(string name)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.745039506", name);
        byte[] rawData = form.data;
        WWW www = new WWW(BASE_URL, rawData);
        yield return www;
    }


    public void Send()
    {

        Name = username.GetComponent<InputField>().text;

        Debug.Log("Information sent " + Name);
        StartCoroutine(Post(Name));

    }
}
