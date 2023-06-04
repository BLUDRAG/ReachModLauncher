using System;

namespace ReachModLauncher
{
    [Serializable]
    public class SuperUserData
    {
        public string VerificationKey;
        public string User;
        public bool   BypassPOIUploadLimits;
    }
}
