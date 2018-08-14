using UnityEngine;
using System.Collections;
using System.IO;

public class TakeImage : MonoBehaviour
{
    //public int captureWidth = 1920;
    //public int captureHeight = 1080;
    
    // configure with raw, jpg, png, or ppm (simple raw format)
    public enum Format { RAW, JPG, PNG, PPM };
    public Format format = Format.PNG;

    // folder to write output (defaults to data path)
    public string folder;

    // private vars for screenshot
    private Rect rect;
    private Texture2D screenShot;
    private int counter = 0; // image #

    // create a unique filename using a one-up variable
    private string uniqueFileName()
    {
        // if folder not specified by now use a good default
        if (folder == null || folder.Length == 0)
        {
            folder = Application.dataPath;
            if (Application.isEditor)
            {
                // put screenshots in folder above asset path so unity doesn't index the files
                var stringPath = folder + "/..";
                folder = Path.GetFullPath(stringPath);
            }
            folder += "/Screenshots";

            // make sure directoroy exists
            System.IO.Directory.CreateDirectory(folder);

            // count number of files of specified format in folder
            string mask = string.Format("Image_*{0}", format.ToString().ToLower());
            counter = Directory.GetFiles(folder, mask, SearchOption.TopDirectoryOnly).Length;
        }

        // use width, height, and counter for unique file name
        var filename = string.Format("{0}/Image_{1}.{2}", folder, counter, format.ToString().ToLower());

        // up counter for next call
        counter++;

        // return unique filename
        return filename;
    }

    public void CaptureScreenshot(RenderTexture r)
    {
        rect = new Rect(0, 0, r.width, r.height);
        screenShot = new Texture2D(r.width, r.height, TextureFormat.RGB24, false);

        // read pixels will read from the currently active render texture so make our offscreen 
        // render texture active and then read the pixels
        RenderTexture.active = r;
        screenShot.ReadPixels(rect, 0, 0);

        // reset render texture
        RenderTexture.active = null;

        // get our unique filename
        string filename = uniqueFileName();

        // pull in our file header/data bytes for the specified image format (has to be done from main thread)
        byte[] fileHeader = null;
        byte[] fileData = null;
        if (format == Format.RAW)
        {
            fileData = screenShot.GetRawTextureData();
        }
        else if (format == Format.PNG)
        {
            fileData = screenShot.EncodeToPNG();
        }
        else if (format == Format.JPG)
        {
            fileData = screenShot.EncodeToJPG();
        }
        else // PPM
        {
            // create a file header for PPM formatted file
            string headerStr = string.Format("P6\n{0} {1}\n255\n", rect.width, rect.height);
            fileHeader = System.Text.Encoding.ASCII.GetBytes(headerStr);
            fileData = screenShot.GetRawTextureData();
        }

        // create new thread to save the image to file (only operation that can be done in background)
        new System.Threading.Thread(() =>
        {
            // create file and write optional header with image bytes
            var f = System.IO.File.Create(filename);
            if (fileHeader != null) f.Write(fileHeader, 0, fileHeader.Length);
            f.Write(fileData, 0, fileData.Length);
            f.Close();
            //Debug.Log(string.Format("Wrote screenshot {0} of size {1}", filename, fileData.Length));
        }).Start();

        // cleanup
        screenShot = null;
    }
    
    void Update()
    {

    }
}
