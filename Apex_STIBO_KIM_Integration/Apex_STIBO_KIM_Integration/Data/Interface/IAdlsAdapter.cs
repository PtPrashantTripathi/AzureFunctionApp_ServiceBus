using System.Threading.Tasks;

namespace Apex_STIBO_KIM_Integration
{
    /// <summary>
    /// AdlsAdapter Interface
    /// </summary>
    public interface IAdlsAdapter
    {
        /// <summary>
        /// CreateFileAsync interface
        /// </summary>
        /// <param name="PosData"></param>
        /// <param name="FilePath"></param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        Task CreateFileAsync(string PosData, string FilePath, string FileName);
    }
}
