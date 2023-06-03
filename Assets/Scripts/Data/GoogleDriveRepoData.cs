using UnityEngine;

namespace ReachModLauncher
{
    public sealed class GoogleDriveRepoData : ScriptableObject
    {
        public string RepoDirectory => _repoDirectory;
        public string UploadRules   => _uploadRulesFile;
        public string UserData      => _userData.text;
        
        [SerializeField] private string    _repoDirectory;
        [SerializeField] private string    _uploadRulesFile;
        [SerializeField] private TextAsset _userData;
    }
}
