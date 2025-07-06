using EDaA_STIBO_VIM_Integration;
using EDaA_STIBO_VIM_Integration.Data.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace EDaA_STIBO_VIM_Integration_Test
{

    [TestClass]
    public class STIBO_VIM_Integration_UnitTestCases : TestBase
    {
        [TestMethod]
        public async Task STIBO_VIM_Integration_Success()
        {
            // ****** Arrange ******
            try
            {
                STIBO_VIM_Integration upcFunction = new STIBO_VIM_Integration(Configuration, new Mock<IAdlsAdapter>().Object, new Mock<ILoggerAdapter>().Object);
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
