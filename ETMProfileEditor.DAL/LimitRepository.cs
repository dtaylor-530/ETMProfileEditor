using CsvHelper;
using ETMProfileEditor.Contract;
using ETMProfileEditor.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ETMProfileEditor.DAL
{
    public class LimitRepository : ISelect<Limit>
    {
        public IEnumerable<Limit> Select()
        {
            using (var reader = new StringReader(Resource.ETMLimits1))
            using (var csv = new CsvReader(reader))
            {
                return csv.GetRecords<Limit>().ToArray();
            }
        }
    }
}