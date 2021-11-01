using System;
using System.IO;
using System.Text;
using Modding;

namespace CompletionPercentage
{
    public class CompletionPercentage: Mod
    {
        private float _completionPercentage = 0;
        private const string _version = "1.0.0";
        
        public override string GetVersion()
        {
            return _version;
        }
        
        public override void Initialize()
        {
            ModHooks.Instance.SetPlayerBoolHook += PlayerBoolSet;
        }
        
        public void PlayerBoolSet(string target, bool value)
        {
            PlayerData.instance.SetBoolInternal(target, value);

            if (PlayerData.instance.completionPercentage != _completionPercentage)
            {
                _completionPercentage = PlayerData.instance.completionPercentage;
                WriteHtmlDataFile();
            }
        }
        
        public void WriteHtmlDataFile()
        {
            string path = Directory.GetCurrentDirectory() + @"\hollow_knight_Data\Managed\Mods\CompletionPercentage";
            string fileName = "data.html";
            string fullPath = path + "\\" + fileName;
            string timeString = DateTime.Now.ToString("hh:mm:ss");

            // Create folder if doesn't exist 
            System.IO.Directory.CreateDirectory(path);

            try    
            {    
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(fullPath))    
                {    
                    File.Delete(fullPath);    
                }    
    
                // Create a new file     
                using (FileStream fs = File.Create(fullPath))
                {
                    string html = $"<html><body><script>" +
                                  $"const data = {{" +
                                  $"completionPercentage: {_completionPercentage}," +
                                  $"lastUpdateTime: '{timeString}'" +
                                  $"}};" +
                                  $"parent.postMessage(data, \"*\");" +
                                  $"</script></body></html>";
                    
                    fs.Write(new UTF8Encoding(true).GetBytes(html), 0, html.Length);
                }
            }    
            catch (Exception Ex)    
            {    
                Log(Ex.ToString());    
            }
        }
    }
}