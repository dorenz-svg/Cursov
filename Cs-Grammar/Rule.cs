using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class Rule
    {
        /// <summary>
        /// нетерминал
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// правила к этому нетерминалу
        /// </summary>
        public List<string> Rules  { get; set; }
    }
}
