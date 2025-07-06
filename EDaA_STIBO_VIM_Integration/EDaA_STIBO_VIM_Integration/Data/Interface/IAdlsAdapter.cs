using System.Threading.Tasks;

namespace EDaA_STIBO_VIM_Integration
{
    /// <summary>
    /// AdlsAdapter Interface
    /// </summary>
    public interface IAdlsAdapter
    {
        /// <summary>
        /// CreateFileAsync interface
        /// </summary>
        /// <param name="ProductData"></param>
        /// <param name="FilePath"></param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        Task CreateFileAsync(string ProductData, string FilePath, string FileName);
    }
}
