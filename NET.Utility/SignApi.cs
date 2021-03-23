using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Utilities
{
    /// <summary>
    /// 签名公司
    /// </summary>
    public enum CompanyProduct
    {
        /// <summary>
        /// 万科集团智慧工地智慧工地E劳务系统
        /// </summary>
        Vanke_E_LaborService
    }


    public class SignApi
    {
        CompanyProduct Product;
        /// <summary>
        /// 接口签名
        /// </summary>
        /// <param name="product"></param>
        public SignApi(CompanyProduct product)
        {
            Product = product;
        }

        /// <summary>
        /// 接口签名
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string UrlFormat(string url, string postData, string key)
        {
            string result = string.Empty;
            switch (Product)
            {
                case CompanyProduct.Vanke_E_LaborService:
                    string base64 = Base64Helper.Base64Encode(postData);
                    string sign = base64 + $",key={key}";
                    string signed = MD5Helper.MD5(sign);
                    result = url + $"?sign={signed}";
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
