using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NewStudio.Models
{
    public class fvm
    {
        public fvm(List<Tovar> Tovar, int? selected, string name)
        {
            Tovar.Insert(0,new Tovar { Name = "Все", Id=0});
        }
        public SelectList Tovars { get; private set; } // список товара
        public int? SelectedStuff { get; private set; }   // выбранный товар
        public category SelectedCategory { get; private set; }    // введенное название
    }
}
