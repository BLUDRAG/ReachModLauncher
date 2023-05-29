using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace ReachModLauncher
{
    public class POIEntry : MonoBehaviour
    {
        public POIData  Data;
        public TextLink Link;
        public Image    Preview;

        public void Init(POIData data)
        {
            Data           = data;
            Link.Text.text = Path.GetFileName(Data.File.Name);
            Link.Link      = Data.File.WebContentLink;

            if(Data.Preview is not null)
            {
                Texture2D tex = new Texture2D(1, 1);
                tex.LoadImage(Data.Preview);

                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                                              new Vector2(tex.width / 2f, tex.height / 2f));
                
                Preview.sprite = sprite;
            }
        }
    }
}
