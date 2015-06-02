using System.Linq;
using System.Management.Automation;

namespace poshring.cmdlets
{
    [Cmdlet(VerbsCommon.Get, "PSCredentials")]
    public class GetCredentialsCommand : Cmdlet
    {
        [Parameter]
        public string TargetName { get; set; }

        [Parameter]
        public string UserName { get; set; }

        [Parameter]
        public string Comment { get; set; }

        protected override void ProcessRecord()
        {
            var cm = new CredentialsManager();
            var credentials = cm.GetCredentials();

            var isTargetValid = !string.IsNullOrWhiteSpace(TargetName);
            if (isTargetValid)
            {
                credentials = credentials.Where(c => c.TargetName == TargetName);
            }

            var isUserValid = !string.IsNullOrWhiteSpace(UserName);
            if (isUserValid)
            {
                credentials = credentials.Where(c => c.UserName == UserName);
            }

            var isCommentValid = !string.IsNullOrWhiteSpace(Comment);
            if (isCommentValid)
            {
                credentials = credentials.Where(c => c.Comment == Comment);
            }

            WriteObject(credentials, true);
        }
    }
}
