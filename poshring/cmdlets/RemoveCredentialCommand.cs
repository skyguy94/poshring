using System.Linq;
using System.Management.Automation;

namespace poshring.cmdlets
{
    [Cmdlet(VerbsCommon.Remove, "PSCredential")]
    public class RemoveCredentialCommand : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public string TargetName { get; set; }

        [Parameter(Mandatory = true)]
        public string UserName { get; set; }

        protected override void ProcessRecord()
        {
            var cm = new CredentialsManager();
            using (var credential = cm.GetCredentials().Single(c => c.UserName == UserName && c.TargetName == TargetName))
            {
                cm.DeleteCredential(credential);
            }
        }
    }
}