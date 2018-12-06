using System;
using System.Web.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WS.Business;

namespace UnitTests_WebStorage
{
    [TestClass]
    public class UnitestPathProvider
    {
        [TestMethod]
        public void TestMethod1()
        {
            HostingEnvironment environment = null;
            PathProvider provider=new PathProvider(environment);
            string path = "dim/dom/dam/text.txt";
            string userId = "1234qwerasdf";
            string actual=provider.SplitPath(path, userId);
        }
    }
}
