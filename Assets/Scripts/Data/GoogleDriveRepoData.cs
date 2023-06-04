using UnityEngine;

namespace ReachModLauncher
{
    public sealed class GoogleDriveRepoData : ScriptableObject
    {
        public string RepoDirectory => _repoDirectory;
        public string UploadRules   => _uploadRulesFile;
        public string VerificationKey => _superUserVerificationKey;
        public string UserData      => _userData.text;
        
        [SerializeField] private string    _repoDirectory;
        [SerializeField] private string    _uploadRulesFile;
        [SerializeField] private string    _superUserVerificationKey;
        [SerializeField] private TextAsset _userData;
    }
}
