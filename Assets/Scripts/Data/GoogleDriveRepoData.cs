using UnityEngine;

namespace ReachModLauncher
{
    public class GoogleDriveRepoData : ScriptableObject
    {
        public string RepoDirectory => _repoDirectory;
        public string UserData      => _userData.text;
        
        [SerializeField] private string    _repoDirectory;
        [SerializeField] private TextAsset _userData;
    }
}
