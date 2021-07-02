using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class TexturesFromWeb : MonoBehaviour
{
    public GameObject cube;
    public Sprite spriteRenderer;
    public Image image;
    public RawImage rawImage;

    public List<InputField> texUrlInput = new List<InputField>();

    public GameObject urlFieldPrefabParent;
    public GameObject urlFieldPrefab;

    int textureCounter = 0;
    bool textureLoaded = false;
    Texture downloadedTexture;
    Texture2D optimizedTexture;

    public void StartDownloadingTextures()
    {
        textureCounter = 0;
        StartCoroutine(getAllTextures());
    }

    IEnumerator getAllTextures()
    {
        while (textureCounter < texUrlInput.Count)
        {
            setTextureFromWeb(texUrlInput[textureCounter]);
            textureCounter++;

            yield return new WaitForSeconds(4.0f);
        }

        yield return new WaitForEndOfFrame();
    }

    public void AddField()
    {
        GameObject obj = Instantiate(urlFieldPrefab, urlFieldPrefabParent.transform.position, Quaternion.identity);
        obj.transform.SetParent(urlFieldPrefabParent.transform);
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;

        obj.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { DeleteField(obj); });

        texUrlInput.Add(obj.GetComponent<InputField>());
    }

    public void DeleteField(GameObject toDelete)
    {
        texUrlInput.Remove(toDelete.GetComponent<InputField>());
        Destroy(toDelete);
    }

    public void setTextureFromWeb(InputField inputText)
    {
        string textureurl = inputText.text;
        StartCoroutine(DownloadTexture(textureurl));
    }

    IEnumerator DownloadTexture(string textureurl)
    {
        Debug.Log("Loading... " + textureurl);

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(textureurl);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            if (downloadedTexture != null)
                Destroy(downloadedTexture);

            downloadedTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            downloadedTexture.name = "MyTexture";
            optimizedTexture = ((Texture2D)downloadedTexture);
            optimizedTexture.Compress(false);
            ApplyTextureToCube();
            ApplyTextureToImage();
            ApplyTextureToRawImage();
            textureLoaded = true;
        }
    }

    private void ApplyTextureToRawImage()
    {
        rawImage.texture = optimizedTexture;
    }

    private void ApplyTextureToImage()
    {
        Sprite imageSprite = Sprite.Create(optimizedTexture, new Rect(0, 0, optimizedTexture.width, optimizedTexture.height), new Vector2(0, 0));
        image.sprite = imageSprite;
    }

    void ApplyTextureToCube()
    {
        cube.GetComponent<Renderer>().material.mainTexture = optimizedTexture;
    }
}