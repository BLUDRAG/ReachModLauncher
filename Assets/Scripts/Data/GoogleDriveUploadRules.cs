using System;
using System.Collections.Generic;

namespace ReachModLauncher
{
    [Serializable]
    public class GoogleDriveUploadRules
    {
        public int                         DailyUploadLimit;
        public List<GoogleDriveUploadUser> UserBlacklist = new List<GoogleDriveUploadUser>();
    }
}
