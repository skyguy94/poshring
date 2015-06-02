using System.Linq;
using System.Management.Automation;

namespace poshring.cmdlets
{
    [Cmdlet(VerbsCommon.Get, "PSCredential")]
    public class GetCredentialCommand : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public string TargetName { get; set; }

        [Parameter(Mandatory = true)]
        public string UserName { get; set; }

        protected override void ProcessRecord()
        {
            var cm = new CredentialsManager();
            using (var credential = cm.GetCredentials().SingleOrDefault(c => c.UserName == UserName && c.TargetName == TargetName))
            {
                WriteObject(credential);
            }
        }
    }
}