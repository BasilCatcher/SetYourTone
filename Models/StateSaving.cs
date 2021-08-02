using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetYourTone.Models
{
    public class StateSaving
    {
        //Текущие триггеры
        public string Triggers { get; set; }
        //Координаты
        public string Frame { get; set; }
        //Пары символ/цвет
        public string Colors { get; set; }
        //"Флажок" режима ограниченного пространства
        public string Limitation { get; set;}
        //"Флажок" режима буквальных триггеров
        public string Verbatim { get; set; }
    }

}
