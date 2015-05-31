using System.Linq;
using NUnit.Framework;

namespace poshring.tests
{
    [TestFixture]
    public class IntegrationTests
    {
        [Test]
        public void ReadsAllCredentials()
        {
            var cm = new CredentialsManager();
            var credentials = cm.GetCredentials().ToList();

            Assert.That(credentials.Count, Is.GreaterThan(0));
        }

        [Test]
        public void ManagesPasswordCredential()
        {
            var cm = new CredentialsManager();

            try
            {
                cm.AddPasswordCredential("test", "testy McTester", "dothetestydance", "test credential");
                var credential = cm.GetCredentials().Single(s => s.TargetName == "test");
                Assert.That(credential.UserName, Is.EqualTo("testy McTester"));
                Assert.That(credential.Comment, Is.EqualTo("test credential"));
                Assert.That(credential.CredentialBlob, Is.EqualTo("dothetestydance"));

                credential.Comment = "Hobos ride the train.";
                credential.UserName = "Biscut";
                credential.CredentialBlob = "sekret";
                credential.Save();

                credential = cm.GetCredentials().Single(s => s.TargetName == "test");
                Assert.That(credential.Comment, Is.EqualTo("Hobos ride the train."));
                Assert.That(credential.UserName, Is.EqualTo("Biscut"));
                Assert.That(credential.CredentialBlob, Is.EqualTo("sekret"));
            }
            finally
            {
                var credentials = cm.GetCredentials().Where(s => s.TargetName == "test").ToList();
                credentials.ForEach(s => cm.DeleteCredential(s));
            }
        }
    }
}