using System;
using System.Data.Entity;
using System.Linq;

namespace Diary
{
    public class ApplicationBbContext : DbContext
    {
         public ApplicationBbContext()
            : base("name=ApplicationBbContext")
        {
        }
    }
}