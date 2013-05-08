using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Tarus.LiveCasino.Entities;
using Sunplus.Accounting.Entities.SpecialDelete;

namespace Sunplus.Accounting.Business.SpecialDelete
{
    public interface IUpdateBatchMsg
    {
        Dictionary<string, string> UploadMsg(Message pmsg, string TransIds);
    }
}
