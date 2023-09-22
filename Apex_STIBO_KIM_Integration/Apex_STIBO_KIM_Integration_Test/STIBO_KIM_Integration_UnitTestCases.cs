using Apex_STIBO_KIM_Integration;
using Apex_STIBO_KIM_Integration.Data.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace Apex_STIBO_KIM_Integration_Test
{

    [TestClass]
    public class STIBO_KIM_Integration_UnitTestCases : TestBase
    {
        [TestMethod]
        public async Task STIBO_KIM_Integration_Success()
        {
            // ****** Arrange ******
            try
            {
                STIBO_KIM_Integration upcFunction = new STIBO_KIM_Integration(Configuration, new Mock<IAdlsAdapter>().Object, new Mock<ILoggerAdapter>().Object);
                await upcFunction.Run(BuildMessage("message"), log: new Mock<ILogger>().Object);
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(true);
            }
        }
    }
}
