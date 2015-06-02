using System.Linq;
using System.Management.Automation;

namespace poshring.cmdlets
{
    [Cmdlet(VerbsCommon.Set, "PSCredential")]
    public class SetCredentialCommand : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public string TargetName { get; set; }

        [Parameter(Mandatory = true)]
        public string UserName { get; set; }

        [Parameter(Mandatory = true)]
        public string Password { get; set; }

        protected override void ProcessRecord()
        {
            var cm = new CredentialsManager();
            using (var credential = cm.GetCredentials().Single(c => c.UserName == UserName && c.TargetName == TargetName))
            {
                credential.CredentialBlob = Password;
                credential.Save();
            }
        }
    }
}