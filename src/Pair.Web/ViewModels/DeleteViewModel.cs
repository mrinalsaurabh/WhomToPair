using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pair.Web.Models;

namespace Pair.Web.ViewModels
{
    public class DeleteViewModel
    {
        public List<Individual> AllIndividuals { get; set; }
        public string SelectedIndividual { get; set; }
    }
}
