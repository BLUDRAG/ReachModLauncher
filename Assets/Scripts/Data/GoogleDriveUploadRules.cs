using System;
using System.Collections.Generic;

namespace ReachModLauncher
{
    [Serializable]
    public sealed class GoogleDriveUploadRules
    {
        public string                      VerificationKey;
        public int                         DailyUploadLimit;
        public List<GoogleDriveUploadUser> UserBlacklist = new List<GoogleDriveUploadUser>();
    }
}
