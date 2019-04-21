using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace AwesomeTechnologies.Editor.CustomNodes
{
    #if UNITY_2019_1_OR_NEWER
    [InitializeOnLoad]
    public class VegetationStudioAssemblyUpdater
    {
        private const bool LogWhatsHappening = false;
        static VegetationStudioAssemblyUpdater()
        {
            RegisterAssembly(LogWhatsHappening);
        }
        
        [MenuItem("Window/Awesome Technologies/ShaderGraph/Register ShaderGraph Nodes")]
        private static void NewMenuOption()
        {
            RegisterAssembly(true);
        }

        static void RegisterAssembly(bool log)
        {
            var basePath = Path.GetFullPath(Application.dataPath);
            var projectFolder = Directory.GetParent(basePath);
            var packageCacheFolder = Path.Combine(new[] {projectFolder.FullName, "Library", "PackageCache"});
            
            if (!Directory.Exists(packageCacheFolder))
            {
                return;
            }

            var packageDirectories = Directory.GetDirectories(packageCacheFolder);
            bool shadergraphPackageFound = false;
            string shadergraphPackageDirectory = null;
            for (int i = 0; i < packageDirectories.Length; i++)
            {
                var directory = packageDirectories[i];
                if (Regex.IsMatch(directory, "shadergraph"))
                {
                    shadergraphPackageFound = true;
                    shadergraphPackageDirectory = directory;
                    if (log)
                        Debug.Log("ShaderGraph package directory found at " + shadergraphPackageDirectory);
                    break;
                }
            }

            if (!shadergraphPackageFound)
            {
                Debug.LogError("ShaderGraph package not found. Are you sure you imported ShaderGraph?");
                return;
            }
            
            var filepath = Path.Combine(new[] {packageCacheFolder, shadergraphPackageDirectory, "Editor", "AssemblyInfo.cs"});

            if (!File.Exists(filepath))
            {
                Debug.LogError("ShaderGraph package file not found at" + filepath + "\n Are you sure you imported ShaderGraph package?");
            }
            else
            {
                var text = File.ReadAllText(filepath);
                if (Regex.IsMatch(text, "VegetationStudioCustomNodes"))
                {
                    if (log)
                        Debug.Log("Assembly already registered!");
                    return;
                }
                
                #if UNITY_EDITOR_WIN
                    var assemblyFile = new FileInfo(filepath);
                    var security = assemblyFile.GetAccessControl();
                    security.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), 
                        FileSystemRights.FullControl, AccessControlType.Allow));
                    assemblyFile.SetAccessControl(security);
                    
                    if (log)
                        Debug.Log("Changed rights to this file on windows: " + filepath);
                #elif UNITY_EDITOR_OSX
                    Process.Start("chmod", "777 " + filepath);
                    if (log)
                        Debug.Log("Changed rights to this file on osx: " + filepath);
                #endif
                
                File.SetAttributes(filepath, File.GetAttributes(filepath) & ~FileAttributes.ReadOnly);
                
                using (var stream = File.AppendText(filepath))
                {
                    stream.Write("\n[assembly: InternalsVisibleTo(\"VegetationStudioCustomNodes\")]");
                }
            }
        }
    }
    #endif
}
