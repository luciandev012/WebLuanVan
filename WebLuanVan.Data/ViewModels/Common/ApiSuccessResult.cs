using System;
using System.Collections.Generic;
using System.Text;

namespace WebLuanVan.Data.ViewModels.Common
{
    public class ApiSuccessResult<T> : ApiResult<T>
    {
        public ApiSuccessResult(T obj)
        {
            IsSuccessed = true;
            ResultObj = obj;
        }
        public ApiSuccessResult()
        {
            IsSuccessed = true;
        }
    }
}
