using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NewStudio.Models
{
    public class searchViewModel
    {
        public searchViewModel(List<Tovar> mebel, int? selected, string name)
        {
            mebel.Insert(0, new Tovar { Name = "Все", Id = 0 });
        }
        public SelectList Mebels { get; private set; } // список товара
        public int? SelectedStuff { get; private set; }   // выбранный товар
        public string SelectedName { get; private set; }    // введенное название
    }
}
