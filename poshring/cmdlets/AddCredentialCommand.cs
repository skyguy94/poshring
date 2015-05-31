using System.Management.Automation;

namespace poshring.cmdlets
{
    [Cmdlet(VerbsCommon.Add, "Credential")]
    public class AddCredentialCommand : Cmdlet
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
            cm.AddPasswordCredential(TargetName, UserName, Password, "Created by Poshring.");
            WriteVerbose(string.Format("Added credentials for {0} at {1} to credential store.", UserName, TargetName));
        }
    }
}