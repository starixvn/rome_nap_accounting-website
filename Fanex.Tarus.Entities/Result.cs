// -----------------------------------------------------------------------
// <copyright file="Result.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Runtime.Serialization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    /// 
    
    public class ResultDetail
    {
       
        public int ErrCode { get; set; }

        
        public string ErrMsg { get; set; }

        public ResultDetail()
        {
            ErrCode = 0;
            ErrMsg = string.Empty;
        }
    }
}
