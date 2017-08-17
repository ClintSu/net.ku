using System.Text;

namespace System
{
    public static class ExceptionExtensions
    {
        /// <summary>
        ///     获取详细错误堆栈信息
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string DetailMessage(this Exception ex)
        {
            var expt = ex;
            var sb = new StringBuilder();
            while (expt != null)
            {
                if (!expt.Message.IsNullOrEmpty())
                {
                    sb.AppendLine("→" + expt.Message);
                }
                expt = expt.InnerException;
            }
            return sb.ToString();
        }
    }
}