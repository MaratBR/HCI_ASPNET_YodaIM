using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YodaIM.Models;

namespace YodaIM.Services.Database
{
    public class DatabaseServiceBase
    {
        protected readonly Context context;
        public DatabaseServiceBase(Context context)
        {
            this.context = context;
        }
    }
}
